using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MedievalWarfare.WcfLib.Entities;

namespace MedievalWarfare.WcfLib
{
    [ServiceContract(CallbackContract = typeof (IClientCallback) )]
    public interface IServerMethods
    {

        [OperationContract]
        void Join(PlayerInfo info);
       
        [OperationContract]
        void Leave(PlayerInfo info);
        
        [OperationContract]
        MapInfo GetGameState();
        
        [OperationContract]
        void EndTurn();
        
        [OperationContract]
        void UpdateMap(MapInfo mapInfo, Command cmd);


        [OperationContract]
        string GetData(int value);

    }
}
