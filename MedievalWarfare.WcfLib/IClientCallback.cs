using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.WcfLib.Entities;

namespace MedievalWarfare.WcfLib
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void ActionResult(bool result);

        [OperationContract(IsOneWay = true)]
        void StartTurn(MapInfo mapInfo);

        [OperationContract(IsOneWay = true)]
        void Update(Command cmd);


    }
}
