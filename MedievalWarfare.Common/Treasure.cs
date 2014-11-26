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
    public class Treasure : GameObject
    {
        [DataMember]
        public int Value { get; set; }
        public Treasure(int value, Tile tile, Player owner)
        {
            Id = Guid.NewGuid();
            Value = value;
            Tile = tile;
            Owner = owner;
        }
    }
}
