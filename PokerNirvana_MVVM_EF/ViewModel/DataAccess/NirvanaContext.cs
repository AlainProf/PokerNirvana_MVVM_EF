using PokerNirvana_MVVM_EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PokerNirvana_MVVM_EF
{
    public class NirvanaContext : DbContext
    {
        public NirvanaContext() : base("name=NirvanaConnexion_MySQL") { }
        public DbSet<Partie> Parties { get; set; }
        public DbSet<Joueur> Joueurs { get; set; }
        public DbSet<Historique> Historiques { get; set; }
    }
}
