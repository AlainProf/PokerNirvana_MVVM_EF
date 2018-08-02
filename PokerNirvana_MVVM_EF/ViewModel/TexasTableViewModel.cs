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
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PokerNirvana_MVVM_ADO.ViewModel;

namespace PokerNirvana_MVVM_EF.ViewModel
{
    class TexasTableViewModel : INotifyPropertyChanged
    {
        public ICommand cmdSuivre { get; set; }
        public ICommand cmdRelancer { get; set; }
        public ICommand cmdAbandonner { get; set; }
        public ICommand cmdGratos { get; set; }
        public ICommand cmdDistribuer { get; set; }
        public string ValRelanceTxt { get; set; }

        public string Titre { get; set; }

        public JoueurPartie JoueurA { get; set; }
        public JoueurPartie JoueurB { get; set; }
        public JoueurPartie JoueurC { get; set; }
        public JoueurPartie JoueurD { get; set; }
        public JoueurPartie JoueurE { get; set; }
        public JoueurPartie JoueurF { get; set; }

        public CartesPubliques CartesCommunes { get; set; }
              
        public string MsgHistorique { get; set; }
        public TexasTable TableCourante        {            get ; set;         }
        public Main mainCourante { get; set; }
        
        private DispatcherTimer delai = new DispatcherTimer();
        //bool NouvellePartie = true;

        public TexasTableViewModel(TexasTable TT)
        {
            //Principale.DemarreDelaiRefresh();
            TableCourante = TT;
            cmdAbandonner = new Command(Abandonner);
            
            //cmdGratos     = new Command(Suivre);
            //cmdSuivre     = new Command(Suivre);
            //cmdRelancer   = new Command(Relancer);
            //cmdDistribuer = new Command(GestionDistribuer);

            //if (PA.NomJoueurLogue == null)
            //   PA.NomJoueurLogue = "Inconnu";

            //if (TG.Contexte == "RECHARGE_PARTIE_EN_COURS")
            //{
            //    NouvellePartie = false;
            //    TG.SRV.Incarne<iParties_EF_SRV>().RecupUnePA(1);
            Titre = "PokerNirvanus, Partie " + TG.PA.NumPartie + ", main " + TG.PA.NumMainCourante + ", Joueur:" + TG.PA.NomJoueurLogue;
            //"" + PA.NumPartie + ", main " + PA.MainCourante + ". Joueur: " + PA.NomJoueurLogue;

            //    mainCourante = TG.SRV.Incarne<iMains_EF_SRV>().RecupUneMain();

            //    PA.Bouton = mainCourante.Bouton;
            //    PA.NomEtape = mainCourante.NomEtape;
            //    PA.NiveauPourSuivre = mainCourante.NiveauPourSuivre;


            //    Etape etapeCourante = TG.SRV.Incarne<Etapes_EF_SRV>().RecupEtapeDuneMain();
            //    PA.NomEtape = etapeCourante.NomEtape;
            //    PA.NumTour = etapeCourante.TourCourant;

            //    PA.Joueurs = TG.SRV.Incarne<iJoueurs_EF_SRV>().RecupJoueursDunePartie(mainCourante); 
            //    PA.ProchainJoueur = etapeCourante.ProchainJoueur;
            //    //TourParole tourParoleCourant = TG.SRV.Incarne<ToursParole_EF_SRV>().RecupToursParole();

            //    if (etapeCourante.NomEtape == "MAIN_TERMINEE_TRAITEE" || 
            //        etapeCourante.NomEtape == "MAIN_TERMINEE_TRAITEE_OUVERTE") 
            //    {
            //        string[] TabDec = new string[6];
            //        int[] TabEng = new int[6];
            //        int[] TabK = new int[6];
            //        for (int i = 0; i < 6; i++)
            //        {
            //            if (i < PA.Joueurs.Count)
            //            {
            //                TabDec[i] = PA.Joueurs[i].Decision;
            //                TabEng[i] = PA.Joueurs[i].Engagement;
            //                TabK[i] = PA.Joueurs[i].Capital;
            //            }
            //            else
            //            {
            //                TabDec[i] = "MORT";
            //                TabEng[i] = 0;
            //                TabK[i] = 0;
            //            }
            //        }

            //        //PA.croupier = new Croupier(TabDec, TabEng, TabK, PA.JoueurLogue, PA.NomEtape, PA.Bouton);
            //        //PA.ProchainJoueur = PA.croupier.DetermineProchainJoueur("NOUVELLE_MAIN");
            //    }

            Main mainTmp = TG.PA.Mains.ElementAt(0);

            Carte c0 = new Carte((int)mainTmp.J0_C0_V, (int)mainTmp.J0_C0_S);
            Carte c1 = new Carte((int)mainTmp.J0_C1_V, (int)mainTmp.J0_C1_S);
            TG.PA.Joueurs[0].ImageCarte0 = c0.imgCarte;
            TG.PA.Joueurs[0].ImageCarte1 = c1.imgCarte;
            string dec = TG.PA.GetJoueurDecision(0);
            if (dec == "ABANDONNER")
               TG.PA.Joueurs[0].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
            else
               TG.PA.Joueurs[0].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[0].NomJoueur + ".jpg"));
            JoueurA = TG.PA.Joueurs[0];

            c0 = new Carte((int)mainTmp.J1_C0_V, (int)mainTmp.J1_C0_S);
            c1 = new Carte((int)mainTmp.J1_C1_V, (int)mainTmp.J1_C1_S);
            TG.PA.Joueurs[1].ImageCarte0 = c0.imgCarte;
            TG.PA.Joueurs[1].ImageCarte1 = c1.imgCarte;
            dec = TG.PA.GetJoueurDecision(1);
            if (dec == "ABANDONNER")
                TG.PA.Joueurs[1].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
            else
                TG.PA.Joueurs[1].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[1].NomJoueur + ".jpg"));
            JoueurB = TG.PA.Joueurs[1];

            if (TG.PA.Joueurs.Count() > 2)
            {
                c0 = new Carte((int)mainTmp.J2_C0_V, (int)mainTmp.J2_C0_S);
                c1 = new Carte((int)mainTmp.J2_C1_V, (int)mainTmp.J2_C1_S);
                TG.PA.Joueurs[2].ImageCarte0 = c0.imgCarte;
                TG.PA.Joueurs[2].ImageCarte1 = c1.imgCarte;
                dec = TG.PA.GetJoueurDecision(2);
                if (dec == "ABANDONNER")
                    TG.PA.Joueurs[2].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
                else
                    TG.PA.Joueurs[2].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[2].NomJoueur + ".jpg"));
                JoueurC = TG.PA.Joueurs[2];
            }

            if (TG.PA.Joueurs.Count() > 3)
            {
                c0 = new Carte((int)mainTmp.J3_C0_V, (int)mainTmp.J3_C0_S);
                c1 = new Carte((int)mainTmp.J3_C1_V, (int)mainTmp.J3_C1_S);
                TG.PA.Joueurs[3].ImageCarte0 = c0.imgCarte;
                TG.PA.Joueurs[3].ImageCarte1 = c1.imgCarte;
                dec = TG.PA.GetJoueurDecision(3);
                if (dec == "ABANDONNER")
                    TG.PA.Joueurs[3].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
                else
                    TG.PA.Joueurs[3].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[3].NomJoueur + ".jpg"));
                JoueurD = TG.PA.Joueurs[3];
            }

            if (TG.PA.Joueurs.Count() > 4)
            {
                c0 = new Carte((int)mainTmp.J4_C0_V, (int)mainTmp.J4_C0_S);
                c1 = new Carte((int)mainTmp.J4_C1_V, (int)mainTmp.J4_C1_S);
                TG.PA.Joueurs[4].ImageCarte0 = c0.imgCarte;
                TG.PA.Joueurs[4].ImageCarte1 = c1.imgCarte;
                dec = TG.PA.GetJoueurDecision(4);
                if (dec == "ABANDONNER")
                    TG.PA.Joueurs[4].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
                else
                    TG.PA.Joueurs[4].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[4].NomJoueur + ".jpg"));
                JoueurE = TG.PA.Joueurs[4];
            }

            if (TG.PA.Joueurs.Count() > 5)
            {
                c0 = new Carte((int)mainTmp.J5_C0_V, (int)mainTmp.J5_C0_S);
                c1 = new Carte((int)mainTmp.J5_C1_V, (int)mainTmp.J5_C1_S);
                TG.PA.Joueurs[5].ImageCarte0 = c0.imgCarte;
                TG.PA.Joueurs[5].ImageCarte1 = c1.imgCarte;
                dec = TG.PA.GetJoueurDecision(5);
                if (dec == "ABANDONNER")
                    TG.PA.Joueurs[5].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + "abandonner.jpg"));
                else
                    TG.PA.Joueurs[5].ImagePokerman = new BitmapImage(new Uri(TG.PathImage + TG.PA.Joueurs[5].NomJoueur + ".jpg"));
                JoueurF = TG.PA.Joueurs[5];
            }

            
           

            MsgHistorique = TG.RecupHistoriquePartie();
            
            allumeBouton();
            eteintJoueursInactifs();
            attenteOuAction();

            TableCourante.CarteFlop0.Visibility = Visibility.Collapsed;
            TableCourante.CarteFlop1.Visibility = Visibility.Collapsed;
            TableCourante.CarteFlop2.Visibility = Visibility.Collapsed;
            TableCourante.CarteTurn.Visibility = Visibility.Collapsed;
            TableCourante.CarteRiver.Visibility = Visibility.Collapsed;

            CartesCommunes = new CartesPubliques();
            CartesCommunes.ImageFlop0 = new Carte((int)mainTmp.F_C0_V, (int)mainTmp.F_C0_S).imgCarte;
            CartesCommunes.ImageFlop1 = new Carte((int)mainTmp.F_C1_V, (int)mainTmp.F_C1_S).imgCarte;
            CartesCommunes.ImageFlop2 = new Carte((int)mainTmp.F_C2_V, (int)mainTmp.F_C2_S).imgCarte;
            CartesCommunes.ImageTurn = new Carte((int)mainTmp.T_V, (int)mainTmp.T_S).imgCarte;
            CartesCommunes.ImageRiver = new Carte((int)mainTmp.R_V, (int)mainTmp.R_S).imgCarte;


            if (OnAFranchit("FLOP"))
            {
                TableCourante.CarteFlop0.Visibility = Visibility.Visible;
                TableCourante.CarteFlop1.Visibility = Visibility.Visible;
                TableCourante.CarteFlop2.Visibility = Visibility.Visible;
            }
            if (OnAFranchit("TURN"))
            {
                TableCourante.CarteTurn.Visibility = Visibility.Visible;
            }
            if (OnAFranchit("RIVER"))
            {
                TableCourante.CarteRiver.Visibility = Visibility.Visible;
            }
        }

        /*--------------------------------------------------------------
         /
         /---------------------------------------------------------------*/
        private static bool OnAFranchit(string etapeVisee)
          {
            if (etapeVisee == "FLOP")
                if (TG.PA.NomEtape == "PRE_FLOP")
                    return false;
                else
                    return true;

            if (etapeVisee == "TURN")
                if (TG.PA.NomEtape == "PRE_FLOP" || TG.PA.NomEtape == "FLOP")
                    return false;
                else
                    return true;

            if (etapeVisee == "RIVER")
                if (TG.PA.NomEtape == "PRE_FLOP" || TG.PA.NomEtape == "FLOP" || TG.PA.NomEtape == "TURN")
                    return false;
                else
                    return true;
            return false;		
          }

        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void AppliqueDecision()
        {
            string statutDeMise = TG.PA.getEtatDeLaMise();

            switch(statutDeMise)
            {
                case "MISE_CONTINUE":
                    TG.PA.zeCroupier = new Croupier();
                    TG.PA.ProchainJoueur = TG.PA.zeCroupier.DetermineProchainJoueur("USUEL");
                    break;
                case "MISE_TERMINEE_MAIN_TERMINEE_DOMINATION":
                    TG.PA.TraitementMainTerminee("DOMINATION");
                    break;
            }
        }

        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void ConvertirParole()
        {
        //   if ( PA.NumTour > 1)
	       //{
        //      List<string> ListDecisionPrec = TG.SRV.Incarne<ToursParole_EF_SRV>().RecupPrecedent();
        //      for(int i=0; i<PA.Joueurs.Count; i++)
        //      {
        //         if (PA.Joueurs[i].Decision == "Parole")
        //              PA.Joueurs[i].Decision = ListDecisionPrec[i];
        //      }
        //   }
        }

        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void Abandonner(object param)
        {
            TG.AjouteHistorique(TG.PA.Joueurs[TG.PA.JoueurLogue].NomJoueur + " abandonner");
            TG.PA.Joueurs[TG.PA.JoueurLogue].Decision = "ABANDONNER";
            ////TG.NirvContext.SaveChanges();
            //TourParole TP = TG.PA.MainCourante.EtapeCourante.TourCourant;
            //Main mano = TG.PA.Mains.ElementAt<Main>(0);
            //Etape step = mano.Etapes.ElementAt<Etape>(0);
            //TourParole tp = step.ToursParole.ElementAt<TourParole>(0);

            //.Etapes.ElementAt<Etape>(0).ToursParole.ElementAt<TourParole>(0);

            int idxMain = TG.PA.NumMainCourante - 1;
            int idxEtape = TG.GetIdxEtape();
            Main mainCourante = TG.PA.Mains.ElementAt(idxMain);
            //int idxTP = mainCourante.Etapes.ElementAt(idxEtape).NumTourCourant;
            int idxTP = 0;
            switch (TG.PA.JoueurLogue)
            {
                case 0:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J0 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J0 = "ABANDONNER";
                    break;
                case 1:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J1 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J1 = "ABANDONNER";
                    break;
                case 2:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J2 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J2 = "ABANDONNER";
                    break;
                case 3:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J3 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J3 = "ABANDONNER";
                    break;
                case 4:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J4 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J4 = "ABANDONNER";
                    break;
                case 5:
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Date_J5 = DateTime.Now;
                    TG.PA.Mains.ElementAt<Main>(idxMain).Etapes.ElementAt<Etape>(idxEtape).ToursParole.ElementAt<TourParole>(idxTP).Dec_J5 = "ABANDONNER";
                    break;
            }
            AppliqueDecision();
            //TG.PA.Mains.ElementAt<Main>(1).Etapes.ElementAt<Etape>(1).ToursParole.ElementAt<TourParole>(1) = TP;
            TG.NirvContext.SaveChanges();
            MsgHistorique = TG.RecupHistoriquePartie();
            TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
            //PA.Joue("", "Texas");
        }
        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void Suivre(object param)
        {
            //TG.SRV.Incarne<iMains_EF_SRV>().RecupUneMain();


            //int DeltaPoursuivre = PA.NiveauPourSuivre - PA.Joueurs[PA.JoueurLogue].Engagement;
            //PA.Joueurs[PA.JoueurLogue].Engagement = PA.NiveauPourSuivre;
            //PA.Joueurs[PA.JoueurLogue].Capital -= DeltaPoursuivre;

            //Historique_EF_SRV hist_AS = new Historique_EF_SRV();
            //if (PA.Joueurs[PA.JoueurLogue].Capital == 0)
            //{
            //    PA.Joueurs[PA.JoueurLogue].Decision = "ALL_IN_SUIVRE";
            //    hist_AS.AjouteHistorique(PA.Joueurs[PA.JoueurLogue].Nom + " suit ALL IN");
            //}
            //else
            //{
            //    PA.Joueurs[PA.JoueurLogue].Decision = "SUIVRE";
            //    if (DeltaPoursuivre == 0)
            //       hist_AS.AjouteHistorique(PA.Joueurs[PA.JoueurLogue].Nom + " y va gratos");
            //    else
            //        hist_AS.AjouteHistorique(PA.Joueurs[PA.JoueurLogue].Nom + " suit");
            //}
            //AppliqueDecision();
        }
        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void Relancer(object param)
        {
            //TG.Relance = Convert.ToInt32(ValRelanceTxt);
            //if (TG.Relance <= 0)
            //    TG.Relance = 1;

            //PA.Joueurs[PA.JoueurLogue].Capital -= (PA.NiveauPourSuivre - PA.Joueurs[PA.JoueurLogue].Engagement) + TG.Relance;
            //PA.Joueurs[PA.JoueurLogue].Engagement = PA.NiveauPourSuivre + TG.Relance;
            //PA.NiveauPourSuivre += TG.Relance;

            //Historique_EF_SRV hist_AS = new Historique_EF_SRV();
            //if (PA.Joueurs[PA.JoueurLogue].Capital == 0)
            //{
            //    PA.Joueurs[PA.JoueurLogue].Decision = "ALL_IN_RELANCER";
            //    hist_AS.AjouteHistorique(PA.Joueurs[PA.JoueurLogue].Nom + " relance ALL IN (" + TG.Relance + ")");
            //}
            //else
            //{
            //    PA.Joueurs[PA.JoueurLogue].Decision = "RELANCER";
            //    hist_AS.AjouteHistorique(PA.Joueurs[PA.JoueurLogue].Nom + " relance de " + TG.Relance);
            //}
            //AppliqueDecision();
            //PA.Joue("", "Texas");
        }

        /*--------------------------------------------------------------
        /
        /---------------------------------------------------------------*/
        public void MainsJoueurDansMainPartie(Main mc)
        {
            //Tableau d'initialisation ré-utilisable
            Carte[] TabCartes = new Carte[7];

            // Cartes publiques
            TabCartes[2] = new Carte(mainCourante.F_C0_V, mainCourante.F_C0_S);
            TabCartes[3] = new Carte(mainCourante.F_C1_V, mainCourante.F_C1_S);
            TabCartes[4] = new Carte(mainCourante.F_C2_V, mainCourante.F_C2_S);
            TabCartes[5] = new Carte(mainCourante.T_V, mainCourante.T_S);
            TabCartes[6] = new Carte(mainCourante.R_V, mainCourante.R_S);


           // PA.MainDesJoueurs = new mainJoueur[7];
            //Cartes privatives
            // Joueur 0
           // TabCartes[0] = new Carte(mainCourante.J0_C0_V, mainCourante.J0_C0_S);
           // TabCartes[1] = new Carte(mainCourante.J0_C1_V, mainCourante.J0_C1_S);
           //// PA.MainDesJoueurs[0] = new mainJoueur(TabCartes);
           // // Joueur 1
           // TabCartes[0] = new Carte(mainCourante.J1_C0_V, mainCourante.J1_C0_S);
           // TabCartes[1] = new Carte(mainCourante.J1_C1_V, mainCourante.J1_C1_S);
           //// PA.MainDesJoueurs[1] = new mainJoueur(TabCartes);
           // // etc...
           // TabCartes[0] = new Carte(mainCourante.J2_C0_V, mainCourante.J2_C0_S);
           // TabCartes[1] = new Carte(mainCourante.J2_C1_V, mainCourante.J2_C1_S);
           //// PA.MainDesJoueurs[2] = new mainJoueur(TabCartes);

           // TabCartes[0] = new Carte(mainCourante.J3_C0_V, mainCourante.J3_C0_S);
           // TabCartes[1] = new Carte(mainCourante.J3_C1_V, mainCourante.J3_C1_S);
           // //PA.MainDesJoueurs[3] = new mainJoueur(TabCartes);

           // TabCartes[0] = new Carte(mainCourante.J4_C0_V, mainCourante.J4_C0_S);
           // TabCartes[1] = new Carte(mainCourante.J4_C1_V, mainCourante.J4_C1_S);
            //PA.MainDesJoueurs[4] = new mainJoueur(TabCartes);

            //TabCartes[0] = new Carte(mainCourante.J5_C0_V, mainCourante.J5_C0_S);
            //TabCartes[1] = new Carte(mainCourante.J5_C1_V, mainCourante.J5_C1_S);
           // PA.MainDesJoueurs[5] = new mainJoueur(TabCartes);
        }
       

        private void GestionDistribuer(object param)
        {
            //PA.MainCourante += 1;
            //PA.NumTour = 1;
            //PA.NomEtape = "PRE_FLOP";
            //PA.InitJoueurs();
            //PA.Eval = new Evaluateur();
            //PA.NouvelleMain();
            //TG.SRV.Incarne<ToursParole_EF_SRV>().FaireMourirMourant();
            //TG.SRV.Incarne<iParties_EF_SRV>().MAJ();
            //TG.SRV.Incarne<IApplicationService>().ChangerVue(new TexasTable());
        }

      
        private void attenteOuAction()
        {
            TableCourante.bout_Distribuer.Visibility = Visibility.Collapsed;
            TableCourante.bout_Suivre.Visibility = Visibility.Collapsed;
            TableCourante.bout_Abandonner.Visibility = Visibility.Collapsed;
            TableCourante.bout_Relancer.Visibility = Visibility.Collapsed;
            TableCourante.CB_ValRelance.Visibility = Visibility.Collapsed;
            TableCourante.bout_Gestion.Visibility = Visibility.Collapsed;

            if (TG.PA.GagnantPartie != null)
            {
                TableCourante.TB_MsgAttente.Text = TG.PA.GagnantPartie + " gagne la partie!";
                return;
            }

            if (TG.PA.NomEtape == "MAIN_TERMINEE_TRAITEE_OUVERTE" || TG.PA.NomEtape == "MAIN_TERMINEE_TRAITEE")
            {
                // on est entre deux mains on attend que le bouton distribue la prochaine main
                if (TG.PA.NomJoueurLogue == TG.PA.Joueurs[TG.PA.GetNextBouton()].NomJoueur)
                {
                    //Principale.ArreteDelaiRefresh();
                    TableCourante.bout_Distribuer.Visibility = Visibility.Visible;
                }
                else
                {
                    TableCourante.TB_MsgAttente.Text = "On que attend que " + TG.PA.Joueurs[TG.PA.GetNextBouton()].NomJoueur + " passe les cartes";
                }
            }
            else
            {
                int prochainJoueur = TG.PA.Mains.ElementAt(0).Etapes.ElementAt(TG.GetIdxEtape()).ProchainJoueur;
                if (prochainJoueur < 0)
                {
                    return;
                    //Etape EtapeCourante = TG.SRV.Incarne<Etapes_EF_SRV>().RecupEtapeDuneMain();
                    //PA.ProchainJoueur = EtapeCourante.ProchainJoueur;
                }

                if (TG.PA.NomJoueurLogue.ToLower() == TG.PA.Joueurs[prochainJoueur].NomJoueur.ToLower())
                {
                    //Principale.ArreteDelaiRefresh();
                    int implication = TG.PA.NiveauPourSuivre - TG.PA.Joueurs[prochainJoueur].Engagement;
                    if (implication == 0)
                        TableCourante.bout_Suivre.Content = "GRATOS";
                    else
                        TableCourante.bout_Suivre.Content = "SUIVRE (" + implication + ")";
                    TableCourante.bout_Suivre.Visibility = Visibility.Visible;
                    TableCourante.bout_Abandonner.Visibility = Visibility.Visible;
                    fixeRelance();

                }
                else
                {
                    TableCourante.bout_Distribuer.Visibility = Visibility.Collapsed;
                    TableCourante.TB_MsgAttente.Text = "On attend la décision de " + TG.PA.Joueurs[prochainJoueur].NomJoueur;
                }
            }
        }

        /*--------------------------------------------------------------
        /
        /---------------------------------------------------------------*/
        private void fixeRelance()
        {
            //int RelanceMax = PA.CalculeRelanceMaximale();

            //if (RelanceMax == 0)
            //{
            //    TableCourante.bout_Relancer.Visibility = Visibility.Collapsed;
            //    TableCourante.CB_ValRelance.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    TableCourante.bout_Relancer.Visibility = Visibility.Visible;
            //    TableCourante.CB_ValRelance.Visibility = Visibility.Visible;
            //    for (int i = 1; i <= RelanceMax; i++)
            //        TableCourante.CB_ValRelance.Items.Add(i);
            //    TableCourante.CB_ValRelance.Text = "1";
            //}
        }

        private void eteintJoueursInactifs()
        {
            int NbJoueurs = TG.PA.Joueurs.Count;
            if (TG.PA.Joueurs[0].Decision == "MORT")
                TableCourante.J_A.Visibility = Visibility.Collapsed;
            if (TG.PA.Joueurs[1].Decision == "MORT")
                TableCourante.J_B.Visibility = Visibility.Collapsed;

            if (NbJoueurs > 2)
            {
                if (TG.PA.Joueurs[2].Decision == "MORT")
                    TableCourante.J_C.Visibility = Visibility.Collapsed;
            }
            else
                TableCourante.J_C.Visibility = Visibility.Collapsed;

            if (NbJoueurs > 3)
            {
                if (TG.PA.Joueurs[3].Decision == "MORT")
                    TableCourante.J_D.Visibility = Visibility.Collapsed;
            }
            else
                TableCourante.J_D.Visibility = Visibility.Collapsed;

            if (NbJoueurs > 4)
            {
                if (TG.PA.Joueurs[4].Decision == "MORT")
                    TableCourante.J_E.Visibility = Visibility.Collapsed;
            }
            else
                TableCourante.J_E.Visibility = Visibility.Collapsed;

            if (NbJoueurs > 5)
            {
                if (TG.PA.Joueurs[5].Decision == "MORT")
                    TableCourante.J_F.Visibility = Visibility.Collapsed;
            }
            else
                TableCourante.J_F.Visibility = Visibility.Collapsed;
        }

        private void allumeBouton()
        {
            TableCourante.Bouton_A.Visibility = Visibility.Collapsed;
            TableCourante.Bouton_B.Visibility = Visibility.Collapsed;
            TableCourante.Bouton_C.Visibility = Visibility.Collapsed;
            TableCourante.Bouton_D.Visibility = Visibility.Collapsed;
            TableCourante.Bouton_E.Visibility = Visibility.Collapsed;
            TableCourante.Bouton_F.Visibility = Visibility.Collapsed;

            switch (TG.PA.Bouton)
            {
                case (0):
                    TableCourante.Bouton_A.Visibility = Visibility.Visible;
                    break;
                case (1):
                    TableCourante.Bouton_B.Visibility = Visibility.Visible;
                    break;
                case (2):
                    TableCourante.Bouton_C.Visibility = Visibility.Visible;
                    break;
                case (3):
                    TableCourante.Bouton_D.Visibility = Visibility.Visible;
                    break;
                case (4):
                    TableCourante.Bouton_E.Visibility = Visibility.Visible;
                    break;
                case (5):
                    TableCourante.Bouton_F.Visibility = Visibility.Visible;
                    break;
            }
        }

        //-------------------------------------------
        //	  
        //-------------------------------------------
        private void traiteDecision(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)e.OriginalSource;
            string Decision = (string)cmd.Content;
          //  TG.Relance = Convert.ToInt32(TableCourante.CB_ValRelance.Text);
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
