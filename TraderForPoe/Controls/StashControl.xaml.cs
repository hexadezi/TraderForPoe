using System.Windows.Controls;

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
