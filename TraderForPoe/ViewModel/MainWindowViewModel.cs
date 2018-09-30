using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            LogFileCheck.CheckForClientTxt();
        }
    }
}
