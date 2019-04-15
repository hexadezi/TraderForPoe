using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TraderForPoe.Controls;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für TestFenster.xaml
    /// </summary>
    public partial class TestFenster : Window
    {
        public TestFenster()
        {
            InitializeComponent();
            tctrlItems.Items.Add(new CustomTabItem());
            tctrlItems.Items.Add(new CustomTabItem());
            tctrlItems.Items.Add(new CustomTabItem());
        }

        private void TabControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                MessageBox.Show("Middle button clicked");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TabControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TabControl tabControl = tctrlItems;
            if (tabControl != null)
            {
                if (e.Delta < 0)
                {
                    if (tabControl.SelectedIndex + 1 < tabControl.Items.Count)
                        tabControl.SelectedItem = tabControl.Items[tabControl.SelectedIndex + 1];
                }
                else
                {
                    if (tabControl.SelectedIndex - 1 > -1)
                        tabControl.SelectedItem = tabControl.Items[tabControl.SelectedIndex - 1];
                }
            }
        }


        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            stpCmds.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Dispatcher.Invoke(() => stpCmds.Visibility = Visibility.Collapsed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tctrlItems.Items.Remove(tctrlItems.SelectedItem);
        }

        private void mainBrd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
