using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Unit : GameObject
    {
        public int Movement { get; set; }
        public int Strength { get; set; }

        public Unit(int movement, int strength)
        {
            Movement = movement;
            Strength = strength;
        }
    }
}
