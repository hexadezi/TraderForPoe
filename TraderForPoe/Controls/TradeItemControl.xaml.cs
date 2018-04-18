using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WindowsInput;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using TraderForPoe.Windows;
using System.Drawing;
using TraderForPoe.Properties;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Threading;

namespace TraderForPoe
{
    /// <summary>
    /// Interaktionslogik für TradeItemPopup.xaml
    /// </summary>
    public partial class TradeItemControl : UserControl
    {
        

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private static StashGridHighlight stashGridHighlight = null;

        private double dblHeightBeforeCollapse;

        private double dblHeightToCollapse = 26;

        private TradeItem tItem;

        //-----------------------------------------------------------------------------------------

        public TradeItemControl(TradeItem tItemArg)
        {
            InitializeComponent();

            tItem = tItemArg;

            LoadSettings();

            SetupControls(tItem);

            StartAnimatioin();

            OpenStashGridHighlightWindow();


        }
        //-----------------------------------------------------------------------------------------

        private void OpenStashGridHighlightWindow()
        {
            if (stashGridHighlight == null)
            {
                stashGridHighlight = new StashGridHighlight();
                stashGridHighlight.Show();
            }
        }

        private void LoadSettings()
        {
            if (Settings.Default.CollapsedItems == true)
            {
                CollapseExpandItem();
            }

            if (Settings.Default.PlayNotificationSound)
            {
                System.Media.SystemSounds.Hand.Play();
            }
        }

        private void SetupControls(TradeItem tItemArg)
        {
            if (tItem.TradeType == TradeItem.TradeTypes.BUY)
            {
                txt_Item.Foreground = System.Windows.Media.Brushes.GreenYellow;
                btn_InviteCustomer.Visibility = Visibility.Collapsed;
                btn_StartTrade.Visibility = Visibility.Collapsed;
                btn_SearchItem.Visibility = Visibility.Collapsed;
                btn_SendWhisperAgain.Visibility = Visibility.Collapsed;
            }

            else if (tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                txt_Item.Foreground = System.Windows.Media.Brushes.OrangeRed;
                btn_VisitCustomerHideout.Visibility = Visibility.Collapsed;
                btn_VisitOwnHideout.Visibility = Visibility.Collapsed;

            }

            txt_Customer.Text = tItem.Customer;
            txt_Item.Text = tItem.Item;

            try
            {
                txt_Price.Text = TradeItem.ExtractFloatFromString(tItem.Price);
            }
            catch (Exception)
            { }

            if (tItem.Price == null)
            {
                txt_Price.Text = "--";
            }

            txt_League.Text = tItem.League;

            if (tItem.Stash == null)
            {
                btn_stash.Visibility = Visibility.Collapsed;
                btn_stash.Visibility = Visibility.Hidden;
            }
            else
            {
                txt_Stash.Text = tItem.Stash;
                btn_stash.ToolTip = tItem.StashPosition.ToString();
            }



            if (tItem.AdditionalText == "" || tItem.AdditionalText == null)
            {
                btn_AdditionalText.Visibility = Visibility.Collapsed;
                btn_AdditionalText.Visibility = Visibility.Hidden;
            }
            else
            {
                txt_AdditionalText.Text = tItem.AdditionalText;
            }

            img_Currency.Source = tItem.PriceCurrencyBitmap;

        }

        private void StartAnimatioin()
        {
            DoubleAnimation dAnim = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180))
            };

            this.BeginAnimation(Window.OpacityProperty, dAnim);
        }

        private void SendInputToPoe(string input)
        {
            // Get a handle to POE. The window class and window name were obtained using the Spy++ tool.
            IntPtr poeHandle = FindWindow("POEWindowClass", "Path of Exile");

            // Verify that POE is a running process.
            if (poeHandle == IntPtr.Zero)
            {
                // Show message box if POE is not running
                MessageBox.Show("Path of Exile is not running.");
                return;
            }

            InputSimulator iSim = new InputSimulator();

            // Need to press ALT because the SetForegroundWindow sometimes does not work
            iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            // Send the input
            iSim.Keyboard.TextEntry(input);

            // Send RETURN
            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            iSim = null;

        }

        private void ClickToCollapseExpandItem(object sender, RoutedEventArgs e)
        {
            CollapseExpandItem();
        }

        private void CollapseExpandItem()
        {
            if (this.Height != dblHeightToCollapse)
            {
                dblHeightBeforeCollapse = this.Height;
                this.Height = dblHeightToCollapse;
                btn_CollExp.Text = "⏷";

            }
            else
            {
                this.Height = dblHeightBeforeCollapse;
                btn_CollExp.Text = "⏶";
            }
        }

        private void ClickWhisperCustomer(object sender, RoutedEventArgs e)
        {
            // Get a handle to POE. The window class and window name were obtained using the Spy++ tool.
            IntPtr poeHandle = FindWindow("POEWindowClass", "Path of Exile");

            // Verify that POE is a running process.
            if (poeHandle == IntPtr.Zero)
            {
                MessageBox.Show("Path of Exile is not running.");
                return;
            }
            InputSimulator iSim = new InputSimulator();

            // Need to press ALT because the SetForegroundWindow sometimes does not work
            iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            iSim.Keyboard.TextEntry("@" + tItem.Customer + " ");

            iSim = null;
        }

        private void ClickInviteCustomer(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/invite " + tItem.Customer);
            if (tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                stashGridHighlight.AddButton(tItem);
            }
        }

        private void ClickTradeInvite(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/tradewith " + tItem.Customer);
        }

        private void ClickSearchItem(object sender, RoutedEventArgs e)
        {
            // Get a handle to POE. The window class and window name were obtained using the Spy++ tool.
            IntPtr poeHandle = FindWindow("POEWindowClass", "Path of Exile");

            // Verify that POE is a running process.
            if (poeHandle == IntPtr.Zero)
            {
                MessageBox.Show("Path of Exile is not running.");
                return;
            }

            InputSimulator iSim = new InputSimulator();

            // Need to press ALT because the SetForegroundWindow sometimes does not work
            iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            iSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_F);

            iSim.Keyboard.Sleep(500);

            iSim.Keyboard.TextEntry(tItem.Item);

            iSim = null;
        }

        private void ClickSendWhisperAgain(object sender, RoutedEventArgs e)
        {
            //SendInputToPoe("@" + tItem.Customer + " Hi, I would like to buy your " + tItem.Item + " listed for " + tItem.Price + " in " + tItem.League + " (stash tab \"" + tItem.Stash + "\"; position: left " + tItem.StashPosition.X + ", top " + tItem.StashPosition.Y + ")");
        }

        private void ClickKickCustomer(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/kick " + tItem.Customer);
        }

        private void ClickRemoveItem(object sender, RoutedEventArgs e)
        {
            ((StackPanel)Parent).Children.Remove(this);
            stashGridHighlight.RemoveStashControl(tItem);
            stashGridHighlight.ClearCanvas();
        }

        private void ClickThanksForTrade(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " "  + Settings.Default.ThankYouWhisper);
        }

        private void ClickKickMyself(object sender, RoutedEventArgs e)
        {
            //TODO: Implementieren
            SendInputToPoe("/kick " + Settings.Default.PlayerName);
        }

        private void ClickWhoisCustomer(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/whois " + tItem.Customer);
        }

        private void ClickToWikiLeague(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pathofexile.gamepedia.com/" + tItem.League);
        }

        private void ClickVisitHideout(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/hideout " + tItem.Customer);
        }

        private void ClickVisitOwnHideout(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/hideout");
        }

        private void ClickStashIsQuad(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Is this a quad stash?", "Quad stash?", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Settings.Default.QuadStash.Add(tItem.Stash.ToString());
                Settings.Default.Save();
            }
        }

        private void ClickWhisperCustomerBusy(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " " + Settings.Default.ImBusyWhisper);
        }

        private void ClickShowStashOverlay(object sender, RoutedEventArgs e)
        {
            if (tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                stashGridHighlight.AddButton(tItem);
            }
        }

        private void ClickCustomWhisper1(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " " + Settings.Default.CustomWhisper1);
        }

        private void ClickCustomWhisper2(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " " + Settings.Default.CustomWhisper2);
        }
    }


}
