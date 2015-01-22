using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int Gold { get; set; }
        public bool Neutral { get; set; }
    }
}
