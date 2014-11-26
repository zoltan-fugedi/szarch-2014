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
        private const int defaultX = 50;
        private const int defaultY = 50;
        [DataMember]
        public Game Game { get; set; }
        [DataMember]
        public List<Tile> TileList { get; set; }
        [DataMember]
        public List<GameObject> ObjectList { get; set; }
        [DataMember]
        public int MaxX { get; set; }
        [DataMember]
        public int MaxY { get; set; }

        #region Accessors

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

        public List<Tile> visibleTiles(Player player)
        {
            List<Tile> temptiles = new List<Tile>();
            List<Tile> ret = new List<Tile>();
            foreach (Tile tile in TileList)
            {
                if (tile.ContentList.Where(go => go.Owner.PlayerId.Equals(player.PlayerId)).Count() > 0)
                {
                    temptiles.Add(tile);
                }
            }

            foreach (var tile in temptiles)
            {
                ret.AddRange(GetTilesInRange(tile, ConstantValues.BaseVisibility));
            }
            return ret;
        }

        /// <summary>
        /// From the given tile get in rage tiles
        /// </summary>
        /// <param name="thisTile"></param>
        /// <param name="rage"></param>
        /// <returns></returns>
        public List<Tile> GetTilesInRange(Tile thisTile, int range)
        {
            List<Tile> temptiles = new List<Tile>();

            temptiles.Add(thisTile);


            for (int i = 0; i < range; i++)
            {
                List<Tile> temp = new List<Tile>();
                foreach (var tile in temptiles)
                {
                    if (!tile.isVisited) 
                    {
                        var neighbours = tile.Neighbours;
                        foreach (var nb in neighbours)
                        {
                            if (!temp.Contains(nb.Value))
                            {
                                temp.Add(nb.Value);
                                
                            }
                        }
                        tile.isVisited = true;
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
                tile.isVisited = false;
            }

            return temptiles;
        }

        #endregion

        /// <summary>
        /// Initialize a new instance of Map object
        /// </summary>
        public Map(Game game)
        {
            TileList = new List<Tile>();
            ObjectList = new List<GameObject>();
            MaxX = defaultX;
            MaxY = defaultY;
            Game = game;
        }

        #region MapGeneration

        /// <summary>
        /// Generate a map from the given parameter
        /// </summary>
        /// <remarks>
        /// Needs an implicit call to generate the map.
        /// </remarks>
        public void GenerateMap()
        {
            // Init boundaries
            TileList.Add(new Tile(0, 0, this));
            for (int i = 1; i < MaxX; i++)
            {
                var newTile = new Tile(i, 0, this);
                TileList.Add(newTile);

            }

            for (int j = 1; j < MaxY; j++)
            {
                var newTile = new Tile(0, j, this);
                TileList.Add(newTile);
            }
            // Fill up the interior
            for (int i = 1; i < MaxX; i++)
            {
                for (int j = 1; j < MaxY; j++)
                {
                    var newTile = new Tile(i, j, this);
                    TileList.Add(newTile);
                }
            }
            var neut = new Player(0, true);
            Game.AddPlayer(neut);



            AddWater(15, 15, 2);
            AddForest(25, 10, 4);
            AddForest(40, 15, 8);
            AddForest(10, 30, 10);
            AddForest(15, 10, 10);
            for (int i = 0; i < 25; i++)
            {
                for (int j = 25; j < 27; j++)
                {
                    AddWater(i, j, 2);
                }
            }
            AddWater(15, 15, 2);

            AddMountain(25, 10, 2);
            AddNeutCamp(5, 5, neut);
        }

        public void AddNeutCamp(int x, int y, Player neut)
        {
            AddUnit(x,y,neut);
            AddTreasure(x,y,neut);
        }

        public void AddMountain(int x, int y, int radius)
        {
            var temptiles = GetTilesInRange(this[x, y], radius);

            foreach (var tile in temptiles)
            {
                tile.Type = TileType.Mountain;
                tile.traversable = false;
            }
        }

        public void AddForest(int x, int y, int radius)
        {
            var temptiles = GetTilesInRange(this[x, y], radius);

            foreach (var tile in temptiles)
            {
                tile.Type = TileType.Forest;
                
            }
        }
        public void AddWater(int x, int y, int radius)
        {
            var temptiles = GetTilesInRange(this[x, y], radius);

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
                AddBuilding(x, y, owner,true);
                AddUnit(x - 1, y - 1, owner);
            }
            else
            {
                AddBuilding(x, y, owner, true);
                AddUnit(x + 1, y + 1, owner);
            }

        }

        #endregion

        #region Add/RemoveObjects

        public bool AddBuilding(int x, int y, Player owner, bool initial = false)
        {
            if (!initial)
            {
                // Checking prerequirement
                if (!this[x, y].ContentList.Any(unit => (unit is Unit) && ((Unit)unit).Owner.PlayerId == owner.PlayerId && ((Unit)unit).Movement >= ConstantValues.MovementCost))
                {
                    return false;
                }
            }

            var build = new Building(this[x, y]);
            build.Owner = owner;
            this.ObjectList.Add(build);
            this[x, y].ContentList.Add(build);
            return true;

        }

        public void RemoveBuilding(int x, int y, Building building)
        {
                this.ObjectList.Remove(building);
                this[x, y].ContentList.Remove(building);
        }


        public bool AddTreasure(int x, int y, Player owner, bool initial = false)
        {
            if (!initial)
            {
                // Checking prerequirement
                if (!this[x, y].ContentList.Any(unit => (unit is Unit) && ((Unit)unit).Owner.PlayerId == owner.PlayerId && ((Unit)unit).Movement >= ConstantValues.MovementCost))
                {
                    return false;
                }
            }

            var tres = new Treasure(ConstantValues.DefaultTreasure, this[x, y], owner);
            tres.Owner = owner;
            this.ObjectList.Add(tres);
            this[x, y].ContentList.Add(tres);
            return true;
        }

        public void AddUnit(int x, int y, Player owner)
        {
            // TODO check there can be train

            var unit = new Unit(ConstantValues.BaseMovement, ConstantValues.BaseUnitStr, this[x, y]);
            unit.Owner = owner;
            var contents = this[x, y].ContentList.Where(c => (c is Unit) && (c.Owner.Equals(owner)));
            if (contents.Count() != 0)
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

        #endregion

        #region Movement

        public bool MoveUnit(Player owner, Unit unit, int destX, int destY)
        {
            var dest = this[destX, destY];
            var start = unit.Tile;
            int movementcost = 0;


            var tilesInRange = GetTilesInRange(unit.Tile, unit.Movement);
            if (!(unit.Owner.PlayerId==owner.PlayerId)) 
            {
                return false; 
            }
            if(!tilesInRange.Contains(dest))
            {
                return false;
            }
            if (!dest.traversable) 
            {
                return false;
            }
            if (dest == unit.Tile)
            {
                return false;
            }

            for (int i = 1; i <= unit.Movement; i++)
            {
                var tiles = GetTilesInRange(unit.Tile, i);
                if (tiles.Contains(dest))
                {
                    movementcost = i;
                    break;
                }
                    
            }
            //if it is not in range
            if (movementcost == 0)
                return false;

            //friendly units at dest
            var friendlyUnits = dest.ContentList.Where(og => og is Unit && og.Owner.PlayerId == owner.PlayerId);
            //enemy Player units at dest
            var enemyUnits = dest.ContentList.Where(og => og is Unit && og.Owner.PlayerId != owner.PlayerId && !og.Owner.Neutral);
            //neutral Player units at dest
            var neutralUnits = dest.ContentList.Where(og => og is Unit && og.Owner.PlayerId != owner.PlayerId && og.Owner.Neutral);
            //treasures at dest
            var treasures = dest.ContentList.Where(og => og is Treasure && og.Owner.PlayerId != owner.PlayerId && og.Owner.Neutral);
            //enemy buildings at dest
            var enemyBuildings = dest.ContentList.Where(og => og is Building && og.Owner.PlayerId != owner.PlayerId && !og.Owner.Neutral);
  

            //Normal move
            if (treasures.Count() == 0 && enemyUnits.Count() == 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() == 0)
            {
                start.ContentList.Remove(unit);
                dest.ContentList.Add(unit);
                unit.Tile = dest;
                unit.Movement -= movementcost;
                return true;
            }

            //Unit merge
            if (treasures.Count() == 0 && enemyUnits.Count() == 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() > 0 && enemyBuildings.Count() == 0)
            {
                start.ContentList.Remove(unit);
                
                var destUnit = friendlyUnits.ToList()[0];
                ((Unit)destUnit).Strength += unit.Strength;
                ((Unit)destUnit).Movement = 0;
                this.ObjectList.Remove(unit);
                return true;
            }

            //destroy enemy building
            if (treasures.Count() == 0 && enemyUnits.Count() == 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() > 0)
            {
                var enemyBuild = enemyBuildings.ToList()[0];

                start.ContentList.Remove(unit);
                dest.ContentList.Add(unit);
                unit.Tile = dest;
                unit.Movement -= movementcost;
                dest.ContentList.Remove(enemyBuild);
                this.ObjectList.Remove(enemyBuild);

                return true;
            }

            //Simple PVP fight
            if (treasures.Count() == 0 && enemyUnits.Count() > 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() == 0)
            {
                Unit enemyunit = (Unit)enemyUnits.ToList()[0];

                start.ContentList.Remove(unit);
                //attacker wins
                if (unit.Strength > enemyunit.Strength) 
                {
                    unit.Strength -= enemyunit.Strength;
                    dest.ContentList.Remove(enemyunit);
                    dest.ContentList.Add(unit);
                    unit.Movement -= movementcost;
                    this.ObjectList.Remove(enemyunit);
                    return true;
                }
                else
                {
                    //defender wins
                    if (unit.Strength < enemyunit.Strength) 
                    {
                        enemyunit.Strength -= unit.Strength;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                    //in case of draw defender wins
                    else
                    {
                        enemyunit.Strength = 1;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                }
            }

            //PVP with castle on defending side
            if (treasures.Count() == 0 && enemyUnits.Count() > 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() > 0)
            {
                Unit enemyunit = (Unit)enemyUnits.ToList()[0];
                var enemyBuild = enemyBuildings.ToList()[0];

                start.ContentList.Remove(unit);

                //attacker wins
                if (unit.Strength > enemyunit.Strength*ConstantValues.CastleDefenseBoost)
                {
                    unit.Strength -= Convert.ToInt32((enemyunit.Strength * ConstantValues.CastleDefenseBoost));
                    dest.ContentList.Remove(enemyunit);
                    dest.ContentList.Remove(enemyBuild);
                    dest.ContentList.Add(unit);
                    unit.Movement -= movementcost;
                    this.ObjectList.Remove(enemyunit);
                    this.ObjectList.Remove(enemyBuild);
                    return true;
                }
                else
                {
                    //defender wins
                    if (unit.Strength < enemyunit.Strength * ConstantValues.CastleDefenseBoost)
                    {
                        enemyunit.Strength -= unit.Strength;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                    else
                    {
                        enemyunit.Strength = 1;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                }
            }

            //PVE 
            if (treasures.Count() > 0 && enemyUnits.Count() == 0 && neutralUnits.Count() > 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() == 0)
            {
                Unit neutUnit = (Unit)neutralUnits.ToList()[0];
                Treasure treasure = (Treasure)treasures.ToList()[0];

                start.ContentList.Remove(unit);

                //player wins
                if (unit.Strength > neutUnit.Strength)
                {
                    unit.Strength -= neutUnit.Strength;
                    dest.ContentList.Remove(neutUnit);
                    dest.ContentList.Remove(treasure);
                    dest.ContentList.Add(unit);
                    unit.Movement -= movementcost;
                    unit.Owner.Gold += treasure.Value;
                    this.ObjectList.Remove(neutUnit);
                    this.ObjectList.Remove(treasure);
                    return true;
                }
                else
                {
                    //neut wins
                    if (unit.Strength < neutUnit.Strength )
                    {
                        neutUnit.Strength -= unit.Strength;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                    else
                    {
                        neutUnit.Strength = 1;
                        this.ObjectList.Remove(unit);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Updates

        public void UpdateMap(Command command)
        {
            // TODO implement this
        }



        #endregion
        
    }
}
