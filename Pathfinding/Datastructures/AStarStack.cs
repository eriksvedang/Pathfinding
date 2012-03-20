using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding;
namespace Pathfinding.Datastructures
{
    public class AStarStack
    {
        Dictionary<long, IPathNode> _nodes = new Dictionary<long, IPathNode>();
        public void Push(IPathNode pNode)
        {
            _nodes[pNode.GetUniqueID()] = pNode;
        }
        public IPathNode Pop()
        { 
            IPathNode result = null;
            foreach (IPathNode p in _nodes.Values)
            {
                if (result == null || p.CompareTo(result) == 1)
                    result = p; //p has a shorter distance than result
            }
            if (result == null)
            {
                return null;
            }
            else
            {
                _nodes.Remove(result.GetUniqueID());
                return result;
            }

        }
        public int Count
        {
            get { return _nodes.Values.Count; }
        }
        
    }
}
