using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{

    [DataContract(IsReference = true)]
    public class Player : INotifyPropertyChanged
    {
        [DataMember]
        public Guid PlayerId { get; set; }

        int gold;
        
        
        [DataMember]
        public int Gold
        {
            get { return gold; }

            set
            {
                gold = value;
                OnPropertyChanged("Gold");
            }
        }


        [DataMember]
        public bool Neutral { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public bool IsWinner { get; set; }

        public Player()
        {
            PlayerId = Guid.NewGuid();
            Gold = ConstantValues.InitialGold;
            Neutral = false;
        }

        public Player(int gold)
        {
            PlayerId = Guid.NewGuid();
            Gold = gold;
            Neutral = false;
        }

        public Player(Guid id, int gold)
        {
            PlayerId = id;
            Gold = gold;
            Neutral = false;
        }

        public Player(int gold, bool neut)
        {
            PlayerId = Guid.NewGuid();
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
        public Player(Guid id, int gold,  String name)
        {
            PlayerId = id;
            Gold = gold;
            Neutral = false;
            Name = name;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}
