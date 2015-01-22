using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib.Entities
{
    public class Unit : GameObject
    {
        public int Strength { get; set; }
        public int Movement { get; set; }
    }
}
