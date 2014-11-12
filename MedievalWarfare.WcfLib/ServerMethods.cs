using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.WcfLib.Entities;
using System.ServiceModel;

namespace MedievalWarfare.WcfLib
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ServerMethods : IServerMethods
    {
        // Change to dict to easy access to each user
        private static Dictionary<Guid, IClientCallback> callbackList = new Dictionary<Guid, IClientCallback>();

        public ServerMethods()
        {

        }

        public void Join(PlayerInfo info)
        {
            // Subscribe the user to the conversation
            var registeredUser = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            if (!callbackList.ContainsKey(info.Id))
            {
                callbackList.Add(info.Id, registeredUser);
            }
            else
            {
                registeredUser.ActionResult(false);
            }

            registeredUser.ActionResult(true);
        }

        public void Leave(PlayerInfo info)
        {
            throw new NotImplementedException();
        }

        public MapInfo GetGameState()
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }

        public void UpdateMap(MapInfo mapInfo, Command cmd)
        {
            throw new NotImplementedException();
        }

        public string GetData(int value)
        {
            throw new NotImplementedException();
        }
    }
}
