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
            this.numericUpDown_blockSize = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_param = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_uniqueness_threshold = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.backgroundColor_preview = new System.Windows.Forms.PictureBox();
            this.backgroundColor_textbox = new System.Windows.Forms.TextBox();
            this.max_width_numeric = new System.Windows.Forms.NumericUpDown();
            this.max_height_numeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.min_width_numeric = new System.Windows.Forms.NumericUpDown();
            this.min_height_numeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.file_path = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.capture_preview = new System.Windows.Forms.PictureBox();
            this.capture_binarization_preview = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.corrector_ROI_puzzleView = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.corrector_binarization_puzzleView = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.corrector_result_puzzleView = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.recognize_match_puzzleView = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blockSize)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_uniqueness_threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColor_preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_width_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_height_numeric)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.capture_preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.capture_binarization_preview)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 325);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown_blockSize
            // 
            this.numericUpDown_blockSize.Location = new System.Drawing.Point(141, 67);
            this.numericUpDown_blockSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_blockSize.Name = "numericUpDown_blockSize";
            this.numericUpDown_blockSize.Size = new System.Drawing.Size(68, 22);
            this.numericUpDown_blockSize.TabIndex = 5;
            this.numericUpDown_blockSize.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1061, 385);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_param);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.numericUpDown_uniqueness_threshold);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.backgroundColor_preview);
            this.tabPage1.Controls.Add(this.backgroundColor_textbox);
            this.tabPage1.Controls.Add(this.max_width_numeric);
            this.tabPage1.Controls.Add(this.max_height_numeric);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.min_width_numeric);
            this.tabPage1.Controls.Add(this.min_height_numeric);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.file_path);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.numericUpDown_blockSize);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1053, 359);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "參數設定";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_param
            // 
            this.textBox_param.Location = new System.Drawing.Point(342, 67);
            this.textBox_param.Name = "textBox_param";
            this.textBox_param.Size = new System.Drawing.Size(68, 22);
            this.textBox_param.TabIndex = 29;
            this.textBox_param.Text = "0.4";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(233, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "綠色加權值";
            // 
            // numericUpDown_uniqueness_threshold
            // 
            this.numericUpDown_uniqueness_threshold.Location = new System.Drawing.Point(125, 237);
            this.numericUpDown_uniqueness_threshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_uniqueness_threshold.Name = "numericUpDown_uniqueness_threshold";
            this.numericUpDown_uniqueness_threshold.Size = new System.Drawing.Size(68, 22);
            this.numericUpDown_uniqueness_threshold.TabIndex = 25;
            this.numericUpDown_uniqueness_threshold.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(32, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 16);
            this.label9.TabIndex = 24;
            this.label9.Text = "獨特性臨界";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(32, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "桌面顏色";
            // 
            // backgroundColor_preview
            // 
            this.backgroundColor_preview.Location = new System.Drawing.Point(109, 198);
            this.backgroundColor_preview.Name = "backgroundColor_preview";
            this.backgroundColor_preview.Size = new System.Drawing.Size(25, 24);
            this.backgroundColor_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundColor_preview.TabIndex = 20;
            this.backgroundColor_preview.TabStop = false;
            // 
            // backgroundColor_textbox
            // 
            this.backgroundColor_textbox.Location = new System.Drawing.Point(141, 200);
            this.backgroundColor_textbox.Name = "backgroundColor_textbox";
            this.backgroundColor_textbox.Size = new System.Drawing.Size(100, 22);
            this.backgroundColor_textbox.TabIndex = 19;
            this.backgroundColor_textbox.Text = "#5B696A";
            this.backgroundColor_textbox.TextChanged += new System.EventHandler(this.backgroundColor_textbox_TextChanged);
            // 
            // max_width_numeric
            // 
            this.max_width_numeric.Location = new System.Drawing.Point(342, 156);
            this.max_width_numeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.max_width_numeric.Name = "max_width_numeric";
            this.max_width_numeric.Size = new System.Drawing.Size(68, 22);
            this.max_width_numeric.TabIndex = 18;
            this.max_width_numeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // max_height_numeric
            // 
            this.max_height_numeric.Location = new System.Drawing.Point(342, 115);
            this.max_height_numeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.max_height_numeric.Name = "max_height_numeric";
            this.max_height_numeric.Size = new System.Drawing.Size(68, 22);
            this.max_height_numeric.TabIndex = 17;
            this.max_height_numeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(233, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "拼圖最大寬度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(233, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 16);
            this.label6.TabIndex = 15;
            this.label6.Text = "拼圖最大長度";
            // 
            // min_width_numeric
            // 
            this.min_width_numeric.Location = new System.Drawing.Point(141, 156);
            this.min_width_numeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.min_width_numeric.Name = "min_width_numeric";
            this.min_width_numeric.Size = new System.Drawing.Size(68, 22);
            this.min_width_numeric.TabIndex = 14;
            this.min_width_numeric.Value = new decimal(new int[] {
            175,
            0,
            0,
            0});
            // 
            // min_height_numeric
            // 
            this.min_height_numeric.Location = new System.Drawing.Point(141, 115);
            this.min_height_numeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.min_height_numeric.Name = "min_height_numeric";
            this.min_height_numeric.Size = new System.Drawing.Size(68, 22);
            this.min_height_numeric.TabIndex = 13;
            this.min_height_numeric.Value = new decimal(new int[] {
            175,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(32, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "拼圖最小寬度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(32, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "拼圖最小長度";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(379, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "擷取";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(32, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "來源";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(32, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "二值化臨界值";
            // 
            // file_path
            // 
            this.file_path.Location = new System.Drawing.Point(150, 24);
            this.file_path.Name = "file_path";
            this.file_path.Size = new System.Drawing.Size(142, 22);
            this.file_path.TabIndex = 7;
            this.file_path.Text = "samples\\\\Test2.jpg";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(298, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "瀏覽";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flowLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1053, 359);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "擷取預覽";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.capture_preview);
            this.flowLayoutPanel1.Controls.Add(this.capture_binarization_preview);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1047, 353);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // capture_preview
            // 
            this.capture_preview.Location = new System.Drawing.Point(3, 3);
            this.capture_preview.Name = "capture_preview";
            this.capture_preview.Size = new System.Drawing.Size(496, 347);
            this.capture_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.capture_preview.TabIndex = 1;
            this.capture_preview.TabStop = false;
            // 
            // capture_binarization_preview
            // 
            this.capture_binarization_preview.Location = new System.Drawing.Point(505, 3);
            this.capture_binarization_preview.Name = "capture_binarization_preview";
            this.capture_binarization_preview.Size = new System.Drawing.Size(496, 347);
            this.capture_binarization_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.capture_binarization_preview.TabIndex = 0;
            this.capture_binarization_preview.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.corrector_ROI_puzzleView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1053, 359);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "拼圖定位";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // corrector_ROI_puzzleView
            // 
            this.corrector_ROI_puzzleView.AutoScroll = true;
            this.corrector_ROI_puzzleView.Location = new System.Drawing.Point(4, 3);
            this.corrector_ROI_puzzleView.Name = "corrector_ROI_puzzleView";
            this.corrector_ROI_puzzleView.Size = new System.Drawing.Size(1044, 353);
            this.corrector_ROI_puzzleView.TabIndex = 11;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.corrector_binarization_puzzleView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1053, 359);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "拼圖校正-二值化";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // corrector_binarization_puzzleView
            // 
            this.corrector_binarization_puzzleView.AutoScroll = true;
            this.corrector_binarization_puzzleView.Location = new System.Drawing.Point(4, 6);
            this.corrector_binarization_puzzleView.Name = "corrector_binarization_puzzleView";
            this.corrector_binarization_puzzleView.Size = new System.Drawing.Size(1044, 347);
            this.corrector_binarization_puzzleView.TabIndex = 8;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.corrector_result_puzzleView);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1053, 359);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "拼圖校正-結果";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // corrector_result_puzzleView
            // 
            this.corrector_result_puzzleView.AutoScroll = true;
            this.corrector_result_puzzleView.Location = new System.Drawing.Point(4, 3);
            this.corrector_result_puzzleView.Name = "corrector_result_puzzleView";
            this.corrector_result_puzzleView.Size = new System.Drawing.Size(1044, 353);
            this.corrector_result_puzzleView.TabIndex = 10;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.recognize_match_puzzleView);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1053, 359);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // recognize_match_puzzleView
            // 
            this.recognize_match_puzzleView.AutoScroll = true;
            this.recognize_match_puzzleView.Location = new System.Drawing.Point(4, 3);
            this.recognize_match_puzzleView.Name = "recognize_match_puzzleView";
            this.recognize_match_puzzleView.Size = new System.Drawing.Size(1044, 353);
            this.recognize_match_puzzleView.TabIndex = 11;
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Control";
            this.Size = new System.Drawing.Size(1101, 481);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blockSize)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_uniqueness_threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColor_preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_width_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_height_numeric)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.capture_preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.capture_binarization_preview)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown_blockSize;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox file_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown max_width_numeric;
        private System.Windows.Forms.NumericUpDown max_height_numeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown min_width_numeric;
        private System.Windows.Forms.NumericUpDown min_height_numeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox backgroundColor_preview;
        private System.Windows.Forms.TextBox backgroundColor_textbox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox capture_binarization_preview;
        private System.Windows.Forms.FlowLayoutPanel corrector_ROI_puzzleView;
        private System.Windows.Forms.FlowLayoutPanel corrector_binarization_puzzleView;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.FlowLayoutPanel corrector_result_puzzleView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox capture_preview;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.FlowLayoutPanel recognize_match_puzzleView;
        private System.Windows.Forms.NumericUpDown numericUpDown_uniqueness_threshold;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_param;
        private System.Windows.Forms.Label label8;
    }
}
