using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace TraderForPoe.Classes
{
    public class NotActivatableWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;

        private const int WS_EX_NOACTIVATE = 0x08000000;

        private IntPtr windowHandle;

        public NotActivatableWindow()
        {
            Loaded += NotActivatableWindow_Loaded;
        }

        private void NotActivatableWindow_Loaded(object sender, RoutedEventArgs e)
        {
            windowHandle = new WindowInteropHelper(this).Handle;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //Set the window style to noactivate.
            SetWindowLong(windowHandle, GWL_EXSTYLE,
            GetWindowLong(windowHandle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }
    }
}
