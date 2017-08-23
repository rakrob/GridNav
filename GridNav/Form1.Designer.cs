namespace GridNav
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridWidthBox = new System.Windows.Forms.TextBox();
            this.gridHeightBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gridPercentageObstructedBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.droneStartYBox = new System.Windows.Forms.TextBox();
            this.droneStartXBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.droneEndYBox = new System.Windows.Forms.TextBox();
            this.droneEndXBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.generateGridBtn = new System.Windows.Forms.Button();
            this.numObstructionsBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gridWidthBox
            // 
            this.gridWidthBox.Location = new System.Drawing.Point(15, 25);
            this.gridWidthBox.Name = "gridWidthBox";
            this.gridWidthBox.Size = new System.Drawing.Size(127, 20);
            this.gridWidthBox.TabIndex = 1;
            this.gridWidthBox.Text = "1280";
            // 
            // gridHeightBox
            // 
            this.gridHeightBox.Location = new System.Drawing.Point(150, 25);
            this.gridHeightBox.Name = "gridHeightBox";
            this.gridHeightBox.Size = new System.Drawing.Size(127, 20);
            this.gridHeightBox.TabIndex = 2;
            this.gridHeightBox.Text = "720";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grid Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Grid Width";
            // 
            // gridPercentageObstructedBox
            // 
            this.gridPercentageObstructedBox.Location = new System.Drawing.Point(15, 64);
            this.gridPercentageObstructedBox.Name = "gridPercentageObstructedBox";
            this.gridPercentageObstructedBox.Size = new System.Drawing.Size(127, 20);
            this.gridPercentageObstructedBox.TabIndex = 3;
            this.gridPercentageObstructedBox.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "% of Grid Obstructed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 1;
            // 
            // droneStartYBox
            // 
            this.droneStartYBox.Location = new System.Drawing.Point(150, 103);
            this.droneStartYBox.Name = "droneStartYBox";
            this.droneStartYBox.Size = new System.Drawing.Size(127, 20);
            this.droneStartYBox.TabIndex = 6;
            this.droneStartYBox.Text = "0";
            // 
            // droneStartXBox
            // 
            this.droneStartXBox.Location = new System.Drawing.Point(15, 103);
            this.droneStartXBox.Name = "droneStartXBox";
            this.droneStartXBox.Size = new System.Drawing.Size(127, 20);
            this.droneStartXBox.TabIndex = 5;
            this.droneStartXBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Starting Coordinates X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(147, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Starting Coordinates Y";
            // 
            // droneEndYBox
            // 
            this.droneEndYBox.Location = new System.Drawing.Point(150, 142);
            this.droneEndYBox.Name = "droneEndYBox";
            this.droneEndYBox.Size = new System.Drawing.Size(127, 20);
            this.droneEndYBox.TabIndex = 8;
            this.droneEndYBox.Text = "720";
            // 
            // droneEndXBox
            // 
            this.droneEndXBox.Location = new System.Drawing.Point(15, 142);
            this.droneEndXBox.Name = "droneEndXBox";
            this.droneEndXBox.Size = new System.Drawing.Size(127, 20);
            this.droneEndXBox.TabIndex = 7;
            this.droneEndXBox.Text = "1280";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ending Coordinates X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(150, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Ending Coordinates Y";
            // 
            // generateGridBtn
            // 
            this.generateGridBtn.Location = new System.Drawing.Point(168, 180);
            this.generateGridBtn.Name = "generateGridBtn";
            this.generateGridBtn.Size = new System.Drawing.Size(109, 23);
            this.generateGridBtn.TabIndex = 0;
            this.generateGridBtn.Text = "Generate Grid";
            this.generateGridBtn.UseVisualStyleBackColor = true;
            this.generateGridBtn.Click += new System.EventHandler(this.generateGridBtn_Click);
            // 
            // numObstructionsBox
            // 
            this.numObstructionsBox.Location = new System.Drawing.Point(150, 64);
            this.numObstructionsBox.Name = "numObstructionsBox";
            this.numObstructionsBox.Size = new System.Drawing.Size(127, 20);
            this.numObstructionsBox.TabIndex = 4;
            this.numObstructionsBox.Text = "10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(147, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "# of Obstructions";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 221);
            this.Controls.Add(this.generateGridBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.droneEndXBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.droneStartXBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.droneEndYBox);
            this.Controls.Add(this.gridHeightBox);
            this.Controls.Add(this.droneStartYBox);
            this.Controls.Add(this.numObstructionsBox);
            this.Controls.Add(this.gridPercentageObstructedBox);
            this.Controls.Add(this.gridWidthBox);
            this.Name = "Form1";
            this.Text = "Grid Generation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox gridWidthBox;
        private System.Windows.Forms.TextBox gridHeightBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gridPercentageObstructedBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox droneStartYBox;
        private System.Windows.Forms.TextBox droneStartXBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox droneEndYBox;
        private System.Windows.Forms.TextBox droneEndXBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button generateGridBtn;
        private System.Windows.Forms.TextBox numObstructionsBox;
        private System.Windows.Forms.Label label9;
    }
}

