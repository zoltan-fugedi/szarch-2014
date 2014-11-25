using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    
    [DataContract]
    public class Player
    {
        [DataMember]
        public Guid PlayerId { get; set; }

        [DataMember]
        public int Gold { get; set; }

        [DataMember]
        public bool Neutral { get; set; }

        [DataMember]
        public String Name { get; set; }

        public Player()
        {
            PlayerId = new Guid();
            Gold = ConstantValues.InitialGold;
            Neutral = false;
        }
        
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

        public Player(Guid id, int gold, bool neut, String name)
        {
            PlayerId = id;
            Gold = gold;
            Neutral = neut;
            Name = name;
        }
    }
}
