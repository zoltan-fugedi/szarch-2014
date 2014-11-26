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
            Map = new Map();
            Players = new List<Player>();
           
        }

        public void AddPlayer(Guid id) 
        {
            var player = new Player(id, ConstantValues.InitialGold);
            if(Players.Where(c => c.Neutral == false).Count()!=0)
            {
                Map.AddNewPlayerObjects(48, 48, player);
            }
            Players.Add(player); 
        }

        public void AddPlayer(Player p)
        {
            
            Players.Add(p);
        }

        public void AddNeutralPlayer(Guid id)
        {
            if (Players.Where(c => c.Neutral == true).Count() == 0)
            {
                var player = new Player(id, 0, true);
                Players.Add(player);
            }  
        }
        #region queries
        public Player GetPlayer(Guid id) 
        {
            return Players.FirstOrDefault(p => p.PlayerId == id); 
        }


        #endregion
    }
}
