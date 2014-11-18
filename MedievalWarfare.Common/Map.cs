using MedievalWarfare.Common.Entities;
using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    public class Map
    {

        // ToDo Read This -> https://hexgridutilities.codeplex.com

        private const int defaultX = 50;
        private const int defaultY = 50;

        public List<Tile> TileList { get; set; }
        public List<EntityBase> ObjectList { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        /// <summary>
        /// Gets the Tile by the given coordinates
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <remarks>
        /// The (0,0) is the left upper tile
        /// </remarks>
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

                return TileList.Single(t => t.X == x && t.Y == y);
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
        /// <remarks>
        /// Needs an implicit call to generate the map.
        /// </remarks>
        public void GenerateMap()
        {
            // Init boundaries
            TileList.Add(new Tile(0, 0));

            Direction dir;
            Direction dir2;
            for (int i = 1; i < MaxX; i++)
            {
                var newTile = new Tile(0, i);
                dir = i % 2 == 0 ? Direction.SE : Direction.NE;
                this[0, i - 1][dir] = newTile;
                dir2 = i % 2 != 0 ? Direction.SW : Direction.NW;
                newTile[dir2] = this[0, i - 1];

                TileList.Add(newTile);

            }

            for (int j = 1; j < MaxY; j++)
            {
                var newTile = new Tile(j, 0);
                this[j - 1, 0][Direction.S] = newTile;
                newTile[Direction.N] = this[j - 1, 0];

                TileList.Add(newTile);
            }

            // Fill up the interior
            for (int i = 1; i < MaxX-1; i++)
            {
                for (int j = 1; j < MaxY-1; j++)
                {
                    var newTile = new Tile(i, j);
                    if (j % 2 != 0)
                    {
                        // ps 4 szomszéd
                        newTile[Direction.SW] = this[i, j - 1];
                        newTile[Direction.NW] = this[i - 1, j - 1];
                        newTile[Direction.N] = this[i - 1, j];
                        newTile[Direction.NE] = this[i - 1, j + 1];

                        this[i, j - 1][Direction.NE] = newTile;
                        this[i - 1, j - 1][Direction.SE] = newTile;
                        this[i - 1, j][Direction.S] = newTile;
                        this[i - 1, j + 1][Direction.SW] = newTile;
                    }
                    else
                    {
                        // prt 2 szomszéd
                        newTile[Direction.N] = this[i - 1, j];
                        newTile[Direction.NW] = this[i, j - 1];

                        this[i - 1, j][Direction.S] = newTile;
                        this[i, j - 1][Direction.SE] = newTile;
                    }

                    TileList.Add(newTile);
                }
            }
        }
    }
}
