using System;
using System.IO;
using System.Text;
using System.Windows.Threading;

namespace TraderForPoe.Classes
{
    public class LogReaderLineEventArgs : EventArgs
    {
        public string Line { get; set; }
    }

    public class LogReader
    {
        private readonly string path;
        private readonly string delimiter;
        private readonly DispatcherTimer timer;
        private string buffer;
        private long size;
        private bool monitoring;

        public event EventHandler<LogReaderLineEventArgs> OnLineAddition;

        public TimeSpan Interval
        {
            get { return this.timer.Interval; }
            set { this.timer.Interval = value; }
        }

        public LogReader(string path, TimeSpan interval, string delimiter = "\n")
        {
            this.path = path;

            this.delimiter = delimiter;

            this.timer = new DispatcherTimer
            {
                Interval = interval,
            };
            timer.Tick += this.Check;
        }

        public void Start()
        {
            this.size = new FileInfo(this.path).Length;

            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private bool StartMonitoring()
        {
            lock (this.timer)
            {
                if (this.monitoring) return true;

                this.monitoring = true;
                return false;
            }
        }

        private void Check(object sender, EventArgs e)
        {
            if (!this.StartMonitoring()) return;

            var newSize = new FileInfo(this.path).Length;

            if (this.size >= newSize) return;

            using (var stream = File.Open(this.path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                sr.BaseStream.Seek(this.size, SeekOrigin.Begin);

                var data = this.buffer + sr.ReadToEnd();

                if (!data.EndsWith(this.delimiter))
                {
                    if (data.IndexOf(this.delimiter, StringComparison.Ordinal) == -1)
                    {
                        this.buffer += data;

                        data = string.Empty;
                    }
                    else
                    {
                        var pos = data.LastIndexOf(this.delimiter, StringComparison.Ordinal) + this.delimiter.Length;

                        this.buffer = data.Substring(pos);

                        data = data.Substring(0, pos);
                    }
                }

                var lines = data.Split(new[] { this.delimiter }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    this.OnLineAddition(this, new LogReaderLineEventArgs { Line = line.Trim() });
                }
            }

            this.size = newSize;

            lock (this.timer) this.monitoring = false;
        }
    }
}