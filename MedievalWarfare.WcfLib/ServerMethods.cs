using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

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

        public void Join(Player info)
        {
            // Subscribe the user to the conversation
            var registeredUser = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            if (!callbackList.ContainsKey(info.PlayerId))
            {
                callbackList.Add(info.PlayerId, registeredUser);
            }
            else
            {
                registeredUser.ActionResult(false);
            }

            registeredUser.ActionResult(true);
        }

        public void Leave(Player info)
        {
            throw new NotImplementedException();
        }

        public Map GetGameState()
        {
            throw new NotImplementedException();
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
