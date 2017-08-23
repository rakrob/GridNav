using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GridNav.Visualizer
{
    public partial class Visualizer : Form
    {
        Graphics g;
        Grid.Grid srcGrid;
        Drone.Drone srcDrone;

        Drone.FlightPath preComputeFlightPath;
        Drone.FlightPath currFlightPath;

        private System.Timers.Timer calcTimer;

        private bool flightPathOptimized;

        public Visualizer(Grid.Grid xGrid, Drone.Drone xDrone)
        {
            InitializeComponent();

            srcGrid = xGrid;
            srcDrone = xDrone;

            flightPathOptimized = false;

            preComputeFlightPath = xDrone.preComputeFlightPath;
            currFlightPath = null;
            calcTimer = null;

            //set up our window to be the proper aspect ratio for our grid
            float aspectRatio = srcGrid.width / srcGrid.height;
            float frameBufferX = 17;
            float frameBufferY = 40;

            //our maximum height should be 720 pixels (720p), but we still want to fit everything on the screen in a way that looks neat
            //if the aspect ratio is > 16:9, then we should set our width at 1280 since height will be smaller than 720 at that point

            if (aspectRatio >= 16D/9D)
            {
                Width = 1280;
                Height = (int)(Width / aspectRatio);
            }
            else
            {
                Height = 720;
                Width = (int)(Height * aspectRatio);         
            }

            //add the necessary height & width for our tool panel and frame buffers
            Height = Height + toolPanel.Height + (int)frameBufferY;
            Width = Width + (int)frameBufferX;

            //create our graphics object that we will be using to draw
            g = drawPanel.CreateGraphics();

            //calculate the transform that we will need to use to display graphics objects on our form

            float panelBufferX = 1;
            float panelBufferY = 1;

            RectangleF mapFrame = new RectangleF(0, 0, srcGrid.width, srcGrid.height);
            PointF[] drawCorners = new PointF[] { new PointF(0, 0), new PointF(drawPanel.Width - panelBufferX, 0), new PointF(0, drawPanel.Height - panelBufferY)};

            //scale the transform properly
            System.Drawing.Drawing2D.Matrix tMatrix = new System.Drawing.Drawing2D.Matrix(mapFrame, drawCorners);            
            g.Transform = tMatrix;

            g.ScaleTransform(1, -1);
            g.TranslateTransform(0, -1 * (drawPanel.Height) / tMatrix.Elements.ElementAt(3) + panelBufferY);

            //initialize window and draw the control
            Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            drawBorder();
            drawObstructions();
            drawFlightPaths();
        }

        void drawBorder()
        {
            Pen grayDashPen = new Pen(Color.Gray, 1);
            grayDashPen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            grayDashPen.DashPattern = new float[] { 4, 2 };

            g.DrawRectangle(grayDashPen, 0, 0, srcGrid.width, srcGrid.height);
        }

        void drawObstructions()
        {
            foreach (GridNav.Grid.Obstruction o in srcGrid.objList)
            {
                g.DrawRectangle(Pens.Red, o.originX, o.originY, o.width, o.height);
            }
        }

        void drawFlightPaths()
        {
            Pen blueDashPen = new Pen(Color.CornflowerBlue, 2);
            blueDashPen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            blueDashPen.DashPattern = new float[] { 4, 2 };

            Pen blueSolidPen;

            if (flightPathOptimized)
            {
                blueSolidPen = new Pen(Color.Green, 2);
            }
            else
            {
                blueSolidPen = new Pen(Color.DarkBlue, 2);
            }

            g.DrawEllipse(blueSolidPen, srcGrid.startX - srcDrone.radius, srcGrid.startY - srcDrone.radius, srcDrone.radius * 2, srcDrone.radius * 2);

            if (currFlightPath != null)
            {
                for (int i = 1; i < currFlightPath.path.Count; i++)
                {
                    PointF p1 = new PointF(currFlightPath.path.ElementAt(i - 1).node.X, currFlightPath.path.ElementAt(i - 1).node.Y);
                    PointF p2 = new PointF(currFlightPath.path.ElementAt(i).node.X, currFlightPath.path.ElementAt(i).node.Y);
                    g.DrawLine(blueSolidPen, p1, p2);
                    g.DrawEllipse(blueSolidPen, p2.X - srcDrone.radius, p2.Y - srcDrone.radius, srcDrone.radius * 2, srcDrone.radius * 2);

                }

                if (!flightPathOptimized)
                {
                    flightStatusLabel.Text = "Showing Pre-Computed Flight Plan and Previous Actual Flight Data";
                }
                else
                {
                    flightStatusLabel.Text = "Showing Optimized Flight Plan";
                    button1.Enabled = false;
                    button3.Enabled = false;
                }

            }

            if (preComputeFlightPath != null && flightPathOptimized == false)
            {
                for (int i = 1; i < preComputeFlightPath.path.Count; i++)
                {
                    PointF p1 = new PointF(preComputeFlightPath.path.ElementAt(i - 1).node.X, preComputeFlightPath.path.ElementAt(i - 1).node.Y);
                    PointF p2 = new PointF(preComputeFlightPath.path.ElementAt(i).node.X, preComputeFlightPath.path.ElementAt(i).node.Y);
                    g.DrawLine(blueDashPen, p1, p2);
                    g.DrawEllipse(blueDashPen, p2.X - srcDrone.radius, p2.Y - srcDrone.radius, srcDrone.radius * 2, srcDrone.radius * 2);
                }
            }
                      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currFlightPath = srcDrone.BeginFlight();
            preComputeFlightPath = srcDrone.OptimizeFlightPath();

            if (currFlightPath.isEqual(preComputeFlightPath))
            {
                flightPathOptimized = true;
            }

            button2.Enabled = true;

            Refresh();
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button3.Enabled = true;
            flightStatusLabel.Text = "Showing Pre-Computed Flight Plan";
            button2.Enabled = false;

            if (calcTimer != null)
            {
                calcTimer.Enabled = false;
                calcTimer = null;
            }

            srcDrone = new Drone.Drone(5, 5, srcGrid);

            flightPathOptimized = false;

            preComputeFlightPath = srcDrone.preComputeFlightPath;
            currFlightPath = null;

            Refresh();
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (calcTimer == null)
            {
                calcTimer = new System.Timers.Timer();
                calcTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnCalculate);
                calcTimer.Interval = 750;

                button2.Enabled = true;
                button1.Enabled = false;
                button3.Text = "Stop Iteration";

                calcTimer.Enabled = true;
            }
            else
            {
                calcTimer.Enabled = false;
                calcTimer = null;

                button3.Text = "Optimize Flight Path";
                button1.Enabled = true;
            }
        }

        private void OnCalculate(object source, System.Timers.ElapsedEventArgs e)
        {
            currFlightPath = srcDrone.BeginFlight();
            preComputeFlightPath = srcDrone.OptimizeFlightPath();

            if (currFlightPath.isEqual(preComputeFlightPath))
            {
                System.Timers.Timer srcTimer = (System.Timers.Timer)source;
                srcTimer.Enabled = false;
                flightPathOptimized = true;
                calcTimer = null;

                if (button3.InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate () { button3.Text = "Optimize Flight Path"; });
                }
                else
                {
                    button3.Text = "Optimize Flight Path";
                }

            }

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate () { Refresh(); Invalidate(); });
            }
            else
            {
                Refresh();
                Invalidate();
            }
        }
    }
}
