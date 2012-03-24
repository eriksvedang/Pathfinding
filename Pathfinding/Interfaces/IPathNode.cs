using System;
using System.Collections.Generic;

namespace Pathfinding
{
    public interface IPathNode : IPoint, IComparable
    {
        float baseCost {
            get;
        }

        float pathCostHere {
            get;
            set;
        }

        float distanceToGoal {
            get;
            set;
        }

        bool isStartNode {
            get;
            set;
        }

        bool isGoalNode {
            set;
            get;
        }

        void AddLink(PathLink pLink);

        void RemoveLink(PathLink pLink);

        List<PathLink> links {
            get;
        }

        PathLink GetLinkTo(IPathNode pNode);

        PathLink linkLeadingHere {
            get;
            set;
        }

        bool visited {
            get;
            set;
        }

        long GetUniqueID();
    }
}
