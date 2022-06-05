using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Puzzle.Concrete;
using Puzzle.Framework;
using RASDK.Arm;
using RASDK.Basic;
using RASDK.Basic.Message;
using RASDK.Gripper;

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
            var image = VisualSystem.LoadImageFromFile("samples\\Puzzles\\puzzle (1).jpg");
            //var image=VisualSystem.LoadImageFromFile("samples\\sample1.png");
            pictureBox_raw.Image = image.ToBitmap();
            var dst = new Image<Bgr, byte>(image.Size);
            CvInvoke.MedianBlur(image, dst, 27);
            dst = VisualSystem.WhiteBalance(dst);
            pictureBox_preprocess.Image = dst.ToBitmap();
            AA(dst);
        }

        private void AA(Image<Bgr, byte> image)
        {

            var gray = VisualSystem.BgrToGray(image);
            var bin = VisualSystem.Binarization(gray,((int)numericUpDown_threshold.Value));
            pictureBox_bin.Image = bin.ToBitmap();


            PuzzleLocator locator = new PuzzleLocator();
            //PuzzleCorrector corrector = new PuzzleCorrector(100);

            List<PuzzleData> dataList = locator.Locate(bin);

            var frame = image.Clone();
            foreach (PuzzleData data in dataList)
            {
                Point StartPoint = new Point((int)(data.CentralPosition.X - (data.Size.Width / 2.0f)), (int)(data.CentralPosition.Y - (data.Size.Height / 2.0f)));

                CvInvoke.Rectangle(frame, new Rectangle(StartPoint.X, StartPoint.Y, data.Size.Width, data.Size.Height), new MCvScalar(0, 0, 255), 3);
                CvInvoke.PutText(frame, "Angle:" + data.Angle, new Point(StartPoint.X, StartPoint.Y - 15), FontFace.HersheyComplex, 0.5, new MCvScalar(0, 0, 255));
                //CvInvoke.Imshow("A"+data.Angle,corrector.Correct(image,data));

            }
            pictureBox_puzzle_location.Image = frame.ToBitmap();
        }
    }
}
