using Microsoft.Win32;
using System.Windows;
using TraderForPoe.Properties;
using TraderForPoe.ViewModel;

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
            DataContext = new UserSettingsViewModel();
        }
        
        //TODO Implement mvvm 
        private void Click_SearchFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                txt_PathToClientTxt.Text = openFileDialog.FileName;
                txt_PathToClientTxt.Focus();
                txt_PathToClientTxt.CaretIndex = txt_PathToClientTxt.Text.Length;
            }
        }

    }
}