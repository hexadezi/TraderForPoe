using System.Windows;
using TraderForPoe.ViewModel;

namespace TraderForPoe.Windows
{
    /// <summary>
    /// Interaktionslogik für LogReader.xaml
    /// </summary>
    public partial class LogMonitor : Window
    {
        public LogMonitor()
        {
            InitializeComponent();
            DataContext = new LogMonitorViewModel();
        }
    }
}
