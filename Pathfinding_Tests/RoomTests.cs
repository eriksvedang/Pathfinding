using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NUnit.Framework;
using Pathfinding;
namespace Pathfinding_Tests
{
    [TestFixture]
    public class RoomTests
    {
        [SetUp]
        public void Setup()
        {

            
        }
        [Test]
        public void LoadLevel()
        {

            Room[] rooms = LoadFromFile();
            PrintWorld(rooms, Path<TileNode>.EMPTY );
            TileNode start = null, goal = null;
            foreach (Room r in rooms)
            {
                GameTypes.IntPoint localStart = r.WorldToLocalPoint(new GameTypes.IntPoint(-8, 1));
                GameTypes.IntPoint localGoal = r.WorldToLocalPoint(new GameTypes.IntPoint(17, 6));
                if (r.GetTileType(localStart) == TileType.FLOOR)
                {
                    start = r.GetTile(localStart);
                }
                if (r.GetTileType(localGoal) == TileType.FLOOR)
                {
                    goal = r.GetTile(localGoal);
                }

            }
            Assert.NotNull(start);
            Assert.NotNull(goal);
        }
        [Test]
        public void FindPathInOneRoom()
        {
            Room[] rooms = LoadFromFile();
            PrintWorld(rooms, Path<TileNode>.EMPTY);
            TileNode start = null, goal = null;
            foreach (Room r in rooms)
            {
                GameTypes.IntPoint localStart = r.WorldToLocalPoint(new GameTypes.IntPoint(-8, 1));
                GameTypes.IntPoint localGoal = r.WorldToLocalPoint(new GameTypes.IntPoint(-1, 6));
                if (r.GetTileType(localStart) == TileType.FLOOR)
                {
                    start = r.GetTile(localStart);
                }
                if (r.GetTileType(localGoal) == TileType.FLOOR)
                {
                    goal = r.GetTile(localGoal);
                }

            }
            Assert.NotNull(start);
            Assert.NotNull(goal);
            PathSolver<TileNode> solver = new PathSolver<TileNode>();
            MultiRoomNetwork roomNetwork = new MultiRoomNetwork(rooms);
            var foundPath = solver.FindPath(start, goal, roomNetwork);
            Assert.AreEqual(PathStatus.FOUND_GOAL, foundPath.status);
            PrintWorld(rooms, foundPath);
        }

        [Test]
        public void FindPathManyRooms()
        {
            Room[] rooms = LoadFromFile();
            PrintWorld(rooms, Path<TileNode>.EMPTY);
            TileNode start = null, goal = null;
            foreach (Room r in rooms)
            {
                GameTypes.IntPoint localStart = r.WorldToLocalPoint(new GameTypes.IntPoint(-8, 1));
                GameTypes.IntPoint localGoal = r.WorldToLocalPoint(new GameTypes.IntPoint(17, 6));
                if (r.GetTileType(localStart) == TileType.FLOOR)
                {
                    start = r.GetTile(localStart);
                }
                if (r.GetTileType(localGoal) == TileType.FLOOR)
                {
                    goal = r.GetTile(localGoal);
                }

            }
            Assert.NotNull(start);
            Assert.NotNull(goal);
            PathSolver<TileNode> solver = new PathSolver<TileNode>();
            MultiRoomNetwork roomNetwork = new MultiRoomNetwork(rooms);
            var foundPath = solver.FindPath(start, goal, roomNetwork);
            Assert.AreEqual(PathStatus.FOUND_GOAL, foundPath.status);
            PrintWorld(rooms, foundPath);
        }
		
		[Test]
        public void FindImpossiblePath()
        {
            Room[] rooms = LoadFromFile();
            PrintWorld(rooms, Path<TileNode>.EMPTY);
            TileNode start = null, goal = null;
            foreach (Room r in rooms)
            {
                GameTypes.IntPoint localStart = r.WorldToLocalPoint(new GameTypes.IntPoint(-8, 1));
                GameTypes.IntPoint localGoal = r.WorldToLocalPoint(new GameTypes.IntPoint(13, 4));
                if (r.GetTileType(localStart) == TileType.FLOOR)
                {
                    start = r.GetTile(localStart);
                }
                if (r.GetTileType(localGoal) == TileType.FLOOR)
                {
                    goal = r.GetTile(localGoal);
                }

            }
            Assert.NotNull(start);
            Assert.NotNull(goal);
            PathSolver<TileNode> solver = new PathSolver<TileNode>();
            MultiRoomNetwork roomNetwork = new MultiRoomNetwork(rooms);
            var foundPath = solver.FindPath(start, goal, roomNetwork);
            Assert.AreEqual(PathStatus.DESTINATION_UNREACHABLE, foundPath.status);
            PrintWorld(rooms, foundPath);
        }
		
        private static Room[] LoadFromFile()
        {
            RoomBuilder rb = new RoomBuilder();
            StreamReader s = new StreamReader(File.Open("Maze2.txt", FileMode.Open));
            while (!s.EndOfStream)
            {
                string data = s.ReadLine();
                if (data.Length > 0)
                {
                    switch (data[0])
                    {
                        case '|':
                            rb.AppendWallsAndDoors(data.Substring(1));
                            break;
                        case ':':
                            rb.BeginNewRoom(data.Substring(1));
                            break;
                        default: break;
                    }
                }
            }
            s.Dispose();
            return rb.GetRooms();
        }

        private static void PrintWorld(Room[] rooms, Path<TileNode> pPath )
        {
            StringBuilder sb = new StringBuilder();
            for (int y = -1; y < 20; y++)
            {
                sb.Append(y.ToString()[y.ToString().Length - 1]);
                for (int x = -10; x < 20; x++)
                {
                    char result = ' ';
                    foreach (TileNode t in pPath.nodes)
                    {
                        if (t.worldPosition.x == x && t.worldPosition.y == y)
                        {
                            result = 'o';
                        }
                    }
                    if (result == ' ')
                    {
                        if (y == -1 && x > -10)
                        {
                            result = x.ToString()[x.ToString().Length - 1];
                        }
                        else
                        {

                            foreach (Room r in rooms)
                            {
                                GameTypes.IntPoint point = r.WorldToLocalPoint(new GameTypes.IntPoint(x, y));
                                if (r.GetTileType(point.x, point.y) == TileType.WALL)
                                {
                                    result = 'x';
                                }
                                else if (r.GetTileType(point.x, point.y) == TileType.DOOR)
                                {
                                    string matches = Regex.Replace((r.GetTile(point.x, point.y) as Door).name, "[A-z]", "");
                                    result = matches[0];
                                }
                            }
                        }
                    }
                    sb.Append(result);
                }
                sb.Append("\r\n");
            }
            Console.Write(sb.ToString());
        }
        
        
    }
    class RoomBuilder
    {
        List<Room> _rooms = new List<Room>();
        public Room currentRoom = null;
        bool roomHasBeenMoved = false;
        public int yValue = 0;
        internal void AppendWallsAndDoors(string p)
        {
            for (int x = 0; x < p.Length; x++)
            {
                if (p[x] == '#')
                    currentRoom.SetTile(new TileNode(currentRoom, x,yValue, TileType.WALL));
                else if (p[x] == ' ')
                    currentRoom.SetTile(new TileNode(currentRoom, x, yValue, TileType.FLOOR));
                else if (Regex.IsMatch("" + p[x], "[0-9]"))
                {
                    Door d = new Door(currentRoom, "door" + p[x], new GameTypes.IntPoint(x, yValue));
                    currentRoom.AddDoor(d);
                    foreach (Room r in _rooms)
                    {
                        Door otherDoor = r.GetDoor(d.name);    
                        if (otherDoor != null)
                        {
                            d.target = otherDoor; //door target is set here!
                            otherDoor.target = d;
                            if (!roomHasBeenMoved)
                            {
                
                                currentRoom.worldPosition = otherDoor.worldPosition - d.localPosition;
                                Console.WriteLine("moving room to " + currentRoom.worldPosition.ToString());
                                roomHasBeenMoved = true;
                            }
                        }
                    }
                
                }
            }
            yValue++;
        }

        internal void BeginNewRoom(string p)
        {
            if (currentRoom != null)
                _rooms.Add(currentRoom);
            currentRoom = new Room(p);
            roomHasBeenMoved = false;
            yValue = 0;
        }
        
        public Room[] GetRooms()
        {
            if (currentRoom != null)
                _rooms.Add(currentRoom);
            return _rooms.ToArray();
        }
        void ConnectDoors()
        { 
            
        }
        public void Clear()
        {
            _rooms.Clear();
            yValue = 0;
            currentRoom = null;
        }
    }
}
