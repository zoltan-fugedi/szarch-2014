using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.TestClient.Utils
{
    public class ServerCallbacks : Proxy.IServerMethodsCallback
    {
        public Map Map { get; set; }

        public bool ServerResult { get; set; }

        public void ActionResult(Command command, bool result, string msg)
        {
        }

        public void StartGame(Game game, bool isYourTurn)
        {
            Map = game.Map;
        }

        public void StartTurn()
        {
           
        }

        public void Update(Command command)
        {
        }

        public void EndGame(bool winner)
        {
        }
    }
}
