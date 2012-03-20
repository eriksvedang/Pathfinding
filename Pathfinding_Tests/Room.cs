using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding;
using GameTypes;
namespace Pathfinding_Tests
{
    public enum TileType
    { 
        NOT_SET,
        WALL,
        FLOOR,
        DOOR,
    }
    public class Room
    {
        
        public string name;
        Dictionary<string, Door> _doorsByName = new Dictionary<string, Door>();
        internal Dictionary<int, TileNode> _tiles = new Dictionary<int, TileNode>();

        public Room(string pName)
        {
            name = pName;
            worldPosition = IntPoint.Zero;
        }
        
        internal void SetTile(TileNode tileNode)
        {
            _tiles.Add(tileNode.GetHashCode(), tileNode);
            TileNode start = tileNode;
            int x = start.localPosition.x;
            int y = start.localPosition.y;
            TileNode outputNode;
            if (_tiles.TryGetValue(new IntPoint( x + 1, y).GetHashCode(), out outputNode))
            {
                ConnectNodes(start, outputNode);
            }
            if (_tiles.TryGetValue(new IntPoint( x - 1, y).GetHashCode(), out outputNode))
            {
                ConnectNodes(start, outputNode);
            }
            if (_tiles.TryGetValue(new IntPoint( x, y + 1).GetHashCode(), out outputNode))
            {
                ConnectNodes(start, outputNode);
            }
            if (_tiles.TryGetValue(new IntPoint(x, y - 1).GetHashCode(), out outputNode))
            {
                ConnectNodes(start, outputNode);
            }
        }
        public TileType GetTileType(IntPoint pPoint)
        {
            return GetTileType(pPoint.x, pPoint.y);
        }
        public TileType GetTileType(int x, int y)
        {
           // Console.WriteLine("x" + x + ", y" + y);
            TileNode t = null;
            if (!_tiles.TryGetValue(BitCruncher.PackTwo(x, y), out t))
                return TileType.NOT_SET;
            return t.type;
        }
        public TileNode GetTile(IntPoint pPoint)
        {
            return GetTile(pPoint.x, pPoint.y);
        }
        public TileNode GetTile(int x, int y)
        {
            // Console.WriteLine("x" + x + ", y" + y);
            TileNode t = null;
            _tiles.TryGetValue(BitCruncher.PackTwo(x, y), out t);
            return t;
        }


        public Door GetDoor(string pName)
        {
            Door result = null;
            _doorsByName.TryGetValue(pName, out result);
            return result;
        }

        internal void AddDoor(Door door)
        {
            SetTile(door);
            _doorsByName.Add(door.name, door);
        }
        public IntPoint worldPosition { set; get; }

        public IntPoint WorldToLocalPoint(IntPoint pSource)
        {
            return pSource - worldPosition;
        }

        private void ConnectNodes(TileNode pA, TileNode pB)
        {
            if ((pA.type == TileType.FLOOR || pA.type == TileType.DOOR ) &&
                (pB.type == TileType.FLOOR || pB.type == TileType.DOOR ))
            {                 
                PathLink l = pB.GetLinkTo(pA);
                if (l == null)
                    l = new PathLink(pA, pB);
                l.distance = 1f;
                pA.links.Add(l);
                pB.links.Add(l);
            }
        }

        

    }
}
