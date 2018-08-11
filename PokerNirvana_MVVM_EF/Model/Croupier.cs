using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.Model
{
    public class Croupier
    {
        public int JoueurLogue;
        public string Etape;
        public List<string> TabDecision = new List<string>();
        public List<int> TabDecActive = new List<int>();
        public List<int> TabActifAvecK = new List<int>();
        public List<int?> TabEng = new List<int?>();
        public List<int> TabK = new List<int>();
        public bool flagRelanceMuette;

        public int NPS;
        public int Petit_Blind;
        public int Gros_Blind;
        public int ProchainJoueur;
        private int Bouton;

        public Croupier()
        {
            for (int i = 0; i < TG.PA.Joueurs.Count; i++)
            {
                 TabDecision.Add(TG.PA.GetJoueurDecision(i));
                 TabEng.Add(TG.PA.GetJoueurEngagement(i));
                 TabK.Add(TG.PA.GetJoueurCapital(i));
            }
            for (int i = TG.PA.Joueurs.Count; i < 6; i++)
            {
                 TabDecision.Add("MORT");
                 TabEng.Add(0);
                 TabK.Add(0);
            }
           
            JoueurLogue = TG.PA.JoueurLogue;
            Etape = TG.PA.NomEtape;
            ConstruireTabDecisionActive();
        }

        public Croupier(string[] TabDD, int[] TabE, int[] TaK, int DernierAAvoirParler, string etape, int bouton)
        {
            TG.Tr("Cons du Croup");
            for (int i = 0; i < 6; i++)
            {
                TabDecision.Add(TabDD[i]);
                TabEng.Add(TabE[i]);
                TabK.Add(TaK[i]);
            }
            Etape = etape;
            Bouton = bouton;
            JoueurLogue = DernierAAvoirParler;

            ConstruireTabDecisionActive();
            ProchainJoueur = TrouveProchain();

            TG.Tr("fin du Cons du Croup");
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private int TrouveProchain()
        {
            TG.Tr("TrouveProchain in", 10);
            int indice = (JoueurLogue + 1) % 6;
            TG.Tr("indice:" + indice, 10);
            for (int i = 0; i < 6; i++)
            {
                TG.Tr("Decision" + indice + ":" + TabDecision[indice], 10);
                if (TabDecision[indice] == "Muet" ||
                     TabDecision[indice] == "MORT" ||
                     TabDecision[indice] == "ABANDONNER" ||
                     TabDecision[indice] == "ALL_IN_RELANCER" ||
                     TabDecision[indice] == "ALL_IN_SUIVRE")
                {
                    indice = (indice + 1) % 6;
                    TG.Tr("un muet on aug indice:" + indice, 10);
                }
                else
                {
                    TG.Tr("un non muet :" + indice, 10);
                    break;
                }
            }
            return indice;
        }

        // Cette fonction possède trois comportements distincts selon le contexte de l'appel:
        // 1- Lors d'une changement d'étape (par exemple quand on passe de turn à river)
        // 2- Lors d'une nouvelle main
        // 3- Le cas le plus usuel, on est en train de miser sur une main X à une étape Y


        public int DetermineProchainJoueur(string Contexte)
        {
            TG.Tr("DPJ entrée", 1);
            if (Contexte == "CHANGEMENT_ETAPE")
            {
                DPJ_ChangementEtape();
                return ProchainJoueur;
            }

            if (Contexte == "NOUVELLE_MAIN")
            {
                DPJ_NouvelleMain();
                return ProchainJoueur;
            }
            DPJ_CasUsuel();
            return ProchainJoueur;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private void DPJ_CasUsuel()
        {
            // -1 Signifie que l'étape de mise est terminée
            // ProchainJoueur = -1;
            // Construction du tableau des dernieres décisions ACTIVE

            TG.Tr("DPJ_CasUsuel", 2);
            if (TraiteCasInitiaux())
            {
                if (ParalysieInitiale("USUEL"))
                {
                    ProchainJoueur = -2;
                }
                TG.Tr("Casinitiaux", 3);
                TG.Tr("Prochain:" + ProchainJoueur, 10);
                return;
            }


            if (TraitePlusVieilleEst_ALL_IN_RELANCER())
            {
                TG.Tr("TPVE+ALL_IN_REL", 3);
                return;
            }
            if (TraiteDerniereEst_RELANCER())
            {
                TG.Tr("TDE_REL", 3);
                return;
            }
            if (TraiteDerniereEst_ALL_IN_SUIVRE())
            {
                TG.Tr("TDE_ALL_IN_SUIVRE", 3);
                return;
            }

            if (Pattern_RR_RRAI() || Pattern_SR_SRAI())
            {
                TG.Tr("RR_RRAI ou SR_SRAI", 3);
                TrouveProchain();
            }
            else
            {

                if (ParalysieUsuelle())
                {
                    TG.Tr("inspect 6", 10);
                    ProchainJoueur = -2;
                }
                else
                {
                    TG.Tr("Ce n'est pas une para usuelle", 10);
                    if (ActifsAvecK_ontTousParle())
                    {
                        TG.Tr("ActifsAvecK_ontTousParle", 3);
                        ProchainJoueur = -1;
                    }
                    else
                        TG.Tr("Les Actif avec K n'ont pas tous parlé", 10);

                    flagRelanceMuette = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (TabDecision[i] == "Muet")
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                if (TabDecision[j] == "ABANDONNER" || TabDecision[j] == "MORT")
                                {
                                    continue;
                                }
                                if (TabEng[j] < TabEng[i])
                                {
                                    TG.Tr("Eng" + j + "=" + TabEng[j] + "<Eng" + i + "=" + TabEng[i], 4);
                                    flagRelanceMuette = true;
                                    break;
                                }
                            }
                            if (flagRelanceMuette)
                            {
                                TG.Tr("Vraie Relance Muette", 3);
                                TrouveProchain();
                                break;
                            }
                        }
                    }
                    if (!flagRelanceMuette)
                    {
                        TG.Tr("NOT flagRelanceMuette", 3);
                        ProchainJoueur = -1;
                    }
                }
            }
            if (ProchainJoueur == -1)
            {
                TG.Tr("Dernier PJ =-1", 3);
                TG.Tr("Etape:" + Etape, 3);
                if (Etape == "RIVER")
                {
                    TG.Tr("Etape RIVER", 3);
                    ProchainJoueur = -2;
                }
            }
            TG.Tr("sortie de DPJ_USUEL", 2);
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private bool TraiteDerniereEst_ALL_IN_SUIVRE()
        {
            if (TabDecision[0] != "ALL_IN_SUIVRE")
                return false;

            int NbParalyse = 0;
            for (int i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[i] == "ALL_IN_RELANCER" ||
                     TabDecision[i] == "ALL_IN_SUIVRE")
                    NbParalyse++;
            }
            if (NbParalyse == TabDecActive.Count)
            {
                ProchainJoueur = -2;
                return true;
            }

            if (Pattern_RR_RRAI() ||
                Pattern_SR_SRAI())
            {
                TrouveProchain();
                return true;
            }
            else
            {
                if (ParalysieUsuelle())
                {
                    TG.Tr("inspect3", 10);
                    ProchainJoueur = -2;
                }
                else
                {
                    TG.Tr("inspect3.b", 10);
                    ProchainJoueur = -1;
                }
                return true;
            }
        }

        //----------------------------------------------
        //
        //----------------------------------------------

        private bool TraiteDerniereEst_RELANCER()
        {
            if (TabDecision[TabDecActive[0]] != "RELANCER")
            {
                return false;
            }
            int NbParalysee = 0;
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "ABANDONNER" ||
                    TabDecision[i] == "MORT" ||
                    TabDecision[i] == "ALL_IN_SUIVRE" ||
                    TabDecision[i] == "Muet")
                    NbParalysee++;
            }
            if (NbParalysee >= 5)
            {
                ProchainJoueur = -2;

                // Avant de stopper la main (-2) il faut vérifier s'il n'y pas un muet qui a 
                // relancer après la relance courante, donc le relanceur doit se prononcer sur le aLL_in qui le
                // surpasse
                for (int i = 0; i < 6; i++)
                {
                    if (RelanceMuette(i))
                    {
                        ProchainJoueur = TrouveProchain();
                    }
                }
                return true;
            }

            bool TouteAutreDecisionEst_SUIVRE = true;
            for (int i = 1; i < TabDecActive.Count; i++)
            {
                int idx = TabDecActive[i];
                if (TabDecision[idx] != "SUIVRE" &&
                    TabDecision[idx] != "ALL_IN_SUIVRE" &&
                    TabDecision[idx] != "Muet")
                {
                    TouteAutreDecisionEst_SUIVRE = false;
                    break;
                }
                if (TabDecision[idx] == "Muet")
                {
                    if (RelanceMuette(idx))
                    {
                        TouteAutreDecisionEst_SUIVRE = false;
                        break;
                    }
                }
            }

            if (TouteAutreDecisionEst_SUIVRE)
            {
                ProchainJoueur = -1;
                return true;
            }
            else
            {
                TrouveProchain();
                return true;
            }
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool TraitePlusVieilleEst_ALL_IN_RELANCER()
        {
            if (TabDecision[TabDecActive[0]] != "ALL_IN_RELANCER")
            {
                return false;
            }

            int NbMuet = 0;
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "ABANDONNER" ||
                    TabDecision[i] == "MORT" ||
                    TabDecision[i] == "ALL_IN_RELANCER" ||
                    TabDecision[i] == "ALL_IN_SUIVRE" ||
                    TabDecision[i] == "Muet")
                    NbMuet++;
            }
            if (NbMuet == 6)
            {
                ProchainJoueur = -2;
                return true;
            }

            bool TouteAutreDecisionEst_SUIVRE = true;
            for (int i = 1; i < TabDecActive.Count; i++)
            {
                int idx = TabDecActive[i];
                if (TabDecision[idx] != "SUIVRE" &&
                    TabDecision[idx] != "ALL_IN_SUIVRE")
                {
                    TouteAutreDecisionEst_SUIVRE = false;
                    break;
                }
            }

            if (TouteAutreDecisionEst_SUIVRE)
            {
                ProchainJoueur = -1;
                if (NbMuet == 5)
                    ProchainJoueur = -2;
                return true;
            }
            else
            {
                if (Pattern_RR_RRAI())
                {
                    TrouveProchain();
                    return true;
                }
                if (Pattern_SR_SRAI())
                {
                    TrouveProchain();
                    return true;
                }
                if (Pattern_RS())
                {
                    ProchainJoueur = -1;
                    return true;
                }
                if (Pattern_RSAI())
                {
                    ProchainJoueur = -2;
                    return true;
                }
                if (Pattern_SR_SRAI())
                {
                    TrouveProchain();
                    return true;
                }
                else
                {
                    if (ParalysieUsuelle())
                    {
                        TG.Tr("inspect 4", 10);
                        ProchainJoueur = -2;
                    }
                    else
                    {
                        TG.Tr("inspect 4b", 10);
                        ProchainJoueur = -1;
                    }
                    return true;
                }
            }
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private bool ParalysieInitiale(string Contexte)
        {
            int pj = ProchainJoueur;
            if (pj == -1)
                return false;
            if (pj == -2)
                return true;

            // tr("504 atteint. Prochain:pj"); 
            int NbParalyse = 0;
            if (TabDecision[pj] == "ALL_IN_RELANCER" ||
                 TabDecision[pj] == "GROS_BLIND" ||
                 TabDecision[pj] == "Muet")
            {

                for (int i = 0; i < 6; i++)
                {
                    if (i == pj)
                        continue;
                    if (TabDecision[i] == "GROS_BLIND" ||
                        TabDecision[i] == "Muet" ||
                        TabDecision[i] == "ABANDONNER" ||
                        TabDecision[i] == "MORT")
                    {
                        // tr ("i : PARA: " . TabDecision[i]);
                        NbParalyse++;
                    }
                    else
                    {
                        if (Contexte == "NOUVELLE_MAIN" &&
                            i == Petit_Blind &&
                            TabDecision[i] == "ALL_IN_SUIVRE")
                        {
                            // tr("i : PARA: " . TabDecision[i]);
                            NbParalyse++;
                        }
                        else
                        {
                            // tr("i : pas de paralysie: " .  TabDecision[i]);
                        }
                    }
                }
                if (NbParalyse == 5)
                {
                    return true;
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool ParalysieUsuelle()
        {
            TG.Tr("ParalysieUsuelle in", 10);
            int Actif = 0;
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "SUIVRE" ||
                    TabDecision[i] == "RELANCER" ||
                    TabDecision[i] == "Attente")
                {
                    Actif++;
                }
                if (RelanceMuette(i))
                    Actif++;
            }
            TG.Tr("Actif:" + Actif, 5);

            if (Actif > 0)
            {
                bool b = ActifOntDesK();
                if (b)
                {
                    TG.Tr("Les actifs ont des K", 10);
                    return false;
                }
            }
            TG.Tr("Pas de joueurs actifs, ou ceux-ci n'ont pas de Kaps ", 10);
            return true;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool ActifsAvecK_ontTousParle()
        {
            TG.Tr("AAKTP in", 10);
            //Tr("TAAK [0]:" + TabActifAvecK[0],12);
            //Tr("TAAK [1]:" + TabActifAvecK[1],12);
            //Tr("TAAK [2]:" + TabActifAvecK[2],12);

            int MinEng = (int)TabEng[TabActifAvecK[0]];
            TG.Tr("MinEng :" + MinEng, 10);
            for (int i = 0; i < TabActifAvecK.Count; i++)
            {
                int indAvecK = TabActifAvecK[i];
                if (TabDecision[indAvecK] == "Attente")
                {
                    TG.Tr("Non ils n'ont pas tous parlé", 12);
                    return false;
                }
                if (TabEng[indAvecK] < MinEng)
                {
                    MinEng = (int)TabEng[indAvecK];
                    TG.Tr("MinEng:" + MinEng, 12);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                TG.Tr("TabEng[" + i + "]:" + TabEng[i], 12);
                if (TabEng[i] > MinEng)
                {
                    TG.Tr("Non car eng est plus grand que l'eng min", 12);
                    return false;
                }
            }
            TG.Tr("Oui ils ont pas TOUS parlé");
            return true;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool ActifOntDesK()
        {
            TG.Tr("ActifOntDesK in", 6);

            int ActifSuivre = 0;
            int ActifAvecKapitaux = 0;
            int IndiceActifAvecK = -1;

            for (int i = 0; i < 6; i++)
            {
                TG.Tr(i + ":" + TabDecision[i], 7);
                if (TabDecision[i] == "RELANCER")
                {
                    IndiceActifAvecK = i;
                    TabActifAvecK.Add(i);
                    ActifAvecKapitaux++;
                }
                if (TabDecision[i] == "Attente")
                {
                    IndiceActifAvecK = i;
                    TabActifAvecK.Add(i);
                    ActifAvecKapitaux++;
                }
                if (TabDecision[i] == "SUIVRE")
                {
                    ActifSuivre++;
                    TG.Tr("(SUIVRE)K de " + i + "=" + TabK[i], 7);
                    if (TabK[i] > 0)
                    {
                        IndiceActifAvecK = i;
                        TabActifAvecK.Add(i);
                        ActifAvecKapitaux++;
                    }
                }
            }

            if (ActifAvecKapitaux == 0)
            {
                TG.Tr("Aucun actifs avec des K ret false");
                return false;
            }
            if (ActifAvecKapitaux == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (IndiceActifAvecK == i)
                        continue;

                    if (TabDecision[i] == "ALL_IN_RELANCER" ||
                        TabDecision[i] == "ALL_IN_SUIVRE" ||
                        TabDecision[i] == "Muet")
                    {
                        if (TabEng[IndiceActifAvecK] < TabEng[i])
                        {
                            TG.Tr("en plus de mon actif il y a un all in qui a misé plus que IndiceActifAvecK: IndiceActifAvecK doit se pronocer");
                            return true;
                        }
                    }
                }
                return false;
            }
            TG.Tr("Au moins deux actifs avec des Kaps");
            return true;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private void ConstruireTabDecisionActive()
        {
            // tr("Dernier a avoir parle: JoueurLogue");
            int OffSet = (JoueurLogue + 1) % 6;
            for (int i = 0; i < 6; i++)
            {
                // tr("CTDDA OffSet: " . TabDecision["OffSet"]);
                if (TabDecision[OffSet] != "ABANDONNER" &&
                    TabDecision[OffSet] != "MORT")
                {
                    // tr("DPJ 5.11: Ajoute un actif: OffSet:" . TabDecision[OffSet]); 
                    TabDecActive.Add(OffSet);
                }
                OffSet = (OffSet + 1) % 6;
            }
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool TraiteCasInitiaux()
        {
            if (CasInitiauxPF())
            {
                TraiteCasInitiauxPF();
                return true;
            }

            if (CasInitiauxGen())
            {
                TraiteCasInitiauxGen();
                return true;
            }
            return false;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        private bool CasInitiauxPF()
        {
            for (int i = 0; i < 6; i++)
                if (TabDecision[i] == "PETIT_BLIND" ||
                    TabDecision[i] == "GROS_BLIND")
                {
                    return true;
                }
            return false;
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private void TraiteCasInitiauxPF()
        {
            for (int i = 1; i < 7; i++)
            {
                int idx = (i + JoueurLogue) % 6;
                if (TabDecision[idx] == "PETIT_BLIND" ||
                    TabDecision[idx] == "GROS_BLIND" ||
                    TabDecision[idx] == "Attente")
                {
                    ProchainJoueur = idx;
                    return;
                }
            }
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        private bool CasInitiauxGen()
        {
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "Attente")
                    return true;

                if (TabDecision[i] == "Parole")
                    return true;
            }
            return false;
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private void TraiteCasInitiauxGen()
        {
            bool ParoleTrouvee = false;
            TG.Tr("TCIG", 10);
            for (int i = 1; i <= 6; i++)
            {
                // tr("i: i");
                int idx = (i + JoueurLogue) % 6;
                // tr("idx: idx");

                if (TabDecision[idx] == "Attente")
                {
                    // tr("DPJ 5.2d: On a trouvé le prochain: idx-");
                    ProchainJoueur = idx;
                    return;
                }
                if (TabDecision[idx] == "Parole")
                {
                    // tr("C'est un parole");
                    if (ParoleEstDernierEtPersonneRelance(idx))
                    {
                        // tr("Parole Est Dernier Et Personne Relance");
                        if (PlusDUnActif())
                            ProchainJoueur = -1;
                        else
                            ProchainJoueur = -2;
                        return;
                    }
                    else
                    {
                        // tr("DPJ 5.2d: On a trouvé le prochain: idx-");
                        if (!ParoleTrouvee)
                        {
                            ProchainJoueur = idx;
                            ParoleTrouvee = true;
                        }
                    }
                }
            }
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool PlusDUnActif()
        {
            int nbActifs = 0;
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "SUIVRE" ||
                     TabDecision[i] == "RELANCER" ||
                     TabDecision[i] == "Parole")
                    nbActifs++;
            }
            if (nbActifs > 1)
                return true;
            return false;
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private bool ParoleEstDernierEtPersonneRelance(int indiceDuParole)
        {
            int nbRelancerAvantParole = 0;
            int nbParole = 0;
            for (int i = 0; i < 6; i++)
            {
                int idx = (i + JoueurLogue) % 6;
                if (TabDecision[idx] == "Parole")
                {
                    nbParole++;
                }
                else
                {
                    bool RelMuette = RelanceMuette(idx);
                    if (TabDecision[idx] == "RELANCER" ||
                         TabDecision[idx] == "ALL_IN_RELANCER" ||
                         RelMuette)
                    {
                        nbRelancerAvantParole++;
                    }
                }
            }
            if (nbRelancerAvantParole == 0 &&
                nbParole == 1)
            {
                return true;
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool RelancesMuettes()
        {
            for (int i = 0; i < 6; i++)
            {
                if (TabDecision[i] == "Muet")
                    return true;
            }
            return false;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        private bool RelanceMuette(int idx)
        {
            if (TabDecision[idx] == "Muet")
            {
                for (int i = 0; i < TabDecActive.Count; i++)
                {
                    int idxActive = TabDecActive[i];
                    if ((TabEng[idxActive] < TabEng[idx])
                         &&
                       TabDecision[idxActive] != "Muet")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool Pattern_RR_RRAI()
        {
            int i;
            for (i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER")
                    break;
            }
            i++;
            for (; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER" ||
                    TabDecision[TabDecActive[i]] == "ALL_IN_RELANCER")
                {
                    return true;
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool Pattern_SR_SRAI()
        {
            int i;
            for (i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "SUIVRE")
                    break;
            }
            i++;
            for (; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER" ||
                    TabDecision[TabDecActive[i]] == "ALL_IN_RELANCER")
                {
                    return true;
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool Pattern_S_R()
        {
            int i;
            for (i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "SUIVRE")
                    break;
            }
            i++;
            for (; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER" ||
                    TabDecision[TabDecActive[i]] == "ALL_IN_RELANCER")
                {
                    return true;
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool Pattern_RS()
        {
            int i;
            for (i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER")
                    break;
            }
            i++;
            for (; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "SUIVRE")
                {
                    return true;
                }
            }
            return false;
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private bool Pattern_RSAI()
        {
            int i;
            for (i = 0; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "RELANCER")
                    break;
            }
            i++;
            for (; i < TabDecActive.Count; i++)
            {
                if (TabDecision[TabDecActive[i]] == "ALL_IN_SUIVRE")
                {
                    return true;
                }
            }
            return false;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private void DPJ_NouvelleMain()
        {
            ProchainJoueur = -1;
            // Trois boucles : une pour le petit blind
            // une pour le gros blind
            // une pour le prochain joueur
            bool Cas_PB_Muet = false;
            int i;
            for (i = 1; i < TG.PA.Joueurs.Count; i++)
            {
                int ind = (i + Bouton) % 6;
                if (TabDecision[ind] != "MORT" &&
                    TabDecision[ind] != "MOURANT")
                {
                    Petit_Blind = ind;
                    if (TabDecision[ind] == "ALL_IN_RELANCER" ||
                        TabDecision[ind] == "ALL_IN_SUIVRE" ||
                        TabDecision[ind] == "Muet")
                    {
                        Cas_PB_Muet = true;
                    }
                    break;
                }
            }

            i++;
            for (; i < 12; i++)
            {
                int ind = (i + Bouton) % 6;
                if (TabDecision[ind] != "MORT" &&
                    TabDecision[ind] != "MOURANT")
                {
                    Gros_Blind = ind;
                    break;
                }
            }
            i++;
            for (; i <= 18; i++)
            {
                int ind = (i + Bouton) % 6;
                if (TabDecision[ind] != "MORT" &&
                    TabDecision[ind] != "MOURANT" &&
                    TabDecision[ind] != "ALL_IN_SUIVRE")
                {
                    ProchainJoueur = ind;
                    break;
                }
            }
            if (Cas_PB_Muet)
            {
                if (ParalysieInitiale("NOUVELLE_MAIN"))
                {
                    ProchainJoueur = -2;
                }
            }
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        private void DPJ_ChangementEtape()
        {
            if (ParalysieUsuelle())
            {
                TG.Tr("inspect 5", 10);

                ProchainJoueur = -2;
                return;
            }
            TG.Tr("Inspecte", 10);
            for (int i = 1; i < 6; i++)
            {
                // On inspecte le candidat à la gauche du bouton
                // A-t-il un statut "ACTIF" ou paralysé (ABANDON, MORT, ALL_IN)
                if (TabDecision[(Bouton + i) % 6] != "ABANDONNER" &&
                    TabDecision[(Bouton + i) % 6] != "MORT" &&
                    TabDecision[(Bouton + i) % 6] != "ALL_IN_SUIVRE" &&
                    TabDecision[(Bouton + i) % 6] != "ALL_IN_RELANCER" &&
                    TabDecision[(Bouton + i) % 6] != "Muet")
                {
                    ProchainJoueur = (Bouton + i) % 6;
                    return;
                }
            }
            ProchainJoueur = -2;
        }
    }
}
