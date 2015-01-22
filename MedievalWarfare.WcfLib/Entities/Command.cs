using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.WcfLib.Entities
{
    public class Command
    {
        public Guid Id { get; set; }
        public virtual GameObject TargetObject { get; set; }
        public virtual Player Owner { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public CommandType Type { get; set; }



    }


    public enum CommandType
    {
        MoveUnit,
        CreateUnit,
        CreateBuilding
    }
}
