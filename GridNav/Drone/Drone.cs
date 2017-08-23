using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GridNav.Drone
{
    public class Drone
    {
        public readonly float radius;
        public readonly float speed;

        private float prevDirection;
        private Grid.Edge prevCollidingEdge;
        private Grid.Corner prevCollidingCorner;

        private List<Grid.Corner> eliminatedCorners;

        public float direction { get; private set; }
        public float xPosition { get; private set; }
        public float yPosition { get; private set; }

        private int pathNodeIndex;

        private Grid.Grid xGrid;

        public FlightPath preComputeFlightPath { get; private set; }
        public FlightPath path { get; private set; }
        
        public Drone(float droneRadius, float flightSpeed, Grid.Grid parentGrid)
        {
            //just setting some local variables, properties, things of that nature
            radius = droneRadius; speed = flightSpeed; xPosition = parentGrid.startX; yPosition = parentGrid.startY;
            xGrid = parentGrid;
            prevDirection = float.NaN;

            preComputeFlightPath = new FlightPath();
            preComputeFlightPath.addNode(xGrid.startX, xGrid.startY, false);
            preComputeFlightPath.addNode(xGrid.endX, xGrid.endY, true);
        }

        public FlightPath BeginFlight()
        {
            //Start our index at 1 because 0 will always be our origin and we want to fly towards the first node
            pathNodeIndex = 1;
            path = new FlightPath();

            xPosition = xGrid.startX;
            yPosition = xGrid.startY;

            direction = float.NaN;
            prevCollidingEdge = null;
            prevCollidingCorner = null;

            eliminatedCorners = new List<Grid.Corner>();

            //main control loop for a single flight
            do
            {
                //determine the direction in which we should move via our estimated path
                CalculateDirection(preComputeFlightPath);

                //move in said direction and increment the path node if we have arrived at one
                Move(preComputeFlightPath);

                //terminate loop if we have reached the last node, i.e. the end
            } while (pathNodeIndex < preComputeFlightPath.path.Count());

            //add that end node since we didn't get a chance to do it earlier
            path.addNode(xGrid.endX, xGrid.endY, true);

            return path;
        }

        private void Move(FlightPath estimatedFlightPath)
        {
            float xNew;
            float yNew;
            Forecast(out xNew, out yNew);

            //see if this step takes us through our current target node
            float targetX = estimatedFlightPath.path.ElementAt(pathNodeIndex).node.X;
            float targetY = estimatedFlightPath.path.ElementAt(pathNodeIndex).node.Y;
            if (hasArrived(targetX, targetY))
            {
                //increment to next node if yes
                pathNodeIndex++;
            }

            //update our position
            xPosition = xNew;
            yPosition = yNew;
        }

        private bool hasArrived(float X, float Y)
        {
            float xNew;
            float yNew;
            Forecast(out xNew, out yNew);

            //just do a small bounding box check, since our increments are small enough we can probably assume we have hit our target node if it is in this bounding box
            if (((X <= xNew && X >= xPosition) || (X <= xPosition && X >= xNew)) && ((Y <= yNew && Y >= yPosition) || (Y <= yPosition && Y >= yNew)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Forecast(out float xNew, out float yNew)
        {
            //increments our drone position using basic trig
             xNew = xPosition + speed * (float)Math.Cos(direction);
             yNew = yPosition + speed * (float)Math.Sin(direction);
        }

        private void CalculateDirection(FlightPath estimatedFlightPath)
        {
            bool isOptimalDirection = true;
            float targetX = estimatedFlightPath.path.ElementAt(pathNodeIndex).node.X;
            float targetY = estimatedFlightPath.path.ElementAt(pathNodeIndex).node.Y;

            direction = (float)Math.Atan2(targetY - yPosition, targetX - xPosition);

            //check if we are hitting any of the rectangles
            //since we don't want to actually smack one, we will use a circle size of 5 units larger than our drone radius
            //this can probably be implemented with a 'sensor' class later, but this will suffice for now

            float collisionRadius = radius + 5;
            Grid.Edge collidingEdge = null;
            Grid.Corner collidingCorner = null;

            //loop through obstructions, break on first edge detection encountered
            foreach (Grid.Obstruction rect in xGrid.objList)
            {
                collidingCorner = circleCornerCollision(xPosition, yPosition, collisionRadius, rect);
                collidingEdge = circleEdgeCollision(xPosition, yPosition, collisionRadius, rect);

                if (collidingEdge != null || collidingCorner != null)
                {
                    //flight path is no longer optimal, we will remove these points later when trying to optimize
                    isOptimalDirection = false;
                    break;
                }
            }

            bool isEliminated = false;

            if (collidingCorner != null)
            {
                foreach (Grid.Corner cor in eliminatedCorners)
                {
                    if (collidingCorner.Vertex.X == cor.Vertex.X && collidingCorner.Vertex.Y == cor.Vertex.Y && collidingCorner.Center.X == cor.Center.X && collidingCorner.Center.Y == cor.Center.Y)
                    {
                        isEliminated = true;
                        break;
                    }
                }
            }

            if (collidingCorner != null && isEliminated)
            {
                collidingCorner = null;
                if (collidingEdge == null)
                {
                    isOptimalDirection = true;
                }
            }
            
            if (collidingEdge != null)
            {
                //we had a collision on an edge, now we have to determine the direction in which we should go along the wall until we reach the end of the obstruction
                //if we are still colliding with an edge, previous edge will be populated, which means we must just edge lock and set our previous direction to our new one
                if (prevCollidingEdge != null)
                {
                    direction = prevDirection;
                }
                else
                {
                    //there are two possible angles here, pick the one that will take us closer to the next node
                    //note: this will not always work! there are cases when this becomes counterintuitive, but for our simple test case this will probably work 95% of the time

                    //define two points, these will serve as our vectors
                    //first point is just our direction already, with magnitude 1
                    PointF V1 = new PointF((float)Math.Cos(direction), (float)Math.Sin(direction));

                    //second point is our normalized edge
                    PointF V2 = new PointF(collidingEdge.P2.X - collidingEdge.P1.X, collidingEdge.P2.Y - collidingEdge.P1.Y);

                    //this is the result of our angle calculation between the two edges, if this angle is smaller than its complement, our direction will be the same
                    //if not, we will need to reverse the direction of our colliding edge
                    float resultantAngle = calculateAngle(V1, V2);
                    float resultantCompliment = (float)Math.PI - resultantAngle;

                    if (resultantAngle <= resultantCompliment)
                    {
                        direction = (float)Math.Atan2(collidingEdge.P2.Y - collidingEdge.P1.Y, collidingEdge.P2.X - collidingEdge.P1.X);
                    }
                    else
                    {
                        direction = (float)Math.Atan2(collidingEdge.P1.Y - collidingEdge.P2.Y, collidingEdge.P1.X - collidingEdge.P2.X);
                    }

                    prevCollidingEdge = collidingEdge;
                }
            }
            else if (collidingCorner != null)
            {
                if (prevCollidingCorner != null || prevCollidingEdge != null)
                {
                    direction = prevDirection;
                }
                else
                {
                    //we've hit a corner, set a course to fly straight out to the corner of the obstruction

                    PointF outVector = new PointF(collidingCorner.Vertex.X - collidingCorner.Center.X, collidingCorner.Vertex.Y - collidingCorner.Center.Y);

                    direction = calculateAngle(new PointF(1, 0), outVector);

                    if (outVector.Y < 0)
                    {
                        direction = direction * -1;
                    }

                    prevCollidingCorner = collidingCorner;
                }
            }
            else
            {
                prevCollidingEdge = null;
                if (prevCollidingCorner != null)
                {
                    eliminatedCorners.Add(prevCollidingCorner);
                }
                prevCollidingCorner = null;
            }

            //if direction has changed more than a degree (.017rad = 1deg), add a node to our flight path
            if (float.IsNaN(prevDirection) || Math.Abs(direction - prevDirection) > .017)
            {
                path.addNode(xPosition, yPosition, isOptimalDirection);
            }

            prevDirection = direction;
        }

        private float calculateAngle(PointF V1, PointF V2)
        {
            float dotProduct = V1.X * V2.X + V1.Y * V2.Y;
            return (float)Math.Acos(dotProduct / (Length(V2) * Length(V1)));
        }

        private Grid.Corner circleCornerCollision(float X, float Y, float R, Grid.Obstruction rect)
        {
            PointF P = new PointF(X, Y);

            Grid.Corner C1 = new Grid.Corner(rect.originX, rect.originY, rect.originX + rect.width / 2, rect.originY + rect.width / 2);
            Grid.Corner C2 = new Grid.Corner(rect.originX + rect.width, rect.originY, rect.originX + rect.width / 2, rect.originY + rect.width / 2);
            Grid.Corner C3 = new Grid.Corner(rect.originX + rect.width, rect.originY + rect.height, rect.originX + rect.width / 2, rect.originY + rect.height - rect.width / 2);
            Grid.Corner C4 = new Grid.Corner(rect.originX, rect.originY + rect.height, rect.originX + rect.width / 2, rect.originY + rect.height - rect.width / 2);

            if (Length(new PointF(C1.Vertex.X - P.X, C1.Vertex.Y - P.Y)) <= R)
            {
                return C1;
            }
            if (Length(new PointF(C2.Vertex.X - P.X, C2.Vertex.Y - P.Y)) <= R)
            {
                return C2;
            }
            if (Length(new PointF(C3.Vertex.X - P.X, C3.Vertex.Y - P.Y)) <= R)
            {
                return C3;
            }
            if (Length(new PointF(C4.Vertex.X - P.X, C4.Vertex.Y - P.Y)) <= R)
            {
                return C4;
            }

            return null;
        }

        private Grid.Edge circleEdgeCollision(float X, float Y, float R, Grid.Obstruction rect)
        {
            PointF P = new PointF(X, Y);

            Grid.Edge E1 = new Grid.Edge(rect.originX, rect.originY, rect.originX + rect.width, rect.originY);
            Grid.Edge E2 = new Grid.Edge(rect.originX + rect.width, rect.originY, rect.originX + rect.width, rect.originY + rect.height);
            Grid.Edge E3 = new Grid.Edge(rect.originX + rect.width, rect.originY + rect.height, rect.originX, rect.originY + rect.height);
            Grid.Edge E4 = new Grid.Edge(rect.originX, rect.originY + rect.height, rect.originX, rect.originY);

            //use the point-line theorem to calculate distance between our drone center and each edge of the obstruction
            //normally we'd transpose into a new coordinate system but since all of our obstructions are axis aligned we're going to go with some slight fudgery here
            //this can be changed in the future
            if (Distance(P, E1) <= R && P.X >= rect.originX && P.X <= rect.originX + rect.width)
            {
                return E1;
            }
            if (Distance(P, E2) <= R && P.Y >= rect.originY && P.Y <= rect.originY + rect.height)
            {
                return E2;
            }
            if (Distance(P, E3) <= R && P.X >= rect.originX && P.X <= rect.originX + rect.width)
            {
                return E3;
            }
            if (Distance(P, E4) <= R && P.Y >= rect.originY && P.Y <= rect.originY + rect.height)
            {
                return E4;
            }

            return null;
        }

        private float Length(PointF A)
        {
            return (float)Math.Sqrt(Math.Pow(A.X, 2) + Math.Pow(A.Y, 2));
        }

        private float Distance(PointF P, Grid.Edge E)
        {
            float quotient = Math.Abs((E.P2.X - E.P1.X) * (E.P1.Y - P.Y) - (E.P1.X - P.X) * (E.P2.Y - E.P1.Y));
            float divisor = (float)Math.Sqrt(Math.Pow(E.P2.X - E.P1.X, 2) + Math.Pow(E.P2.Y - E.P1.Y, 2));

            return quotient / divisor;
        }

        public FlightPath OptimizeFlightPath()
        {
            FlightPath copyPath = new FlightPath();

            foreach (FlightPath.PathNode x in path.path)
            {
                copyPath.addNode(x.node.X, x.node.Y, x.optimal);
            }

            float maxDistance = 0;

            for (int i = path.path.Count - 1; i > 0; i--)
            {
                if (!copyPath.path.ElementAt(i).optimal)
                {
                    copyPath.path.RemoveAt(i);
                    continue;
                }

                float d = Length(new PointF(copyPath.path.ElementAt(i).node.X - copyPath.path.ElementAt(copyPath.path.Count - 1).node.X, copyPath.path.ElementAt(i).node.Y - copyPath.path.ElementAt(copyPath.path.Count - 1).node.Y));
                bool removed = false;

                if (d < maxDistance)
                {
                    copyPath.path.RemoveAt(i);
                    removed = true;
                }

                maxDistance = d;

                if (removed) continue;

                if (i < path.path.Count - 2)
                {
                    float l1 = Length(new PointF(copyPath.path.ElementAt(i + 1).node.X - copyPath.path.ElementAt(i - 1).node.X, copyPath.path.ElementAt(i + 1).node.Y - copyPath.path.ElementAt(i - 1).node.Y));
                    float l2 = Length(new PointF(copyPath.path.ElementAt(i).node.X - copyPath.path.ElementAt(i - 1).node.X, copyPath.path.ElementAt(i).node.Y - copyPath.path.ElementAt(i - 1).node.Y));

                    if (l2 > l1)
                    {
                        copyPath.path.RemoveAt(i);
                        continue;
                    }
                }

            }

            preComputeFlightPath = copyPath;
            return copyPath;
        }
    }
}
