using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.WcfLib
{
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IServerMethods
    {

        [OperationContract()]
        void Join(Player info);

        [OperationContract]
        void Leave(Player info);

        [OperationContract]
        Game GetGameState();

        [OperationContract]
        void EndTurn(Player info);

        [OperationContract]
        void UpdateMap(Command command);


    }
}
