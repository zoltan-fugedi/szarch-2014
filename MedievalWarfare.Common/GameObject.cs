using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common.Utility;
using System.ComponentModel;

namespace MedievalWarfare.Common
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Building))]
    [KnownType(typeof(Unit))]
    [KnownType(typeof(Treasure))]
    public class GameObject : INotifyPropertyChanged
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Player Owner { get; set; }
        [DataMember]
        public Tile Tile { get; set; }

        public string Type
        {
            get
            {
                if (this is Unit)
                {
                    return "Unit";
                }
                if (this is Building)
                {
                    return "Building";
                }
                if (this is Treasure)
                {
                    return "Treasure";
                }
                return "ameObject";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }

    }
}
