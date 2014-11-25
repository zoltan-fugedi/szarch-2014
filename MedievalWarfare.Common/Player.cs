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
        public bool Neutral { get; set; }
        public Player(int gold)
        {
            PlayerId = new Guid();
            Gold = gold;
            Neutral = false;
        }

        public Player(Guid id, int gold)
        {
            PlayerId = id;
            Gold = gold;
            Neutral = false;
        }

        public Player(Guid id, int gold, bool neut)
        {
            PlayerId = id;
            Gold = gold;
            Neutral = neut;
        }
    }
}
