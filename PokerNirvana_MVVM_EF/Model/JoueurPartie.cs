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
    [Table("JoueursParties")]
    public class JoueurPartie
    {
        [Key]
        [Column(Order = 0)]
        public int NumPartie { get; set; }
        [Key]
        [Column(Order = 1)]
        public string NomJoueur { get; set; }

        public int Position { get; set; }
        public string Etat { get; set; }
        public int Capital { get; set; }
        public int Engagement { get; set; }

        [NotMapped]
        public string Decision { get; set; }
        [NotMapped]
        public BitmapImage ImagePokerman { get; set; }
        [NotMapped]
        public Carte[] CartesPrivatives { get; set; }
        [NotMapped]
        public BitmapImage ImageCarte0 { get; set; }
        [NotMapped]
        public BitmapImage ImageCarte1 { get; set; }
        [NotMapped]
        public int ValeurMain { get; set; }


        public JoueurPartie()
        {
            ValeurMain = 0;

        }

        public JoueurPartie(string nom, int pos)
        {
            NomJoueur = nom;
            Position = pos;
            Capital = 100;
        }
    }
}
