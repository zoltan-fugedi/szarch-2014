using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract]
    public class Unit : GameObject
    {
        [DataMember]
        public int Movement { get; set; }
        [DataMember]
        public int Strength { get; set; }

        public Unit(int movement, int strength)
        {
            Movement = movement;
            Strength = strength;
        }
    }
}
