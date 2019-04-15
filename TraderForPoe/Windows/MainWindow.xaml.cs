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
        //public MainWindowViewModel mvvm;

        private ClipboardMonitor clipMoni = new ClipboardMonitor();

        //private Regex customerJoinedRegEx = new Regex(".* : (.*) has joined the area");

        //private Regex customerLeftRegEx = new Regex(".* : (.*) has left the area");

        public MainWindow()
        {

            TestFenster tFen = new TestFenster();
            tFen.tctrlItems.Items.Add(new CustomTabItem());
            tFen.tctrlItems.Items.Add(new CustomTabItem());
            tFen.tctrlItems.Items.Add(new CustomTabItem());
            tFen.Show();

            //this.DataContext = StartUpClass.VM_MainWindow;

            InitializeComponent();

            //SubscribeToEvents();


            StartUpClass.LogFileReader.Start();

            StartUpClass.LogFileReader.OnLineAddition += LogReader_OnLineAddition;

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
                if (Poe.IsForegroundWindow())
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }

        private void ClickCollapseExpandMainwindow(object sender, RoutedEventArgs e)
        {
            var mainWindowCollapsed = false;
            if (mainWindowCollapsed == false)
            {
                btn_collapseMainWindow.Width = this.Width;
                btn_collapseMainWindow.Content = "⏷";
                //stk_MainPnl.Visibility = Visibility.Collapsed;
                mainWindowCollapsed = true;
            }
            else
            {
                btn_collapseMainWindow.Width = btn_collapseMainWindow.MinWidth;
                btn_collapseMainWindow.Content = "⏶";
                //stk_MainPnl.Visibility = Visibility.Visible;
                mainWindowCollapsed = false;
            }
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