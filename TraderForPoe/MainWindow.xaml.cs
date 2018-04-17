using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using TraderForPoe.Properties;
using TraderForPoe.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace TraderForPoe
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        // Needed to prevent window getting focus
        const int GWL_EXSTYLE = -20;
        const int WS_EX_NOACTIVATE = 134217728;
        const int LSFW_LOCK = 1;

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // Needed to prevent Application taking focus
        [DllImport("user32")]
        public static extern bool LockSetForegroundWindow(uint UINT);

        [DllImport("user32")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // Get a handle to POE. The window class and window name were obtained using the Spy++ tool.
        IntPtr poeHandle = FindWindow("POEWindowClass", "Path of Exile");

        // Variables for reading the Client.txt
        string filePath = Settings.Default.PathToClientTxt;

        long initialFileSize;

        long lastReadLength;

        ClipboardMonitor clipMoni = new ClipboardMonitor();

        InputSimulator iSim = new InputSimulator();

        About aboutWindow = new About();

        public MainWindow()
        {
            if (poeHandle == IntPtr.Zero)
            {
                // Show message box if POE is not running
                MessageBox.Show("Path of Exile is not running.");
                Application.Current.Shutdown();
            }

            InitializeComponent();



            Loaded += (object sender, RoutedEventArgs e) => SetNoActiveWindow();

            clipMoni.OnClipboardContentChanged += ClipMoni_OnClipboardContentChanged;


            this.Top = Settings.Default.WindowLocation.X;

            this.Left = Settings.Default.WindowLocation.Y;

            // Set variables befor Timer start
            initialFileSize = new FileInfo(filePath).Length;
            lastReadLength = initialFileSize - 1024;

            StartDispatcherTimer();

            CreateContextMenu();
        }

        private void ClipMoni_OnClipboardContentChanged(object sender, EventArgs e)
        {
            string strClipboard = Clipboard.GetText(TextDataFormat.UnicodeText);

            if (strClipboard.StartsWith("@"))
            {
                // Verify that POE is a running process.
                if (poeHandle == IntPtr.Zero)
                {
                    // Show message box if POE is not running
                    MessageBox.Show("Path of Exile is not running.");
                    return;
                }

                // Need to press ALT because the SetForegroundWindow sometimes does not work
                iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

                // Make POE the foreground application and send input
                SetForegroundWindow(poeHandle);

                Thread.Sleep(100);

                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                // Send the input
                iSim.Keyboard.TextEntry(strClipboard);

                // Send RETURN
                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            }
        }

        private void CreateContextMenu()
        {
            nIcon.MouseClick += NIcon_MouseClick;
            System.IO.Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/ico_Application.ico")).Stream;
            nIcon.Icon = new System.Drawing.Icon(iconStream);
            nIcon.Visible = true;
            System.Windows.Forms.ContextMenu cMenu = new System.Windows.Forms.ContextMenu();
            cMenu.MenuItems.Add("Settings", new EventHandler(CMenu_Settings));
            cMenu.MenuItems.Add("About", new EventHandler(CMenu_About));
            cMenu.MenuItems.Add("Exit", new EventHandler(CMenu_Close));
            nIcon.ContextMenu = cMenu;
        }

        private void CMenu_Settings(object sender, EventArgs e)
        {
            // TODO: Implementieren
        }

        private void CMenu_About(object sender, EventArgs e)
        {
            // TODO: Implementieren
        }

        private void StartDispatcherTimer()
        {
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (lastReadLength < 0)
            {
                lastReadLength = 0;
            }

            try
            {
                var fileSize = new FileInfo(filePath).Length;
                if (fileSize > lastReadLength)
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(lastReadLength, SeekOrigin.Begin);
                        var buffer = new byte[1024];

                        while (true)
                        {
                            var bytesRead = fs.Read(buffer, 0, buffer.Length);
                            lastReadLength += bytesRead;

                            if (bytesRead == 0)
                                break;

                            var text = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                            using (var reader = new StringReader(text))
                            {
                                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                                {
                                    if (line.Contains(" @"))
                                    {
                                        try
                                        {
                                            TradeItem tItem = new TradeItem(line);
                                            TradeItemControl uc = new TradeItemControl(tItem);
                                            stk_MainPnl.Children.Add(uc);
                                        }
                                        catch (Exception)
                                        {
                                            // Ignore Exception. Exception was thrown, because no RegEx pattern matched
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void CMenu_Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void SetNoActiveWindow()
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE, WS_EX_NOACTIVATE);
            LockSetForegroundWindow(LSFW_LOCK);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String strWhisper = "@From Labooo: wtb Caer Blaidd, Wolfpack's Den Underground River Map listed for 100 alch in bestiary (stash \"~price 1 alt\"; left 2, top 8)";
            TradeItem tItem = new TradeItem(strWhisper);
            TradeItemControl uc = new TradeItemControl(tItem);
            stk_MainPnl.Children.Add(uc);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            nIcon.Visible = false;
            nIcon.Dispose();
            System.Windows.Forms.Application.DoEvents();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Settings.Default.WindowLocation = new System.Drawing.Point((int)this.Top, (int)this.Left);
            Settings.Default.Save();
        }
    }
}
