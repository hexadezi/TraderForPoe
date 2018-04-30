﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WindowsInput;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using TraderForPoe.Windows;
using TraderForPoe.Properties;
using System.Media;
using System.ComponentModel;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Media;

namespace TraderForPoe
{
    /// <summary>
    /// Interaktionslogik für TradeItemPopup.xaml
    /// </summary>
    public partial class TradeItemControl : UserControl
    {

        private static int intNumberItems = 0;

        public static event EventHandler MoreThanThreeItems;

        public static event EventHandler EqualThreeItems;


        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private static StashGridHighlight stashGridHighlight;

        public TradeItem tItem;

        Stopwatch stopwatch = new Stopwatch();

        //-----------------------------------------------------------------------------------------

        public TradeItemControl(TradeItem tItemArg)
        {
            tItem = tItemArg;

            intNumberItems++;

            if (intNumberItems > 3)
            {
                OnMoreThanThreeItems();
            }

            InitializeComponent();


            LoadSettings();

            SetupControls(tItem);

            StartAnimatioin();

            OpenStashGridHighlightWindow();

            StartTime();
        }


        //-----------------------------------------------------------------------------------------

        private void StartTime()
        {

            DispatcherTimer dispatcherTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            dispatcherTimer.Tick += Timer_Tick;

            stopwatch.Start();
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            int tsMinutes = ts.Minutes + ( ts.Hours * 60 );
            string currentTime = String.Format("{0:00}:{1:00}",
                tsMinutes, ts.Seconds);
            txt_Time.Text = currentTime;
        }

        private void OpenStashGridHighlightWindow()
        {
            if (stashGridHighlight == null)
            {
                stashGridHighlight = new StashGridHighlight();
            }

            stashGridHighlight.Show();
        }

        private void LoadSettings()
        {
            if (Settings.Default.CollapsedItems == true)
            {
                CollapseExpandItem();
            }

            if (Settings.Default.PlayNotificationSound)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.notification);
                player.Play();
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
                btn_AskIfInterested.Visibility = Visibility.Collapsed;
                btn_SendBusyMessage.Visibility = Visibility.Collapsed;
            }

            else if (tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                txt_Item.Foreground = System.Windows.Media.Brushes.OrangeRed;
                btn_VisitCustomerHideout.Visibility = Visibility.Collapsed;
                btn_VisitOwnHideout.Visibility = Visibility.Collapsed;
                btn_SendWhisperAgain.Visibility = Visibility.Collapsed;

            }





            if (tItem.ItemIsCurrency == true)
            {

                txt_Customer.Text = tItem.Customer;
                txt_Item.Text = tItem.ItemCurrencyQuant;
                img_ItemCurrency.Source = tItem.ItemCurrencyBitmap;
                txt_League.Text = tItem.League;

                try
                {
                    txt_Price.Text = TradeItem.ExtractFloatFromString(tItem.Price);
                }
                catch (Exception)
                { }


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

                double dblPrice = 0;
                double dblItemPrice = 0;
                try
                {
                    dblPrice = Double.Parse(TradeItem.ExtractFloatFromString(tItem.Price));
                    dblItemPrice = Double.Parse(TradeItem.ExtractFloatFromString(tItem.ItemCurrencyQuant));
                }
                catch (Exception) { }



                if ((dblItemPrice / dblPrice) >= 1)
                {
                    txt_Ratio1.Text = "1";
                    img_Ratio1.Source = tItem.PriceCurrencyBitmap;

                    txt_Ratio2.Text = (dblItemPrice / dblPrice).ToString();
                    img_Ratio2.Source = tItem.ItemCurrencyBitmap;
                }
                else
                {
                    txt_Ratio1.Text = "1";
                    img_Ratio1.Source = tItem.ItemCurrencyBitmap;

                    txt_Ratio2.Text = (dblPrice / dblItemPrice).ToString();
                    img_Ratio2.Source = tItem.PriceCurrencyBitmap;
                }

                btn_stash.Visibility = Visibility.Collapsed;
                btn_stash.Visibility = Visibility.Hidden;
            }





            else
            {
                txt_Customer.Text = tItem.Customer;
                txt_Item.Text = tItem.Item;
                spnl_CurrencyRatio.Visibility = Visibility.Collapsed;
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
            if (this.grd_SecondRow.Visibility == Visibility.Visible)
            {
                this.grd_SecondRow.Visibility = Visibility.Hidden;
                this.grd_SecondRow.Visibility = Visibility.Collapsed;
                btn_CollExp.Text = "⏷";
            }
            else
            {
                this.grd_SecondRow.Visibility = Visibility.Visible;
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
            if (tItem.TradeType == TradeItem.TradeTypes.SELL && tItem.Stash != "")
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
            string whisper = tItem.WhisperMessage;
            whisper = whisper.Remove(0, whisper.IndexOf(": ") + 2);
            SendInputToPoe("@" + tItem.Customer + " " + whisper);
        }

        private void ClickKickCustomer(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("/kick " + tItem.Customer);
        }

        private void ClickRemoveItem(object sender, RoutedEventArgs e)
        {
            RemoveItem();
        }

        private void RemoveItem()
        {
            ((StackPanel)Parent).Children.Remove(this);
            stashGridHighlight.RemoveStashControl(tItem);
            TradeItemControl.RemoveTICfromList(this);
            TradeItem.RemoveItemFromList(tItem);
            stashGridHighlight.ClearCanvas();
        }

        private void ClickThanksForTrade(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " " + Settings.Default.ThankYouWhisper);
        }

        private void ClickKickMyself(object sender, RoutedEventArgs e)
        {
            //TODO: Implementieren
            SendInputToPoe("/kick " + Settings.Default.PlayerName);

            if (tItem.TradeType == TradeItem.TradeTypes.SELL && Settings.Default.CloseItemAfterTrade == true)
            {
                RemoveItem();
            }
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
            if (tItem.TradeType == TradeItem.TradeTypes.BUY && Settings.Default.CloseItemAfterTrade == true)
            {
                RemoveItem();
            }
        }

        private void ClickStashIsQuad(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Is this a quad stash?", "Quad stash?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (!Settings.Default.QuadStash.Contains(tItem.Stash.ToString()))
                {
                    Settings.Default.QuadStash.Add(tItem.Stash.ToString());
                    Settings.Default.Save();
                }

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

        private void ClickAskIfInterested(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " Hi, are you still interested in " + tItem.Item + " for " + tItem.Price + "?");
        }

        public static void RemoveTICfromList(TradeItemControl tradeItemControl)
        {
            intNumberItems--;

            if (intNumberItems == 3)
            {
                OnEqualThreeItems();
            }
        }

        protected static void OnMoreThanThreeItems()
        {
            MoreThanThreeItems?.Invoke(typeof(TradeItemControl), EventArgs.Empty);
        }

        protected static void OnEqualThreeItems()
        {
            EqualThreeItems?.Invoke(typeof(TradeItemControl), EventArgs.Empty);
        }

        public void CustomerJoined()
        {
            txt_Customer.Foreground = Brushes.GreenYellow;
        }
        public void CustomerLeft()
        {
            txt_Customer.Foreground = Brushes.White;
        }
    }


}
