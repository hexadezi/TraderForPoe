using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TraderForPoe.Classes;
using TraderForPoe.Controls;
using TraderForPoe.Properties;
using TraderForPoe.ViewModel;

namespace TraderForPoe
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel mvvm;

        private ClipboardMonitor clipMoni = new ClipboardMonitor();

        private Regex customerJoinedRegEx = new Regex(".* : (.*) has joined the area");

        private Regex customerLeftRegEx = new Regex(".* : (.*) has left the area");
        
        public static UserSettingsViewModel usvm;
        public static LogMonitorViewModel lmvm;
        private LogReader logReader;

        public MainWindow()
        {
            //mvvm = new MainWindowViewModel();

            //this.DataContext = mvvm;

            InitializeComponent();

            SubscribeToEvents();

            logReader = new LogReader(Settings.Default.PathToClientTxt, TimeSpan.FromMilliseconds(200));
            lmvm = new LogMonitorViewModel(logReader);
            usvm = new UserSettingsViewModel();
            notifyIcon.DataContext = new NotifyIconViewModel();
            notifyIcon.UpdateLayout();

            logReader.Start();

            logReader.OnLineAddition += LogReader_OnLineAddition;

            clipMoni.OnChange += ClipMoni_OnChange;

        }

        private void ClipMoni_OnChange(object sender, ClipboardTextEventArgs e)
        {
            if (TradeObject.IsTradeWhisper(e.Line))
            {
                Poe.SendCommand(e.Line);
            }
        }

        private void LogReader_OnLineAddition(object sender, LogReaderLineEventArgs e)
        {
            if (TradeObject.IsLogTradeWhisper(e.Line))
            {
                new TradeObject(e.Line);
                TradeItem tItem = new TradeItem(e.Line);
                TradeItemControl uc = new TradeItemControl(tItem);
                stk_MainPnl.Children.Add(uc);
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
            var mainWindowCollapsed = false;
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
            CheckIfPoeIsForegroundWindow();

            TradeItem tItem = new TradeItem("");
            TradeItemControl uc = new TradeItemControl(tItem);
            stk_MainPnl.Children.Add(uc);
            var customMenuItem = new CustMenuItem(uc);
            customMenuItem.Click += CustomMenuItem_Click;
        }

        private void SubscribeToEvents()
        {
            TradeItemControl.MoreThanThreeItems += TradeItemControl_MoreThanThreeItems;
            TradeItemControl.EqualThreeItems += TradeItemControl_LessThanThreeItems;
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