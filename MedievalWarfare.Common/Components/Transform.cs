using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common.Components
{
    /// <summary>
    /// Describe the Entity position, size, rotation
    /// </summary>
    public class Transform : ComponentBase
    {
        /// <summary>
        /// Gets or sets the x coord
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the  y coord
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the entity size
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Gets or sets the enitity rotation 
        /// </summary>
        public float Rotation { get; set; } // maybe this can be an enum

    }
}
