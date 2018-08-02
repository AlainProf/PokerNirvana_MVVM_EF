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




            //try //var context = new NirvanaContext())
            //{
                //Historique histo = new Historique("Initialisation de la BD", TG.PA.NumPartie);
                //histo.Date = DateTime.Now;
                //histo.NumPartie = 0;
                //context.Historiques.Add(histo);

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
    }

}
