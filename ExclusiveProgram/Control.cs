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
        private readonly IPuzzleRecognizer recognizer;
        private readonly IPuzzleLocator locator;
        private readonly IPuzzleCorrector corrector;

        public Control()
        {
            InitializeComponent();
            Config = new Config();
            locator = new PuzzleLocator(new Size(300,300),new Size(100000,100000));
            corrector = new PuzzleCorrector(240);
            var image = VisualSystem.LoadImageFromFile("samples\\modelImage.jpg");
            recognizer = new PuzzleRecognizer(image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var factory = new DefaultPuzzleFactory((int)numericUpDown_threshold.Value,locator,recognizer,corrector);
            var image = VisualSystem.LoadImageFromFile("samples\\Test.jpg");

            List<Puzzle_sturct> results = factory.Execute(image);

            foreach (Puzzle_sturct result in results)
            {
                //Point StartPoint = new Point((int)(location.Coordinate.X - (location.Size.Width / 2.0f)), (int)(location.Coordinate.Y - (location.Size.Height / 2.0f)));
                //CvInvoke.Rectangle(showImage, new Rectangle(StartPoint.X, StartPoint.Y, location.Size.Width, location.Size.Height), new MCvScalar(0, 0, 255), 3);
                //CvInvoke.PutText(showImage, "Angle:" + location.Angle, new Point(StartPoint.X, StartPoint.Y - 15),FontFace.HersheySimplex,2, new MCvScalar(0, 0, 255),3);
                var co= new UserControl1();
                co.setImage(VisualSystem.Mat2Image<Bgr>(result.image).ToBitmap());
                co.setLabel(result.Angel, result.coordinate);
                //p.Image = VisualSystem.Mat2Image<Bgr>(result.image).ToBitmap();
                puzzleView.Controls.Add(co);

                Console.WriteLine("" + result.position);
            }
        }

    }
}
