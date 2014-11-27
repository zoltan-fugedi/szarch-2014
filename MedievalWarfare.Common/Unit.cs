using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract(IsReference = true)]
    public class Unit : GameObject
    {

        int movement;
        
        
        [DataMember]
        public int Movement {
            get { return movement; }

            set
            {
                movement = value;
                OnPropertyChanged("Movement");
            }
        }


        int strength;
        
        [DataMember]
        public int Strength {
            get { return strength; }

            set
            {
                strength = value;
                OnPropertyChanged("Strength");
            } 
        }

        public Unit(int movement, int strength, Tile tile)
        {
            Id = Guid.NewGuid();
            Movement = movement;
            Strength = strength;
            Tile = tile;
        }
    }
}
