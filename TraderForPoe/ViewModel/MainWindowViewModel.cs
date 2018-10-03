using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraderForPoe.Classes;
using TraderForPoe.Controls;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            LogFileCheck.CheckForClientTxt();
        }
        
        public static ObservableCollection<CustomTestCtrl> TradeControlList { get; set; } = CustomTestCtrl.TradeControlList;

    }
}
