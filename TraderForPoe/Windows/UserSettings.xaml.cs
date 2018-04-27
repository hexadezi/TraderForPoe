using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using TraderForPoe.Properties;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Window
    {

        public UserSettings()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IEditableCollectionView items = lsb_QuadStash.Items;
            items.RemoveAt(lsb_QuadStash.SelectedIndex);
            lsb_QuadStash.Items.Refresh();
        }
        
        private void Click_SaveSettings(object sender, RoutedEventArgs e)
        {
            Settings.Default.PlayerName = txt_PlayerName.Text;
            Settings.Default.CollapsedItems = cbx_CollapseItems.IsChecked.Value;
            Settings.Default.QuadStash = (StringCollection)lsb_QuadStash.Items.SourceCollection;
            Settings.Default.PlayNotificationSound = cbx_PlaySound.IsChecked.Value;
            Settings.Default.PathToClientTxt = txt_PathToClientTxt.Text;
            Settings.Default.CustomWhisper1 = txt_CustomWhisper1.Text;
            Settings.Default.CustomWhisper2 = txt_CustomWhisper2.Text;
            Settings.Default.ThankYouWhisper = txt_ThankYouWhisper.Text;
            Settings.Default.ImBusyWhisper = txt_ImBusyWhisper.Text;
            Settings.Default.UseClipboardMonitor = cbx_UseClipBoardMonitor.IsChecked.Value;
            Settings.Default.CloseItemAfterTrade = cbx_RemoveAfterTrade.IsChecked.Value;

            Settings.Default.Save();
            Settings.Default.Reload();

            System.Windows.Forms.MessageBox.Show("Settings saved successfully.", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

        }

        private void Click_RestartApp(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
