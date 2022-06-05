namespace ExclusiveProgram
{
    partial class Control
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox_raw = new System.Windows.Forms.PictureBox();
            this.pictureBox_preprocess = new System.Windows.Forms.PictureBox();
            this.pictureBox_puzzle_location = new System.Windows.Forms.PictureBox();
            this.pictureBox_bin = new System.Windows.Forms.PictureBox();
            this.numericUpDown_threshold = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_raw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preprocess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_puzzle_location)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(149, 26);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox_raw
            // 
            this.pictureBox_raw.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_raw.Name = "pictureBox_raw";
            this.pictureBox_raw.Size = new System.Drawing.Size(255, 255);
            this.pictureBox_raw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_raw.TabIndex = 1;
            this.pictureBox_raw.TabStop = false;
            // 
            // pictureBox_preprocess
            // 
            this.pictureBox_preprocess.Location = new System.Drawing.Point(264, 3);
            this.pictureBox_preprocess.Name = "pictureBox_preprocess";
            this.pictureBox_preprocess.Size = new System.Drawing.Size(255, 255);
            this.pictureBox_preprocess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_preprocess.TabIndex = 2;
            this.pictureBox_preprocess.TabStop = false;
            // 
            // pictureBox_puzzle_location
            // 
            this.pictureBox_puzzle_location.Location = new System.Drawing.Point(786, 3);
            this.pictureBox_puzzle_location.Name = "pictureBox_puzzle_location";
            this.pictureBox_puzzle_location.Size = new System.Drawing.Size(255, 255);
            this.pictureBox_puzzle_location.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_puzzle_location.TabIndex = 3;
            this.pictureBox_puzzle_location.TabStop = false;
            // 
            // pictureBox_bin
            // 
            this.pictureBox_bin.Location = new System.Drawing.Point(525, 3);
            this.pictureBox_bin.Name = "pictureBox_bin";
            this.pictureBox_bin.Size = new System.Drawing.Size(255, 255);
            this.pictureBox_bin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_bin.TabIndex = 4;
            this.pictureBox_bin.TabStop = false;
            // 
            // numericUpDown_threshold
            // 
            this.numericUpDown_threshold.Location = new System.Drawing.Point(60, 33);
            this.numericUpDown_threshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_threshold.Name = "numericUpDown_threshold";
            this.numericUpDown_threshold.Size = new System.Drawing.Size(68, 22);
            this.numericUpDown_threshold.TabIndex = 5;
            this.numericUpDown_threshold.Value = new decimal(new int[] {
            170,
            0,
            0,
            0});
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_raw);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_preprocess);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_bin);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_puzzle_location);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 61);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1084, 351);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.numericUpDown_threshold);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Control";
            this.Size = new System.Drawing.Size(1129, 438);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_raw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preprocess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_puzzle_location)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox_raw;
        private System.Windows.Forms.PictureBox pictureBox_preprocess;
        private System.Windows.Forms.PictureBox pictureBox_puzzle_location;
        private System.Windows.Forms.PictureBox pictureBox_bin;
        private System.Windows.Forms.NumericUpDown numericUpDown_threshold;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
