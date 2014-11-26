using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;

namespace MedievalWarfare.WcfLib
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void ActionResult(Command command, bool result, string msg = null);

        [OperationContract(IsOneWay = true)]
        void StartTurn();

        [OperationContract(IsOneWay = true)]
        void Update(Command command);

        [OperationContract(IsOneWay = true)]
        void EndGame(bool winner);


    }
}
