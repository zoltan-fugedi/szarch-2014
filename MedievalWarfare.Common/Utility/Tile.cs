using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MedievalWarfare.Common.Utility
{
    public enum Direction
    {
        N,
        NE,
        SE,
        S,
        SW,
        NW
    }

    public enum TileType
    {
        Field,
        Water,
        Mountain,
        Forest
    }

    [DataContract(IsReference = true)]
    public class Tile
    {
        public bool isVisited = false;
        [DataMember]
        public Map Map { get; set; }
        [DataMember]
        public List<GameObject> ContentList { get; set; }
        [DataMember]
        public int X { get; private set; }
        [DataMember]
        public int Y { get; private set; }
        [DataMember]
        public bool traversable = true;
        [DataMember]
        public TileType Type { get; set; }
        public Tile(int x, int y, Map map)
        {
            Map = map;
            Y = y;
            X = x;
            
            ContentList = new List<GameObject>();

        }

        public Dictionary<Direction, Tile> Neighbours
        {
            get
            {
                Dictionary<Direction, Tile> ret = new Dictionary<Direction, Tile>();
                foreach (Direction item in Enum.GetValues(typeof(Direction)))
                {
                    try
                    {
                        var temp = this[item];
                        ret.Add(item, temp);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                return ret;
            }
        }

        public Tile this[Direction d]
        {
            get
            {
                try
                {
                    switch (d)
                    {
                        case Direction.N:
                            return Map[X, Y - 1];
                        case Direction.NE:
                            if (X % 2 == 0)
                                return Map[X + 1, Y - 1];
                            else
                                return Map[X + 1, Y];
                        case Direction.SE:
                            if (X % 2 == 0)
                                return Map[X + 1, Y ];
                            else
                                return Map[X + 1, Y + 1];
                        case Direction.S:
                            return Map[X, Y + 1];
                        case Direction.SW:
                            if (X % 2 == 0)
                                return Map[X - 1, Y];
                            else
                                return Map[X - 1, Y + 1];
                        case Direction.NW:
                            if (X % 2 == 0)
                                return Map[X - 1, Y - 1];
                            else
                                return Map[X - 1, Y];
                        default:
                            throw new ArgumentOutOfRangeException("d");
                    }
                }
                catch (Exception)
                {
                    throw new KeyNotFoundException(string.Format("The required neighbour not existing: {0}", d.ToString()));
                }
            }
        }
    }
}

