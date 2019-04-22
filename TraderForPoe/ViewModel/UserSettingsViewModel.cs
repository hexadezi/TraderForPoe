using System.ComponentModel;
using System.Windows;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class UserSettingsViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string path = Settings.Default.PathToClientTxt;
        private string playerName = Settings.Default.PlayerName;
        private bool playNotificationSound = Settings.Default.PlayNotificationSound;
        private bool closeItemAfterTrade = Settings.Default.CloseItemAfterTrade;
        private bool checkForUpdatesOnStart = Settings.Default.CheckForUpdatesOnStart;
        private bool hideIfPoeNotForeGround = Settings.Default.HideIfPoeNotForeGround;
        private bool collapsedItems = Settings.Default.CollapsedItems;

        #endregion Fields

        #region Constructors

        public UserSettingsViewModel()
        {
            CmdRestart = new RelayCommand(() => RestartApp());
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public RelayCommand CmdRestart { get; private set; }

        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    Settings.Default.PathToClientTxt = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(Path));
                }
            }
        }

        public string PlayerName
        {
            get { return Settings.Default.PlayerName; }
            set
            {
                if (playerName != value)
                {
                    playerName = value;
                    Settings.Default.PlayerName = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(Path));
                }
            }
        }

        public bool PlayNotificationSound
        {
            get { return playNotificationSound; }
            set
            {
                if (playNotificationSound != value)
                {
                    playNotificationSound = value;
                    Settings.Default.PlayNotificationSound = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(PlayNotificationSound));
                }
            }
        }

        public bool CloseItemAfterTrade
        {
            get { return closeItemAfterTrade; }
            set
            {
                if (closeItemAfterTrade != value)
                {
                    closeItemAfterTrade = value;
                    Settings.Default.CloseItemAfterTrade = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(CloseItemAfterTrade));
                }
            }
        }

        public bool CheckForUpdatesOnStart
        {
            get { return checkForUpdatesOnStart; }
            set
            {
                if (checkForUpdatesOnStart != value)
                {
                    checkForUpdatesOnStart = value;
                    Settings.Default.CheckForUpdatesOnStart = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(CheckForUpdatesOnStart));
                }
            }
        }

        public bool HideIfPoeNotForeGround
        {
            get { return hideIfPoeNotForeGround; }
            set
            {
                if (hideIfPoeNotForeGround != value)
                {
                    hideIfPoeNotForeGround = value;
                    Settings.Default.HideIfPoeNotForeGround = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(HideIfPoeNotForeGround));
                }
            }
        }

        public bool CollapsedItems
        {
            get { return collapsedItems; }
            set
            {
                if (collapsedItems != value)
                {
                    collapsedItems = value;
                    Settings.Default.CollapsedItems = value;
                    Settings.Default.Save();
                    this.NotifyPropertyChanged(nameof(CollapsedItems));
                }
            }
        }

        #endregion Properties

        #region Methods

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void RestartApp()
        {
            //TODO is this mvvm?
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }

        #endregion Methods
    }
}