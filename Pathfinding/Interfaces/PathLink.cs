using System;
using System.Collections.Generic;

namespace Pathfinding
{
    public class PathLink
    {
        public float distance;
        public IPathNode nodeA;
        public IPathNode nodeB;

        public PathLink(IPathNode pNodeA, IPathNode pNodeB)
        {
            distance = pNodeA.DistanceTo(pNodeB);
            nodeA = pNodeA;
            nodeB = pNodeB;
        }

        public IPathNode GetOtherNode(IPathNode pSelf)
        {
            if (nodeA == pSelf) {
                return nodeB;
            }
            else if (nodeB == pSelf) {
                return nodeA;
            }
            else {
                throw new Exception("Function must be used with a parameter that's contained by the link");
            }
        }

        public int IndexOf(IPathNode item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IPathNode item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IPathNode this[int index] {
            get {
                if (index == 0) {
                    return nodeA;
                }
                
                if (index == 1) {
                    return nodeB;
                }
                
                return null;
            }
            set {
                if (index == 0) {
                    nodeA = value;
                }
                
                if (index == 1) {
                    nodeB = value;
                }
            }
        }

        public void Add(IPathNode item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            nodeA = null;
            nodeB = null;
        }

        public bool Contains(IPathNode item)
        {
            if (nodeA == item || nodeB == item) {
                return true;
            }
            
            return false;
        }

        public int Count {
            get {
                return 2;
            }
        }
		
		/*
        public IEnumerator<IPathNode> GetEnumerator()
        {
            yield return nodeA;
            yield return nodeB;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield return nodeA;
            yield return nodeB;
        }
        */
    }
}
