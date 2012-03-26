using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding;
using NUnit.Framework;

namespace Pathfinding_Tests
{
    [TestFixture]
    public class Tests
    {
        private string BuildPathString(Path<SampleNode> pPath)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int y = 0; y < 10; y++) {
                for (int x = 0; x < 10; x++) {
                    if (IsInPath(pPath, x, y)) {
                        sb.Append("x,");
                    }
                    else {
                        sb.Append("0,");
                    }
                }
            }
            
            return sb.ToString();
        }

        private void PrintPath(Path<SampleNode> pPath)
        {
            for (int y = 0; y < 10; y++) {
                for (int x = 0; x < 10; x++) {
                    if (IsInPath(pPath, x, y)) {
                        Console.Write("x");
                    }
                    else {
                        Console.Write("0");
                    }
                }
                
                Console.Write("\r\n");
            }
        }
        
        private bool IsInPath(Path<SampleNode> pPath, int x, int y)
        {
            foreach (SampleNode n in pPath.nodes) {
                if (x == (int)n.localPoint.x && y == (int)n.localPoint.y) {
                    return true;
                }
            }
            
            return false;
        }
        
        public static string dataA =
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0," +
            "0,0,0,0,0,0,0,0,0,0,";

        PathSolver<SampleNode> solver = null;
        SamplePathNetwork network = null;

        [SetUp]
        public void Setup()
        {
            solver = new PathSolver<SampleNode>();
        }
        
        [Test]
        public void GotoSameTileTest()
        {
            network = new SamplePathNetwork(dataA);
            IPathNode start = network.GetNode(0, 0);
            IPathNode goal = network.GetNode(0, 0);
            Path<SampleNode> p = solver.FindPath(start, goal, network);

            Assert.AreEqual(PathStatus.ALREADY_THERE, p.status);
        }

        [Test]
        public void DiagonalFind()
        {
            network = new SamplePathNetwork(dataA);
            IPathNode start = network.GetNode(0, 0);
            IPathNode goal = network.GetNode(9, 9);
            Path<SampleNode> p = solver.FindPath(start, goal, network);

            Assert.AreEqual(PathStatus.FOUND_GOAL, p.status);
            Assert.AreEqual(18, (int)p.pathLength);
        }
        
        [Test]
        public void EasyMaze()
        {
            const string maze =
                "0,0,0,0,1,0,0,0,0,0," +
                "1,1,1,0,1,0,0,0,0,0," +
                "0,0,0,0,1,0,0,0,0,0," +
                "0,1,1,1,1,0,0,0,0,0," +
                "0,0,0,0,1,0,0,0,0,0," +
                "0,0,0,0,1,0,0,0,0,0," +
                "0,0,0,0,1,0,0,0,0,0," +
                "0,1,1,1,1,0,0,0,0,0," +
                "0,0,0,0,0,0,0,0,0,0," +
                "0,0,0,0,0,1,1,1,0,0,";

            const string mazeResult =
                "x,x,x,x,0,0,0,0,0,0," +
                "0,0,0,x,0,0,0,0,0,0," +
                "x,x,x,x,0,0,0,0,0,0," +
                "x,0,0,0,0,0,0,0,0,0," +
                "x,0,0,0,0,0,0,0,0,0," +
                "x,0,0,0,0,0,0,0,0,0," +
                "x,0,0,0,0,0,0,0,0,0," +
                "x,0,0,0,0,0,0,0,0,0," +
                "x,x,x,x,x,x,x,x,x,x," +
                "0,0,0,0,0,0,0,0,0,x,";

            network = new SamplePathNetwork(maze);

            IPathNode start = network.GetNode(0, 0);
            IPathNode goal = network.GetNode(9, 9);
            Path<SampleNode> p = solver.FindPath(start, goal, network);
            PrintPath(p);

            Assert.AreEqual(PathStatus.FOUND_GOAL, p.status);
            Assert.AreEqual(25, (int)p.nodes.Length);
            Assert.AreEqual(mazeResult, BuildPathString(p));
        }
    }
}