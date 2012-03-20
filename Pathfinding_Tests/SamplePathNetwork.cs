using System;
using System.Collections;
using System.Collections.Generic;


using Pathfinding;
namespace Pathfinding_Tests
{
    public class SamplePathNetwork : IPathNetwork<SampleNode>
    {
        Dictionary<int, SampleNode> _nodes = new Dictionary<int, SampleNode>();
        IPathNode _pathStart = null;
        IPathNode _pathGoal = null;

        internal SamplePathNetwork(string data)
        {
            
           string[] values = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine("value count " + values.Length);
           for (int i = 0; i < values.Length; i++ )
           {
               
               SampleNode n = new SampleNode(i % 10, i / 10);
               n.number = Convert.ToInt32(values[i]);
               _nodes.Add((int)n.x + (int)n.y * 1000, n);
           }
           for (int i = 0; i < 100; i++)
           {
               int x = i % 10;
               int y = i / 10;
              // Console.WriteLine("setting links for " + x + ", " + y + ", i " + i);
               SampleNode start = GetNode(x, y);
               SampleNode outputNode;
               if (TryGetNode(x + 1, y, out outputNode))
               {
                   AddNode(start, outputNode);
               }
               if (TryGetNode(x - 1, y, out outputNode))
               {
                   AddNode(start, outputNode);
               }
               if (TryGetNode(x, y + 1, out outputNode))
               {
                   AddNode(start, outputNode);
               }
               if (TryGetNode(x, y - 1, out outputNode))
               {
                   AddNode(start, outputNode);
               }
               
           }


        }
        private void AddNode(SampleNode pA, SampleNode pB )
        {
            PathLink l = pB.GetLinkTo(pA);
            if (l == null)
                l = new PathLink(pA, pB);
            l.distance = 1f;
            pA.links.Add(l);
            pB.links.Add(l);
        }
        internal bool TryGetNode(int pX, int pY, out SampleNode outputNode)
        {
            return _nodes.TryGetValue(pX + pY * 1000, out outputNode);
        }
        public SampleNode GetNode(int pX, int pY)
        {
            return _nodes[pX + pY * 1000];
        }
        public SampleNode GetNode(IPoint pPoint)
        {
            return _nodes[pPoint.GetHashCode()];
        }
        public void SetGoal(IPoint pPosition)
        {
            _pathGoal = _nodes[pPosition.GetHashCode()];
            _pathGoal.isGoalNode = true;
        }

        public void SetStart(IPoint pPosition)
        {
            _pathStart = _nodes[pPosition.GetHashCode()];
            _pathStart.isStartNode = true;
        }

        public void Reset()
        {
            foreach (IPathNode node in _nodes.Values)
            {
                node.isGoalNode = false;
                node.isStartNode = false;
                node.distanceToGoal = 0f;
                node.pathCostHere = 0f;
                node.visited = false;
                node.linkLeadingHere = null;
            }
        }
    }
}
