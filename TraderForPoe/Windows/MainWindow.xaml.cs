using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TraderForPoe.Classes;
using TraderForPoe.Controls;
using TraderForPoe.Properties;
using TraderForPoe.ViewModel;
using TraderForPoe.Windows;

namespace TraderForPoe
{
    public partial class MainWindow : NotActivatableWindow
    {
        public MainWindowViewModel mvvm;

        private ClipboardMonitor clipMoni = new ClipboardMonitor();

        private Regex customerJoinedRegEx = new Regex(".* : (.*) has joined the area");

        private Regex customerLeftRegEx = new Regex(".* : (.*) has left the area");

        public static UserSettingsViewModel usvm;
        public static LogMonitorViewModel lmvm;
        public static LogReader logReader;







        public MainWindow()
        {
            InitializeComponent();

            //this.DataContext = new MainWindowViewModel();














            ////SubscribeToEvents();

            //logReader = new LogReader(Settings.Default.PathToClientTxt, TimeSpan.FromMilliseconds(200));
            //lmvm = new LogMonitorViewModel(logReader);
            //usvm = new UserSettingsViewModel();

            //logReader.Start();

            //logReader.OnLineAddition += LogReader_OnLineAddition;

            //clipMoni.OnChange += ClipMoni_OnChange;

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
                //new TradeObject(e.Line);
                TradeObject tItem = new TradeObject(e.Line);
                CustomTestCtrl uc = new CustomTestCtrl(tItem);
                //stk_MainPnl.Children.Add(uc);

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

        //private void ClickCollapseExpandMainwindow(object sender, RoutedEventArgs e)
        //{
        //    var mainWindowCollapsed = false;
        //    if (mainWindowCollapsed == false)
        //    {
        //        btn_collapseMainWindow.Width = this.Width;
        //        btn_collapseMainWindow.Content = "⏷";
        //        //stk_MainPnl.Visibility = Visibility.Collapsed;
        //        mainWindowCollapsed = true;
        //    }
        //    else
        //    {
        //        btn_collapseMainWindow.Width = btn_collapseMainWindow.MinWidth;
        //        btn_collapseMainWindow.Content = "⏶";
        //        //stk_MainPnl.Visibility = Visibility.Visible;
        //        mainWindowCollapsed = false;
        //    }
        //}


        //private void SubscribeToEvents()
        //{
        //    TradeItemControl.MoreThanThreeItems += TradeItemControl_MoreThanThreeItems;
        //    TradeItemControl.EqualThreeItems += TradeItemControl_LessThanThreeItems;
        //}

        //private void TradeItemControl_LessThanThreeItems(object sender, EventArgs e)
        //{
        //    btn_collapseMainWindow.Visibility = Visibility.Collapsed;
        //    brd_collapseMainWindow.Visibility = Visibility.Collapsed;
        //}

        //private void TradeItemControl_MoreThanThreeItems(object sender, EventArgs e)
        //{
        //    btn_collapseMainWindow.Visibility = Visibility.Visible;
        //    brd_collapseMainWindow.Visibility = Visibility.Visible;
        //}

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

        private void NotActivatableWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (mainTabControl != null)
            {
                if (e.Delta < 0)
                {
                    if (mainTabControl.SelectedIndex + 1 < mainTabControl.Items.Count)
                        mainTabControl.SelectedItem = mainTabControl.Items[mainTabControl.SelectedIndex + 1];
                }
                else
                {
                    if (mainTabControl.SelectedIndex - 1 > -1)
                        mainTabControl.SelectedItem = mainTabControl.Items[mainTabControl.SelectedIndex - 1];
                }
            }
        }

        //private void TabControl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
        //    {
        //        MessageBox.Show("Middle button clicked");
        //    }
        //}




    }
}