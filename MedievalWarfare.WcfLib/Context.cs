using MedievalWarfare.WcfLib.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib
{
    class Context : DbContext
    {
        public Context(string connString)
            : base(connString)
        {
        }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Building> Buildings { get; set; }
    }
}
