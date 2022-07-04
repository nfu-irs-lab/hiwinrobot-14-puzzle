using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.ui.component;

namespace ExclusiveProgram
{
    public partial class Control : MainForm.ExclusiveControl
    {

        public Control()
        {
            InitializeComponent();
            Config = new Config();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            corrector_binarization_puzzleView.Controls.Clear();
            recognizer_puzzleView.Controls.Clear();
            corrector_ROI_puzzleView.Controls.Clear();

            var minSize = new Size((int)min_width_numeric.Value,(int)min_height_numeric.Value);
            var maxSize = new Size((int)max_width_numeric.Value,(int)max_height_numeric.Value);
            var threshold = (int)numericUpDown_threshold.Value;
            var locator = new PuzzleLocator(threshold,minSize,maxSize);
            locator.setListener(new MyPuzzleLocatorListener(this));


            var corrector_threshold = (int)corrector_threshold_numric.Value;
            Color backgroundColor = getColorFromTextBox();
            var corrector = new PuzzleCorrector(corrector_threshold,backgroundColor);
            corrector.setListener(new MyPuzzleCorrectorListener(this));

            var modelImage = VisualSystem.LoadImageFromFile("samples\\modelImage.jpg");
            var recognizer = new PuzzleRecognizer(modelImage);

            var factory = new DefaultPuzzleFactory(locator,recognizer,corrector);
            var image = VisualSystem.LoadImageFromFile(file_path.Text);

            capture_preview.Image = image.ToBitmap();
            try
            {
                List<Puzzle_sturct> results = factory.Execute(image);

                foreach (Puzzle_sturct result in results)
                {
                    //Point StartPoint = new Point((int)(location.Coordinate.X - (location.Size.Width / 2.0f)), (int)(location.Coordinate.Y - (location.Size.Height / 2.0f)));
                    //CvInvoke.Rectangle(showImage, new Rectangle(StartPoint.X, StartPoint.Y, location.Size.Width, location.Size.Height), new MCvScalar(0, 0, 255), 3);
                    //CvInvoke.PutText(showImage, "Angle:" + location.Angle, new Point(StartPoint.X, StartPoint.Y - 15),FontFace.HersheySimplex,2, new MCvScalar(0, 0, 255),3);
                    var co = new UserControl1();
                    co.setImage(result.image.ToBitmap());
                    co.setLabel(result.Angel + "", result.coordinate.ToString());
                    //p.Image = VisualSystem.Mat2Image<Bgr>(result.image).ToBitmap();
                    recognizer_puzzleView.Controls.Add(co);

                    Console.WriteLine("" + result.position);
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Error:"+ex.Message);
                MessageBox.Show(ex.Message,"辨識錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "D:\\git_projects\\Windows\\nfu-irs-lab\\hiwinrobot-14-puzzle\\ExclusiveProgram\\bin\\x64\\Debug" ;
            openFileDialog1.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png" ;
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true ;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                file_path.Text = selectedFileName;
                //...
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void backgroundColor_textbox_TextChanged(object sender, EventArgs e)
        {
            var colorCode = backgroundColor_textbox.Text;
            if (colorCode == null || colorCode.Length != 7 || !colorCode.StartsWith("#"))
                return;
            Color preview_color = getColorFromTextBox(); 
            Bitmap Bmp = new Bitmap(100, 100);
            using (Graphics gfx = Graphics.FromImage(Bmp))
            using (SolidBrush brush = new SolidBrush(preview_color))
            {
                gfx.FillRectangle(brush, 0, 0, 100, 100);
            }
            backgroundColor_preview.Image = Bmp;
        }

        private Color getColorFromTextBox()
        {
            String colorCode = backgroundColor_textbox.Text;
            return (Color)new ColorConverter().ConvertFromString(colorCode);
        }


        private void corrector_binarization_puzzleView_Paint(object sender, PaintEventArgs e)
        {

        }

        private class MyPuzzleCorrectorListener : PuzzleCorrectorListener
        {
            public MyPuzzleCorrectorListener(Control ui)
            {
                this.ui = ui;
            }

            private readonly Control ui;


            public void onBinarizationDone(Image<Gray, byte> result)
            {

            }
            public void onPreprocessDone(Image<Gray, byte> result)
            {
                var control = new UserControl1();
                control.setImage(result.ToBitmap());
                control.setLabel("", "");
                this.ui.corrector_binarization_puzzleView.Controls.Add(control);
            }

            public void onROIDetected(Image<Bgr, byte> result, LocationResult locationResult)
            {
                var control = new UserControl1();
                control.setImage(result.ToBitmap());
                control.setLabel(locationResult.Size.ToString(),""+locationResult.Angle);
                this.ui.corrector_ROI_puzzleView.Controls.Add(control);
            }
        }

        private class MyPuzzleLocatorListener : PuzzleLocatorListener
        {

            public MyPuzzleLocatorListener(Control ui)
            {
                this.ui = ui;
            }

            private readonly Control ui;

            public void onBinarizationDone(Image<Gray, byte> result)
            {
            }

            public void onLocated(LocationResult result)
            {

            }

            public void onPreprocessDone(Image<Gray, byte> result)
            {
                ui.capture_binarization_preview.Image=result.ToBitmap();
            }
        }

    }

}
