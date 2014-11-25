using MedievalWarfare.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class GameObject : EntityBase
    {
        public Player Player { get; set; }
    }
}
