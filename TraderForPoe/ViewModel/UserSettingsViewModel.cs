using System.Collections.ObjectModel;
using System.Windows;
using TraderForPoe.Properties;

namespace TraderForPoe.ViewModel
{
    public class UserSettingsViewModel : ViewModelBase
    {
        #region Constructors

        public UserSettingsViewModel()
        {
            CmdRestart = new RelayCommand(() => RestartApp());
            CmdDeleteQuadStash = new RelayCommand(() => QuadStashList.Remove(SelectedQuadStashListItem));
            CmdAddToQuadStashList = new RelayCommand(() => AddToQuadStashList());
        }

        #endregion Constructors

        #region Properties

        public bool CheckForUpdatesOnStart
        {
            get { return Settings.Default.CheckForUpdatesOnStart; }
            set
            {
                if (Settings.Default.CheckForUpdatesOnStart != value)
                {
                    Settings.Default.CheckForUpdatesOnStart = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterTrade
        {
            get { return Settings.Default.CloseItemAfterTrade; }
            set
            {
                if (Settings.Default.CloseItemAfterTrade != value)
                {
                    Settings.Default.CloseItemAfterTrade = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public RelayCommand CmdAddToQuadStashList { get; private set; }

        public RelayCommand CmdDeleteQuadStash { get; private set; }

        public RelayCommand CmdRestart { get; private set; }

        public bool CollapsedItems
        {
            get { return Settings.Default.CollapsedItems; }
            set
            {
                if (Settings.Default.CollapsedItems != value)
                {
                    Settings.Default.CollapsedItems = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public float ControlOpacity
        {
            get { return Settings.Default.ControlOpacity; }
            set { Settings.Default.ControlOpacity = value; OnPropertyChanged(); }
        }

        public bool HideIfPoeNotForeGround
        {
            get { return Settings.Default.HideIfPoeNotForeGround; }
            set
            {
                if (Settings.Default.HideIfPoeNotForeGround != value)
                {
                    Settings.Default.HideIfPoeNotForeGround = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public string Path
        {
            get { return Settings.Default.PathToClientTxt; }
            set
            {
                if (Settings.Default.PathToClientTxt != value)
                {
                    Settings.Default.PathToClientTxt = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }



        public string ThankYouWhisper
        {
            get { return Settings.Default.ThankYouWhisper; }
            set
            {
                if (Settings.Default.ThankYouWhisper != value)
                {
                    Settings.Default.ThankYouWhisper = value;
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
                if (Settings.Default.PlayerName != value)
                {
                    Settings.Default.PlayerName = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool PlayNotificationSound
        {
            get { return Settings.Default.PlayNotificationSound; }
            set
            {
                if (Settings.Default.PlayNotificationSound != value)
                {
                    Settings.Default.PlayNotificationSound = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> QuadStashList
        {
            get { return Settings.Default.QuadStash; }
            set { Settings.Default.QuadStash = value; OnPropertyChanged(); }
        }

        public string QuadStashText { get; set; }

        public string SelectedQuadStashListItem { get; set; }

        #endregion Properties

        #region Methods

        private void AddToQuadStashList()
        {
            if (!string.IsNullOrEmpty(QuadStashText) && !string.IsNullOrWhiteSpace(QuadStashText) && !QuadStashList.Contains(QuadStashText))
            {
                QuadStashList.Add(QuadStashText);
            }
        }

        private void RestartApp()
        {
            //TODO is this mvvm?
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion Methods
    }
}