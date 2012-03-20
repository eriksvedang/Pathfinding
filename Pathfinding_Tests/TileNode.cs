using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding;
using GameTypes;
namespace Pathfinding_Tests
{
    public class TileNode : IPathNode
    {
        
        #region IPathNode Members
        
        public TileNode(Room pRoom, int pX, int pY, TileType pType )
        {
            localPosition = new IntPoint(pX, pY);
            room = pRoom;
            isStartNode = false;
            isGoalNode = false;
            visited = false;
            distanceToGoal = 0f;
            pathCostHere = 0f;
            baseCost = 1f;
            type = pType;
        }
        public IntPoint localPosition { set; get; }
        public IntPoint worldPosition
        {
            get { return room.worldPosition + localPosition; }
        }
        public TileType type { set; get; }
        public Room room { get; private set; }
        public float pathCostHere { get; set; }
        public float distanceToGoal { get; set; }
        public float baseCost { set; get; }
        public bool isStartNode { get; set; }
        public bool isGoalNode { get; set; }
        public bool visited { get; set; }

        private List<PathLink> _links = new List<PathLink>();
        public List<PathLink> links { get { return _links; } set { _links = value; } }
        private PathLink _previousLink = null;
        public PathLink linkLeadingHere { get { return _previousLink; } set { _previousLink = value; } }
        public PathLink GetLinkTo(IPathNode pNode)
        {
            if (_links != null)
            {
                foreach (PathLink p in _links)
                {
                    if (p.Contains(pNode))
                        return p;
                }
            }
            return null;
        }
		public void AddLink(PathLink pLink) 
        {
            List<PathLink> newLinks = links == null ? new List<PathLink>():new List<PathLink>(links);
            newLinks.Add(pLink);
            links = newLinks.ToArray();
        }
        public void RemoveLink(PathLink pLink)
        {
            List<PathLink> newLinks = links == null ? new List<PathLink>() : new List<PathLink>(links);
            newLinks.Remove(pLink);
            links = newLinks.ToArray();
        }

        #endregion

        #region IPoint Members

        public float DistanceTo(Pathfinding.IPoint pPoint)
        {
            if (pPoint is TileNode)
            {
                TileNode otherNode = pPoint as TileNode;
                return this.worldPosition.DistanceTo(otherNode.worldPosition);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public override int GetHashCode()
        {
            return localPosition.GetHashCode();
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
#if DEBUG
            D.assert(obj is TileNode);
#endif
            TileNode target = obj as TileNode;
            float targetValue = target.pathCostHere + target.distanceToGoal; 
            float thisValue = pathCostHere + distanceToGoal;
            if (targetValue > thisValue)
                return 1;
            else if (targetValue == thisValue)
                return 0;
            else
                return -1;
        }

        #endregion
    }
}
