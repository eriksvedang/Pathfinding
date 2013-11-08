using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding;

namespace Pathfinding_Tests
{
    public class MainClass
    {
        void OriginalTest() {
            RoomTests rt = new RoomTests();
            rt.LoadLevel();
            rt.FindPathInOneRoom();
            rt.FindPathManyRooms();
        }

        static Random rand;

        static string MakeRandomMazeString(int length) {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append(rand.Next(2).ToString() + ",");
            }
            return sb.ToString();
        }

        static void SpeedTest(string maze, int width, int height, int iterations) {
            var network = new SamplePathNetwork(maze, width, height);
            var solver = new PathSolver<SampleNode>();
            for (int i = 0; i < iterations; i++) {
                IPathNode start = network.GetNode(rand.Next(width), rand.Next(height));
                IPathNode goal = network.GetNode(rand.Next(width), rand.Next(height));
                Path<SampleNode> p = solver.FindPath(start, goal, network, true);
                Console.WriteLine(p.status.ToString());
            }
        }

        public static void Main()
        {
            rand = new Random((int)DateTime.Now.Ticks);

            int w = 50;
            int h = w;

            int iterationsPerMaze = 5;

            for (int i = 0; i < 10; i++) {
                var mazeString = MakeRandomMazeString(w * h);
                SpeedTest(mazeString, w, h, iterationsPerMaze);
            }
        }
    }
}
