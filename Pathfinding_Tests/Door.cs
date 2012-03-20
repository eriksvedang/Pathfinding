using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTypes;
using Pathfinding;
namespace Pathfinding_Tests
{
    public class Door : TileNode
    {
        public Door(Room pRoom, string pName, IntPoint pPosition ) 
            : base(pRoom, pPosition.x,pPosition.y, TileType.DOOR)
        {
            name = pName;
        }
        public string name { get; private set; }
        Door _target = null;
        public Door target
        {
            set
            {
                //check for link duplicates
                for (int i = links.Count - 1; i >= 0; i--)
                {
                    PathLink l = links[i];
                    Door d = l.GetOtherNode(this) as Door;
                    if (d == value)
                    {
                        return; //return if any duplicate was found.
                    }
                }

                //we should remove any old links to other doors
                for(int i=links.Count-1; i>=0 ; i--)
                {
                    PathLink l = links[i];
                    Door d = l.GetOtherNode(this) as Door;
                    if (d != null &&d.room != this.room)
                    {
                        links.RemoveAt(i);
                    }
                }
                //check if the other node already has made a link for us to use.
                PathLink pl = value.GetLinkTo(this);
                if (pl == null)
                {
                    pl = new PathLink(this, value);
                }
                Console.WriteLine("added link between " + (pl.nodeA as Door).name);
                links.Add(pl);
            }
            get
            {
                return _target;
            }
        }


    }
}
