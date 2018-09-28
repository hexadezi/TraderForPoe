using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace TraderForPoe
{
    public class ClipboardTextEventArgs : EventArgs
    {
        public string Line { get; set; }
    }
    public sealed class ClipboardMonitor : IDisposable
    {
        private static class NativeMethods
        {
            /// <summary>
            /// Places the given window in the system-maintained clipboard format listener list.
            /// </summary>
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            /// <summary>
            /// Removes the given window from the system-maintained clipboard format listener list.
            /// </summary>
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

            /// <summary>
            /// Sent when the contents of the clipboard have changed.
            /// </summary>
            public const int WM_CLIPBOARDUPDATE = 0x031D;

            /// <summary>
            /// To find message-only windows, specify HWND_MESSAGE in the hwndParent parameter of the FindWindowEx function.
            /// </summary>
            public static IntPtr HWND_MESSAGE = new IntPtr(-3);
        }

        private HwndSource hwndSource = new HwndSource(0, 0, 0, 0, 0, 0, 0, null, NativeMethods.HWND_MESSAGE);

        public ClipboardMonitor()
        {
            hwndSource.AddHook(WndProc);
            NativeMethods.AddClipboardFormatListener(hwndSource.Handle);
        }

        public void Dispose()
        {
            NativeMethods.RemoveClipboardFormatListener(hwndSource.Handle);
            hwndSource.RemoveHook(WndProc);
            hwndSource.Dispose();
        }

        public void Stop()
        {
            hwndSource.RemoveHook(WndProc);
            NativeMethods.RemoveClipboardFormatListener(hwndSource.Handle);
        }

        public void Start()
        {
            hwndSource.AddHook(WndProc);
            NativeMethods.AddClipboardFormatListener(hwndSource.Handle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {
                if (Clipboard.ContainsText())
                {

                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            OnChange?.Invoke(this, new ClipboardTextEventArgs { Line = Clipboard.GetText(TextDataFormat.UnicodeText) });
                            break;
                        }
                        catch (COMException ex)
                        {
                            //fix for OpenClipboard Failed (Exception from HRESULT: 0x800401D0 (CLIPBRD_E_CANT_OPEN))
                            //https://stackoverflow.com/questions/12769264/openclipboard-failed-when-copy-pasting-data-from-wpf-datagrid
                            //https://stackoverflow.com/questions/68666/clipbrd-e-cant-open-error-when-setting-the-clipboard-from-net
                            if (ex.ErrorCode == -2147221040)
                                System.Threading.Thread.Sleep(10);
                            else
                                throw new Exception("Unable to get Clipboard text. Message: \n" + ex.Message);
                        }
                    }


                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Occurs when the clipboard content changes and the content is text.
        /// </summary>
        public event EventHandler<ClipboardTextEventArgs> OnChange;
    }
}
