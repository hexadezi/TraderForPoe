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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TraderForPoe.Controls
{
    /// <summary>
    /// Interaktionslogik für StashControl.xaml
    /// </summary>
    public partial class StashControl : UserControl
    {
        public StashControl(TradeItem tItemArgs)
        {
            InitializeComponent();
            GetTItem = tItemArgs;
            txt_StashName.Text = GetTItem.Stash;
        }

        public TradeItem GetTItem { get; set; }

    }



}
