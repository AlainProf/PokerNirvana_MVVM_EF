using PokerNirvana_MVVM_EF.View;
using PokerNirvana_MVVM_EF.ViewModel.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PokerNirvana_MVVM_EF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class Principale : Window, IApplicationService
    {

        public Principale()
        {
            InitializeComponent();
            Configurer();
            TG objetTG = new TG();
            presenteurContenu.Content = new MenuPrincipal();
        }

        public void Configurer()
        {
            TG.SRV.Inscrit<IApplicationService, Principale>(this);
        }

        public void ChangerVue<T>(T vue)
        {
            presenteurContenu.Content = vue as UserControl;
        }



        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.MainWindow.ResizeMode = ResizeMode.CanMinimize;
            ////App.Current.MainWindow.Width = App.APP_WIDTH;
            ////App.Current.MainWindow.Height = App.APP_HEIGHT;
            //App.Current.MainWindow.WindowState = WindowState.Normal;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new MenuPrincipal());
        }


        private void btnCerts_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "certs";
            TG.PA.JoueurLogue = 0;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }

        private void btnCheen_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "cheen";
            TG.PA.JoueurLogue = 1;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }
        private void btnGos_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "gos";
            TG.PA.JoueurLogue = 2;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }
        private void btnK_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "k";
            TG.PA.JoueurLogue = 3;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }
        private void btnPough_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "pough";
            TG.PA.JoueurLogue = 4;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }
        private void btnSpeed_Click(object sender, RoutedEventArgs e)
        {
            TG.PA.NomJoueurLogue = "speed";
            TG.PA.JoueurLogue = 5;
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }

        /// <summary>
        /// Ouvre la fenêtre des infos sur l'application en modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAPropos_Click(object sender, RoutedEventArgs e)
        {
            //(new FenetreAPropos()).ShowDialog();
            //ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new FenetreAPropos());
        }

        /// <summary>
        /// Lorsque l'application de ferme, il faut fermer le Thread de notif.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //ThreadVerifChangement.Abort();
        }
    }
}
