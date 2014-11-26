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
    public class Game
    {
        [DataMember]
        public Map Map { get; set; }

        [DataMember]
        public List<Player> Players { get; set; }

        public Game()
        {
            Map = new Map(this);
            Players = new List<Player>();
           
        }
        public void AddPlayer(Player p)
        {
            
            Players.Add(p);
        }


        #region queries
        public Player GetPlayer(Guid id) 
        {
            return Players.FirstOrDefault(p => p.PlayerId == id); 
        }


        #endregion
    }
}
