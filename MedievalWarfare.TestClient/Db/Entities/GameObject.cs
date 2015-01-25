using System;

namespace MedievalWarfare.TestClient.Db.Entities
{
    public class GameObject
    {
        public Guid Id { get; set; }
        public virtual Player owner { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
    }
}
