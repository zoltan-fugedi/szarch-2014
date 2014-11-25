using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract]
    public class Building : GameObject
    {
        [DataMember]
        public int Population { get; set; }
        public Building()
        {
            Population = 0;
        }
    }
}
