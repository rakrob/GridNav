using System;
using System.Windows.Forms;

namespace GridNav
{
    public partial class Form1 : Form
    {

        Visualizer.Visualizer activeVisualizer;

        public Form1()
        {
            InitializeComponent();
        }

        private void generateGridBtn_Click(object sender, EventArgs e)
        {
            Grid.Grid g = new Grid.Grid(float.Parse(gridHeightBox.Text), float.Parse(gridWidthBox.Text), float.Parse(gridPercentageObstructedBox.Text), int.Parse(numObstructionsBox.Text), float.Parse(droneStartXBox.Text), float.Parse(droneStartYBox.Text), float.Parse(droneEndXBox.Text), float.Parse(droneEndYBox.Text));
            Drone.Drone d = new Drone.Drone(5, 5, g);

            if (activeVisualizer != null) activeVisualizer.Close();
            activeVisualizer = new Visualizer.Visualizer(g, d);
        }
    }
}
