using System;
using System.Collections.Generic;


using Pathfinding;
namespace Pathfinding_Tests
{
    public class SampleNode : IPathNode
    {
        public int number = 0;
        public float x;
        public float y;
        #region IPathNode Members
        
        public SampleNode(int pX, int pY)
        {
            x = pX;
            y = pY;
        }
        float _costHere;
        public float pathCostHere{ get{ return _costHere; } set{ _costHere = value;}}
        float _distanceToGoal;
        public float distanceToGoal{ get{return _distanceToGoal;} set{_distanceToGoal = value;}}
        public float baseCost { get { return number * 10; } }
        private bool _isStartNode = false;
        public bool isStartNode{get{return _isStartNode;} set{_isStartNode = value;}}
        private bool _isGoalNode = false;
        public bool isGoalNode{ get{return _isGoalNode;}set{_isGoalNode = value;}}
        private bool _visited = false;
        public bool visited { get{return _visited;}set{_visited = value;}}

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

        #endregion

        #region IPoint Members

        public float DistanceTo(IPoint pPoint)
        {
            if (pPoint is SampleNode)
            {
                SampleNode otherNode = pPoint as SampleNode;
                return Convert.ToSingle(Math.Sqrt(Convert.ToDouble(this.x * otherNode.x + this.y * otherNode.y)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public override int GetHashCode()
        {
            return (int)x + (int)(y * 1000f);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            SampleNode target = obj as SampleNode;
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
