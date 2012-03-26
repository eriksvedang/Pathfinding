using System.Text;

namespace Pathfinding
{
    public enum PathStatus
    {
        NOT_CALCULATED_YET,
        DESTINATION_UNREACHABLE,
        FOUND_GOAL,
        ALREADY_THERE
    }

    public struct Path<PathNodeType> where PathNodeType : IPathNode
    {
        public PathStatus status;
        public float pathLength;
        public PathNodeType[] nodes;
        public int pathSearchTestCount;

        public Path(PathNodeType[] pNodes, float pPathLength, PathStatus pStatus, int pPathSearchTestCount)
        {
            nodes = pNodes;
            pathLength = pPathLength;
            status = pStatus;
            pathSearchTestCount = pPathSearchTestCount;
        }

        public static Path<PathNodeType> EMPTY {
            get {
                return new Path<PathNodeType>(new PathNodeType[0], 0f, PathStatus.NOT_CALCULATED_YET, 0);
            }
        }
        
        public PathNodeType LastNode {
            get {
                return nodes[nodes.Length - 1];
            }
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Path: \n[ ");
            
            foreach (IPathNode ipn in nodes) {
                sb.Append(ipn.ToString() + ",\n");
            }
            
            sb.Append("]");
            return sb.ToString();
        }
    }
}
