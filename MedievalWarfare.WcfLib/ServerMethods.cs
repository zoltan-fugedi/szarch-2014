using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.WcfLib
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMethods : IServerMethods
    {
        // Change to dict to easy access to each user

        private IClientCallback client;
        private Boolean connected = false;
        private DBManager dbManager;


        public ServerMethods()
        {
            dbManager = new DBManager();
            dbManager.Flush();
            
        }

        public void Join(Player info)
        {
            if (!connected) {
                var registeredUser = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                client = registeredUser;

                var game = new Game();

                game.Map.GenerateMap();
                game.AddPlayer(new Player(info.PlayerId, info.Gold ,info.Name));
                game.Map.AddNewPlayerObjects(2, 2, info);

                game.AddPlayer(new Player(info.Gold));
                game.Map.AddNewPlayerObjects(48, 48, info);

                dbManager.AddPlayer(info);

                client.StartGame(game, true);
                connected = true;
            }
            

        }

        public void Leave(Player info)
        {
            if (connected) 
            {
                dbManager.PrintAllCommands();
                dbManager.Flush();
                connected = false;
            }
            
        }

        public Game GetGameState()
        {
            
            return new Game();
        }

        public void EndTurn(Player info)
        {
            if (connected) 
            {
                client.StartTurn();
            } 
        }

        public void UpdateMap(Command command)
        {
            if (connected) 
            {
                dbManager.AddCommand(command);
                client.ActionResult(command, true);
            }
        }



    }
}
