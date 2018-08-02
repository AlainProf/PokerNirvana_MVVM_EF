using PokerNirvana_MVVM_EF.Model;
using PokerNirvana_MVVM_EF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PokerNirvana_MVVM_EF.View
{
    public partial class TexasTable : UserControl
    {
        //-------------------------------------------
        //	  
        //-------------------------------------------
        public TexasTable()
        {
            InitializeComponent();
            TG.Contexte = "RECHARGE_PARTIE_EN_COURS";
             DataContext = new TexasTableViewModel(this);
        }

    }
}