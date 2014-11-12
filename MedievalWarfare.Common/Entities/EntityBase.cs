using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common.Components;

namespace MedievalWarfare.Common.Entities
{
    public class EntityBase
    {
        private List<ComponentBase> components;

        public EntityBase()
        {
            this.components = new List<ComponentBase>();
        }

        public void RegisterComponent(ComponentBase component)
        {
            // check component constrait like 1 type only once and if one requre other its added first
            components.Add(component);
        }

        public void RemoveComponent(ComponentBase component)
        {
            components.Remove(component);
        }

        public void RemoveComponent(Guid id)
        {
            components.RemoveAll(x => x.Id == id);
        }
    }
}
