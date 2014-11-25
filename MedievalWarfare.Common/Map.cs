using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Common
{
    [DataContract(IsReference = true)]
    public class Map
    {
        private const int defaultX = 10;
        private const int defaultY = 10;
        [DataMember]
        public List<Tile> TileList { get; set; }
        [DataMember]
        public List<GameObject> ObjectList { get; set; }
        [DataMember]
        public int MaxX { get; set; }
        [DataMember]
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
            ObjectList = new List<GameObject>();
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
                var newTile = new Tile(i, 0);
                dir = i % 2 == 0 ? Direction.SE : Direction.NE;
                this[i - 1, 0][dir] = newTile;
                dir2 = i % 2 != 0 ? Direction.SW : Direction.NW;
                newTile[dir2] = this[i - 1, 0];

                TileList.Add(newTile);

            }

            for (int j = 1; j < MaxY; j++)
            {
                var newTile = new Tile(0, j);
                this[0, j - 1][Direction.S] = newTile;
                newTile[Direction.N] = this[0, j - 1];

                TileList.Add(newTile);
            }
            for (int i = 1; i < MaxX; i++)
            {
                for (int j = 1; j < MaxY; j++)
                {
                    var newTile = new Tile(i, j);
                    TileList.Add(newTile);
                }
            }
            // Fill up the interior
            for (int i = 1; i < MaxX; i++)
            {
                for (int j = 1; j < MaxY; j++)
                {
                    var tile = this[i, j];
                    if (i >= 1 && j >= 1)
                    {
                        if (j == MaxY - 1)
                        {
                            if (i == MaxX - 1)
                            {
                                if (i % 2 == 0)
                                {
                                    // add 2 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.NW] = this[i - 1, j - 1];
                                    this[i - 1, j - 1][Direction.SE] = tile;
                                }
                                else
                                {
                                    // add 2 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.NW] = this[i - 1, j];
                                    this[i - 1, j][Direction.SE] = tile;
                                }



                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    //add 5 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.NE] = this[i + 1, j - 1];
                                    this[i + 1, j - 1][Direction.SW] = tile;

                                    tile[Direction.NW] = this[i - 1, j - 1];
                                    this[i - 1, j - 1][Direction.SE] = tile;

                                    tile[Direction.SE] = this[i + 1, j];
                                    this[i + 1, j][Direction.NW] = tile;

                                    tile[Direction.SW] = this[i - 1, j];
                                    this[i - 1, j][Direction.NE] = tile;
                                }
                                else
                                {
                                    //add 3 neighbour

                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.NE] = this[i + 1, j];
                                    this[i + 1, j][Direction.SW] = tile;

                                    tile[Direction.NW] = this[i - 1, j];
                                    this[i - 1, j][Direction.SE] = tile;



                                }
                            }
                        }
                        else
                        {
                            if (i == MaxX - 1)
                            {
                                if (i % 2 == 0)
                                {
                                    // add 4 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.S] = this[i, j + 1];
                                    this[i, j + 1][Direction.N] = tile;

                                    tile[Direction.NW] = this[i - 1, j - 1];
                                    this[i - 1, j - 1][Direction.SE] = tile;
                                    tile[Direction.SW] = this[i - 1, j];
                                    this[i - 1, j][Direction.NE] = tile;
                                }
                                else
                                {
                                    // add 4 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.S] = this[i, j + 1];
                                    this[i, j + 1][Direction.N] = tile;

                                    tile[Direction.NW] = this[i - 1, j];
                                    this[i - 1, j][Direction.SE] = tile;
                                    tile[Direction.SW] = this[i - 1, j + 1];
                                    this[i - 1, j + 1][Direction.NE] = tile;
                                }
                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    // add 6 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.S] = this[i, j + 1];
                                    this[i, j + 1][Direction.N] = tile;

                                    tile[Direction.NE] = this[i + 1, j - 1];
                                    this[i + 1, j - 1][Direction.SW] = tile;

                                    tile[Direction.NW] = this[i - 1, j - 1];
                                    this[i - 1, j - 1][Direction.SE] = tile;

                                    tile[Direction.SE] = this[i + 1, j];
                                    this[i + 1, j][Direction.NW] = tile;

                                    tile[Direction.SW] = this[i - 1, j];
                                    this[i - 1, j][Direction.NE] = tile;
                                }
                                else
                                {
                                    // add 6 neighbour
                                    tile[Direction.N] = this[i, j - 1];
                                    this[i, j - 1][Direction.S] = tile;

                                    tile[Direction.S] = this[i, j + 1];
                                    this[i, j + 1][Direction.N] = tile;

                                    tile[Direction.NE] = this[i + 1, j];
                                    this[i + 1, j][Direction.SW] = tile;

                                    tile[Direction.NW] = this[i - 1, j];
                                    this[i - 1, j][Direction.SE] = tile;

                                    tile[Direction.SE] = this[i + 1, j + 1];
                                    this[i + 1, j + 1][Direction.NW] = tile;

                                    tile[Direction.SW] = this[i - 1, j + 1];
                                    this[i - 1, j + 1][Direction.NE] = tile;
                                }




                            }
                        }


                    }
                }
            }
            //AddWater(15, 15, 2);
            var build = new Building();
            this.ObjectList.Add(build);
            this[2, 2].ContentList.Add(build);
        }

        public void AddMountain(int x, int y, int radius)
        {
            List<Tile> temptiles = new List<Tile>();
            temptiles.Add(this[x, y]);
            var nbs = this[x, y].Neighbours;
            for (int i = 0; i < radius; i++)
            {
                List<Tile> temp = new List<Tile>();
                foreach (var tile in temptiles)
                {
                    var neighbours = tile.Neighbours;
                    foreach (var nb in neighbours)
                    {
                        if (!temp.Contains(nb.Value))
                        {
                            temp.Add(nb.Value);
                        }
                    }
                }
                foreach (var tile in temp)
                {
                    if (!temptiles.Contains(tile))
                    {
                        temptiles.Add(tile);
                    }
                }
            }

            foreach (var tile in temptiles)
            {
                tile.Type = TileType.Mountain;
                tile.traversable = false;
            }
        }
        public void AddWater(int x, int y, int radius)
        {
            List<Tile> temptiles = new List<Tile>();
            temptiles.Add(this[x, y]);
            for (int i = 0; i < radius; i++)
            {
                List<Tile> temp = new List<Tile>();
                foreach (var tile in temptiles)
                {
                    var neighbours = tile.Neighbours;
                    foreach (var nb in neighbours)
                    {

                        temp.Add(nb.Value);

                    }
                }
                foreach (var tile in temp)
                {

                    temptiles.Add(tile);

                }
            }

            foreach (var tile in temptiles)
            {
                tile.Type = TileType.Water;
                tile.traversable = false;
            }
        }

        public void AddNewPlayerObjects(int x, int y, Player owner)
        {
            if (y > (MaxY / 2) && x > (MaxX / 2))
            {
                AddBuilding(x, y, owner);
                AddUnit(x - 1, y - 1, owner);
            }
            else
            {
                AddBuilding(x, y, owner);
                AddUnit(x + 1, y + 1, owner);
            }

        }

        public void AddBuilding(int x, int y, Player owner)
        {
            var build = new Building();
            build.Owner = owner;
            this.ObjectList.Add(build);
            this[x, y].ContentList.Add(build);

        }

        public void RemoveBuilding(int x, int y, Player owner)
        {
            var contents = this[x, y].ContentList.Where(c => (c is Building) && (c.Owner.Equals(owner)));
            foreach (var building in contents)
            {
                this.ObjectList.Remove(building);
                this[x, y].ContentList.Remove(building);
            }
        }

        public void AddUnit(int x, int y, Player owner)
        {
            var unit = new Unit(ConstantValues.BaseMovement, ConstantValues.BaseUnitStr);
            unit.Owner = owner;
            var contents = this[x, y].ContentList.Where(c => (c is Unit) && (c.Owner.Equals(owner)));
            if (contents.Count() == 0)
            {
                foreach (var cont in contents)
                {
                    ((Unit)cont).Strength += ConstantValues.BaseUnitStr;
                }

            }
            else
            {
                this.ObjectList.Add(unit);
                this[x, y].ContentList.Add(unit);
            }
        }

        public void RemoveUnit(int x, int y, Player owner)
        {
            var contents = this[x, y].ContentList.Where(c => (c is Unit) && (c.Owner.Equals(owner)));
            foreach (var unit in contents)
            {
                this.ObjectList.Remove(unit);
                this[x, y].ContentList.Remove(unit);
            }

        }

        public void AddTreasure(int x, int y, Player owner)
        {
            var tres = new Treasure(ConstantValues.DefaultTreasure);
            tres.Owner = owner;
            this.ObjectList.Add(tres);
            this[x, y].ContentList.Add(tres);

        }
    }
}
