using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract(IsReference = true)]
    public class Building : GameObject
    {
        int population;
        
        [DataMember]
        public int Population
        {
            get { return population; }

            set
            {
                population = value;
                OnPropertyChanged("Population");
            }
        }
        public Building(Tile tile)
        {
            Id = Guid.NewGuid();
            Population = 0;
            Tile = tile;
        }

        public Building()
        {
            Id = Guid.NewGuid();
            Population = 0;
        }
    }
}
