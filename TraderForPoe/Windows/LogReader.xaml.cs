using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TraderForPoe.ViewModel;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für LogReader.xaml
    /// </summary>
    public partial class LogReader : Window
    {
        public LogReader(Dispatcher arg)
        {
            InitializeComponent();
            LogReaderViewModel vm = new LogReaderViewModel(arg);
            this.lbLines.ItemsSource = vm.Lines;
        }
    }
}
