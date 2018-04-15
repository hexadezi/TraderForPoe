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

namespace TraderForPoe
{
    /// <summary>
    /// Interaktionslogik für TradeItemPopup.xaml
    /// </summary>
    public partial class TradeItemControl : UserControl
    {
        InputSimulator iSim = new InputSimulator();

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

            if (Settings.Default.CollapsedItems == true)
            {
                CollapseExpandItem();
            }

            tItem = tItemArg;

            FillControl(tItemArg);

            StartAnimatioin();

            if (Settings.Default.PlayNotificationSound)
            {
                System.Media.SystemSounds.Hand.Play();
            }

            if (stashGridHighlight == null)
            {
                stashGridHighlight = new StashGridHighlight();
                stashGridHighlight.Show();
            }

        }

        //-----------------------------------------------------------------------------------------

        private void FillControl(TradeItem tItemArg)
        {

            if (tItem.TradeType == TradeItem.TradeTypes.BUY)
            {
                txt_Item.Foreground = System.Windows.Media.Brushes.GreenYellow;
            }
            else if (tItem.TradeType == TradeItem.TradeTypes.SELL)
            {
                txt_Item.Foreground = System.Windows.Media.Brushes.OrangeRed;
            }

            txt_Customer.Text = tItem.Customer;
            txt_Item.Text = tItem.Item;
            txt_Price.Text = new String(tItem.Price.Where(Char.IsDigit).ToArray()); ;
            txt_League.Text = tItem.League;
            txt_Stash.Text = tItem.Stash;
            btn_stash.ToolTip = tItem.StashPosition.ToString();

            SetCurrencyImage();
        }

        private void SetCurrencyImage()
        {
            string strPrice = tItem.Price.ToLower();

            if (strPrice.Contains("chaos") && !strPrice.Contains("shard"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chaos.png"));
            }

            else if (strPrice.Contains("alch") && !strPrice.Contains("shard"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_alch.png"));
            }

            else if (strPrice.Contains("alt"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_alt.png"));
            }

            else if (strPrice.Contains("ancient"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_ancient.png"));
            }

            else if (strPrice.Contains("annulment") && !strPrice.Contains("shard"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_annul.png"));
            }

            else if (strPrice.Contains("apprentice") && strPrice.Contains("sextant"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_appr_carto_sextant.png"));
            }

            else if (strPrice.Contains("armour") || strPrice.Contains("scrap"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_armour_scrap.png"));
            }

            else if (strPrice.Contains("aug"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_aug.png"));
            }

            else if (strPrice.Contains("bauble"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bauble.png"));
            }

            else if (strPrice.Contains("bestiary") && strPrice.Contains("orb"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bestiary_orb.png"));
            }

            else if (strPrice.Contains("binding") && strPrice.Contains("orb"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_binding.png"));
            }

            else if (strPrice.Contains("whetstone") || strPrice.Contains("blacksmith"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_black_whetstone.png"));
            }

            else if (strPrice.Contains("blessing") && strPrice.Contains("chayulah"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_chayula.png"));
            }

            else if (strPrice.Contains("blessing") && strPrice.Contains("esh"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_chayula.png"));
            }

            else if (strPrice.Contains("blessing") && strPrice.Contains("tul"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_tul.png"));
            }

            else if (strPrice.Contains("blessing") && strPrice.Contains("uul"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_uul.png"));
            }

            else if (strPrice.Contains("blessing") && strPrice.Contains("xoph"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_bless_xoph.png"));
            }

            else if (strPrice.Contains("blesse"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_blessed.png"));
            }

            else if (strPrice.Contains("chance"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chance.png"));
            }

            else if (strPrice.Contains("chisel"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chisel.png"));
            }

            else if (strPrice.Contains("chrom") || strPrice.Contains("chrome"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_chrom.png"));
            }

            else if (strPrice.Contains("divine") || strPrice.Contains("div"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_divine.png"));
            }

            else if (strPrice.Contains("engineer") && strPrice.Contains("orb"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_engineer.png"));
            }

            else if (strPrice.Contains("ete"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_eternal.png"));
            }

            else if (strPrice.Contains("ex") || strPrice.Contains("exa") || strPrice.Contains("exalted") && !strPrice.Contains("shard"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_ex.png"));
            }

            else if (strPrice.Contains("fuse") || strPrice.Contains("fus"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_fuse.png"));
            }

            else if (strPrice.Contains("gcp") || strPrice.Contains("gemc"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_gcp.png"));
            }

            else if (strPrice.Contains("harbinger") && strPrice.Contains("orb"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_harbinger.png"));
            }

            else if (strPrice.Contains("horizon") && strPrice.Contains("orb"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_horizon.png"));
            }

            else if (strPrice.Contains("imprinted") && strPrice.Contains("bestiary"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_impr_bestiary.png"));
            }

            else if (strPrice.Contains("jew"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_jew.png"));
            }

            else if (strPrice.Contains("journeyman") && strPrice.Contains("sextant"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_journ_carto_sextant.png"));
            }

            else if (strPrice.Contains("master") && strPrice.Contains("sextant"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_master_carto_sextant.png"));
            }

            else if (strPrice.Contains("mir") || strPrice.Contains("kal"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_mirror.png"));
            }

            else if (strPrice.Contains("port"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_port.png"));
            }

            else if (strPrice.Contains("rega"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_regal.png"));
            }

            else if (strPrice.Contains("regr"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_regret.png"));
            }

            else if (strPrice.Contains("scour"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_scour.png"));
            }

            else if (strPrice.Contains("silver"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_silver.png"));
            }

            else if (strPrice.Contains("splinter") && strPrice.Contains("chayula"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_chayula.png"));
            }

            else if (strPrice.Contains("splinter") && strPrice.Contains("esh"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_esh.png"));
            }

            else if (strPrice.Contains("splinter") && strPrice.Contains("tul"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_tul.png"));
            }

            else if (strPrice.Contains("splinter") && strPrice.Contains("uul"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_uul.png"));
            }

            else if (strPrice.Contains("splinter") && strPrice.Contains("xoph"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_splinter_xoph.png"));
            }

            else if (strPrice.Contains("tra"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_tra.png"));
            }

            else if (strPrice.Contains("vaal"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_vaal.png"));
            }

            else if (strPrice.Contains("wis"))
            {
                img_Currency.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Currency/curr_wis.png"));
            }
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

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            // Send LSHIFT and RETURN together
            iSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LSHIFT, VirtualKeyCode.RETURN);

            // Send the input
            iSim.Keyboard.TextEntry(input);

            // Send RETURN
            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

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

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            iSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LSHIFT, VirtualKeyCode.RETURN);

            iSim.Keyboard.TextEntry("@" + tItem.Customer + " ");
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

            // Make POE the foreground application and send input
            SetForegroundWindow(poeHandle);

            iSim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_F);

            iSim.Keyboard.Sleep(500);

            iSim.Keyboard.TextEntry(tItem.Item);
        }

        private void ClickSendWhisperAgain(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + " Hi, I would like to buy your " + tItem.Item + " listed for " + tItem.Price + " in " + tItem.League + " (stash tab \"" + tItem.Stash + "\"; position: left " + tItem.StashPosition.X + ", top " + tItem.StashPosition.Y + ")");
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
            SendInputToPoe("@" + tItem.Customer + " Thank you for the trade. Good luck and have fun in " + tItem.League + ".");
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
                Settings.Default.QuadStash.Add(tItem.Stash);
                // Saves settings in application configuration file
                Settings.Default.Save();
            }
        }

        private void ClickWhisperCustomerBusy(object sender, RoutedEventArgs e)
        {
            SendInputToPoe("@" + tItem.Customer + "Hi, I'm busy right now. Shall I write you after?");
        }
    }


}
