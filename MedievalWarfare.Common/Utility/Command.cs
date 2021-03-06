﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common.Utility
{
    [DataContract]
    [KnownType(typeof(MoveUnit))]
    [KnownType(typeof(CreateUnit))]
    [KnownType(typeof(ConstructBuilding))]
    public class Command
    {
        [DataMember]
        public Player Player { get; set; }
    }
}
