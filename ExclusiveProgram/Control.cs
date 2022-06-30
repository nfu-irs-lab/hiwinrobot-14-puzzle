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
        private readonly PuzzleLocator locator;
        private readonly PuzzleCorrector corrector;

        public Control()
        {
            InitializeComponent();
            Config = new Config();
            locator = new PuzzleLocator(new Size(300,300),new Size(100000,100000));
            corrector = new PuzzleCorrector(240);

            var image = VisualSystem.LoadImageFromFile("samples\\modelImage.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AA();
        }

        private void AA()
        {
            var image = VisualSystem.LoadImageFromFile("samples\\Test.jpg");
            //var image=VisualSystem.LoadImageFromFile("samples\\sample1.png");
            Console.WriteLine("A"+image.Size);
            var dst = ColorImagePreprocess(image);
            Console.WriteLine("A"+dst.Size);
            var bin = BinaryImagePreprocess(dst);

            pictureBox_raw.Image = image.ToBitmap();
            pictureBox_preprocess.Image = dst.ToBitmap();
            pictureBox_bin.Image = bin.ToBitmap();


            List<PuzzleData> dataList = locator.Locate(bin);

            CvInvoke.NamedWindow("raw",WindowFlags.Normal);
            var showImage = image.Clone();

            puzzleView.Controls.Clear();
            int num=0;
            List<Image<Bgr, byte>> imageList = new List<Image<Bgr, byte>>(); 
            foreach (PuzzleData data in dataList)
            {
                Console.WriteLine("Posision:" + data.CentralPosition + "\t Size:"+data.Size+ "\t Angle:"+data.Angle);
                Point StartPoint = new Point((int)(data.CentralPosition.X - (data.Size.Width / 2.0f)), (int)(data.CentralPosition.Y - (data.Size.Height / 2.0f)));

                CvInvoke.Rectangle(showImage, new Rectangle(StartPoint.X, StartPoint.Y, data.Size.Width, data.Size.Height), new MCvScalar(0, 0, 255), 3);
                CvInvoke.PutText(showImage, "Angle:" + data.Angle, new Point(StartPoint.X, StartPoint.Y - 15),FontFace.HersheySimplex,2, new MCvScalar(0, 0, 255),3);
                var i = corrector.Correct(image, data);
                imageList.Add(i);
                Console.WriteLine("CSize:"+i.Size);
                //CvInvoke.Imshow("A"+data.Angle,i);
                var p = new PictureBox();
                p.Image = i.ToBitmap();
                p.Size= new Size(200,200);
                p.SizeMode=PictureBoxSizeMode.StretchImage;
                puzzleView.Controls.Add(p);
            }
            pictureBox_puzzle_location.Image = showImage.ToBitmap();


            CvInvoke.Imshow("raw", showImage);
            CvInvoke.WaitKey(10000);
        }
        private Image<Bgr,byte> ColorImagePreprocess(Image<Bgr,byte> image)
        {
            var dst = new Image<Bgr, byte>(image.Size);
            VisualSystem.WhiteBalance(image,dst);
            VisualSystem.ExtendColor(dst,dst);
            CvInvoke.MedianBlur(dst, dst, 27);
            return dst;
        }
        private Image<Gray,byte> BinaryImagePreprocess(Image<Bgr,byte> image)
        {
            var bin=new Image<Gray, byte>(image.Size);
            CvInvoke.CvtColor(image, bin, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(bin, bin, ((int)numericUpDown_threshold.Value), 255, ThresholdType.Binary);
            return bin;
        }

    }
}
