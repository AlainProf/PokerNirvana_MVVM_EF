using PokerNirvana_MVVM_EF.Model;
using PokerNirvana_MVVM_EF.ViewModel.DataAccess;
using PokerNirvana_MVVM_EF.ViewModel.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Logique d'interaction pour MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : UserControl
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void CreationTournoisParties(object sender, RoutedEventArgs e)
        {
           TG.SRV.Incarne<IApplicationService>().ChangerVue(new CreerTournoisParties());
        }

     
        

        //Pour voir les foreign key associés à une table:
        /*
SELECT 
  TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME, REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME
FROM
  INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE
  REFERENCED_TABLE_SCHEMA = 'nirvanaef' AND
  REFERENCED_TABLE_NAME = 'mains';
*/

        private void TruncatePrimitif(object sender, RoutedEventArgs e)
        {
            BD_primitive maBD = new BD_primitive();
            string cmd;
            cmd = "truncate historique";
            maBD.Commande(cmd);

            cmd = "ALTER TABLE joueursParties DROP foreign key FK_JoueursParties_Parties_NumPartie";
            maBD.Commande(cmd);
            cmd = "ALTER TABLE mains DROP foreign key FK_Mains_Parties_NumPartie";
            maBD.Commande(cmd);
            cmd = "truncate parties";
            maBD.Commande(cmd);

            cmd = "ALTER TABLE etapes DROP foreign key FK_Etapes_Mains_NumPartie_NumMain";
            maBD.Commande(cmd);
            cmd = "truncate mains";
            maBD.Commande(cmd);

            cmd = "ALTER TABLE toursParole DROP foreign key FK_ToursParole_Etapes_NumPartie_NumMain_NomEtape";
            maBD.Commande(cmd);
            cmd = "truncate etapes";
            maBD.Commande(cmd);

            cmd = "truncate toursParole";
            maBD.Commande(cmd);

            //cmd = "ALTER TABLE joueursParties DROP foreign key FK_JoueursParties_Joueurs_Joueur_Nom";
            //maBD.Commande(cmd);
            cmd = "truncate joueursParties";
            maBD.Commande(cmd);

            MessageBox.Show("BD vidée");
        }
        private void ChargerPartie1(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Chargement de la partie 1");
            ///// LINQ pour aller récupérer la partie 1
            //var nirvCtxt = new NirvanaContext();
           // var pReq = from p in (TG.NirvContext.Parties.Include("Mains").Include("Joueurs")) where p.NumPartie==1 select p;

            // LINQ pour recharger la Partie 
            TG.PA = (from p in (TG.NirvContext.Parties.Include("Mains.Etapes.ToursParole").Include("Joueurs")) where p.NumPartie == 1 select p).FirstOrDefault();

            /////////////////////////////////////////////////
            //foreach (Partie p in pReq)
            //{
            //    TG.PA = p;
            //    Main[] TabMains = TG.PA.Mains.ToArray();
            //    TG.PA.MainCourante = TabMains[TG.PA.NumMainCourante -1];
            //}
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());

        }
        private void Truncate(object sender, RoutedEventArgs e)
        {
            BD_primitive maBD = new BD_primitive();
            string cmd;
            cmd = "truncate historique";
            maBD.Commande(cmd);

            cmd = "truncate parties";
            maBD.Commande(cmd);

            cmd = "truncate mains";
            maBD.Commande(cmd);

            cmd = "truncate etapes";
            maBD.Commande(cmd);

            cmd = "truncate toursParole";
            maBD.Commande(cmd);

            cmd = "truncate joueursParties";
            maBD.Commande(cmd);

            MessageBox.Show("BD vidée");
        }

       
        private void CreationBDParEntity(object sender, RoutedEventArgs e)
        {
             //Database.SetInitializer<NirvanaContext>(new DropCreateDatabaseAlways<NirvanaContext>());
             Joueur j0 = new Joueur();
            j0.DateCreation = DateTime.Now;
            j0.Courriel = "Abracadabra";
            j0.Nom = "K";
            Joueur j1 = new Joueur();
            j1.DateCreation = DateTime.Now;
            j1.Courriel = "bellcom";
            j1.Nom = "Pough";
            Joueur j2 = new Joueur();
            j2.DateCreation = DateTime.Now;
            j2.Courriel = "vidoetron";
            j2.Nom = "Speed";
            Joueur j3 = new Joueur();
            j3.DateCreation = DateTime.Now;
            j3.Courriel = "homtial";
            j3.Nom = "Gos";
            Joueur j4 = new Joueur();
            j4.DateCreation = DateTime.Now;
            j4.Courriel = "ccadabra";
            j4.Nom = "Certs";
            Joueur j5 = new Joueur();
            j5.DateCreation = DateTime.Now;
            j5.Courriel = "bring brang";
            j5.Nom = "Cheen";

            TG.NirvContext.Joueurs.Add(j0);
            TG.NirvContext.Joueurs.Add(j1);
            TG.NirvContext.Joueurs.Add(j2);
            TG.NirvContext.Joueurs.Add(j3);
            TG.NirvContext.Joueurs.Add(j4);
            TG.NirvContext.Joueurs.Add(j5);

            TG.NirvContext.SaveChanges();
                
           // }
            //catch(Exception ex)
           // {
            //    MessageBox.Show("Exeption 227:" + ex.Message);
           // }
            MessageBox.Show("NirvanaEF initialisée");
        }

        private void CreationPartieSixJoueurs(object sender, RoutedEventArgs e)
        {
            BD_primitive maBD = new BD_primitive();
            string cmd;
            cmd = "truncate historique";
            maBD.Commande(cmd);

            cmd = "truncate parties";
            maBD.Commande(cmd);

            cmd = "truncate mains";
            maBD.Commande(cmd);

            cmd = "truncate etapes";
            maBD.Commande(cmd);

            cmd = "truncate toursParole";
            maBD.Commande(cmd);

            cmd = "truncate joueursParties";
            maBD.Commande(cmd);

            cmd = "truncate joueurs";
            maBD.Commande(cmd);

            MessageBox.Show("BD vidée");

            Joueur j0 = new Joueur();
            j0.DateCreation = DateTime.Now;
            j0.Courriel = "Abracadabra";
            j0.Nom = "Certs";
            Joueur j1 = new Joueur();
            j1.DateCreation = DateTime.Now;
            j1.Courriel = "bellcom";
            j1.Nom = "Cheen";
            Joueur j2 = new Joueur();
            j2.DateCreation = DateTime.Now;
            j2.Courriel = "vidoetron";
            j2.Nom = "Gos";
            Joueur j3 = new Joueur();
            j3.DateCreation = DateTime.Now;
            j3.Courriel = "homtial";
            j3.Nom = "K";
            Joueur j4 = new Joueur();
            j4.DateCreation = DateTime.Now;
            j4.Courriel = "ccadabra";
            j4.Nom = "Pough";
            Joueur j5 = new Joueur();
            j5.DateCreation = DateTime.Now;
            j5.Courriel = "bring brang";
            j5.Nom = "Speed";

            MessageBox.Show("6 joueurs insérés");
            TG.NirvContext.Joueurs.Add(j0);
            TG.NirvContext.Joueurs.Add(j1);
            TG.NirvContext.Joueurs.Add(j2);
            TG.NirvContext.Joueurs.Add(j3);
            TG.NirvContext.Joueurs.Add(j4);
            TG.NirvContext.Joueurs.Add(j5);

            //TG.NirvContext.SaveChanges();

            List<Joueur> lstInvites = new List<Joueur>();
            lstInvites.Add(j0);
            lstInvites.Add(j1);
            lstInvites.Add(j2);
            lstInvites.Add(j3);
            lstInvites.Add(j4);
            lstInvites.Add(j5);

            TG.PA.NomJoueurLogue = "Certs";
            TG.PA.JoueurLogue = 0;

            TG.PA.NouvellePartie(lstInvites);

            TG.NirvContext.Parties.Add(TG.PA);
            TG.NirvContext.SaveChanges();

            // LINQ pour recharger la Partie 
            // si je ne mets pas le ...Include("Mains.Etapes.ToursParole"), les mains, Etaoes et ToursParole ne sont pas loadés
            TG.PA = (from p in (TG.NirvContext.Parties.Include("Mains.Etapes.ToursParole").Include("Joueurs")) where p.NumPartie == 1 select p).FirstOrDefault();

            TG.PA.ProchainJoueur = TG.PA.MainCourante.EtapeCourante.ProchainJoueur;

            TG.NirvContext.Historiques.Add(new Historique("Début de la partie " + TG.PA.NumPartie, TG.PA.NumPartie));
            TG.NirvContext.Historiques.Add(new Historique(TG.PA.Joueurs[0].NomJoueur + " distribue la main " + TG.PA.NumMainCourante, TG.PA.NumPartie));
            TG.NirvContext.Historiques.Add(new Historique("Gros blind " + TG.PA.NiveauPourSuivre, TG.PA.NumPartie));
            TG.NirvContext.SaveChanges();//   int NumPartie = 1; // TG.SRV.Incarne<iParties_EF_SRV>().InsereNouvellePartie(ListeJoueurs);
                                         //PA partieActive =  TG.SRV.Incarne<iParties_EF_SRV>().RecupUnePA(NumPartie);

            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }
    }

}
