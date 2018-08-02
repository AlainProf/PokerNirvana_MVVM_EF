using PokerNirvana_MVVM_EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace PokerNirvana_MVVM_EF.Model
{
    public class Carte
    {

        public int Valeur { get; set; }
        public int Sorte { get; set; }

        public string NomPhysique;
        public DoubleAnimation posLeft;
        public DoubleAnimation posTop;
        public BitmapImage imgCarte;
        public double positionLeft;
        public double positionTop;

        public Carte()
        {
            Valeur = -1;
            Sorte = -1;
            imgCarte = new BitmapImage(new Uri(TG.PathImage + "Cardz/endos.gif"));
        }

        public Carte(int v, int s)
        {
            Valeur = v;
            Sorte = s;
           
            NomPhysique = GetNomTextuel();
            if (NomPhysique != "vide_vide.gif")
                imgCarte = new BitmapImage(new Uri(TG.PathImage + "Cardz/" + NomPhysique));
            else
                imgCarte = null;
        }

        public Carte(string v, string s)
        {
            Valeur = Convert.ToInt32(v);
            Sorte = Convert.ToInt32(s);
            string NomTex = GetNomTextuel();
            if (NomTex != "vide_vide")
                imgCarte = new BitmapImage(new Uri(TG.PathImage + "Cardz/" + NomTex));
            else
                imgCarte = null;
        }

        public void CacheImage()
        {
            imgCarte = new BitmapImage(new Uri(TG.PathImage + "Cardz/endos.gif"));
        }


        public void InitPosition(int joueur, int posCarte)
        {
            if (joueur < 3)
                positionLeft = 285 + (joueur * 400) + (posCarte * 25);
            else
                positionLeft = 1085 - (joueur - 3) * 400 + (posCarte * 25);

            if (joueur < 3)
            {
                positionTop = 305;
                if (joueur == 1)
                    positionTop = 145;
            }
            else
            {
                positionTop = 580;
                if (joueur == 4)
                    positionTop = 740;
            }
        }

        public void InitPosition(string Etape, int posCarte)
        {
            if (Etape == "FLOP")
            {
                positionLeft = 450 + (posCarte * 75);
                positionTop = 415;
            }
        }
        public void InitPosition(string Etape)
        {
            int offset;
            offset = 800;
            if (Etape == "TURN")
                offset = 720;

            positionLeft = offset;
            positionTop = 415;
        }

        public void InitAnimation(int joueur, int posCarte, double duree)
        {
            posLeft = new DoubleAnimation();
            if (joueur < 3)
                posLeft.To = 285 + (joueur * 400) + (posCarte * 25);
            else
                posLeft.To = 1085 - ((joueur - 3) * 400) + (posCarte * 25);
            posLeft.Duration = TimeSpan.FromSeconds(duree);

            posTop = new DoubleAnimation();
            if (joueur < 3)
            {
                posTop.To = 305;
                if (joueur == 1)
                    posTop.To = 145;
            }
            else
            {
                posTop.To = 580;
                if (joueur == 4)
                    posTop.To = 740;
            }
            posTop.Duration = TimeSpan.FromSeconds(duree);
        }

        public void InitAnimation(string Etape, int noCarte, double duree)
        {
            if (Etape == "FLOP")
            {
                posLeft = new DoubleAnimation();
                posLeft.To = 450 + (noCarte * 75);
                posLeft.Duration = TimeSpan.FromSeconds(duree);

                posTop = new DoubleAnimation();
                posTop.To = 415;
                posTop.Duration = TimeSpan.FromSeconds(duree);
            }
        }
        public void InitAnimation(string Etape, double duree)
        {
            int offset;
            offset = 800;
            if (Etape == "TURN")
                offset = 720;

            posLeft = new DoubleAnimation();
            posLeft.To = offset;
            posLeft.Duration = TimeSpan.FromSeconds(duree);

            posTop = new DoubleAnimation();
            posTop.To = 415;
            posTop.Duration = TimeSpan.FromSeconds(duree);
        }

        public string GetNomTextuel()
        {
            string s = "", v = "";
            switch (Sorte)
            {
                case -1: s = "vide"; break;
                case 0: s = "Pique"; break;
                case 1: s = "Trefle"; break;
                case 2: s = "Carreau"; break;
                case 3: s = "Coeur"; break;
                default: Console.WriteLine("couleur de carte inexistante: Sorte");
                    break;
            }
            switch (Valeur)
            {
                case -1: v = "vide"; break;
                case 0: v = "Deux"; break;
                case 1: v = "Trois"; break;
                case 2: v = "Quatre"; break;
                case 3: v = "Cinq"; break;
                case 4: v = "Six"; break;
                case 5: v = "Sept"; break;
                case 6: v = "Huit"; break;
                case 7: v = "Neuf"; break;
                case 8: v = "Dix"; break;
                case 9: v = "Valet"; break;
                case 10: v = "Reine"; break;
                case 11: v = "Roi"; break;
                case 12: v = "As"; break;
                default: Console.WriteLine("valeur de carte inexistante:" + Valeur);
                    break;
            }
            return s + "_" + v + ".gif";
        }

        public string afficheTexte()
        {
            string CarteTexte = "";
            switch (Valeur)
            {
                case -1: break;
                case 0: CarteTexte = "2"; break;
                case 1: CarteTexte = "3"; break;
                case 2: CarteTexte = "4"; break;
                case 3: CarteTexte = "5"; break;
                case 4: CarteTexte = "6"; break;
                case 5: CarteTexte = "7"; break;
                case 6: CarteTexte = "8"; break;
                case 7: CarteTexte = "9"; break;
                case 8: CarteTexte = "10"; break;
                case 9: CarteTexte = "J"; break;
                case 10: CarteTexte = "Q"; break;
                case 11: CarteTexte = "K"; break;
                case 12: CarteTexte = "A"; break;
                default: CarteTexte = "valeur de carte inexistante:" + Valeur; break;
            }
            switch (Sorte)
            {
                case -1: break;
                case 0: CarteTexte += " de pique"; break;
                case 1: CarteTexte += " de trèfle"; break;
                case 2: CarteTexte += " de carreau"; break;
                case 3: CarteTexte += " de coeur"; break;
                default: CarteTexte += " couleur de carte inexistante:" + Sorte; break;
            }
            return CarteTexte;
        }
    }
}
