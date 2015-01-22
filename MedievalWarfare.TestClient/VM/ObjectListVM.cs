using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;

namespace MedievalWarfare.TestClient.VM
{
    public class ObjectListVM : VmBase
    {
        public ObservableCollection<GameObject> objectList;
        

        public Guid Id { get; set; }

        public ObjectListVM(Guid playerId)
        {
            this.Id = playerId;
            objectList = new ObservableCollection<GameObject>();
        }

        public ObservableCollection<GameObject> UnitList
        {
            get { return objectList ; }

        }


    }
}
