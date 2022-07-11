using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.concrete;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram
{
    public class RecognizerTest
    {
        private static int index = 0;
        public static void main(string[] args){
            var modelImage = VisualSystem.LoadImageFromFile("samples\\modelImage2.jpg");
            var recognizer = new PuzzleRecognizer(modelImage,0.8,new SiftFlannPuzzleRecognizerImpl());
            recognizer.setListener(new MyTestListener());
            for (int i = 1; i <= 4; i++)
            {
                var results = recognizer.Recognize(VisualSystem.LoadImageFromFile("samples\\Test"+i+".jpg"));
                index = i;
            }

        }

        private class MyTestListener : PuzzleRecognizerListener
        {
            public void OnMatched(Image<Bgr, byte> modelImage, VectorOfKeyPoint modelKeyPoints, Image<Bgr, byte> observedImage, VectorOfPoint vp, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, Mat mask, long matchTime, double Slope, double Angle)
            {
                Mat resultImage = new Mat();
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                   matches, resultImage, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask);

                CvInvoke.PutText(resultImage, string.Format("{0}ms , Slop:{1:0.00} , Angle:{2:0.00}", matchTime, Slope,Angle), new Point(1, 50), FontFace.HersheySimplex, 1, new MCvScalar(100, 100, 255), 2, LineType.FourConnected);
                resultImage.Save("r"+index+".jpg");
            }

            public void OnPerspective(Mat observedImage, Mat pts, Mat homography, System.Drawing.PointF[] prespective_pts)
            {

            }
        }

    }
}
