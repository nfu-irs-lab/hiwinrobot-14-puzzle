using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{



    public class PuzzleRecognizer : IPuzzleRecognizer
    {
        
        private static bool Check_Image_Bool = true;

        public List<int> Check_Error_Index = new List<int>();
        private readonly Image<Bgr,byte> readOnlyModleImage;

        public PuzzleRecognizer(Image<Bgr,byte> modelImage)
        {
            //-------------------------------------------------------------------------------------
            var clonedModelImage = modelImage.Clone();
            //樣板圖片
            VisualSystem.ExtendColor(clonedModelImage,clonedModelImage);
            VisualSystem.WhiteBalance(clonedModelImage,clonedModelImage);

            readOnlyModleImage = clonedModelImage;

            //-------------------------------------------------------------------------------------

            //Mat Whole_Inside_Puzzle = new Mat(@"C:\Emgu.CV.Mat.jpg");
        }

        public List<Puzzle_sturct> Recognize(List<CorrectedPuzzleArgument> arguments)
        {
            List<Puzzle_sturct> puzzles = new List<Puzzle_sturct>();
            Stopwatch time;

            time = Stopwatch.StartNew();
        
            foreach(CorrectedPuzzleArgument argument in arguments)
            {
                puzzles.Add(doSingleRecognize(readOnlyModleImage.Clone().Mat,argument.image.Clone().Mat, argument.Angle, argument.Coordinate));
            }
            

            foreach (var k in Check_Error_Index)
            {
                //Puzzle.RemoveAt(k);6
            }

            time.Stop();
            long Time = time.ElapsedMilliseconds;

            return puzzles;
            //return Time;
        }
        
        private Puzzle_sturct doSingleRecognize(Mat modelImage,Mat image,double angle,Point coordinate)
        {
            //Puzzle[i].image.Dispose();
            //-------------------------------------------------------------------------------------
            //Image<Bgr, byte> register_Puzzle = new Image<Bgr, byte>($@"C:\Users\HIWIN\Desktop\第十三屆上銀程式\Tzu-Fu\HIWIN_Robot_13th_puzzle\Images\{i}.jpg");
            //result Answer = Test(Puzzle[i].image, Whole_Inside_Puzzle);
            //-------------------------------------------------------------------------------------
            Image<Bgr,byte> register_Puzzle = image.ToImage<Bgr, byte>();

            VisualSystem.ExtendColor(register_Puzzle,register_Puzzle);
            VisualSystem.WhiteBalance(register_Puzzle,register_Puzzle);

            long matchTime;
            long score;
            RecognizeResult result= MatchFeaturePoints(register_Puzzle.Mat,modelImage, out matchTime, out score);

            //-------------------------------------------------------------------------------------
            /*
            if (Answer.image == null || Answer.pts == null)
            {
                Check_Error_Index.Add(i);
                continue;
            }
            */
            

            int x_P = (int)Math.Abs(result.pts[2].X + result.pts[3].X) / 2;
            int y_P = (int)Math.Abs(result.pts[0].Y + result.pts[3].Y) / 2;

            if (Math.Abs(result.Angel) == 90)
            {
                x_P = (int)Math.Abs(result.pts[3].X + result.pts[0].X) / 2;
                y_P = (int)Math.Abs(result.pts[1].Y + result.pts[0].Y) / 2;
            }

            int x = (int)x_P / (modelImage.Width / 7);
            int y = (int)y_P / (modelImage.Height / 5);

            if (x >= 7)
            { x = 6; }
            if (y >= 5)
            { y = 4; }

            Puzzle_sturct register = new Puzzle_sturct();
            register.image = register_Puzzle.Clone();

            register.coordinate = coordinate;
            register.position = y.ToString() + x.ToString();
            register.Angel = result.Angel + angle;

            register_Puzzle.Dispose();
            //register.puzzle_region = Puzzle[i].puzzle_region;

            //register.image.Save(path_image_folder + "test" + i.ToString() + "_" + register.position + ".jpg");

            return register;
        }

        //四捨五入至想要位數
        private static double Round(double value, int d)
        {
            return Math.Round(value / Math.Pow(10, d)) * Math.Pow(10, d);
        }


        int a = 0;

        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        private RecognizeResult MatchFeaturePoints(Mat observedImage,Mat modelImage, out long matchTime, out long score)
        {
            RecognizeResult result_ = new RecognizeResult();
            Mat homography;
            VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
            VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                CvInvoke.Imshow("ModelImage",modelImage);
                Mat mask;
                FindMatch(modelImage, observedImage, out matchTime, modelKeyPoints, observedKeyPoints, matches,
                   out mask, out homography, out score);

                //Draw the matched keypoints
                Mat resultImage = new Mat();
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                   matches, resultImage, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask);

                #region draw the projected region on the image

                if (homography != null)
                {
                    //draw a rectangle along the projected model
                    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);

                    PointF[] pts = new PointF[]
                    {
                          new PointF(rect.Left, rect.Bottom),
                          new PointF(rect.Right, rect.Bottom),
                          new PointF(rect.Right, rect.Top),
                          new PointF(rect.Left, rect.Top)
                    };
                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    result_.pts = pts;
#if NETFX_CORE
               Point[] points = Extensions.ConvertAll<PointF, Point>(pts, Point.Round);
#else
                    Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
#endif
                    using (VectorOfPoint vp = new VectorOfPoint(points))
                    {
                        //CvInvoke.Polylines(resultImage, vp, true, new MCvScalar(255, 0, 0, 255), 5);
                        double Slope = Math.Atan2(points[2].Y - points[3].Y, points[2].X - points[3].X) * (180 / Math.PI);
                        double Angel = Round((int)Slope, 1);

                        if (Math.Abs(Angel) > 75 && Math.Abs(Angel) < 115)
                        {
                            if (Angel > 0)
                            { Angel = 90; }
                            else
                            { Angel = -90; }
                        }

                        int w = Math.Abs(points[2].X - points[3].X);
                        int h = Math.Abs(points[0].Y - points[3].Y);

                        if (w < 50 && h < 50)
                        {
                            h = Math.Abs(points[3].X - points[0].X);
                            w = Math.Abs(points[1].Y - points[0].Y);
                        }

                        int width = modelImage.Width,
                            height = modelImage.Height;
                        if (w >= modelImage.Width * 0.75 && h >= modelImage.Height * 0.75 && w < modelImage.Width * 1.3 && h < modelImage.Height * 1.3)
                        { Check_Image_Bool = true; }

                        CvInvoke.PutText(resultImage, string.Format("{0}ms , {1:0.00}", matchTime, Slope), new Point(1, 50), FontFace.HersheySimplex, 1, new MCvScalar(100, 100, 255), 3, LineType.FourConnected);
                        result_.Angel = Angel;
                        result_.image = resultImage.ToImage<Bgr,byte>();
                        CvInvoke.Imshow("A"+a++,resultImage);
                    }
                }

                #endregion draw the projected region on the image

                return result_;
            }
        }

        private void FindMatch(Mat modelImage, Mat observedImage, out long matchTime, VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography, out long score)
        {
            int k = 2;
            double uniquenessThreshold = 0.80;

            Stopwatch watch;
            homography = null;

            //using (UMat uModelImage = modelImage.GetUMat(AccessType.ReadWrite))
            //using (UMat uObservedImage = observedImage.GetUMat(AccessType.ReadWrite))
            {
                KAZE featureDetector = new KAZE();

                Mat modelDescriptors = new Mat();
                featureDetector.DetectAndCompute(modelImage, null, modelKeyPoints, modelDescriptors, false);

                watch = Stopwatch.StartNew();

                Mat observedDescriptors = new Mat();
                featureDetector.DetectAndCompute(observedImage, null, observedKeyPoints, observedDescriptors, false);

                // KdTree for faster results / less accuracy
                using (var ip = new Emgu.CV.Flann.KdTreeIndexParams(10))
                using (var sp = new SearchParams(200))

                using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
                {
                    matcher.Add(modelDescriptors);

                    matcher.KnnMatch(observedDescriptors, matches, k, null);
                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));
                    Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                    // Calculate score based on matches size
                    // ---------------------------------------------->
                    score = 0;
                    for (int i = 0; i < matches.Size; i++)
                    {
                        if (mask.GetRawData(i)[0] == 0) continue;
                        foreach (var e in matches[i].ToArray())
                            ++score;
                    }
                    // <----------------------------------------------

                    int nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
                    }
                }
                watch.Stop();
            }
            matchTime = watch.ElapsedMilliseconds;
        }


    }
}
