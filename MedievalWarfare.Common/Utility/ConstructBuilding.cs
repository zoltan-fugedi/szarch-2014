﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common.Utility
{
    [DataContract]
    public class ConstructBuilding : Command
    {
        [DataMember]
        public Tile Position { get; set; }

        [DataMember]
        public Building Building { get; set; }
    }
}
