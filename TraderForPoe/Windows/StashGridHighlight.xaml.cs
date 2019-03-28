using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TraderForPoe.Classes;
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

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Used to get the client area of POE and the height and widht
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        IntPtr poeHandle = FindWindow("POEWindowClass", "Path of Exile");

        // Needed to determine if poe window location changed
        private IntPtr hWinEventHook;
        protected Hook.WinEventDelegate WinEventDelegate;
        uint poeProcessId;

        public StashGridHighlight()
        {
            // Verify that POE is a running process.
            if (poeHandle == IntPtr.Zero)
            {
                MessageBox.Show("Path of Exile is not running.");
                return;
            }

            InitializeComponent();

            Loaded += (object sender, RoutedEventArgs e) => SetNoActiveWindow();

            UpdateLocationAndSize();

            WinEventDelegate = new Hook.WinEventDelegate(WinEventCallback);

            try
            {
                if (poeHandle != IntPtr.Zero)
                {
                    uint TargetThreadId = Hook.GetWindowThread(poeHandle);
                    UnsafeNativeMethods.GetWindowThreadProcessId(poeHandle, out poeProcessId);
                    hWinEventHook = Hook.WinEventHookOne(Hook.SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE,
                                                         WinEventDelegate,
                                                         poeProcessId,
                                                         TargetThreadId);
                }

            }
            catch (Exception ex)
            {
                //ErrorManager.Logger(this, this.InitializeComponent(), ex.HResult, ex.Data, DateTime.Now);
                throw ex;
            }
        }

        protected void WinEventCallback(IntPtr hWinEventHook, Hook.SWEH_Events eventType, IntPtr hWnd, Hook.SWEH_ObjectId idObject, long idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hWnd == poeHandle && eventType == Hook.SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE && idObject == (Hook.SWEH_ObjectId)Hook.SWEH_CHILDID_SELF)
            {
                // Occurs when POE window is moved or size changed
                UpdateLocationAndSize();
            }
        }

        private void UpdateLocationAndSize()
        {

            GetClientRect(poeHandle, out RECT clientRect);
            GetWindowRect(poeHandle, out RECT windowRect);

            double borderSize = (windowRect.Right - (windowRect.Left + clientRect.Right)) / 2;
            double titleBarSize = (windowRect.Bottom - (windowRect.Top + clientRect.Bottom)) - borderSize;

            double x = windowRect.Left + borderSize;
            double y = windowRect.Top + titleBarSize;
            double h = clientRect.Bottom;
            double w = clientRect.Right;

            row_1.Height = new GridLength(h * 0.02);
            row_2.Height = new GridLength(h * 0.035);
            row_3.Height = GridLength.Auto;

            if ((w / h) < 1.35)
            {
                this.Top = y + (h * 0.094);

                this.Left = x + (w * 0.011);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }
            else if ((w / h) < 1.9)
            {
                this.Top = y + (h * 0.094);

                this.Left = x + (w * 0.0088);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }
            else if ((w / h) < 1.99)
            {
                this.Top = y + (h * 0.097);

                this.Left = x + (w * 0.008);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }

            else if ((w / h) < 2.3)
            {

                this.Top = y + (h * 0.097);

                this.Left = x + (w * 0.007);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }

            else if ((w / h) < 2.58)
            {
                this.Top = y + (h * 0.096);

                this.Left = x + (w * 0.006);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);

            }

            else if ((w / h) < 2.8)
            {
                this.Top = y + (h * 0.0959);

                this.Left = x + (w * 0.0055);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);

            }

            else if ((w / h) < 3.1)
            {
                this.Top = y + (h * 0.0955);

                this.Left = x + (w * 0.005);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }

            else
            {
                this.Top = y + (h * 0.094);

                this.Left = x + (w * 0.004);

                this.Height = (h * 0.585) + row_1.Height.Value + row_2.Height.Value;

                this.Width = (h * 0.585);
            }

            front_canvas.Width = this.Width;

            front_canvas.Height = this.Height;
        }

        public void AddButton(TradeItem tItemArgs)
        {
            UpdateLocationAndSize();

            if (ContainsItem(tItemArgs) == false && !String.IsNullOrEmpty(tItemArgs.Stash))
            {
                StashControl sCtrl = new StashControl(tItemArgs);

                sCtrl.MouseEnter += MouseEnterDrawRectangle;

                spnl_Buttons.Children.Add(sCtrl);
            }

        }

        private bool ContainsItem(TradeItem tItemArgs)
        {
            foreach (StashControl item in spnl_Buttons.Children)
            {
                if (item.GetTItem.Item == tItemArgs.Item && item.GetTItem.Customer == tItemArgs.Customer && item.GetTItem.Price == tItemArgs.Price && item.GetTItem.StashPosition == tItemArgs.StashPosition)
                {
                    return true;
                }
            }
            return false;
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

        private void MouseEnterDrawRectangle(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Clear canvas before drawing new rectangle
            front_canvas.Children.Clear();

            // Start timer to clear canvas after some time
            StartDispatcherTimer();

            // Cast sender as StashControl
            StashControl stashControl = (StashControl)sender;

            double x = stashControl.GetTItem.StashPosition.X;

            double y = stashControl.GetTItem.StashPosition.Y;

            // Nomber of stash columns
            int nbrRectStash = 12;

            // Set rectangle size by dividing canvas by number of columns
            double rectDimensionX = ((front_canvas.Width) / 12);

            // Check settings if the stashtab is quad or not
            foreach (var item in Settings.Default.QuadStash)
            {
                if (item == stashControl.GetTItem.Stash)
                {
                    rectDimensionX = rectDimensionX / 2;
                    nbrRectStash = nbrRectStash * 2;
                }
            }
                
            // Check if stash is quad. If true divide rectangle size and multiply number of columns by 2
            if ((x > 12 && x < 25) || (y > 12 && y < 25))
            {
                rectDimensionX = rectDimensionX / 2;
                nbrRectStash = nbrRectStash * 2;
                Settings.Default.QuadStash.Add(stashControl.GetTItem.Stash);
                Settings.Default.Save();
            }


            for (int iX = 1; iX <= nbrRectStash; iX++)
            {
                // Create the rectangle
                Rectangle rectangleHighlight = new Rectangle()
                {
                    Width = rectDimensionX,
                    Height = rectDimensionX,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                };

                if (iX == x)
                {

                    for (int iY = 1; iY <= nbrRectStash; iY++)
                    {
                        if (iY == y)
                        {
                            front_canvas.Children.Add(rectangleHighlight);
                            Canvas.SetLeft(rectangleHighlight, (iX - 1) * rectDimensionX);
                            Canvas.SetTop(rectangleHighlight, (iY - 1) * rectDimensionX);
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
                if (item.GetTItem.Item == tItemArgs.Item && item.GetTItem.Customer == tItemArgs.Customer && item.GetTItem.Price == tItemArgs.Price)
                {
                    spnl_Buttons.Children.Remove(item);
                    break; //important step
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Unhook event when closing
            Hook.WinEventUnhook(hWinEventHook);
        }
    }
}
