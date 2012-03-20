using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding;
using GameTypes;
namespace Pathfinding_Tests
{
    class MultiRoomNetwork : IPathNetwork<TileNode>
    {

        TileNode[] nodes = null;

        public MultiRoomNetwork(IList<Room> pRooms)
        {
            List<TileNode> tNodes = new List<TileNode>();
            foreach (Room r in pRooms)
            {
                tNodes.AddRange(r._tiles.Values);
            }
            nodes = tNodes.ToArray();
            
        }
        public void Reset()
        {
            foreach (TileNode t in nodes)
            {
                t.isGoalNode = false;
                t.isStartNode = false;
                t.distanceToGoal = 0f;
                t.pathCostHere = 0f;
                t.visited = false;
                t.linkLeadingHere = null;
            }
        }

    }
}
