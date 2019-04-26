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
            CmdQuit = new RelayCommand(() => Application.Current.Shutdown());
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

        public bool CloseItemAfterCustomWhisper1
        {
            get { return Settings.Default.CloseItemAfterCustomWhisper1; }
            set
            {
                if (Settings.Default.CloseItemAfterCustomWhisper1 != value)
                {
                    Settings.Default.CloseItemAfterCustomWhisper1 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterCustomWhisper2
        {
            get { return Settings.Default.CloseItemAfterCustomWhisper2; }
            set
            {
                if (Settings.Default.CloseItemAfterCustomWhisper2 != value)
                {
                    Settings.Default.CloseItemAfterCustomWhisper2 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterCustomWhisper3
        {
            get { return Settings.Default.CloseItemAfterCustomWhisper3; }
            set
            {
                if (Settings.Default.CloseItemAfterCustomWhisper3 != value)
                {
                    Settings.Default.CloseItemAfterCustomWhisper3 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterCustomWhisper4
        {
            get { return Settings.Default.CloseItemAfterCustomWhisper4; }
            set
            {
                if (Settings.Default.CloseItemAfterCustomWhisper4 != value)
                {
                    Settings.Default.CloseItemAfterCustomWhisper4 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterImBusyWhisper
        {
            get { return Settings.Default.CloseItemAfterImBusyWhisper; }
            set
            {
                if (Settings.Default.CloseItemAfterImBusyWhisper != value)
                {
                    Settings.Default.CloseItemAfterImBusyWhisper = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public bool CloseItemAfterThankYouWhisper
        {
            get { return Settings.Default.CloseItemAfterThankYouWhisper; }
            set
            {
                if (Settings.Default.CloseItemAfterThankYouWhisper != value)
                {
                    Settings.Default.CloseItemAfterThankYouWhisper = value;
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

        public RelayCommand CmdQuit { get; private set; }

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

        public string CustomWhisper1
        {
            get { return Settings.Default.CustomWhisper1; }
            set
            {
                if (Settings.Default.CustomWhisper1 != value)
                {
                    Settings.Default.CustomWhisper1 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public string CustomWhisper2
        {
            get { return Settings.Default.CustomWhisper2; }
            set
            {
                if (Settings.Default.CustomWhisper2 != value)
                {
                    Settings.Default.CustomWhisper2 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public string CustomWhisper3
        {
            get { return Settings.Default.CustomWhisper3; }
            set
            {
                if (Settings.Default.CustomWhisper3 != value)
                {
                    Settings.Default.CustomWhisper3 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
        }

        public string CustomWhisper4
        {
            get { return Settings.Default.CustomWhisper4; }
            set
            {
                if (Settings.Default.CustomWhisper4 != value)
                {
                    Settings.Default.CustomWhisper4 = value;
                    Settings.Default.Save();
                    this.OnPropertyChanged();
                }
            }
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

        public string ImBusyWhisper
        {
            get { return Settings.Default.ImBusyWhisper; }
            set
            {
                if (Settings.Default.ImBusyWhisper != value)
                {
                    Settings.Default.ImBusyWhisper = value;
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