using System;
using System.Collections;
using System.Collections.Generic;


using Pathfinding;

namespace Pathfinding_Tests
{
    public class SamplePathNetwork : IPathNetwork<SampleNode>
    {
        static int OFFSET = 10000;

        Dictionary<int, SampleNode> _nodes = new Dictionary<int, SampleNode>(100000);
        IPathNode _pathStart = null;
        IPathNode _pathGoal = null;

        internal SamplePathNetwork(string data) {
            Setup(data, 10, 10);
        }

        internal SamplePathNetwork(string data, int width, int height)
        {
            Setup(data, width, height);
        }

        static int allocCount = 0;

        static void IncAlloc() {
            allocCount++;
            Console.WriteLine("Alloc count: " + allocCount);
        }

        internal void Setup(string data, int width, int height) {

            string[] values = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            //Console.WriteLine("value count " + values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                SampleNode n = new SampleNode(i % height, i / width);
                n.number = Convert.ToInt32(values[i]);
                _nodes.Add((int)n.localPoint.x + (int)n.localPoint.y * OFFSET, n);
            }

            int size = (width * height);
            for (int i = 0; i < size; i++) {
                int x = i % height;
                int y = i / width;
                // Console.WriteLine("setting links for " + x + ", " + y + ", i " + i);
                SampleNode start = GetNode(x, y);
                SampleNode outputNode;
                
                if (TryGetNode(x + 1, y, out outputNode)) {
                    AddNode(start, outputNode);
                }
                
                if (TryGetNode(x - 1, y, out outputNode)) {
                    AddNode(start, outputNode);
                }
                
                if (TryGetNode(x, y + 1, out outputNode)) {
                    AddNode(start, outputNode);
                }
                
                if (TryGetNode(x, y - 1, out outputNode)) {
                    AddNode(start, outputNode);
                }
                
            }
            
            
        }

        private void AddNode(SampleNode pA, SampleNode pB)
        {
            PathLink l = pB.GetLinkTo(pA);
            
            if (l == null) {
                l = new PathLink(pA, pB);
            }
            
            l.distance = 1f;
            pA.links.Add(l);
            pB.links.Add(l);
        }

        internal bool TryGetNode(int pX, int pY, out SampleNode outputNode)
        {
            return _nodes.TryGetValue(pX + pY * OFFSET, out outputNode);
        }

        public SampleNode GetNode(int pX, int pY)
        {
            return _nodes[pX + pY * OFFSET];
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
            foreach (IPathNode node in _nodes.Values) {
                node.isGoalNode = false;
                node.isStartNode = false;
                node.distanceToGoal = 0f;
                node.pathCostHere = 0f;
                node.visited = false;
                node.linkLeadingHere = null;
            }
            Console.WriteLine("Reset " + _nodes.Values.Count + " nodes");
        }
    }
}
