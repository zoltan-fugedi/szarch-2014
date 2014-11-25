using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract]
    [KnownType(typeof(Building))]
    [KnownType(typeof(Unit))]
    [KnownType(typeof(Treasure))]
    public class GameObject
    {

        [DataMember]
        public Player Player { get; set; }

        public Player Owner { get; set; }

    }
}
