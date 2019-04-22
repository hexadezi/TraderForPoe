using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TraderForPoe.Classes;
using TraderForPoe.ViewModel.Base;

namespace TraderForPoe.ViewModel
{
    public class TradeHistoryViewModel : ViewModelBase
    {
        public ObservableCollection<TradeObject> TradeObjectsList { get; set; } = TradeObject.TradeObjectList;

        public TradeHistoryViewModel()
        {
            CmdTestObject = new RelayCommand(() => Add());
            CmdClear = new RelayCommand(() => TradeObjectsList.Clear());
        }

        private void Add()
        {
            new TradeObject("@To Labooooooo: Hi, I would like to buy your Cybil's Paw Thresher Claw listed for 1 jewellers in Bestiary (stash tab \"~b / o 0 alt\"; position: left 23, top 8)");
        }

        public RelayCommand CmdTestObject { get; private set; }
        public RelayCommand CmdClear { get; private set; }
    }

    public class UriToBitmapConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            return new BitmapImage(new Uri(value as string)); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}