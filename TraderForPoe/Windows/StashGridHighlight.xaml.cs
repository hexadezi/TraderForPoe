using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TraderForPoe.Controls;
using TraderForPoe.Properties;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für StashGridHighlight.xaml
    /// </summary>
    public partial class StashGridHighlight : Window
    {
        // Needed to prevent window getting focus
        const int GWL_EXSTYLE = -20;
        const int WS_EX_NOACTIVATE = 134217728;
        const int LSFW_LOCK = 1;

        [DllImport("user32")]
        public static extern bool LockSetForegroundWindow(uint UINT);

        [DllImport("user32")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public StashGridHighlight()
        {
            InitializeComponent();

            Loaded += (object sender, RoutedEventArgs e) => SetNoActiveWindow();

            this.Top = (SystemParameters.PrimaryScreenHeight * 0.15) - 70;
            //this.Top = 162;

            this.Height = (SystemParameters.PrimaryScreenHeight * 0.58475925925925924) + 70;
            //this.Height = 631;

            this.Left = (SystemParameters.PrimaryScreenWidth * 0.008854166666666666);
            //this.Left = 17;

            this.Width = (SystemParameters.PrimaryScreenHeight * 0.58475925925925924);
            //this.Width = 631;

        }

        public void AddButton(TradeItem tItemArgs)
        {
            foreach (StashControl item in spnl_Buttons.Children)
            {
                if (item.GetTItem.Item == tItemArgs.Item && item.GetTItem.Customer == tItemArgs.Customer)
                {
                    return;
                }
            }
            StashControl sCtrl = new StashControl(tItemArgs);

            sCtrl.MouseEnter += MouseEnterDrawPoint;

            spnl_Buttons.Children.Add(sCtrl);
        }

        private void SetNoActiveWindow()
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE, WS_EX_NOACTIVATE);
            LockSetForegroundWindow(LSFW_LOCK);
        }

        private void StartDispatcherTimer()
        {
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 5);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            front_canvas.Children.Clear();
        }

        private void MouseEnterDrawPoint(object sender, System.Windows.Input.MouseEventArgs e)
        {
            front_canvas.Children.Clear();

            StartDispatcherTimer();

            StashControl stashControl = (StashControl)sender;

            double x = stashControl.GetTItem.StashPosition.X - 1;

            double y = stashControl.GetTItem.StashPosition.Y - 1;

            int number = 48;

            double dim = (this.Width) / 12;

            foreach (var item in Settings.Default.QuadStash)
            {
                if (item == stashControl.GetTItem.Stash)
                {
                    dim = dim / 2;
                }
            }

            int top = 0;

            int left = 0;

            for (int i = 0; i < number; i++)
            {
                // Create the rectangle
                Rectangle rec = new Rectangle()
                {
                    Width = dim,
                    Height = dim,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                };
                //front_canvas.Children.Add(rec);

                if (i == x)
                {
                    for (int i2 = 1; i2 < number; i2++)
                    {
                        Rectangle rec2 = new Rectangle()
                        {
                            Width = dim,
                            Height = dim,
                            Stroke = Brushes.Red,
                            StrokeThickness = 1,
                        };


                        if (i2 == y)
                        {
                            front_canvas.Children.Add(rec2);
                            Canvas.SetLeft(rec2, left + (i * dim));
                            Canvas.SetTop(rec2, top + (i2 * dim));
                        }

                    }
                }

            }
        }

        public void ClearCanvas()
        {
            this.front_canvas.Children.Clear();

        }

        internal void RemoveStashControl(TradeItem tItemArgs)
        {
            foreach (StashControl item in spnl_Buttons.Children)
            {
                if (item.GetTItem.Item == tItemArgs.Item && item.GetTItem.Customer == tItemArgs.Customer)
                {
                    spnl_Buttons.Children.Remove(item);
                    break; //important step
                }
            }
        }
    }
}
