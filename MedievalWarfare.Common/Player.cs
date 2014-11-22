using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public int Gold { get; set; }
        public Player(int gold)
        {
            PlayerId = new Guid();
            Gold = gold;
        }
    }
}
