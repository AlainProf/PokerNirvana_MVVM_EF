using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.Model
{
    [Table("ToursParole")]
    public class TourParole
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
        [Key]
        [Column(Order = 3)]
        public virtual int NumTour { get; set; }

        public virtual string Dec_J0 { get; set; }
        public virtual int? Eng_J0 { get; set; }
        public virtual DateTime? Date_J0 { get; set; }

        public virtual string Dec_J1 { get; set; }
        public virtual int? Eng_J1 { get; set; }
        public virtual DateTime? Date_J1 { get; set; }

        public virtual string Dec_J2 { get; set; }
        public virtual int? Eng_J2 { get; set; }
        public virtual DateTime? Date_J2 { get; set; }

        public virtual string Dec_J3 { get; set; }
        public virtual int? Eng_J3 { get; set; }
        public virtual DateTime? Date_J3 { get; set; }

        public virtual string Dec_J4 { get; set; }
        public virtual int? Eng_J4 { get; set; }
        public virtual DateTime? Date_J4 { get; set; }

        public virtual string Dec_J5 { get; set; }
        public virtual int? Eng_J5 { get; set; }
        public virtual DateTime? Date_J5 { get; set; }

        public TourParole()
        { }
        public void NouveauTour(int numero)
        {
            NumTour = numero;
            NomEtape = TG.PA.NomEtape;
            Dec_J0 = TG.PA.Joueurs[0].Decision;
            Dec_J1 =TG.PA.Joueurs[1].Decision;
            Eng_J0 = TG.PA.Joueurs[0].Engagement;
            Eng_J1 = TG.PA.Joueurs[1].Engagement;
            Date_J0 = DateTime.Now;
            Date_J1 = DateTime.Now;
            if (TG.PA.Joueurs.Count > 2)
            {
                Dec_J2 = TG.PA.Joueurs[2].Decision;
                Date_J2 = DateTime.Now;
                Eng_J2 = TG.PA.Joueurs[2].Engagement;
            }
            if (TG.PA.Joueurs.Count > 3)
            {
                Dec_J3 = TG.PA.Joueurs[3].Decision;
                Date_J3 = DateTime.Now;
                Eng_J3 = TG.PA.Joueurs[3].Engagement;
            }
            if (TG.PA.Joueurs.Count > 4)
            {
                Dec_J4 = TG.PA.Joueurs[4].Decision;
                Date_J4 = DateTime.Now;
                Eng_J4 = TG.PA.Joueurs[4].Engagement;
            }
            if (TG.PA.Joueurs.Count > 5)
            {
                Dec_J5 = TG.PA.Joueurs[5].Decision;
                Date_J5 = DateTime.Now;
                Eng_J5 = TG.PA.Joueurs[5].Engagement;
            }
        }
    }
}
