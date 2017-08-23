using System.Drawing;

namespace GridNav.Grid
{
    public class Obstruction
    {
        public readonly float originX;
        public readonly float originY;
        public readonly float height;
        public readonly float width;

        public Obstruction(float X, float Y, float H, float W)
        {
            originX = X; originY = Y; height = H; width = W;
        }

        //checks if a particular X and Y pair is inside the obstruction, inclusive
        public bool isColliding(Obstruction obj)
        {
            if (originX + width + 20 < obj.originX || originX > obj.originX + obj.width + 20 || originY > obj.originY + obj.height + 20 || obj.originY > originY + height + 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isInside(float X, float Y)
        {
            if (X > originX - 20 && X < originX + width + 20 && Y > originY - 20 && Y < originY + height + 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Edge
    {
        public PointF P1;
        public PointF P2;

        public Edge(float X1, float Y1, float X2, float Y2)
        {
            P1 = new PointF();
            P1.X = X1;
            P1.Y = Y1;

            P2 = new Point();
            P2.X = X2;
            P2.Y = Y2;
        }
    }

    public class Corner
    {
        public PointF Vertex;
        public PointF Center;

        public Corner(float vertexX, float vertexY, float centerX, float centerY)
        {
            Vertex = new PointF();
            Vertex.X = vertexX;
            Vertex.Y = vertexY;

            Center = new PointF();
            Center.X = centerX;
            Center.Y = centerY;
        }
    }
}