using PokerNirvana_MVVM_EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokerNirvana_MVVM_EF
{
    class ServicePlus
    {
        private readonly Dictionary<Type, object> inscription = new Dictionary<Type, object>();

        public void Inscrit<TInterface, TClass>(TClass service)
        {
            inscription[typeof(TInterface)] = service;
        }

        public T Incarne<T>()
        {
            return (T)inscription[typeof(T)];
        }

        public ServicePlus() { }
    }

    class TG
    {
        public TG()
        {
         //   Database.SetInitializer<NirvanaContext>(new DropCreateDatabaseAlways<NirvanaContext>());
            Database.SetInitializer<NirvanaContext>(new CreateDatabaseIfNotExists<NirvanaContext>());
            NirvContext = new NirvanaContext();
            PA = new Partie();
            //PA = NirvContext.Parties.Create();

          
        }

        public static string Contexte;
        public static ServicePlus SRV = new ServicePlus();

        public static NirvanaContext NirvContext;
        public static Partie PA;
        


        //public static string DernierRefresh;
        //public static int Relance;
        public static string PathImage = "pack://application:,,,/View/Images/";

        public static int CompteurTimer = 0;







        /**************************************
         *
        **************************************/
        public static void OuvrirEcran(Window Parent, string NomEcran)
        {
            Type type = Parent.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance(
                type.Namespace + "." + NomEcran);

            // Show the window.
            Parent.Close();
            win.ShowDialog();
        }

        /**************************************
         *
        **************************************/
        public static string GetDernierHistorique()
        {
            //string sel = "select max(date) from historique where NumPartie = " + PA.NumPartie;
            ////List<string>[] res = MaBD.Select(sel);
            ////if (res[0][0]!= "")
            ////   return res[0][0];
            ////else
            return "1999-12-31 00:59:59";
        }

        
        public static int GetIdxEtape()
        {
            string etape = PA.NomEtape;
            if (PA.MainCourante == null)
                return 0;

            ICollection<Etape> lstEtape = PA.MainCourante.Etapes;
            return lstEtape.Count - 1;
           
        }

        //public static int GetIdxTourParole()
        //{
        //    int numTour = PA.MainCourante.EtapeCourante.NumTourCourant;
        //    ICollection<Etape> lstEtape = PA.Mains.ElementAt<Main>(PA.NumMainCourante).Etapes;

        //    for (int i = 0; i < lstEtape.Count; i++)
        //    {
        //        if (lstEtape.ElementAt<Etape>(i).NomEtape == etape)
        //            return i;
        //    }
        //    return -1;
        //}

        /**************************************
         *
        **************************************/
        public static string RecupHistoriquePartie()
        {
            string LHistorique = "";

            /// LINQ pour récupérer l'historique de la partie
            //var nirvCtxt = new NirvanaContext();

            var histoReq = from h in (NirvContext.Historiques) where h.NumPartie==1 orderby h.NumEvenement descending select h;
            int cmp = histoReq.Count();
            foreach (Historique h in histoReq)
            {
                LHistorique += cmp + ": " + h.Description + "\n";
                cmp--;
            }
            return LHistorique;
        }


        /**************************************
         *
        **************************************/
        public static void AjouteHistorique(string Evenement)
        {
            int Longueur = Evenement.Length;
            string MsgHistoriqueFormate = "";

            if (Longueur > 22)
            {
                while (Longueur > 22)
                {
                    MsgHistoriqueFormate += Evenement.Substring(0, 22) + "\n";
                    Evenement = Evenement.Substring(22);
                    Longueur = Evenement.Length;
                }
                Evenement = MsgHistoriqueFormate + Evenement;
            }

            //var context = new NirvanaContext();
            Historique histo = new Historique();
            histo.Description = Evenement;
            histo.Date = DateTime.Now;
            histo.NumPartie = TG.PA.NumPartie;
            TG.NirvContext.Historiques.Add(histo);
            TG.NirvContext.SaveChanges();

            
        }
        /**************************************
         *
        **************************************/
        public static int maxEntre(int v1, int v2)
        {
            if (v1 > v2)
                return v1;
            return v2;
        }
        /**************************************
        *
        **************************************/
        public static int minEntre(int v1, int v2)
        {
            if (v1 < v2)
                return v1;
            return v2;
        }

        /**************************************
        *
        **************************************/
        public static void changeEtat(string E)
        {
            //string upd = "update joueursParties set Etat='" + E + "' where Nom = '" + TG.NomJoueurLogue + "' and NumPartie=" + TG.NumPartie;
            //MaBD.Update(upd);
        }

        /**************************************
        *
        **************************************/
        public static string recupEtat(string N)
        {
            // string sel = "select Etat from joueursParties where Nom = '" + TG.NomJoueurLogue + "' and NumPartie=" + TG.NumPartie;
            //    List<string>[] res = MaBD.Select(sel);
            //    string Etat = res[0][0];
            //    return Etat; 
            return "";
        }

        /**************************************
        *
        **************************************/


        public static void Tr(string msg, int niveau = 1)
        {
            bool TraceOn = false;
            int NiveauTrace = 100;

            if (TraceOn)
                if (niveau < NiveauTrace)
                {
                    for (int i = 0; i < niveau; i++)
                        Console.Write("-");
                    Console.WriteLine(msg);
                }
        }
    }
}
