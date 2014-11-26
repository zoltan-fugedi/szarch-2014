using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract]
    public class Building : GameObject
    {
        [DataMember]
        public int Population { get; set; }
        public Building(Tile tile)
        {
            Id = Guid.NewGuid();
            Population = 0;
            Tile = tile;
        }
    }
}
