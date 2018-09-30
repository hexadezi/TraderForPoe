using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using TraderForPoe.Classes;
using TraderForPoe.Controls;
using TraderForPoe.Properties;
using TraderForPoe.ViewModel;
using TraderForPoe.Windows;

namespace TraderForPoe
{
    public partial class MainWindow : Window
    {
        private ClipboardMonitor clipMoni = new ClipboardMonitor();
        
        private Regex customerJoinedRegEx = new Regex(".* : (.*) has joined the area");

        private Regex customerLeftRegEx = new Regex(".* : (.*) has left the area");

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        
        private System.Windows.Forms.ToolStripMenuItem itmHistory = new System.Windows.Forms.ToolStripMenuItem("History");

        // Variables for reading the Client.txt
        private long lastReadLength;

        public static UserSettingsViewModel usvm;
        public static LogMonitorViewModel lmvm;
        private Classes.LogReader logReader;

        private bool mainWindowCollapsed = false;

        private System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            SubscribeToEvents();

            InitializeComponent();

            CheckForClientTxt();

            //StartFileMonitoring();

            // Subscribe to SetNoActiveWindow. Prevent window from focus
            Loaded += (object sender, RoutedEventArgs e) => WinAPI.SetNoActiveWindow(this);

            logReader = new LogReader(@"C:\Program Files (x86)\Steam\steamapps\common\Path of Exile\logs\Client.txt", TimeSpan.FromMilliseconds(200));
            lmvm = new LogMonitorViewModel(logReader);
            usvm = new UserSettingsViewModel();
            notifyIcon.DataContext = new NotifyIconViewModel();
            logReader.Start();

            logReader.OnLineAddition += LogReader_OnLineAddition;

        }

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            if (TradeObject.IsTradeWhisper(e.Line))
            {
                MessageBox.Show(new TradeObject(e.Line).ToString());
            }
        }

        private void CheckForClientTxt()
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Grinding Gear Games\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Grinding Gear Games\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Steam\steamapps\common\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Steam\steamapps\common\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Grinding Gear Games\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Grinding Gear Games\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\Path of Exile\logs\Client.txt")))
            {
                Settings.Default.PathToClientTxt = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\Path of Exile\logs\Client.txt");
                Settings.Default.Save();
                return;
            }
        }

        private void CheckIfPoeIsForegroundWindow()
        {
            if (Settings.Default.HideIfPoeNotForeGround)
            {
                //if (GetForegroundWindow() != FindWindow("POEWindowClass", "Path of Exile"))
                //    Hide();
                //else
                //    Show();
            }
        }

        private void ClickCollapseExpandMainwindow(object sender, RoutedEventArgs e)
        {
            if (mainWindowCollapsed == false)
            {
                btn_collapseMainWindow.Width = this.Width;
                btn_collapseMainWindow.Content = "⏷";
                stk_MainPnl.Visibility = Visibility.Collapsed;
                mainWindowCollapsed = true;
            }
            else
            {
                btn_collapseMainWindow.Width = btn_collapseMainWindow.MinWidth;
                btn_collapseMainWindow.Content = "⏶";
                stk_MainPnl.Visibility = Visibility.Visible;
                mainWindowCollapsed = false;
            }
        }

        private void CustMenuItem_OnItemCountExceed(object sender, EventArgs e)
        {
            // remove the first item in history list, if limit is reached
            itmHistory.DropDownItems.RemoveAt(0);
        }

        // Handle click event for the history menu item
        private void CustomMenuItem_Click(object sender, EventArgs e)
        {
            var s = sender as CustMenuItem;

            // Avoid exception by checking if the item is already in the panel
            if (!stk_MainPnl.Children.Contains(s.GetTradeItemCtrl))
            {
                stk_MainPnl.Children.Add(s.GetTradeItemCtrl);
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            string filePath = Settings.Default.PathToClientTxt;

            CheckIfPoeIsForegroundWindow();

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

                        var bytesRead = fs.Read(buffer, 0, buffer.Length);
                        lastReadLength += bytesRead;

                        var text = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        using (var reader = new StringReader(text))
                        {
                            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                            {
                                // Check for trade whispers
                                if (line.Contains(" @"))
                                {
                                    // Get a handle to POE. The window class and window name were obtained using the Spy++ tool.
                                    IntPtr poeHandle = Poe.GetHandle();

                                    // Verify that POE is a running process.
                                    if (poeHandle != IntPtr.Zero)
                                    {
                                        TradeItem tItem = new TradeItem(line);
                                        TradeItemControl uc = new TradeItemControl(tItem);
                                        stk_MainPnl.Children.Add(uc);
                                        var customMenuItem = new CustMenuItem(uc);
                                        customMenuItem.Click += CustomMenuItem_Click;
                                        itmHistory.DropDownItems.Add(customMenuItem);
                                    }
                                }

                                // Check if customer joined or left
                                else if (customerJoinedRegEx.IsMatch(line))
                                {
                                    MatchCollection matches = Regex.Matches(line, customerJoinedRegEx.ToString());

                                    foreach (Match match in matches)
                                    {
                                        foreach (TradeItemControl item in stk_MainPnl.Children)
                                        {
                                            if (item.tItem.Customer == match.Groups[1].Value)
                                            {
                                                item.CustomerJoined();
                                            }
                                        }
                                    }
                                }

                                // Check if customer left
                                else if (customerLeftRegEx.IsMatch(line))
                                {
                                    MatchCollection matches = Regex.Matches(line, customerLeftRegEx.ToString());

                                    foreach (Match match in matches)
                                    {
                                        foreach (TradeItemControl item in stk_MainPnl.Children)
                                        {
                                            if (item.tItem.Customer == match.Groups[1].Value)
                                            {
                                                item.CustomerLeft();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                dispatcherTimer.Tick -= DispatcherTimer_Tick;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }


        private void SubscribeToEvents()
        {
            TradeItemControl.MoreThanThreeItems += TradeItemControl_MoreThanThreeItems;
            TradeItemControl.EqualThreeItems += TradeItemControl_LessThanThreeItems;
            CustMenuItem.OnItemCountExceed += CustMenuItem_OnItemCountExceed;
        }

        private void TradeItemControl_LessThanThreeItems(object sender, EventArgs e)
        {
            btn_collapseMainWindow.Visibility = Visibility.Collapsed;
            brd_collapseMainWindow.Visibility = Visibility.Collapsed;
        }

        private void TradeItemControl_MoreThanThreeItems(object sender, EventArgs e)
        {
            btn_collapseMainWindow.Visibility = Visibility.Visible;
            brd_collapseMainWindow.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

    }
}