using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.concrete.locator;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.ui.component;

namespace ExclusiveProgram
{


    public partial class Control : MainForm.ExclusiveControl
    {
        //private VideoCapture capture;
        private delegate void DelShowResult(Puzzle_sturct puzzles);


        public Control()
        {
            InitializeComponent();
            Config = new Config();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            corrector_binarization_puzzleView.Controls.Clear();
            corrector_result_puzzleView.Controls.Clear();
            corrector_ROI_puzzleView.Controls.Clear();
            recognize_match_puzzleView.Controls.Clear();
            Thread thread = new Thread(DoPuzzleVisual);
            thread.Start();
        }

        private void DoPuzzleVisual()
        {

            var minSize = new Size((int)min_width_numeric.Value, (int)min_height_numeric.Value);
            var maxSize = new Size((int)max_width_numeric.Value, (int)max_height_numeric.Value);
            var blockSize = (int)numericUpDown_blockSize.Value;
            var param1 = Double.Parse(textBox_param.Text);

            IPuzzleFactory factory = null;
            try
            {
                var preProcessImpl = new GreenBackgroundPuzzlePreProcessImpl(blockSize, param1);

                var locator = new PuzzleLocator(minSize, maxSize);
                var uniquenessThreshold = ((double)numericUpDown_uniqueness_threshold.Value) * 0.01f;


                Color backgroundColor = getColorFromTextBox();
                var corrector = new PuzzleCorrector(preProcessImpl, backgroundColor);

                var modelImage = CvInvoke.Imread("samples\\modelImage3.jpg").ToImage<Bgr, byte>();
                var recognizer = new PuzzleRecognizer(modelImage, uniquenessThreshold, new SiftFlannPuzzleRecognizerImpl(), corrector);
                recognizer.setListener(new MyRecognizeListener(this));

                factory = new DefaultPuzzleFactory(locator,corrector, recognizer, new PuzzleResultMerger(), preProcessImpl, 5);
                factory.setListener(new MyFactoryListener(this));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "辨識錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var image = CvInvoke.Imread(file_path.Text).ToImage<Bgr, byte>();
                capture_preview.Image = image.ToBitmap();
                List<Puzzle_sturct> results = factory.Execute(image);

                foreach (Puzzle_sturct result in results)
                {
                    ShowResult(result);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "辨識錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowResult(Puzzle_sturct result)
        {
            if (this.InvokeRequired)
            {
                DelShowResult del = new DelShowResult(ShowResult);
                this.Invoke(del, result);
            }
            else
            {
                var control = new UserControl1();
                control.setImage(result.image.ToBitmap());
                control.setLabel("Angle:" + Math.Round(result.Angel, 2), result.position);
                recognize_match_puzzleView.Controls.Add(control);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "D:\\git_projects\\Windows\\nfu-irs-lab\\hiwinrobot-14-puzzle\\ExclusiveProgram\\bin\\x64\\Debug";
            openFileDialog1.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
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



        private class MyRecognizeListener : PuzzleRecognizerListener
        {
            int index = 0;
            public MyRecognizeListener(Control ui)
            {
                this.ui = ui;
            }

            private readonly Control ui;

            public void OnMatched(Image<Bgr, byte> modelImage, VectorOfKeyPoint modelKeyPoints, Image<Bgr, byte> observedImage, VectorOfPoint vp, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, Mat mask, long matchTime, double Slope, double Angle)
            {
                Mat resultImage = new Mat();
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                   matches, resultImage, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask);

                CvInvoke.PutText(resultImage, string.Format("{0}ms , Slop:{1:0.00} , Angle:{2:0.00}", matchTime, Slope, Angle), new Point(1, 50), FontFace.HersheySimplex, 1, new MCvScalar(100, 100, 255), 2, LineType.FourConnected);

                resultImage.Save("matching_results\\" + index + ".jpg");
                resultImage.Dispose();
            }

            public void OnPerspective(Mat observedImage, Mat modelImage, Mat homography, PointF[] kk,double Angle)
            {
                var perspective_image = new Mat(observedImage.Size, observedImage.Depth, 3);
                CvInvoke.WarpPerspective(modelImage, perspective_image, homography, observedImage.Size);
                observedImage.Save("perspective_results\\R" + index + ".jpg");
                perspective_image.Save("perspective_results\\P" + index + ".jpg");
                /*
                Mat inv_homography=homography.Clone();
                CvInvoke.Invert(inv_homography, inv_homography,DecompMethod.Svd);
                var inv_perspective_image = new Mat(modelImage.Size, modelImage.Depth,3);
                CvInvoke.WarpPerspective(observedImage, inv_perspective_image, inv_homography, modelImage.Size);
                inv_perspective_image.Save("perspective_results\\G"+ index + ".jpg");
                */



                Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);

                PointF[] points_on_modelImage = new PointF[]
                {
                      new PointF(rect.Left, rect.Bottom),
                      new PointF(rect.Right, rect.Bottom),
                      new PointF(rect.Right, rect.Top),
                      new PointF(rect.Left, rect.Top)
                };

                var perspective_pts = CvInvoke.PerspectiveTransform(points_on_modelImage, homography);

                var p = modelImage.Clone();

                Point[] points = Array.ConvertAll<PointF, Point>(perspective_pts, Point.Round);


                int x_P = (int)Math.Abs(perspective_pts[2].X + perspective_pts[3].X) / 2;
                int y_P = (int)Math.Abs(perspective_pts[0].Y + perspective_pts[3].Y) / 2;

                if (Math.Abs(Angle) == 90)
                {
                    x_P = (int)Math.Abs(perspective_pts[3].X + perspective_pts[0].X) / 2;
                    y_P = (int)Math.Abs(perspective_pts[1].Y + perspective_pts[0].Y) / 2;
                }

                int x = (int)x_P / (modelImage.Width / 7);
                int y = (int)y_P / (modelImage.Height / 5);

                if (x >= 7)
                { x = 6; }
                if (y >= 5)
                { y = 4; }

                var a=y.ToString() + x.ToString();
                CvInvoke.PutText(p,a, new Point(1, 50), FontFace.HersheySimplex, 1, new MCvScalar(100, 100, 255), 2, LineType.FourConnected);
                p.Save("perspective_results\\A" + index + ".jpg");

            }

            public void OnCorrected(Image<Bgr, byte> image)
            {
            }
        }

        private class MyFactoryListener : PuzzleFactoryListener
        {
            private delegate void DelonLocated(List<LocationResult> results);
            private delegate void DelOnCorrected(Image<Bgr, byte> result);
            private delegate void DelonPreprocessDone(Image<Gray, byte> result);
            public MyFactoryListener(Control ui)
            {
                this.ui = ui;
            }

            private readonly Control ui;

            public void onPreprocessDone(Image<Gray, byte> result)
            {
                if (this.ui.InvokeRequired)
                {
                    DelonPreprocessDone del = new DelonPreprocessDone(onPreprocessDone);
                    this.ui.Invoke(del, result);
                }
                else
                {

                    ui.capture_binarization_preview.Image = result.ToBitmap();
                }
            }

            public void onLocated(List<LocationResult> results)
            {
                if (ui.InvokeRequired)
                {
                    DelonLocated del = new DelonLocated(onLocated);
                    ui.Invoke(del, results);
                }
                else
                {
                    foreach (var result in results)
                    {
                        var control = new UserControl1();
                        control.setImage(result.ROI.ToBitmap());
                        control.setLabel(result.Size.ToString(), result.Coordinate.ToString());
                        ui.corrector_ROI_puzzleView.Controls.Add(control);
                    }
                }
            }



            public void onRecognized(RecognizeResult result)
            {
            }

            public void onCorrected(Image<Bgr, byte> result)
            {
                if (ui.InvokeRequired)
                {
                    DelOnCorrected del = new DelOnCorrected(onCorrected);
                    ui.Invoke(del, result);
                }
                else
                {
                    var control = new UserControl1();
                    control.setImage(result.ToBitmap());
                    control.setLabel("", "");
                    this.ui.corrector_result_puzzleView.Controls.Add(control);
                }
            }
        }



    }

}
