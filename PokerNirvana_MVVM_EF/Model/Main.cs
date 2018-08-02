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
    public class Main
    {
        [Key]
        [Column(Order = 0)]
        public virtual int NumPartie { get; set; }
        [Key]
        [Column(Order = 1)]
        public virtual int NumMain { get; set; }

        public virtual int Bouton { get; set; }
        public virtual string NomEtapeCourante { get; set; }
        public virtual DateTime Debut { get; set; }
        public virtual int NiveauPourSuivre { get; set; }
        public virtual DateTime? Fin { get; set; }
        public virtual string Gagnant { get; set; }
        public virtual ICollection<Etape> Etapes { get; set; }

        public virtual int? J0_C0_V { get; set; }
        public virtual int? J0_C0_S { get; set; }
        public virtual int? J0_C1_V { get; set; }
        public virtual int? J0_C1_S { get; set; }
        public virtual int? J1_C0_V { get; set; }
        public virtual int? J1_C0_S { get; set; }
        public virtual int? J1_C1_V { get; set; }
        public virtual int? J1_C1_S { get; set; }
        public virtual int? J2_C0_V { get; set; }
        public virtual int? J2_C0_S { get; set; }
        public virtual int? J2_C1_V { get; set; }
        public virtual int? J2_C1_S { get; set; }
        public virtual int? J3_C0_V { get; set; }
        public virtual int? J3_C0_S { get; set; }
        public virtual int? J3_C1_V { get; set; }
        public virtual int? J3_C1_S { get; set; }
        public virtual int? J4_C0_V { get; set; }
        public virtual int? J4_C0_S { get; set; }
        public virtual int? J4_C1_V { get; set; }
        public virtual int? J4_C1_S { get; set; }
        public virtual int? J5_C0_V { get; set; }
        public virtual int? J5_C0_S { get; set; }
        public virtual int? J5_C1_V { get; set; }
        public virtual int? J5_C1_S { get; set; }

        public virtual int F_C0_V { get; set; }
        public virtual int F_C0_S { get; set; }
        public virtual int F_C1_V { get; set; }
        public virtual int F_C1_S { get; set; }
        public virtual int F_C2_V { get; set; }
        public virtual int F_C2_S { get; set; }

        public virtual int T_V { get; set; }
        public virtual int T_S { get; set; }

        public virtual int R_V { get; set; }
        public virtual int R_S { get; set; }

        public virtual int? Valeur_J0 { get; set; }
        public virtual int? Valeur_J1 { get; set; }
        public virtual int? Valeur_J2 { get; set; }
        public virtual int? Valeur_J3 { get; set; }
        public virtual int? Valeur_J4 { get; set; }
        public virtual int? Valeur_J5 { get; set; }
        [NotMapped]
        public virtual Paquet LePaquet { get; set; }
        //[NotMapped]
        public MainDeJoueur[] MainsDesJoueurs;
        [NotMapped]
        Carte[] Flop;
        [NotMapped]
        Carte Turn;
        [NotMapped]
        Carte River;
        [NotMapped]
        virtual public Etape EtapeCourante { get; set; }

        public Main()
        {
            //Etapes = new List<Etape>();
        }

        //--------------------------------------------------------------
        //
        //--------------------------------------------------------------
        public void NouvelleMain(int numeroMain)
        {
            Etapes = new List<Etape>();
            NumMain = numeroMain;
            LePaquet = new Paquet();
            LePaquet.brasse();
            distribueMains();
            //determineValeur();
            Debut = DateTime.Now;
            NomEtapeCourante = "PRE_FLOP";

            EtapeCourante = new Etape();
            EtapeCourante.NouvelleEtape(NomEtapeCourante);
            //EtapeCourante.ProchainJoueur = TG.PA.ProchainJoueur;
            Etapes.Add(EtapeCourante);
            //NiveauPourSuivre = TG.PA.NiveauPourSuivre;
        }

        /*--------------------------------------------------------------
        /---------------------------------------------------------------*/
        private void distribueMains()
        {
            NomEtapeCourante = "PRE_FLOP";
            if (NumMain == 1)
                Bouton = 0;
            else
                Bouton = 0;
            //Bouton = GetNextBouton();

            MainsDesJoueurs = new MainDeJoueur[TG.PA.Joueurs.Count];

            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
            {
                MainsDesJoueurs[j] = new MainDeJoueur();
                MainsDesJoueurs[j].mainOrigine = new Carte[7];
                if (TG.PA.Joueurs[j].Decision != "MORT")
                {
                    MainsDesJoueurs[j].mainOrigine[0] = LePaquet.donneProchaineCarte();
                    MainsDesJoueurs[j].mainOrigine[1] = LePaquet.donneProchaineCarte();
                }
            }

            Flop = new Carte[3];
            Flop[0] = LePaquet.donneProchaineCarte();
            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
                MainsDesJoueurs[j].mainOrigine[2] = Flop[0];

            Flop[1] = LePaquet.donneProchaineCarte();
            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
                MainsDesJoueurs[j].mainOrigine[3] = Flop[1];

            Flop[2] = LePaquet.donneProchaineCarte();
            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
                MainsDesJoueurs[j].mainOrigine[4] = Flop[2];

            Turn = LePaquet.donneProchaineCarte();
            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
                MainsDesJoueurs[j].mainOrigine[5] = Turn;

            River = LePaquet.donneProchaineCarte();
            for (int j = 0; j < TG.PA.Joueurs.Count; j++)
                MainsDesJoueurs[j].mainOrigine[6] = River;

            DateTime DateDebut = DateTime.Now;
            distribueMainBD();
        }

        /*--------------------------------------------------------------
        /
        /---------------------------------------------------------------*/
        private void distribueMainBD()
        {
            J0_C0_V = MainsDesJoueurs[0].mainOrigine[0].Valeur;
            J0_C0_S = MainsDesJoueurs[0].mainOrigine[0].Sorte;
            J0_C1_V = MainsDesJoueurs[0].mainOrigine[1].Valeur;
            J0_C1_S = MainsDesJoueurs[0].mainOrigine[1].Sorte;

            J1_C0_V = MainsDesJoueurs[1].mainOrigine[0].Valeur;
            J1_C0_S = MainsDesJoueurs[1].mainOrigine[0].Sorte;
            J1_C1_V = MainsDesJoueurs[1].mainOrigine[1].Valeur;
            J1_C1_S = MainsDesJoueurs[1].mainOrigine[1].Sorte;

            if (TG.PA.Joueurs.Count > 2)
            {
                J2_C0_V = MainsDesJoueurs[2].mainOrigine[0].Valeur;
                J2_C0_S = MainsDesJoueurs[2].mainOrigine[0].Sorte;
                J2_C1_V = MainsDesJoueurs[2].mainOrigine[1].Valeur;
                J2_C1_S = MainsDesJoueurs[2].mainOrigine[1].Sorte;
            }
            if (TG.PA.Joueurs.Count > 3)
            {
                J3_C0_V = MainsDesJoueurs[3].mainOrigine[0].Valeur;
                J3_C0_S = MainsDesJoueurs[3].mainOrigine[0].Sorte;
                J3_C1_V = MainsDesJoueurs[3].mainOrigine[1].Valeur;
                J3_C1_S = MainsDesJoueurs[3].mainOrigine[1].Sorte;
            }

            if (TG.PA.Joueurs.Count > 4)
            {
                J4_C0_V = MainsDesJoueurs[4].mainOrigine[0].Valeur;
                J4_C0_S = MainsDesJoueurs[4].mainOrigine[0].Sorte;
                J4_C1_V = MainsDesJoueurs[4].mainOrigine[1].Valeur;
                J4_C1_S = MainsDesJoueurs[4].mainOrigine[1].Sorte;
            }
            if (TG.PA.Joueurs.Count > 5)
            {
                J5_C0_V = MainsDesJoueurs[5].mainOrigine[0].Valeur;
                J5_C0_S = MainsDesJoueurs[5].mainOrigine[0].Sorte;
                J5_C1_V = MainsDesJoueurs[5].mainOrigine[1].Valeur;
                J5_C1_S = MainsDesJoueurs[5].mainOrigine[1].Sorte;
            }

            F_C0_V = Flop[0].Valeur;
            F_C0_S = Flop[0].Sorte;
            F_C1_V = Flop[1].Valeur;
            F_C1_S = Flop[1].Sorte;
            F_C2_V = Flop[2].Valeur;
            F_C2_S = Flop[2].Sorte;

            T_V = Turn.Valeur;
            T_S = Turn.Sorte;

            R_V = River.Valeur;
            R_S = River.Sorte;
        }
 
    }
}
