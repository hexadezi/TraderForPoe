using System.ComponentModel;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class UserSettingsViewModel : INotifyPropertyChanged
    {
        private static Settings userSettings = Properties.Settings.Default;


        public UserSettingsViewModel()
        {
        }

        private static string path = userSettings.PathToClientTxt;
        public string Path
        {
            get { return Settings.Default.PathToClientTxt; }
            set
            {
                if (path != value)
                {
                    path = value;
                    Settings.Default.PathToClientTxt = value;
                    this.NotifyPropertyChanged("Path");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}