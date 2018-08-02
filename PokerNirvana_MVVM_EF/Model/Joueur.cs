using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PokerNirvana_MVVM_EF.Model
{
    public class Joueur
    {
        [Key]
        public string Nom { get; set; }
        public string MotDePasse { get; set; }
        public string Courriel { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DernierLogon { get; set; }
        public int NbrLogon { get; set; }
        //[NotMapped]
        //public List<JoueurPartie> Parties { get; set; }
        [NotMapped]
        public bool Inviter { get; set; }
        [NotMapped]
        public BitmapImage ImagePokerman { get; set; }
    }
}
