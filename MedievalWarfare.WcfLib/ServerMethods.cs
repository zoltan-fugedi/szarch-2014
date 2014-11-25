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
        private ConcurrentDictionary<Guid, IClientCallback> callbackList;

        private Map currentMap;

        public ServerMethods()
        {
            callbackList = new ConcurrentDictionary<Guid, IClientCallback>();
            currentMap = new Map();
            currentMap.GenerateMap();
        }

        public void Join(Player info)
        {
            // Subscribe the user to the conversation
            var registeredUser = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            if (callbackList.TryAdd(info.PlayerId, registeredUser))
            {
                registeredUser.ActionResult(true);
            }
            else
            {
                registeredUser.ActionResult(false);
            }
        }

        public void Leave(Player info)
        {
            IClientCallback callBack;
            if (callbackList.TryRemove(info.PlayerId, out callBack))
            {
                callBack.ActionResult(true);
            }
            else
            {
                callBack.ActionResult(false);
            }

            foreach (var clientCallback in callbackList)
            {
                clientCallback.Value.EndGame(true);
            }
        }

        public Map GetGameState()
        {
            return currentMap;
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }

        public void UpdateMap(Command command)
        {
            throw new NotImplementedException();
        }

    }
}
