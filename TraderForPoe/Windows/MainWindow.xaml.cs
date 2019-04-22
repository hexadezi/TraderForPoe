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
        private Regex customerJoinedRegEx = new Regex(".* : (.*) has joined the area");

        private Regex customerLeftRegEx = new Regex(".* : (.*) has left the area");

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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
    }
}