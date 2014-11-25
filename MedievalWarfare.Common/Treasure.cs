using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Treasure : GameObject
    {
        
        public int Value { get; set; }
        public Treasure(int value)
        {
            Value = value;
        }
    }
}
