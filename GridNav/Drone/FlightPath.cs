using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;

namespace GridNav.Drone
{
    public class FlightPath
    {
        public List<PathNode> path { get; private set; }

        public struct PathNode
        {
            public PointF node;
            public bool optimal;
        }

        public FlightPath()
        {
            path = new List<PathNode>();
        }

        public void addNode(float x, float y, bool optimal)
        {
            PathNode t = new PathNode();
            t.node = new PointF(x, y);
            t.optimal = optimal;
            path.Add(t);
        }

        public bool isEqual(FlightPath x)
        {
            if (x.path.Count != path.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (!(Math.Abs(x.path.ElementAt(i).node.X - path.ElementAt(i).node.X) < .1 && Math.Abs(x.path.ElementAt(i).node.Y - path.ElementAt(i).node.Y) < .1 && x.path.ElementAt(i).optimal == path.ElementAt(i).optimal))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
