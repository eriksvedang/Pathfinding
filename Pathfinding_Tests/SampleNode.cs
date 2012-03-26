using System;
using System.Collections.Generic;
using Pathfinding;
using GameTypes;

namespace Pathfinding_Tests
{
    public class SampleNode : IPathNode
    {
        public int number = 0;

        private IntPoint _localPoint;
        private float _costHere;
        private float _distanceToGoal;
        private bool _isStartNode = false;
        private bool _isGoalNode = false;
        private bool _visited = false;
        private List<PathLink> _links = new List<PathLink>();
        private PathLink _previousLink = null;

        #region IPathNode Members

        public SampleNode(int pX, int pY)
        {
            _localPoint = new IntPoint(pX, pY);
        }

        public IntPoint localPoint {
            get {
                return _localPoint;
            }
        }

        public float pathCostHere {
            get {
                return _costHere;
            }
            set {
                _costHere = value;
            }
        }

        public float distanceToGoal {
            get {
                return _distanceToGoal;
            }
            set {
                _distanceToGoal = value;
            }
        }

        public float baseCost {
            get {
                return number * 10;
            }
        }

        public bool isStartNode {
            get {
                return _isStartNode;
            }
            set {
                _isStartNode = value;
            }
        }

        public bool isGoalNode {
            get {
                return _isGoalNode;
            }
            set {
                _isGoalNode = value;
            }
        }

        public bool visited {
            get {
                return _visited;
            }
            set {
                _visited = value;
            }
        }

        public List<PathLink> links {
            get {
                return _links;
            }
            set {
                _links = value;
            }
        }

        public PathLink linkLeadingHere {
            get {
                return _previousLink;
            }
            set {
                _previousLink = value;
            }
        }

        public PathLink GetLinkTo(IPathNode pNode)
        {
            if (_links != null) {
                foreach (PathLink p in _links) {
                    if (p.Contains(pNode)) {
                        return p;
                    }
                }
            }
            
            return null;
        }

        public void AddLink(PathLink pLink)
        {
            _links.Add(pLink);
        }

        public void RemoveLink(PathLink pLink)
        {
            _links.Remove(pLink);
        }

        #endregion
        
        #region IPoint Members

        public virtual float DistanceTo(Pathfinding.IPoint pPoint)
        {
            if (pPoint is SampleNode) {
                SampleNode otherNode = pPoint as SampleNode;
            return _localPoint.EuclidianDistanceTo(otherNode._localPoint);
            }
            else {
                throw new NotImplementedException();
            }
        }

        public override int GetHashCode()
        {
            return _localPoint.x + (int)(_localPoint.y * 1000f);
        }
        
        #endregion
        
        #region IComparable Members
        
        public int CompareTo(object obj)
        {
            SampleNode target = obj as SampleNode;
            float targetValue = target.pathCostHere + target.distanceToGoal;
            float thisValue = pathCostHere + distanceToGoal;
            
            if (targetValue > thisValue) {
                return 1;
            }
            else if (targetValue == thisValue) {
                return 0;
            }
            else {
                return -1;
            }
        }
        
        #endregion

        public virtual long GetUniqueID()
        {
            return BitCruncher.PackTwoInts(_localPoint.x, _localPoint.y);
        }
    }
}
