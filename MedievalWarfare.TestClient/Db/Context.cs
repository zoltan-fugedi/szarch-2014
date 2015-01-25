using System.Data.Entity;
using MedievalWarfare.TestClient.Db.Entities;

namespace MedievalWarfare.TestClient.Db
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
