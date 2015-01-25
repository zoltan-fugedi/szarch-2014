using System;

namespace MedievalWarfare.TestClient.Db.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int Gold { get; set; }
        public bool Neutral { get; set; }
    }
}
