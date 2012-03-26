using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Pathfinding;
using GameTypes;

namespace Pathfinding
{
    public class TileNode : IPathNode
    {
		const int NR_OF_LINKS = 5;
		
        private PathLink[] _links;
        private int _linkInsertPosition;

        public TileNode(IntPoint pLocalPoint)
        {
            _links = new PathLink[NR_OF_LINKS];
            _linkInsertPosition = 0;
            localPoint = pLocalPoint;
        }

        public void Reset()
        {
            distanceToGoal = 0f;
            isGoalNode = false;
            isStartNode = false;
            linkLeadingHere = null;
            pathCostHere = 0f;
            visited = false;
        }
        
        public IntPoint localPoint {
            set;
            get;
        }
        
        public float pathCostHere {
            get;
            set;
        }
        
        public float distanceToGoal {
            get;
            set;
        }
        
        public float baseCost {
            set;
            get;
        }
        
        public bool isStartNode {
            get;
            set;
        }
        
        public bool isGoalNode {
            get;
            set;
        }
        
        public bool visited {
            get;
            set;
        }
        
        public PathLink linkLeadingHere {
            get;
            set;
        }
        
        public PathLink[] links {
            get {
                return _links;
            }
            set {
                _links = value;
            }
        }
        
        public void AddLink(PathLink pLink)
        {
			if(_linkInsertPosition < NR_OF_LINKS) {
            	_links[_linkInsertPosition++] = pLink;
			}
			else {
				for(int i = 0; i < _links.Length; i++) {
	                if(_links[i] == null) {
	                    _links[i] = pLink;
						return;
	                }
				}
				throw new Exception("Can't find free place in _links");
			}
        }

        public void RemoveLink(PathLink pLink)
        {
            for(int i = 0; i < _links.Length; i++) {
                if(_links[i] == pLink) {
                    _links[i] = null;
					return;
                }
            }
        }
        
        public PathLink GetLinkTo(IPathNode pNode)
        {
			/*
            foreach (PathLink p in links) {
                if (p != null && p.Contains(pNode)) {
                    return p;
                }
            }*/
			
			// OPTIMIZATION OF PREVIOUS CODE:
			
			PathLink p = _links[0];
			if(p != null && p.Contains(pNode)) return p;
            
			p = _links[1];
			if(p != null && p.Contains(pNode)) return p;
			
			p = _links[2];
			if(p != null && p.Contains(pNode)) return p;
			
			p = _links[3];
			if(p != null && p.Contains(pNode)) return p;
			
			p = _links[4];
			if(p != null && p.Contains(pNode)) return p;
			
            return null;
        }
        
        #region IPoint Members

        public virtual float DistanceTo(Pathfinding.IPoint pPoint)
        {
            if (pPoint is TileNode) {
                TileNode otherNode = pPoint as TileNode;
                return localPoint.EuclidianDistanceTo(otherNode.localPoint);
            }
            else {
                throw new NotImplementedException();
            }
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
        
        #region IPathNode Members

        public virtual long GetUniqueID()
        {
            return BitCruncher.PackTwoInts(localPoint.x, localPoint.y);
        }

        #endregion
    }
}
