using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.Model
{
    class Gestionnaire
    {
        public int NbJoueur = 6;
        public List<JoueurPartie> TabJoueurs;

        public List<int> TabAllIn = new List<int>();
        public List<int> TabAllInTrie = new List<int>();

        public int NbGagnant = 0;
        public List<int> TabGagnants = new List<int>();
        public List<int> GagnantsMainExport = new List<int>();

        public Gestionnaire(List<JoueurPartie> TabJ)
        {
            TabJoueurs = TabJ;
        }
        //----------------------------------------------
        //
        //----------------------------------------------
        public List<JoueurPartie> TransfereLesCapitaux()
        {

            ConstruitTableaudesJoueursAllIn();
            // Les joueurs ALL-IN doivent être triés selon un ordre croissant d'engagement financier
            // On traitera le plus petit engagement en premier
            TriAllIn();
            int IndiceAllIn = 0;
            while (TabAllInTrie.Count() > 0)
            {
                int j = TabAllInTrie[0];
                Console.WriteLine("ALL_IN(" + IndiceAllIn + ") :" + TabJoueurs[j].NomJoueur);
                if (CeAllInEstGagnant(j))
                {
                    TG.Tr("GAGNANT" + TabJoueurs[j].Engagement);
                    RassembleAutreGagnants(j);
                    TG.Tr("Rassemblé aux gag" + TabJoueurs[j].Engagement);
                    GagnantsMainExport.Add(j);
                    TG.Tr("Kap de Antoine: " + TabJoueurs[0].Capital);
                    RepartiLesGainsAllInCourant(CollecteLesPerdants(j), j);

                    TG.Tr("Répartition gain all_in courant");
                    TabJoueurs[j].Decision = "a empoché";
                }
                else
                {
                    Console.WriteLine("looooser");
                }
                IndiceAllIn++;

                TabJoueurs[j].Decision = "AllIn traité";
                // On retranche ce ALL-IN du tableau, son cas est traité
                TabAllInTrie.RemoveAt(0);
            }
            // Tous les ALL_IN (déficitaires) ont été traités
            // Il faut traiter les joueurs qui avaient les fonds pour suivre normalement
            ConstruitTabDesGagnants();
            RepartiLesGainsFinaux();

            return TabJoueurs;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public void RepartiLesGainsAllInCourant(int gainAPartager, int IdxAllInMin)
        {
            Console.WriteLine("Kapi A de Antoine:" + TabJoueurs[0].Capital);

            int EngagementAllIN_MinCourant = TabJoueurs[IdxAllInMin].Engagement;
            string Gagnant;

            //Console.WriteLine("Engagement de 0: " + TabJoueurs[0].Engagement);
            //Console.WriteLine(gainAPartager + " a partager entre: " + TabGagnants.Count() + " gagnants");
            if ((gainAPartager % (TabGagnants.Count())) != 0)
            {
                //Console.WriteLine(gainAPartager + " non divisible par " + TabGagnants.Count());
                // Si les gains à partager ne se divisent pas exactement en entier
                // On donne un  de plus aux premiers et on enlève les portions
                // décimales du ratio à partager
                float gainIndividu = (float)gainAPartager / (float)TabGagnants.Count();
                int GainIndividuEntier = (int)gainIndividu;
                float GainIndividuDecimal = gainIndividu - GainIndividuEntier;
                float PortionResiduelle = GainIndividuDecimal * TabGagnants.Count();

                //Console.WriteLine("Gain indiv         : " + gainIndividu);
                //Console.WriteLine("Gain indiv entier  : " + GainIndividuEntier);
                //Console.WriteLine("Gain indiv decim   : " + GainIndividuDecimal);
                //Console.WriteLine("Portion résidu     : " + PortionResiduelle);

                for (int i = 0; i < TabGagnants.Count(); i++)
                {
                    // Prenons l'engagement de ce joueur
                    int NeoMiseEngagee = TabJoueurs[TabGagnants[i]].Engagement;
                    // Soustrayons y l'engagement du ALLIN Minimum courant		
                    NeoMiseEngagee -= EngagementAllIN_MinCourant;
                    TabJoueurs[TabGagnants[i]].Engagement = NeoMiseEngagee;
                    if (PortionResiduelle > 0.1)
                    {
                        // S'il reste une protion résiduelle suite à la collecte des restes décimaux
                        // ce joueur est chanceux car on arrondit par le haut pour lui
                        Gagnant = TabJoueurs[TabGagnants[i]].NomJoueur;
                        int gainTmp = GainIndividuEntier + 1;

                        // Faut dÉsactiver les TG pour les tests unitaires
                        //TG.AjouteHistorique(Gagnant + " gagne " + gainTmp);


                        TG.Tr("On ajoute " + gainTmp + " a " + TabJoueurs[TabGagnants[i]].NomJoueur + " kap: " + TabJoueurs[TabGagnants[i]].Capital);
                        TabJoueurs[TabGagnants[i]].Capital += GainIndividuEntier + 1;
                        TG.Tr("Aug A " + TabJoueurs[TabGagnants[i]].NomJoueur + " de " + gainTmp + "");
                        TG.Tr("Enga: " + TabJoueurs[TabGagnants[i]].Engagement);
                        PortionResiduelle--;
                    }
                    else if (GainIndividuEntier > 0)
                    {
                        Gagnant = TabJoueurs[TabGagnants[i]].NomJoueur;

                        // Faut dÉsactiver les TG pour les tests unitaires
                        //TG.AjouteHistorique(Gagnant + " gagne " + GainIndividuEntier);


                        TG.Tr("On ajoute " + GainIndividuEntier + " a " + TabJoueurs[TabGagnants[i]].NomJoueur + " kap: " + TabJoueurs[TabGagnants[i]].Capital);
                        TG.Tr("Enga: " + TabJoueurs[TabGagnants[i]].Engagement);
                        TabJoueurs[TabGagnants[i]].Capital += GainIndividuEntier;
                        TG.Tr("Aug B " + TabJoueurs[TabGagnants[i]].NomJoueur + " de " + GainIndividuEntier + "");
                    }
                }
            }
            else
            {
                // Console.WriteLine(gainAPartager + " DIVISIBLE par " + TabGagnants.Count());
                // Les gains se divisent sans reste entre les gagnant;
                float ratioDuGain = gainAPartager / TabGagnants.Count();

                for (int i = 0; i < TabGagnants.Count(); i++)
                {
                    // Prenons l'engagement de ce joueur
                    int NeoMiseEngagee = TabJoueurs[TabGagnants[i]].Engagement;
                    // Soustrayons y l'engagement du ALLIN Minimum courant		
                    NeoMiseEngagee -= EngagementAllIN_MinCourant;
                    TabJoueurs[TabGagnants[i]].Engagement = NeoMiseEngagee;
                    TabJoueurs[TabGagnants[i]].Capital += Convert.ToInt32(ratioDuGain);
                    Gagnant = TabJoueurs[TabGagnants[i]].NomJoueur;

                    // Pour les unittest il faut déactiver les historiques
                    TG.AjouteHistorique(Gagnant + " gagne " + ratioDuGain);
                }
            }
            TG.Tr("Kapi B de Antoine:" + TabJoueurs[0].Capital);
            TG.Tr("out RGAIC. K de " + IdxAllInMin + ":" + TabJoueurs[IdxAllInMin].Capital);
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public int CollecteLesPerdants(int AllInCourant)
        {
            int EngagementAllIN_MinCourant = TabJoueurs[AllInCourant].Engagement;
            int gainAPartager = 0;
            bool EstGagnant = false;

            for (int i = 0; i < TabJoueurs.Count; i++)
            {
                //Console.WriteLine("Traitement de " + i);
                for (int g = 0; g < TabGagnants.Count(); g++)
                {
                    if (i == TabGagnants[g])
                    {
                        //Console.WriteLine(i + "est dans tab gagnant");
                        EstGagnant = true;
                    }
                    else
                    {
                        //Console.WriteLine(i + " est perdant");
                    }
                }
                //Console.WriteLine("Sortie du for");
                if (EstGagnant == true)
                {
                    //Console.WriteLine("Entrée du if " + i );
                    //Console.WriteLine("OK index: " + i + " indexTabGagnant " + TabJoueurs[i]);
                    // C'est un gagnant, on n'a pas à le collecter
                    // On enlève son engagement pour le remettre dans son capital
                    TabJoueurs[i].Engagement -= EngagementAllIN_MinCourant;
                    TabJoueurs[i].Capital += EngagementAllIN_MinCourant;
                    //Console.WriteLine("ici Capital [" + i + "]  :" + TabJoueurs[i].Capital);
                    EstGagnant = false;
                    continue;
                }
                //Console.WriteLine("Sortie du if");

                if (TabJoueurs[i].Engagement > 0)
                {
                    //C'est un perdant on le collecte
                    if (TabJoueurs[i].Engagement > EngagementAllIN_MinCourant)
                    {

                        gainAPartager += EngagementAllIN_MinCourant;
                        TabJoueurs[i].Engagement -= EngagementAllIN_MinCourant;
                        //Console.WriteLine("Collect:" + i + " (" + EngagementAllIN_MinCourant+ ") total");

                    }
                    else
                    {

                        gainAPartager += TabJoueurs[i].Engagement;
                        //Console.WriteLine("Collect:" + i + " ("+ TabJoueurs[i].Engagement+ ") partiel");
                        //TabJoueurs[i].Capital  -= TabJoueurs[i].Engagement;				 
                        TabJoueurs[i].Engagement = 0;
                    }
                    EstGagnant = false;
                }
            }
            //tr("Le gain a partager: gainAPartager");
            Console.WriteLine("Gain à partager:" + gainAPartager);
            //Console.WriteLine("ici2 Capital   :" + TabJoueurs[4].Capital);
            return gainAPartager;
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        public void RepartiLesGainsFinaux()
        {
            int gain = 0;
            int NbGagnants = TabGagnants.Count();

            //Console.WriteLine(TabGagnants.Count() + " gagnant:" + TabGagnants[0] + " " + TabJoueurs[TabGagnants[0]].Nom);
            for (int i = 0; i < TabJoueurs.Count; i++)
            {
                bool joueurInGagnant = false;
                for (int j = 0; j < TabGagnants.Count(); j++)
                {
                    if (TabGagnants[j] == i)
                        joueurInGagnant = true;
                }

                if (joueurInGagnant)
                {
                    TabJoueurs[i].Capital += TabJoueurs[i].Engagement;
                    TabJoueurs[i].Engagement = 0;
                    // C'est un des joueurs gagnants, pas besoin de collecter
                    continue;
                }
                // Le perdant courant a-t-il quelque mise d'engagée?	
                if (TabJoueurs[i].Engagement > 0)
                {
                    gain += TabJoueurs[i].Engagement;
                    Console.WriteLine("Prélève " + TabJoueurs[i].Engagement + " a " + TabJoueurs[i].NomJoueur);
                    TabJoueurs[i].Engagement = 0;
                }
            }

            // On a extrait tous les gains. Il faut les partager avec les gagnants
            int gainTotal = gain;
            if (TabGagnants.Count() > 0)
                gain /= TabGagnants.Count();
            gain = Convert.ToInt32(gain);

            if (gain > 0)
            {
                for (int i = 0; i < TabGagnants.Count(); i++)
                {
                    string Gagnant = TabJoueurs[TabGagnants[i]].NomJoueur;
                    // Pour les unittest il faut déactiver les historiques
                    TG.AjouteHistorique(Gagnant + " gagne " + gain);
                    TabJoueurs[TabGagnants[i]].Capital += gain;
                    Console.WriteLine(TabJoueurs[TabGagnants[i]].NomJoueur + " recoit " + gain);
                }
            }
            // Régler le cas ou la cagnotte n'est pas divisible exactement en autant de part qu'il y a de gagnants
            if ((gain * TabGagnants.Count()) < gainTotal)
            {
                float gainIndividu;
                int GainIndividuEntier;
                float GainIndividuDecimal;
                float PortionDecimalTotalDuGainApartager;
                float PortionResiduelle;

                if (TabGagnants.Count() > 0)
                    // Ça ne devrait jamais se produire mais au cas où
                    gainIndividu = gainTotal / TabGagnants.Count();
                else
                    gainIndividu = gainTotal;

                GainIndividuEntier = (int)gainIndividu;
                GainIndividuDecimal = gainIndividu - GainIndividuEntier;
                PortionDecimalTotalDuGainApartager = GainIndividuDecimal * TabGagnants.Count();
                PortionResiduelle = PortionDecimalTotalDuGainApartager;

                for (int i = 0; i < TabGagnants.Count(); i++)
                {
                    if (PortionResiduelle > 0.1)
                    {
                        // S'il reste une protion résiduelle suite à la collecte des restes décimaux
                        // ce joueur est chanceux car on arrondit par le haut pour lui
                        //echo "Supplément de 1 pour :i<br>";
                        string Gagnant = TabJoueurs[i].NomJoueur;


                        // Pour les unittest il faut déactiver les historiques
                        TG.AjouteHistorique(Gagnant + " gagne 1");


                        TabJoueurs[i].Capital += 1;
                        PortionResiduelle--;
                    }
                }
            }
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public void ConstruitTabDesGagnants()
        {

            int NbGagnants = 0;
            int ValeurMaxi = 0;
            TabGagnants = new List<int>();

            // On commence par trouver la valeur de la main maximale 
            for (int i = 0; i < TabJoueurs.Count(); i++)
            {
                Console.WriteLine(TabJoueurs[i].NomJoueur + " : " + TabJoueurs[i].Decision);
                if ((TabJoueurs[i].Decision != "ABANDONNER") &&
                    (TabJoueurs[i].Decision != "MORT") &&
                    (TabJoueurs[i].Decision != "a empoché") &&
                    (TabJoueurs[i].Decision != "AllIn traité"))
                {
                    Console.WriteLine(TabJoueurs[i].NomJoueur + " : " + TabJoueurs[i].Decision);
                    if (TabJoueurs[i].ValeurMain > ValeurMaxi)
                    {
                        ValeurMaxi = TabJoueurs[i].ValeurMain;
                    }
                }
            }


            // Ensuite on construit un array de tous les joueurs qui ont ces valeurs 
            for (int i = 0; i < TabJoueurs.Count(); i++)
            {
                if ((TabJoueurs[i].Decision != "ABANDONNER") &&
                    (TabJoueurs[i].Decision != "MORT") &&
                    (TabJoueurs[i].Decision != "a empoché") &&
                    (TabJoueurs[i].Decision != "AllIn traité"))
                {
                    if (TabJoueurs[i].ValeurMain == ValeurMaxi)
                    {
                        NbGagnants++;
                        GagnantsMainExport.Add(i);
                        Console.WriteLine(TabJoueurs[i].NomJoueur + " : ajouté aux gagnants");
                        TabGagnants.Add(i);
                    }
                }
            }
            Console.WriteLine("ConstruitTabDesGagnants" + TabGagnants.Count() + " gagnants");

        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool CeAllInEstGagnant(int j)
        {
            int Valeur = TabJoueurs[j].ValeurMain;
            //Console.WriteLine("CeALLInEstGagnant?:" + TabJoueurs[j].Nom); 
            for (int i = 0; i < TabJoueurs.Count; i++)
            {
                if ((TabJoueurs[i].Decision == "ABANDONNER") ||
                     (TabJoueurs[i].Decision == "MORT") ||
                     (TabJoueurs[i].Decision == "a empoché") ||
                     (TabJoueurs[i].Decision == "AllIn traité"))
                {
                    // Console.WriteLine("continue ABA-MMORT-a emp-ALLIn tria");                     
                    continue;
                }
                int ValJoueurCourant = TabJoueurs[i].ValeurMain;
                if (i == j)
                {
                    // On ne veut pas comparer le joueur all_in avec lui-même
                    //Console.WriteLine("continue :" + i + "==" + j);                     
                    continue;
                }
                if (Valeur < ValJoueurCourant)
                {
                    // Ce ALL-IN n'est pas gagnant<br>";
                    // Console.WriteLine("return false :" + Valeur + "<" + ValJoueurCourant);  
                    return false;
                }
                //Console.WriteLine("inaction neutre");  
            }
            Console.WriteLine("C'est un gagnant");
            return true;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        private void RassembleAutreGagnants(int j)
        {
            TabGagnants = new List<int>();
            int Valeur = TabJoueurs[j].ValeurMain;

            for (int i = 0; i < TabJoueurs.Count; i++)
            {
                if (TabJoueurs[i].Decision == "ABANDONNER" || TabJoueurs[i].Decision == "MORT")
                {
                    // Si le joueur s'est couché, il ne peut être gagnant
                    continue;
                }
                if (TabJoueurs[i].Decision == "a empoché")
                {
                    // Si le joueur a déjà empoché, on se fout de la valeur de sa main
                    continue;
                }
                if (TabJoueurs[i].Decision == "AllIn traité")
                {
                    // Si le joueur a déjà empoché, on se fout de la valeur de sa main
                    continue;
                }

                if (Valeur == TabJoueurs[i].ValeurMain)
                {
                    //On vient de trouver un autre gagnant;
                    //Console.WriteLine("On ajoute " + i + " comme co-gagnant");
                    TabGagnants.Add(i);
                }
            }
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public void TriAllIn()
        {

            List<int> TabIndiceTrie = new List<int>();
            int NbTraite = 0;

            //Console.WriteLine("Étape 0: " + TabAllIn.Count() + " joueurs aLL in");
            for (int i = 0; i < TabAllIn.Count(); i++)
            {
                //Console.WriteLine(i + ": Joueur " + TabAllIn[i] + " est ALL_IN!");
            }

            while (NbTraite < TabAllIn.Count())
            {
                int indicePlusPetit = 0;
                int PlusPetitAllIn = 1000000;
                for (int i = 0; i < TabAllIn.Count(); i++)
                {
                    //Console.WriteLine("iter " + NbTraite);
                    if (indiceEstDansTab(TabAllIn[i], TabIndiceTrie))
                    {
                        //Console.WriteLine(TabAllIn[i] + " est déjà dans TabIndiceTrie");
                        continue;
                    }
                    if (TabJoueurs[TabAllIn[i]].Capital + TabJoueurs[TabAllIn[i]].Engagement <= PlusPetitAllIn)
                    {
                        //Console.WriteLine("Joueur: " + TabAllIn[i] + " K + eng <= PlusPetitAllIn + ");
                        indicePlusPetit = TabAllIn[i];
                        PlusPetitAllIn = TabJoueurs[TabAllIn[i]].Capital + TabJoueurs[TabAllIn[i]].Engagement;
                    }
                }
                //Console.WriteLine(" Add " + indicePlusPetit + " a TabIndiceTrie");
                TabIndiceTrie.Add(indicePlusPetit);
                NbTraite++;
            }
            // List<int> TabAllInTrie = new List<int>();
            //Console.WriteLine("Étape Deux. " +  TabIndiceTrie.Count() + " indices tries");
            //for (int i = 0; i < TabIndiceTrie.Count(); i++)
            //{
            //    Console.WriteLine(i + ": Joueur " + TabIndiceTrie[i]);
            //}
            for (int i = 0; i < TabIndiceTrie.Count(); i++)
            {
                TabAllInTrie.Add(TabIndiceTrie[i]);
                //Console.WriteLine(i + ":Joueur "+ ) 
            }
            //Console.WriteLine("Étape Trois. " + TabAllInTrie.Count() + " ALL_IN tries");
        }

        //----------------------------------------------
        //
        //----------------------------------------------
        private bool indiceEstDansTab(int indice, List<int> Tab)
        {
            for (int i = 0; i < Tab.Count(); i++)
            {
                if (Tab[i] == indice)
                {
                    return true;
                }
            }
            return false;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public int NiveauPourSuivre()
        {
            int Niveau = -1;
            for (int j = 0; j < TabJoueurs.Count; j++)
            {
                if (TabJoueurs[j].Decision != "ABANDONNER" && TabJoueurs[j].Decision != "MORT")
                {
                    if (TabJoueurs[j].Engagement > Niveau)
                    {
                        Niveau = TabJoueurs[j].Engagement;
                    }
                }
            }
            return Niveau;
        }


        //----------------------------------------------
        //
        //----------------------------------------------
        public void ConstruitTableaudesJoueursAllIn()
        {
            // Qu'est ce qu'un joueur ALL_IN? c'est un joueur qui n'a pas les capitaux suffisants
            // pour accoter le NiveauPourSuivre, mais qui suit quand même avec tout ce qu'il lui reste

            for (int i = 0; i < TabJoueurs.Count; i++)
            {
                if ((TabJoueurs[i].Decision == "SUIVRE") ||
                    (TabJoueurs[i].Decision == "ALL_IN_SUIVRE") ||
                    (TabJoueurs[i].Decision == "ALL_IN_RELANCER") ||
                    (TabJoueurs[i].Decision == "RELANCER"))
                {

                    // Un all in c'est un joueur n'a pas assez (ou juste assez) pour suivre une mise
                    // et que cette mise met son capital à 0
                    if (TabJoueurs[i].Engagement <= NiveauPourSuivre() && TabJoueurs[i].Capital == 0)
                    {
                        Console.WriteLine(TabJoueurs[i].NomJoueur + " est ALL IN");
                        TabAllIn.Add(i);
                    }
                }
            }

        }

    }
}
