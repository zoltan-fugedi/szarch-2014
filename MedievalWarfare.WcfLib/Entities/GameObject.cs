using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib.Entities
{
    public class GameObject
    {
        public Guid Id { get; set; }
        public virtual Player owner { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
    }
}
