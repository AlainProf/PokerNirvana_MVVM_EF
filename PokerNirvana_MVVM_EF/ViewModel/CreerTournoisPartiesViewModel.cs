using PokerNirvana_MVVM_EF.Model;
using PokerNirvana_MVVM_EF.ViewModel.Service;
using PokerNirvana_MVVM_EF.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Data.Entity;

namespace PokerNirvana_MVVM_EF.ViewModel
{
   class CreerTournoisPartiesViewModel : INotifyPropertyChanged
   {
      
       public ObservableCollection<Joueur> ListeTousJoueurs { get; set; }
      
        // Constructeur
       public CreerTournoisPartiesViewModel()
       {
            // Le databse setinitializer est mtn dans trousse globale
            //Database.SetInitializer<NirvanaContext>(new DropCreateDatabaseAlways<NirvanaContext>());
            ///// LINQ pour aller récupérer tous les joueurs du club
            var nirvCtxt = new NirvanaContext();
            var jReq = from r in (nirvCtxt.Joueurs) select r;
            /////////////////////////////////////////////////

            ListeTousJoueurs = new ObservableCollection<Joueur>();
            foreach (Joueur j in jReq)
            {
                j.ImagePokerman = new BitmapImage(new Uri(TG.PathImage + j.Nom + ".jpg"));
                ListeTousJoueurs.Add(j);
            }
       }

       private void TransformeInviteEnJoueur(List<string> lstInvites)
       {
          //MessageBox.Show("Liste de invités:");
          //ListeJoueurs = new ObservableCollection<Joueur>();

          //for (int i=0; i < lstInvites.Count; i++)
          //{
          //   ListeJoueurs.Add(lstInvites.FindIndex(i));
          //}
       }

       public void CreationPartie()
       {
          List<Joueur> lstInvites = new List<Joueur>();
          for (int i=0; i < ListeTousJoueurs.Count; i++)
          {
             if (ListeTousJoueurs[i].Inviter)
             {
                lstInvites.Add(ListeTousJoueurs[i]);
             }
          }
          TG.PA.NomJoueurLogue = "Certs";
          TG.PA.JoueurLogue = 0;

          TG.PA.NouvellePartie(lstInvites);

          TG.NirvContext.Parties.Add(TG.PA);
          TG.NirvContext.SaveChanges();
            // bizarre mais après le SaveChanges() la main a perdu ses etapes
            // LINQ pour recharger la Partie 
         TG.PA = (from p in (TG.NirvContext.Parties.Include("Mains.Etapes.ToursParole").Include("Joueurs")) where p.NumPartie == 1 select p).FirstOrDefault();
            
         TG.PA.ProchainJoueur = TG.PA.MainCourante.EtapeCourante.ProchainJoueur;

          TG.NirvContext.Historiques.Add(new Historique("Début de la partie " + TG.PA.NumPartie, TG.PA.NumPartie));
          TG.NirvContext.Historiques.Add(new Historique(TG.PA.Joueurs[0].NomJoueur + " distribue la main " + TG.PA.NumMainCourante, TG.PA.NumPartie));
          TG.NirvContext.Historiques.Add(new Historique("Gros blind " + TG.PA.NiveauPourSuivre, TG.PA.NumPartie));
          TG.NirvContext.SaveChanges();//   int NumPartie = 1; // TG.SRV.Incarne<iParties_EF_SRV>().InsereNouvellePartie(ListeJoueurs);
                                         //PA partieActive =  TG.SRV.Incarne<iParties_EF_SRV>().RecupUnePA(NumPartie);

          TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
      }   


      
     
      public event PropertyChangedEventHandler PropertyChanged;

      private void OnPropertyChanged(string propertyName)
      {
         if (PropertyChanged != null)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
      }
   }
}
