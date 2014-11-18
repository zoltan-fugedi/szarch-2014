using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common.Entities;

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

    public class Tile
    {
        public Dictionary<Direction, Tile> Neighbours { get; set; }
        public List<EntityBase> ContentList { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(int x, int y)
        {
            Y = y;
            X = x;
            Neighbours = new Dictionary<Direction, Tile>();
            ContentList = new List<EntityBase>();

        }

        Tile this[Direction d]
        {
            get
            {
                if (!Neighbours.ContainsKey(d)) { throw new KeyNotFoundException(string.Format("The required neighbour not existing: {0}", d.ToString())); }
                switch (d)
                {
                    case Direction.N:
                        return Neighbours[Direction.N];
                    case Direction.NE:
                        return Neighbours[Direction.NE];
                      case Direction.SE:
                        return Neighbours[Direction.SE];
                    case Direction.S:
                        return Neighbours[Direction.S];
                    case Direction.SW:
                        return Neighbours[Direction.SW];
                    case Direction.NW:
                        return Neighbours[Direction.NW];
                    default:
                        throw new ArgumentOutOfRangeException("d");
                }
            }

            set
            {
                if (Neighbours.ContainsKey(d)) { throw new Exception(string.Format("The required neighbour is already set: {0}", d.ToString())); }
                switch (d)
                {
                    case Direction.N:
                        Neighbours[Direction.N] = value;
                        break;
                    case Direction.NE:
                        Neighbours[Direction.NE] = value;
                        break;
                    case Direction.SE:
                        Neighbours[Direction.SE] = value;
                        break;
                    case Direction.S:
                        Neighbours[Direction.S] = value;
                        break;
                    case Direction.SW:
                        Neighbours[Direction.SW] = value;
                        break;
                    case Direction.NW:
                        Neighbours[Direction.N] = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("d");
                }
            }
        }
    }
}
