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

        public void EndPlayerTurn(Player p) 
        {
            var playerobjects = Map.ObjectList.Where(go => go.Owner.PlayerId == p.PlayerId);
            var playerbuildings = playerobjects.Where(go => go is Building);
            var playerunits = playerobjects.Where(go => go is Unit);

            GetPlayer(p.PlayerId).Gold += playerbuildings.Count() * ConstantValues.GoldGainPerBuilding;
            
            foreach (Building build in playerbuildings)
            {
                build.Population += ConstantValues.PopGrowth;
            }

            foreach (Unit unit in playerunits)
            {
                unit.Movement = ConstantValues.BaseMovement;
            }

        }

        #endregion
    }
}
