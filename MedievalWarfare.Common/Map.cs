using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            int x = 0;
            int y = 0;
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\Names.txt");
            var rows = File.ReadAllText(@"Resources\map.csv").Split('\n');
            foreach (var row in rows)
            {
                x = 0;
                foreach (var cell in row.Split(';')) 
                {
                    switch (cell)
                    {
                        case "0":
                            AddForest(x, y, 0);
                            break;
                        case "1":
                            break;
                        case "2":
                            AddWater(x, y, 0);
                            break;
                        case "3":
                            AddMountain(x, y, 0);
                            break;
                        case "4":
                            AddNeutCamp(x, y, neut);
                            break;
                        case "5" :
                            AddForest(x, y, 0);
                            AddNeutCamp(x, y, neut);
                            break;
                        default:
                            break;
                    }
                    x++;
                }
                y++;
            }

        }

        public void AddNeutCamp(int x, int y, Player neut)
        {
            AddUnit(neut,null, x, y, true);
            AddTreasure(x, y, neut);
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
                AddBuilding(owner, null,  x, y,  true);
                AddUnit(owner, null, x - 1, y - 1, true);
            }
            else
            {
                AddBuilding(owner, null, x, y, true);
                AddUnit(owner, null, x + 1, y + 1, true);
            }

        }

        #endregion

        #region Add/RemoveObjects

        public bool AddBuilding(Player owner, Building building, int x, int y, bool initial = false)
        {
            Guid buildingId;
            if (!initial)
            {
                // Checking prerequirement
                if (!this[x, y].ContentList.Any(unit => (unit is Unit) && ((Unit)unit).Owner.PlayerId == owner.PlayerId
                                                                        && ((Unit)unit).Movement >= ConstantValues.MovementCost
                                                                        && ((Unit)unit).Owner.Gold >= ConstantValues.BuildingCost)) 
                {
                    return false;
                }
                ((Unit)this[x, y].ContentList.Single(unit => (unit is Unit) && ((Unit)unit).Owner.PlayerId == owner.PlayerId)).Movement -= ConstantValues.MovementCost;
                ((Unit)this[x, y].ContentList.Single(unit => (unit is Unit) && ((Unit)unit).Owner.PlayerId == owner.PlayerId)).Owner.Gold -= ConstantValues.BuildingCost;
                buildingId = building.Id;
            }
            else
            {
                buildingId = Guid.NewGuid();
            }

            var build = new Building(this[x, y]) { Id = buildingId };
            build.Owner = owner;
            build.Tile = this[x, y];
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
            tres.Tile = this[x, y];
            this.ObjectList.Add(tres);
            this[x, y].ContentList.Add(tres);
            return true;
        }

        public bool AddUnit(Player owner, Unit u, int x, int y, bool initial = false)
        {
            var contents = this[x, y].ContentList.Where(c => (c is Unit) && (c.Owner.Equals(owner)));
            Guid unitId;
            if (!initial)
            {
                // Checking prerequirement
                if (!this[x, y].ContentList.Any(building => (building is Building) && ((Building)building).Owner.PlayerId == owner.PlayerId
                                                                        && ((Building)building).Population >= ConstantValues.PopCost
                                                                        && ((Building)building).Owner.Gold >= ConstantValues.UnitCost))
                {
                    return false;
                }
                //remove cost
                ((Building)this[x, y].ContentList.Single(building => (building is Building) && ((Building)building).Owner.PlayerId == owner.PlayerId)).Owner.Gold -= ConstantValues.UnitCost;
                unitId = u.Id;

                //if neutral
                if (owner.Neutral)
                {
                    var unit = new Unit(ConstantValues.BaseMovement, ConstantValues.BaseNeutStr, this[x, y]) { Id = unitId };
                    unit.Owner = owner;
                    unit.Tile = this[x, y];
                    this.ObjectList.Add(unit);
                    this[x, y].ContentList.Add(unit);
                }
                //if not
                else
                {
                    if (contents.Count() != 0)
                    {
                        foreach (var cont in contents)
                        {
                            ((Unit)cont).Strength += ConstantValues.BaseUnitStr;
                        }

                    }
                    else
                    {
                        var unit = new Unit(0, ConstantValues.BaseUnitStr, this[x, y]) { Id = unitId };
                        unit.Owner = owner;
                        unit.Tile = this[x, y];
                        this.ObjectList.Add(unit);
                        this[x, y].ContentList.Add(unit);
                    }
                }
            }
            else
            {
                unitId = Guid.NewGuid();
                //if neutral
                if (owner.Neutral)
                {
                    var unit = new Unit(ConstantValues.BaseMovement, ConstantValues.BaseNeutStr, this[x, y]) { Id = unitId };
                    unit.Owner = owner;
                    unit.Tile = this[x, y];
                    this.ObjectList.Add(unit);
                    this[x, y].ContentList.Add(unit);
                }
                //if not
                else
                {
                    if (contents.Count() != 0)
                    {
                        foreach (var cont in contents)
                        {
                            ((Unit)cont).Strength += ConstantValues.BaseUnitStr;
                        }

                    }
                    else
                    {
                        var unit = new Unit(ConstantValues.BaseMovement, ConstantValues.BaseUnitStr, this[x, y]) { Id = unitId };
                        unit.Owner = owner;
                        unit.Tile = this[x, y];
                        this.ObjectList.Add(unit);
                        this[x, y].ContentList.Add(unit);
                    }
                }
            }
            return true;
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
            var start = this[unit.Tile.X, unit.Tile.Y];
            var unitowner = this.Game.GetPlayer(owner.PlayerId);
            Unit unittomove = (Unit)start.ContentList.Single(og => og is Unit && og.Id == unit.Id);
            int movementcost = 0;


            var tilesInRange = GetTilesInRange(start, unittomove.Movement);
            if (!(unittomove.Owner.PlayerId == owner.PlayerId))
            {
                return false;
            }
            if (!tilesInRange.Contains(dest))
            {
                return false;
            }
            if (!dest.traversable)
            {
                return false;
            }
            if (dest == unittomove.Tile)
            {
                return false;
            }

            for (int i = 1; i <= unittomove.Movement; i++)
            {
                var tiles = GetTilesInRange(start, i);
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

                //start.ContentList.Remove(start.ContentList.Single(og => og is Unit && og.Id == unittomove.Id));
                start.ContentList.Remove(unittomove);
                dest.ContentList.Add(unittomove);
                unittomove.Tile = dest;
                unittomove.Movement -= movementcost;
                return true;
            }

            //Unit merge
            if (treasures.Count() == 0 && enemyUnits.Count() == 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() > 0 && enemyBuildings.Count() == 0)
            {
                start.ContentList.Remove(unittomove);

                var destUnit = friendlyUnits.ToList()[0];
                ((Unit)destUnit).Strength += unittomove.Strength;
                ((Unit)destUnit).Movement = 0;
                this.ObjectList.Remove(unittomove);
                return true;
            }

            //destroy enemy building
            if (treasures.Count() == 0 && enemyUnits.Count() == 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() > 0)
            {
                var enemyBuild = enemyBuildings.ToList()[0];

                start.ContentList.Remove(unittomove);
                dest.ContentList.Add(unittomove);
                unittomove.Tile = dest;
                unittomove.Movement -= movementcost;
                dest.ContentList.Remove(enemyBuild);
                this.ObjectList.Remove(enemyBuild);

                return true;
            }

            //Simple PVP fight
            if (treasures.Count() == 0 && enemyUnits.Count() > 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() == 0)
            {
                Unit enemyunit = (Unit)enemyUnits.ToList()[0];

                start.ContentList.Remove(unittomove);
                //attacker wins
                if (unittomove.Strength > enemyunit.Strength)
                {
                    unittomove.Strength -= enemyunit.Strength;
                    dest.ContentList.Remove(enemyunit);
                    dest.ContentList.Add(unittomove);
                    unittomove.Movement -= movementcost;
                    unittomove.Tile = dest;
                    this.ObjectList.Remove(enemyunit);
                    return true;
                }
                else
                {
                    //defender wins
                    if (unittomove.Strength < enemyunit.Strength)
                    {
                        enemyunit.Strength -= unittomove.Strength;
                        this.ObjectList.Remove(unittomove);
                        return true;
                    }
                    //in case of draw defender wins
                    else
                    {
                        enemyunit.Strength = 1;
                        this.ObjectList.Remove(unittomove);
                        return true;
                    }
                }
            }

            //PVP with castle on defending side
            if (treasures.Count() == 0 && enemyUnits.Count() > 0 && neutralUnits.Count() == 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() > 0)
            {
                Unit enemyunit = (Unit)enemyUnits.ToList()[0];
                var enemyBuild = enemyBuildings.ToList()[0];

                start.ContentList.Remove(unittomove);

                //attacker wins
                if (unittomove.Strength > enemyunit.Strength * ConstantValues.CastleDefenseBoost)
                {
                    unittomove.Strength -= Convert.ToInt32((enemyunit.Strength * ConstantValues.CastleDefenseBoost));
                    dest.ContentList.Remove(enemyunit);
                    dest.ContentList.Remove(enemyBuild);
                    dest.ContentList.Add(unittomove);
                    unittomove.Movement -= movementcost;
                    unittomove.Tile = dest;
                    this.ObjectList.Remove(enemyunit);
                    this.ObjectList.Remove(enemyBuild);
                    return true;
                }
                else
                {
                    //defender wins
                    if (unittomove.Strength < enemyunit.Strength * ConstantValues.CastleDefenseBoost)
                    {
                        enemyunit.Strength -= unit.Strength;
                        this.ObjectList.Remove(unittomove);
                        return true;
                    }
                    else
                    {
                        enemyunit.Strength = 1;
                        this.ObjectList.Remove(unittomove);
                        return true;
                    }
                }
            }

            //PVE 
            if (treasures.Count() > 0 && enemyUnits.Count() == 0 && neutralUnits.Count() > 0 && friendlyUnits.Count() == 0 && enemyBuildings.Count() == 0)
            {
                Unit neutUnit = (Unit)neutralUnits.ToList()[0];
                Treasure treasure = (Treasure)treasures.ToList()[0];

                start.ContentList.Remove(unittomove);

                //player wins
                if (unittomove.Strength > neutUnit.Strength)
                {
                    unittomove.Strength -= neutUnit.Strength;
                    dest.ContentList.Remove(neutUnit);
                    dest.ContentList.Remove(treasure);
                    dest.ContentList.Add(unittomove);
                    unittomove.Movement -= movementcost;
                    unittomove.Tile = dest;
                    unitowner.Gold += treasure.Value;
                    this.ObjectList.Remove(neutUnit);
                    this.ObjectList.Remove(treasure);
                    return true;
                }
                else
                {
                    //neut wins
                    if (unittomove.Strength < neutUnit.Strength)
                    {
                        neutUnit.Strength -= unittomove.Strength;
                        this.ObjectList.Remove(unittomove);
                        return true;
                    }
                    else
                    {
                        neutUnit.Strength = 1;
                        this.ObjectList.Remove(unittomove);
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
