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
            this.puzzleView = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_raw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preprocess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_puzzle_location)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(103, 4);
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
            this.pictureBox_raw.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_raw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_raw.TabIndex = 1;
            this.pictureBox_raw.TabStop = false;
            // 
            // pictureBox_preprocess
            // 
            this.pictureBox_preprocess.Location = new System.Drawing.Point(209, 3);
            this.pictureBox_preprocess.Name = "pictureBox_preprocess";
            this.pictureBox_preprocess.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_preprocess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_preprocess.TabIndex = 2;
            this.pictureBox_preprocess.TabStop = false;
            // 
            // pictureBox_puzzle_location
            // 
            this.pictureBox_puzzle_location.Location = new System.Drawing.Point(621, 3);
            this.pictureBox_puzzle_location.Name = "pictureBox_puzzle_location";
            this.pictureBox_puzzle_location.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_puzzle_location.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_puzzle_location.TabIndex = 3;
            this.pictureBox_puzzle_location.TabStop = false;
            // 
            // pictureBox_bin
            // 
            this.pictureBox_bin.Location = new System.Drawing.Point(415, 3);
            this.pictureBox_bin.Name = "pictureBox_bin";
            this.pictureBox_bin.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_bin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_bin.TabIndex = 4;
            this.pictureBox_bin.TabStop = false;
            // 
            // numericUpDown_threshold
            // 
            this.numericUpDown_threshold.Location = new System.Drawing.Point(14, 11);
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
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_raw);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_preprocess);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_bin);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_puzzle_location);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1087, 208);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // puzzleView
            // 
            this.puzzleView.AutoScroll = true;
            this.puzzleView.Location = new System.Drawing.Point(6, 6);
            this.puzzleView.Name = "puzzleView";
            this.puzzleView.Size = new System.Drawing.Size(1044, 353);
            this.puzzleView.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1061, 385);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.numericUpDown_threshold);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1053, 359);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flowLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1115, 359);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.puzzleView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1053, 359);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Control";
            this.Size = new System.Drawing.Size(1101, 481);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_raw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preprocess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_puzzle_location)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threshold)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
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
        private System.Windows.Forms.FlowLayoutPanel puzzleView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
    }
}
