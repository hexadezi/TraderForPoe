using System;
using System.IO;
using System.Text;
using System.Windows.Threading;
using TraderForPoe.Properties;

namespace TraderForPoe.Classes
{
    public static class LogReader
    {
        #region Fields

        private static readonly string delimiter = "\n";
        private static readonly string path = Settings.Default.PathToClientTxt;
        private static readonly DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(200) };
        private static string buffer;
        private static bool monitoring;
        private static long size = new FileInfo(path).Length;

        #endregion Fields

        #region MyRegion

        static LogReader()
        {
            path = Settings.Default.PathToClientTxt;
            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += Check;
            Start();
        }

        #endregion MyRegion

        #region Events

        public static event EventHandler<LogReaderLineEventArgs> OnLineAddition;

        #endregion Events

        #region Methods

        public static void Start()
        {
            if (timer.IsEnabled)
            {
                return;
            }

            timer.Start();
        }

        public static void Stop()
        {
            timer.Stop();
        }

        private static void Check(object sender, EventArgs e)
        {
            if (!StartMonitoring()) return;

            var newSize = new FileInfo(path).Length;

            if (size >= newSize) return;

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                sr.BaseStream.Seek(size, SeekOrigin.Begin);

                var data = buffer + sr.ReadToEnd();

                if (!data.EndsWith(delimiter))
                {
                    if (data.IndexOf(delimiter, StringComparison.Ordinal) == -1)
                    {
                        buffer += data;

                        data = string.Empty;
                    }
                    else
                    {
                        var pos = data.LastIndexOf(delimiter, StringComparison.Ordinal) + delimiter.Length;

                        buffer = data.Substring(pos);

                        data = data.Substring(0, pos);
                    }
                }

                var lines = data.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    OnLineAddition(null, new LogReaderLineEventArgs { Line = line.Trim() });
                }
            }

            size = newSize;

            lock (timer) monitoring = false;
        }

        private static bool StartMonitoring()
        {
            lock (timer)
            {
                if (monitoring) return true;
                monitoring = true;
                return false;
            }
        }
    }

    #endregion Methods

    public class LogReaderLineEventArgs : EventArgs
    {
        public string Line { get; set; }
    }
}