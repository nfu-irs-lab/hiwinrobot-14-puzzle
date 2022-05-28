using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Puzzle.Concrete;
using Puzzle.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExclusiveProgram
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm.MainForm(new Control()));
            //var image=VisualSystem.CaptureImage();
            var image=VisualSystem.LoadImageFromFile("samples\\sample1.png");
            var gray=VisualSystem.BgrToGray(image);
            var bin = VisualSystem.Binarization(gray, 100);

            PuzzleLocator locator = new PuzzleLocator();
            PuzzleCorrector corrector= new PuzzleCorrector(100);

            List<PuzzleData> dataList=locator.Locate(bin);

            var frame = image.Clone();
            foreach (PuzzleData data in dataList)
            {
                Point StartPoint = new Point((int)(data.CentralPosition.X - (data.Size.Width / 2.0f)), (int)(data.CentralPosition.Y - (data.Size.Height / 2.0f)));

                CvInvoke.Rectangle(frame, new Rectangle(StartPoint.X,StartPoint.Y,data.Size.Width, data.Size.Height),new MCvScalar(0,0,255),3);
                CvInvoke.PutText(frame,"Angle:"+data.Angle,new Point(StartPoint.X,StartPoint.Y-20), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, new MCvScalar(0, 0, 255));

                CvInvoke.Imshow("A"+data.Angle,corrector.Correct(image,data));

            }

            CvInvoke.Imshow("raw",frame);
            CvInvoke.Imshow("bin",bin);
            CvInvoke.WaitKey(100000);
            
            //定義結構元素
            //Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            //Erode:侵蝕，Dilate:擴張
            //CvInvoke.Dilate(Out_img, Out_img, Struct_element, new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            //CvInvoke.Erode(Out_img, Out_img, Struct_element, new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));

            
        }
    }
}
