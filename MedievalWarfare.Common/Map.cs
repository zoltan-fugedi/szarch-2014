﻿using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Map
    {
        private const int defaultX = 50;
        private const int defaultY = 50;

        public List<Tile> TileList { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        /// <summary>
        /// Gets the Tile by the given coordinates
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The requested tile</returns>
        public Tile this[int x, int y]
        {
            get
            {
                // Check input
                if (x < 0) { throw new ArgumentOutOfRangeException("The coordinate X can't be negative."); }
                if (y < 0) { throw new ArgumentOutOfRangeException("The coordinate Y can't be negative."); }
                if (x > MaxX) { throw new ArgumentOutOfRangeException(string.Format("The coordinate X can't be greater than the given ({0}) maximum.", MaxX)); }
                if (y > MaxY) { throw new ArgumentOutOfRangeException(string.Format("The coordinate Y can't be greater than the given ({0}) maximum.", MaxY)); }

                // ToDo Find and return the requested tile
                return new Tile();
            }
        }

        /// <summary>
        /// Initialize a new instance of Map object
        /// </summary>
        public Map()
        {
            TileList = new List<Tile>();
            MaxX = defaultX;
            MaxY = defaultY;
        }

        /// <summary>
        /// Generate a map from the given parameter
        /// </summary>
        public void GenerateMap()
        {
            // ToDo do this 
        }
    }
}
