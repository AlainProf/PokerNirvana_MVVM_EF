using PokerNirvana_MVVM_EF.Model;
using PokerNirvana_MVVM_EF.ViewModel;
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

namespace PokerNirvana_MVVM_EF.View
{
    /// <summary>
    /// Logique d'interaction pour CreerTournoisParties.xaml
    /// </summary>
    public partial class CreerTournoisParties : UserControl
    {
        private PokerNirvana_MVVM_EF.ViewModel.CreerTournoisPartiesViewModel CTP_vm; 
        public CreerTournoisParties()
        {
            InitializeComponent();
            TG.Contexte = "RECHARGE_PARTIE_EN_COURS";
            CTP_vm = new CreerTournoisPartiesViewModel();
            DataContext = CTP_vm;
        }

        private void CreationPartie(object sender, RoutedEventArgs e)
        {
            CTP_vm.CreationPartie();
        }
    }
}
