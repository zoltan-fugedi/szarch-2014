using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.Common
{
    [DataContract]
    [KnownType(typeof(Building))]
    [KnownType(typeof(Unit))]
    [KnownType(typeof(Treasure))]
    public class GameObject
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Player Owner { get; set; }
        [DataMember]
        public Tile Tile { get; set; }

    }
}
