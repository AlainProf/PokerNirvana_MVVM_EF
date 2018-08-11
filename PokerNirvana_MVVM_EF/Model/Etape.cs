using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokerNirvana_MVVM_EF.Model
{
    public class Etape
    {
        [Key]
        [Column(Order = 0)]
        public virtual int NumPartie { get; set; }
        [Key]
        [Column(Order = 1)]
        public virtual int NumMain { get; set; }
        [Key]
        [Column(Order = 2)]
        public virtual string NomEtape { get; set; }
        
        public virtual int NumTourCourant { get; set; }
        public virtual DateTime Debut { get; set; }
        public virtual DateTime? Fin { get; set; }
        public virtual ICollection<TourParole> ToursParole {get;set;}
        public virtual int ProchainJoueur { get; set; }

        [NotMapped]
        public virtual TourParole TourCourant { get; set; }

        public Etape()
        {
        }

        public Etape(string nom)
        {
            ToursParole = new List<TourParole>();
            NomEtape = nom;
            NumMain = TG.PA.NumMainCourante;
            NumPartie = TG.PA.NumPartie;
            TG.PA.zeCroupier = new Croupier();

            if (NomEtape == "PRE_FLOP")
            {
                InitDecetEng();
                CalculDesBlinds();
                ProchainJoueur = TG.PA.zeCroupier.DetermineProchainJoueur("NOUVELLE_MAIN");
            }
            else
            {
                ProchainJoueur = TG.PA.zeCroupier.DetermineProchainJoueur("NOUVELLE_ETAPE");
            }
                
            NumTourCourant = 1;
            Debut = DateTime.Now;
            
            //ProchainJoueur = TG.PA.ProchainJoueur;
            //TG.PA.ProchainJoueur = TG.PA.zeCroupier.DetermineProchainJoueur("NOUVELLE_MAIN");
            
            TourCourant = new TourParole(NumTourCourant);
            
            ToursParole.Add(TourCourant);
        }

        /**************************************
        *
        **************************************/
        private void InitDecetEng()
        {
            for(int i=0; i<TG.PA.Joueurs.Count; i++)
            {
                if (TG.PA.Joueurs[i].Decision != "MORT" || TG.PA.Joueurs[i].Decision != "MOURANT")
                {
                    TG.PA.Joueurs[i].Decision = "Attente";
                    TG.PA.Joueurs[i].Engagement = 0;
                }
            }
        }
        /**************************************
        *
        **************************************/
        private static void CalculDesBlinds()
        {
            int PB = TrouvePetitBlind();
            int P_K = TG.PA.Joueurs[PB].Capital;

            int GB = TrouveGrosBlind();
            int G_K = TG.PA.Joueurs[GB].Capital;

            int UTG_K = CapitalMaxOutreBlind(PB, GB);
            int PetitBlindTheo = CalculePetitBlindTheorique();

            int GrosBlindTheo = 2 * PetitBlindTheo;


            // On déduit les valeurs Engagement et Decision du joueur petit blind
            if (P_K <= PetitBlindTheo)
            {
                if ((P_K > G_K) &&
                     (P_K > UTG_K))
                {
                    TG.PA.Joueurs[PB].Engagement = TG.maxEntre(G_K, UTG_K);
                    TG.PA.Joueurs[PB].Capital -= TG.PA.Joueurs[PB].Engagement;
                    TG.PA.Joueurs[PB].Decision = "PETIT_BLIND";

                    TG.PA.Joueurs[GB].Engagement = TG.PA.Joueurs[GB].Capital;
                    TG.PA.Joueurs[GB].Capital = 0;
                    TG.PA.Joueurs[GB].Decision = "ALL_IN_SUIVRE";
                }
                else
                {
                    TG.PA.Joueurs[PB].Engagement = P_K;
                    TG.PA.Joueurs[PB].Capital = 0;
                    TG.PA.Joueurs[PB].Decision = "ALL_IN_RELANCER";
                }
            }
            else
            {
                // Capital du Joueur Petit blind > Petit Blind théorique
                TG.PA.Joueurs[PB].Engagement = TG.minEntre(TG.maxEntre(G_K, UTG_K), PetitBlindTheo);
                TG.PA.Joueurs[PB].Capital -= TG.PA.Joueurs[PB].Engagement;
                TG.PA.Joueurs[PB].Decision = "PETIT_BLIND";
            }

            // ---------------------------------------------------
            // Maintenant le GROS Blind

            if ((G_K > P_K) && (G_K > UTG_K))
            {
                //tr(" Gros blind dominant");
                TG.PA.Joueurs[GB].Decision = "GROS_BLIND";
                TG.PA.Joueurs[GB].Engagement = TG.minEntre(GrosBlindTheo, TG.maxEntre(P_K, UTG_K));
                TG.PA.Joueurs[GB].Capital -= TG.PA.Joueurs[GB].Engagement;
                //tr(" Engagement: ( GB ) " .  TG.PA.Joueurs[GB].Engagement);
            }
            else if (G_K > P_K)
            {
                if (G_K > GrosBlindTheo)
                {
                    TG.PA.Joueurs[GB].Decision = "GROS_BLIND";
                    TG.PA.Joueurs[GB].Engagement = TG.minEntre(GrosBlindTheo, UTG_K);
                    TG.PA.Joueurs[GB].Capital -= TG.PA.Joueurs[GB].Engagement;
                }
                else if (G_K > UTG_K)
                {
                    //tr(" Gros blind domine UTG");
                    TG.PA.Joueurs[GB].Decision = "GROS_BLIND";
                    TG.PA.Joueurs[GB].Engagement = UTG_K;
                    TG.PA.Joueurs[GB].Capital -= TG.PA.Joueurs[GB].Engagement;
                }
                else
                {
                    //tr(" Gros blind domine par gros theo et UTG");
                    TG.PA.Joueurs[GB].Decision = "ALL_IN_RELANCER";
                    TG.PA.Joueurs[GB].Engagement = G_K;
                    TG.PA.Joueurs[GB].Capital -= TG.PA.Joueurs[GB].Engagement;
                }
            }
            else if (G_K <= P_K)
            {
                //tr(" Gros blind est domine par petit");
                if (G_K <= PetitBlindTheo)
                {
                    TG.PA.Joueurs[GB].Decision = "ALL_IN_SUIVRE";
                    TG.PA.Joueurs[GB].Engagement = TG.PA.Joueurs[GB].Capital;
                    TG.PA.Joueurs[GB].Capital = 0;
                }
                else if (G_K <= GrosBlindTheo)
                {
                    TG.PA.Joueurs[GB].Decision = "ALL_IN_RELANCER";
                    TG.PA.Joueurs[GB].Engagement = TG.PA.Joueurs[GB].Capital;
                    TG.PA.Joueurs[GB].Capital = 0;
                }
                else
                {
                    TG.PA.Joueurs[GB].Decision = "GROS_BLIND";
                    TG.PA.Joueurs[GB].Engagement = TG.minEntre(GrosBlindTheo, G_K);
                    TG.PA.Joueurs[GB].Capital -= TG.PA.Joueurs[GB].Engagement;
                }
            }
            TG.PA.NiveauPourSuivre =TG.maxEntre(TG.PA.Joueurs[PB].Engagement, TG.PA.Joueurs[GB].Engagement);
            //string upd = "update mains set NiveauPourSuivre = " + NiveauPourSuivre + " where numpartie = " + NumPartie + " and nummain = " + NumMain;
            //MaBd.Update(upd); 
        }

        /**************************************
        *
        **************************************/
        private static int TrouvePetitBlind()
        {
            for (int i = 1; i < TG.PA.Joueurs.Count; i++)
            {
                int indice = (i + TG.PA.Bouton) % 6;
                if (TG.PA.Joueurs[indice].Decision != "MORT" &&
                    TG.PA.Joueurs[indice].Decision != "MOURANT")
                {
                    return indice;
                }
            }
            return 8;
        }
        /**************************************
        *
        **************************************/
        private static int TrouveGrosBlind()
        {
            int i = 1;
            int nbJoueurs = TG.PA.Joueurs.Count;
            for (; i < nbJoueurs; i++)
            {
                int indice = (i + TG.PA.Bouton) % nbJoueurs;
                if (TG.PA.Joueurs[indice].Decision != "MORT" &&
                    TG.PA.Joueurs[indice].Decision != "MOURANT")
                {
                    break;
                }
            }
            i++;
            for (; i <= nbJoueurs; i++)
            {
                int indice = (i + TG.PA.Bouton) % nbJoueurs;
                if (TG.PA.Joueurs[indice].Decision != "MORT" &&
                    TG.PA.Joueurs[indice].Decision != "MOURANT")
                {
                    return indice;
                }
            }
            MessageBox.Show("Erreur pas de gros blind");
            return -1;
        }
        /**************************************
      *
      **************************************/
        private static int CapitalMaxOutreBlind(int PB, int GB)
        {
            int KMax = 0;
            for (int i = 0; i < TG.PA.Joueurs.Count; i++)
            {
                if (i != PB && i != GB)
                {
                    if (TG.PA.Joueurs[i].Capital > KMax)
                        KMax = TG.PA.Joueurs[i].Capital;
                }
            }
            return KMax;
        }

        /*--------------------------------------------------------------
         /
         /---------------------------------------------------------------*/
        private static int CalculePetitBlindTheorique()
        {
            string Type = "AugmenteAddition";
            switch (Type)
            {
                case "Aucune":
                    return 0;
                case "SansCroissance":
                    return 1;
                case "AugmenteAddition":
                    int PetitBlind = 1;
                    //int indice = (TG.PA.NumMain - (TG.PA.NumMain % 3)) / Config.RythmeCroissance;
                    //for (int i = 0; i < indice; i++)
                    //{
                    //    PetitBlind++;
                    //    if (PetitBlind >= (Config.Maximum))
                    //        break;
                    //}
                    return PetitBlind;


                case "AugmenteMulti":
                    PetitBlind = 1;
                    //indice = (MainCourante - (MainCourante % Config.RythmeCroissance)) / Config.RythmeCroissance;

                    //for (int i = 0; i < indice; i++)
                    //{
                    //    PetitBlind *= 2;
                    //    if (PetitBlind >= (Config.Maximum))
                    //        break;
                    //}

                    return PetitBlind;
            }
            return -1;
        }

    }
}
