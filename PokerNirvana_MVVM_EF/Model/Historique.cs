using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerNirvana_MVVM_EF.Model
{
    [Table("Historique")]
    public class Historique
    {
        [Key]
        public int NumEvenement { get; set; }
        public int NumPartie { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Historique() { }
        public Historique(string message, int numeroPartie)
        {
            Description = message;
            Date = DateTime.Now;
            NumPartie = numeroPartie;
        }



    }
}
