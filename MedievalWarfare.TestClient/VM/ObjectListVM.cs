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
        private bool filter;
        public bool Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged("UnitList");
            }
        }

        public Guid Id { get; set; }

        public ObjectListVM(Guid playerId)
        {
            this.Id = playerId;
            filter = false;
            objectList = new ObservableCollection<GameObject>();
        }

        public ObservableCollection<GameObject> UnitList
        {
            get
            {
                if (Filter)
                {
                    return new ObservableCollection<GameObject>(objectList.Where(o => o.Owner.PlayerId == Id));
                }

                return objectList;
            }

        }


    }
}
