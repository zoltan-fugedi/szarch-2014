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

        public void ActionResult(Command command, bool result, string msg)
        {
            var a = 10;

        }

        public void StartGame(Game game, bool isYourTurn)
        {
            var a = 10;
            Map = game.Map;
        }

        public void StartTurn()
        {
            var a = 10;

        }

        public void Update(Command command)
        {

            var a = 10;

        }

        public void EndGame(bool winner)
        {
            var a = 10;
        }
    }
}
