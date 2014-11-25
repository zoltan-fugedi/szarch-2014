using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Game
    {
        public Map Map { get; set; }
        public List<Player> Players { get; set; }

        public Game()
        {
            Map = new Map();
            Players = new List<Player>();
            Map.GenerateMap();
        }

        public void addPlayer(Guid id) 
        {
            var player = new Player(id, ConstantValues.InitialGold);
            if(Players.Where(c => c.Neutral == false).Count()!=0)
            {
                Map.AddNewPlayerObjects(48, 48, player);
            }
            Players.Add(player); 
        }

        public void addNeutralPlayer(Guid id)
        {
            if (Players.Where(c => c.Neutral == true).Count() == 0)
            {
                var player = new Player(id, 0, true);
                Players.Add(player);
            }  
        }
    }
}
