using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokerNirvana_MVVM_EF.Model
{
    public class Partie
    {
        [Key]
        public virtual int NumPartie { get; set; }
        public virtual int? NumTournoi { get; set; }
        public virtual ICollection<Main> Mains { get; set; }
        [Column("MainCourante")]
        public virtual int NumMainCourante { get; set; }
        public virtual DateTime Debut { get; set; }
        public virtual DateTime? Fin { get; set; }
        public virtual string Perdant_1 { get; set; }
        public virtual DateTime? Perdant_1_date { get; set; }
        public virtual string Perdant_2 { get; set; }
        public virtual DateTime? Perdant_2_date { get; set; }
        public virtual string Perdant_3 { get; set; }
        public virtual DateTime? Perdant_3_date { get; set; }
        public virtual string Perdant_4 { get; set; }
        public virtual DateTime? Perdant_4_date { get; set; }
        public virtual string Perdant_5 { get; set; }
        public virtual DateTime? Perdant_5_date { get; set; }
        public virtual int? Gagnant { get; set; }
        public virtual List<JoueurPartie> Joueurs { get; set; }
        [NotMapped]
        public virtual string NomJoueurLogue { get; set; }
        [NotMapped]
        public virtual int JoueurLogue { get; set; }
        [NotMapped]
        public virtual int Bouton { get; set; }
        [NotMapped]
        public virtual string NomEtape { get; set; }
        [NotMapped]
        public virtual int NiveauPourSuivre { get; set; }
        [NotMapped]
        public virtual Croupier zeCroupier { get; set; }

        private Main mainCourante;
        [NotMapped]
        public virtual Main MainCourante
        {
            get
            {
                if (Mains.Count == 0)
                    return null;
                return Mains.ElementAt(NumMainCourante - 1);
            }
            set { mainCourante = value; }
        }
        [NotMapped]
        public virtual string GagnantPartie { get; set; }
        [NotMapped]
        public virtual int ProchainJoueur
        {
            get
            {
                return MainCourante.EtapeCourante.ProchainJoueur;
            }
            set
            {
                MainCourante.EtapeCourante.ProchainJoueur = value;
            }
        }


        public Partie()
        {
            NomJoueurLogue = "Certs";
        }

        public string GetJoueurDecision(int numJoueur)
        {
            if (TG.PA.Mains.Count == 0)
                return "inconnue";
            int NumTour = MainCourante.EtapeCourante.NumTourCourant;
            TourParole tourParole = MainCourante.EtapeCourante.ToursParole.ElementAt(NumTour - 1);
            string dec = "inconnue";
            switch (numJoueur)
            {
                case 0:
                    dec = tourParole.Dec_J0;
                    break;
                case 1:
                    dec = tourParole.Dec_J1;
                    break;
                case 2:
                    dec = tourParole.Dec_J2;
                    break;
                case 3:
                    dec = tourParole.Dec_J3;
                    break;
                case 4:
                    dec = tourParole.Dec_J4;
                    break;
                case 5:
                    dec = tourParole.Dec_J5;
                    break;
            }
            return dec;
        }


        public int? GetJoueurEngagement(int numJoueur)
        {
            if (TG.PA.Mains.Count == 0)
                return 0;

            int NumTour = MainCourante.EtapeCourante.NumTourCourant;
            TourParole tourParole = MainCourante.EtapeCourante.ToursParole.ElementAt(NumTour - 1);
            int? eng = -1;
            switch (numJoueur)
            {
                case 0:
                    eng = tourParole.Eng_J0;
                    break;
                case 1:
                    eng = tourParole.Eng_J1;
                    break;
                case 2:
                    eng = tourParole.Eng_J2;
                    break;
                case 3:
                    eng = tourParole.Eng_J3;
                    break;
                case 4:
                    eng = tourParole.Eng_J4;
                    break;
                case 5:
                    eng = tourParole.Eng_J5;
                    break;
            }
            return eng;
        }
        public int GetJoueurCapital(int numJoueur)
        {
            if (TG.PA.Mains.Count == 0)
                return 0;

            int NumTour = MainCourante.EtapeCourante.NumTourCourant;
            TourParole tourParole = MainCourante.EtapeCourante.ToursParole.ElementAt(NumTour - 1);
            //int kap = -1;
            //switch (numJoueur)
            //{
            //    case 0:
            //        kap = tourParole..Dec_J0;
            //        break;
            //    case 1MainCourante
            //        dec = tourParole.Dec_J1;
            //        break;
            //    case 2:
            //        dec = tourParole.Dec_J2;
            //        break;
            //    case 3:
            //        dec = tourParole.Dec_J3;
            //        break;
            //    case 4:
            //        dec = tourParole.Dec_J4;
            //        break;
            //    case 5:
            //        dec = tourParole.Dec_J5;
            //        break;
            //}
            return 100;
        }

        /*--------------------------------------------------------------
        /
        /---------------------------------------------------------------*/
        public void NouvellePartie(List<Joueur> ListJoueurs)
        {
            //Eval = new Evaluateur();
            Joueurs = new List<JoueurPartie>();

            try// var context = new NirvanaContext())
            {
                int position = 0;
                foreach (Joueur J in ListJoueurs)
                {
                    JoueurPartie jp = new JoueurPartie(J.Nom, position);

                    Joueurs.Add(jp);
                    position++;
                }

                Debut = DateTime.Now;
                NumMainCourante = 1;
                NomEtape = "PRE_FLOP";

                Mains = new List<Main>();
                Main laMainCourante = new Main();
                laMainCourante.NouvelleMain(NumMainCourante);
                Mains.Add(laMainCourante);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception 102:" + e.ToString());
            }

            //List<string> ListeInvites = new List<string>();




            //TG.SRV.Incarne<joueursParties_EF_SRV>().inserejoueursParties(ListeInvites);
            //TG.AjouteHistorique("Début de la partie " + NumPartie);

            //InitJoueurs(ListJoueurs);
            //NouvelleMain();
            //NumTour = 1;
            //NomEtape = "PRE_FLOP";
            //Bouton = 0;

        }
        public int GetNextBouton()
        {
            return 1;
        }



        /*--------------------------------------------------------------
        /
        /   getEtatDeLaMise()
        /
        /---------------------------------------------------------------*/
        public string getEtatDeLaMise()
        {
            int NbAbandon = 0;
            int NbSuivre = 0;
            int NbAttente = 0;
            int NbAllIn = 0;

            if (ProchainJoueur == -1)
            {
                if (NomEtape == "RIVER")
                {
                    ProchainJoueur = -2;
                    return "MAIN_TERMINEE_DEPARTAGE";
                }
                return "MISE_TERMINEE_MAIN_CONTINUE";
            }

            //	Recensement des catégories de décisions
            for (int i = 0; i < 6; i++)
            {
                if (Joueurs.Count > i)
                {
                    string d = GetJoueurDecision(i);
                    if (d.Equals("ABANDONNER") || d.Equals("MORT"))
                        NbAbandon++;
                    if (d.Equals("SUIVRE"))
                        NbSuivre++;
                    if (d.Equals("ALL_IN_SUIVRE") || d.Equals("ALL_IN_RELANCER"))
                        NbAllIn++;
                    if (d.Equals("Attente") || d.Equals("PETIT_BLIND") || d.Equals("GROS_BLIND"))
                    {
                        if (Joueurs[i].Capital == 0)
                            // Cas ou un blind a tout mis sur son blind
                            NbAllIn++;
                        else
                            NbAttente++;
                    }
                }
                else
                {
                    NbAbandon++;
                }
            }

            if (NbAbandon == 5)
            {
                ProchainJoueur = -2;
                return "MISE_TERMINEE_MAIN_TERMINEE_DOMINATION";
            }

            if (ProchainJoueur == -2)
            {
                return "MISE_PARALYSEE";
            }

            if (NbAttente > 0)
            {
                return "MISE_CONTINUE";
            }

            // Si le joueur qui vient de prendre une décision a décidé de relancer
            // on ne s'obstine pas: la mise va en relance
            if (Joueurs[JoueurLogue].Decision.Equals("RELANCER"))
            {
                return "MISE_CONTINUE";
            }

            // On vérifie s'il y a une relance dans les dernières décisions des autres joueurs
            List<int> TabSuivre = new List<int>();
            List<int> TabRelance = new List<int>();
            int indTabSuivre = 0;
            int indTabRelance = 0;
            for (int i = 0; i < Joueurs.Count; i++)
            {
                // Trouver la relance la plus récente 
                int indice = (JoueurLogue - i + 6) % 6;
                if (Joueurs[indice].Decision.Equals("RELANCER") ||
                    Joueurs[indice].Decision.Equals("ALL_IN_RELANCER"))
                {
                    TabRelance.Add(indice);
                }

                if (Joueurs[indice].Decision.Equals("SUIVRE"))
                {
                    TabSuivre.Add(indice);
                }
            }

            if (TabRelance.Count > 1)
            {
                return "MISE_CONTINUE";
            }
            if (TabRelance.Count == 1)
            {
                if (TabSuivre.Count > 0)
                {
                    int indiceAbsoluRelance = TabRelance[TabRelance.Count - 1];
                    int indiceAbsoluSuivre = TabSuivre[TabSuivre.Count - 1];

                    int indiceRelatifRelance = (JoueurLogue - indiceAbsoluRelance + 6) % 6;
                    int indiceRelatifSuivre = (JoueurLogue - indiceAbsoluSuivre + 6) % 6;

                    // Si l'unique relance est plus ancienne que la plus ancienne des suivre
                    // La mise est terminée, la main continue
                    if (indiceRelatifRelance > indiceRelatifSuivre)
                    {
                        // La relance à été suivie on arrête les mises
                        if (NomEtape.Equals("RIVER"))
                        {
                            return "MAIN_TERMINEE_DEPARTAGE";
                        }
                        return "MISE_TERMINEE_MAIN_CONTINUE";
                    }
                    else
                    {
                        //t(true, "un relancer précédé d'un suivre: Mise continue",  "", 'e', false);   
                        return "MISE_CONTINUE";
                    }
                }
            }
            if (NbAllIn + NbAbandon >= 5)
            {
                //tr("Paralysie type B");   
                return "MISE_PARALYSEE";
            }


            if (NbAbandon + NbSuivre + NbAllIn == 6)
            {
                if (NomEtape == "RIVER")
                {
                    //tr("Main terminée:départage");
                    return "MAIN_TERMINEE_DEPARTAGE";
                }
                else
                {
                    //tr("GEDLM c: this.ProchainJoueur");
                    if (NomEtape == "RIVER")
                    {
                        //tr("Main terminée:départage c");
                        return "MAIN_TERMINEE_DEPARTAGE";
                    }
                    return "MISE_TERMINEE_MAIN_CONTINUE";
                }
            }
            //tr("Mise continue: Cas non attrappé");        
            return "MISE_CONTINUE";
        }

        public void TraitementMainTerminee(string denouement)
        {
            TG.AjouteHistorique("Main " + NumMainCourante + " terminée");
            string TypeDeFin;

            if (denouement == "DOMINATION")
                TypeDeFin = "MAIN_TERMINEE_TRAITEE";
            else
                TypeDeFin = "MAIN_TERMINEE_TRAITEE_OUVERTE";
            determineValeur();
            Gestionnaire g = new Gestionnaire(Joueurs);
            Joueurs = g.TransfereLesCapitaux();
            MainCourante.Fin = DateTime.Now;
            MainCourante.Gagnant = "Jingle belle";
            TG.NirvContext.SaveChanges();
        }

        /*--------------------------------------------------------------
         /
         /   tourComplete()
         /
         /---------------------------------------------------------------*/
        public bool tourComplete()
        {
            for (int i = 0; i < Joueurs.Count; i++)
            {
                if (Joueurs[i].Decision == "Attente" ||
                    Joueurs[i].Decision == "GROS_BLIND" ||
                    Joueurs[i].Decision == "Parole")
                    return false;
            }
            return true;
        }
        /*--------------------------------------------------------------
        /
        /---------------------------------------------------------------*/
        public void insereNouveauTour()
        {
            //InitJoueurs();
            for (int i = 0; i < Joueurs.Count; i++)
            {
                if (Joueurs[i].Decision == "ALL_IN_RELANCER" ||
                    Joueurs[i].Decision == "ALL_IN_SUIVRE")
                    Joueurs[i].Decision = "Muet";

                if (Joueurs[i].Decision == "RELANCER" ||
                    Joueurs[i].Decision == "SUIVRE" ||
                    Joueurs[i].Decision == "GRATOS")
                    Joueurs[i].Decision = "Parole";
            }
            TourParole tp = new TourParole();
            tp.NumTour = TG.PA.MainCourante.EtapeCourante.NumTourCourant;
            tp.NumTour++;
            TG.PA.MainCourante.EtapeCourante.NumTourCourant = tp.NumTour;

            tp.Date_J0 = DateTime.Now;
            tp.Dec_J0 = Joueurs[0].Decision;
            tp.Eng_J0 = Joueurs[0].Engagement;
            tp.Date_J1 = DateTime.Now;
            tp.Dec_J1 = Joueurs[1].Decision;
            tp.Eng_J1 = Joueurs[1].Engagement;
            tp.Date_J2 = DateTime.Now;
            tp.Dec_J2 = Joueurs[2].Decision;
            tp.Eng_J2 = Joueurs[2].Engagement;
            tp.Date_J3 = DateTime.Now;
            tp.Dec_J3 = Joueurs[3].Decision;
            tp.Eng_J3 = Joueurs[3].Engagement;
            tp.Date_J4 = DateTime.Now;
            tp.Dec_J4 = Joueurs[4].Decision;
            tp.Eng_J4 = Joueurs[4].Engagement;
            tp.Date_J5 = DateTime.Now;
            tp.Dec_J5 = Joueurs[5].Decision;
            tp.Eng_J5 = Joueurs[5].Engagement;
            MainCourante.Etapes.ElementAt(0).ToursParole.Add(tp);
            //Num_Tour++;
            //ToursParole_ADO_SRV ToursParole_AS = new ToursParole_ADO_SRV();
            //ToursParole_AS.InsereTourParole();
        }

        public void InsereEtape()
        {
            Etape uneEtape = new Etape();
            uneEtape.NomEtape = NomEtape;

            uneEtape.NumPartie = TG.PA.NumPartie;
            uneEtape.NumMain = TG.PA.NumMainCourante;
            uneEtape.NumTourCourant = 1;
            uneEtape.Debut = DateTime.Now;
            
            for (int i = 0; i < Joueurs.Count; i++)
            {
                if (Joueurs[i].Decision == "ALL_IN_RELANCER" ||
                    Joueurs[i].Decision == "ALL_IN_SUIVRE")
                    Joueurs[i].Decision = "Muet";

                if (Joueurs[i].Decision == "RELANCER" ||
                    Joueurs[i].Decision == "SUIVRE" ||
                    Joueurs[i].Decision == "GRATOS")
                    Joueurs[i].Decision = "Attente";
            }
            uneEtape.ProchainJoueur = TG.PA.zeCroupier.DetermineProchainJoueur("CHANGEMENT_ETAPE");

            TourParole unNeoTour = new TourParole(1);

            uneEtape.ToursParole = new List<TourParole>();
            uneEtape.ToursParole.Add(unNeoTour);

            MainCourante.Etapes.Add(uneEtape);
        }

    /*--------------------------------------------------------------
  /
  /---------------------------------------------------------------*/
    public void DetermineProchaineEtape()
        {
            switch (NomEtape)
            {
                case "PRE_FLOP":
                    NomEtape = "FLOP";
                    break;

                case "FLOP":
                    NomEtape = "TURN";
                    break;

                case "TURN":
                    NomEtape = "RIVER";
                    break;

                case "RIVER":
                    NomEtape = "POST_RIVER";
                    break;
                default:
                    NomEtape = "Inconnue";
                    break;
            }

            //TG.SRV.Incarne<iMains_ADO_SRV>().MAJ_etape();
        }

        private void determineValeur()
        {
            string Message = "";
            int NbActif = 0;

            for (int i = 0; i < Joueurs.Count; i++)
            {
                if (Joueurs[i].Decision != "ABANDONNER" &&
                    Joueurs[i].Decision != "MORT")
                {
                    //int val = Eval.CalculeValeurPostRiver(MainDesJoueurs[i].mainOrigine);
                    //Joueurs[i].ValeurMain = val;
                    //string valF = Eval.ConvertEvalEnFrancais(val);
                    //Message += Joueurs[i].Nom + " a " + valF + " ";
                    NbActif++;
                }
                else
                {
                    //Joueurs[i].ValeurMain = -1;
                }
            }
            //     if (NbActif > 1)
            //         TG.AjouteHistorique(Message, Numero);
        }
        /*--------------------------------------------------------------
 /
 /
 /---------------------------------------------------------------*/
        public int CalculeRelanceMaximale()
        {
            int RelanceMaxJoueur = TG.PA.Joueurs[TG.PA.JoueurLogue].Capital - (TG.PA.MainCourante.NiveauPourSuivre - TG.PA.Joueurs[TG.PA.JoueurLogue].Engagement);

            int RelanceMaxTable = 0;
            int RelanceMaxCourante = 0;
            for (int i = 0; i < TG.PA.Joueurs.Count; i++)
            {
                if (TG.PA.Joueurs[i].Decision == "SUIVRE" ||
                   TG.PA.Joueurs[i].Decision == "RELANCER" ||
                   TG.PA.Joueurs[i].Decision == "Attente" ||
                   TG.PA.Joueurs[i].Decision == "PETIT_BLIND" ||
                   TG.PA.Joueurs[i].Decision == "GROS_BLIND" ||
                   TG.PA.Joueurs[i].Decision == "Parole")
                {
                    if (i == TG.PA.JoueurLogue)
                        continue;
                    RelanceMaxCourante = TG.PA.Joueurs[i].Capital - (TG.PA.MainCourante.NiveauPourSuivre - TG.PA.Joueurs[i].Engagement);
                    if (RelanceMaxTable < RelanceMaxCourante)
                    {
                        RelanceMaxTable = RelanceMaxCourante;
                    }
                }
            }

            if (RelanceMaxTable < RelanceMaxJoueur)
                return RelanceMaxTable;
            return RelanceMaxJoueur;
        }
    }
}
