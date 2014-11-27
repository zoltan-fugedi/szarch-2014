using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;
using MedievalWarfare.WcfLib.GameState;

namespace MedievalWarfare.WcfLib.Debug
{
    public class OutputHelper
    {

        private GameStateController gsc;

        public OutputHelper(GameStateController controller)
        {
            gsc = controller;
        }

        public string GetPlayerName(Player p)
        {
            if (p.PlayerId == gsc.PlayerOne.PlayerId)
            {
                return "PlayerOne";
            }
            else
            {
                return "PlayerTwo";
            }
        }

    }
}
