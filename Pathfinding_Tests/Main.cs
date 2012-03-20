using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding_Tests
{
    public class MainClass
    {
        public static void Main()
        {
            RoomTests rt = new RoomTests();
            rt.LoadLevel();
            rt.FindPathInOneRoom();
            rt.FindPathManyRooms();
            
        }
    }
}
