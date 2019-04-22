using System.ComponentModel;
using System.Windows;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class UserSettingsViewModel : ViewModelBase
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
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
                    this.OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        private void RestartApp()
        {
            //TODO is this mvvm?
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion Methods
    }
}