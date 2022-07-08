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
/*
 * https://www.796t.com/post/OXRpbnE=.html
 * https://www.twblogs.net/a/5cb5e690bd9eee0eff45642b
 */
namespace ExclusiveProgram.puzzle.visual.concrete
{

    public class PuzzleRecognizer : IPuzzleRecognizer
    {
        
        private static bool Check_Image_Bool = true;

        public List<int> Check_Error_Index = new List<int>();
        private readonly Image<Bgr,byte> readOnlyModleImage;
        
        private readonly PuzzleRecognizerImpl impl;
        private readonly double uniquenessThreshold;

        public PuzzleRecognizer()
        {
        }

        public PuzzleRecognizer(Image<Bgr,byte> modelImage,double uniquenessThreshold,PuzzleRecognizerImpl impl)
        {

            this.uniquenessThreshold = uniquenessThreshold;
            this.impl = impl;

            //-------------------------------------------------------------------------------------
            var clonedModelImage = modelImage.Clone();
            //樣板圖片
            VisualSystem.ExtendColor(clonedModelImage,clonedModelImage);
            VisualSystem.WhiteBalance(clonedModelImage,clonedModelImage);

            readOnlyModleImage = clonedModelImage;

            //-------------------------------------------------------------------------------------

            //Mat Whole_Inside_Puzzle = new Mat(@"C:\Emgu.CV.Mat.jpg");
        }

        public RecognizeResult Recognize(Image<Bgr,byte> image)
        {
            /*
            List<RecognizeResult> puzzles = new List<RecognizeResult>();
            Stopwatch time;
            time = Stopwatch.StartNew();
            */
            var modelImage = readOnlyModleImage.Clone().Mat;

            //Puzzle[i].image.Dispose();
            //-------------------------------------------------------------------------------------
            //Image<Bgr, byte> register_Puzzle = new Image<Bgr, byte>($@"C:\Users\HIWIN\Desktop\第十三屆上銀程式\Tzu-Fu\HIWIN_Robot_13th_puzzle\Images\{i}.jpg");
            //result Answer = Test(Puzzle[i].image, Whole_Inside_Puzzle);
            //-------------------------------------------------------------------------------------
            Image<Bgr,byte> register_Puzzle = image.Clone();

            VisualSystem.ExtendColor(register_Puzzle,register_Puzzle);
            VisualSystem.WhiteBalance(register_Puzzle,register_Puzzle);

            long matchTime;
            long score;
            RecognizeResult result = MatchFeaturePoints(register_Puzzle.Mat,modelImage, out matchTime);

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

            if (Math.Abs(result.Angle) == 90)
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
            result.position=y.ToString() + x.ToString();

            /*
            Puzzle_sturct register = new Puzzle_sturct();
            register.coordinate = coordinate;
            register.position = y.ToString() + x.ToString();
            register.Angel = result.Angle + angle;

            register_Puzzle.Dispose();
            //register.puzzle_region = Puzzle[i].puzzle_region;

            //register.image.Save(path_image_folder + "test" + i.ToString() + "_" + register.position + ".jpg");

            return register;
            */

            /*
            foreach (var k in Check_Error_Index)
            {
                Puzzle.RemoveAt(k);6
            }

            time.Stop();
            long Time = time.ElapsedMilliseconds;
            */

            //return Time;
            return result; 
        }
        
        private RecognizeResult doSingleRecognize(Mat modelImage,Mat image)
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
            RecognizeResult result = MatchFeaturePoints(register_Puzzle.Mat,modelImage, out matchTime);

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

            if (Math.Abs(result.Angle) == 90)
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
            result.position=y.ToString() + x.ToString();

            /*
            Puzzle_sturct register = new Puzzle_sturct();
            register.coordinate = coordinate;
            register.position = y.ToString() + x.ToString();
            register.Angel = result.Angle + angle;

            register_Puzzle.Dispose();
            //register.puzzle_region = Puzzle[i].puzzle_region;

            //register.image.Save(path_image_folder + "test" + i.ToString() + "_" + register.position + ".jpg");

            return register;
            */

            return result;
        }

        //四捨五入至想要位數
        private static double Round(double value, int d)
        {
            return Math.Round(value / Math.Pow(10, d)) * Math.Pow(10, d);
        }


        int a = 0;
        private PuzzleRecognizerListener listener;

        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        private RecognizeResult MatchFeaturePoints(Mat observedImage,Mat modelImage, out long matchTime)
        {
            RecognizeResult result_ = new RecognizeResult();
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
                VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
                Mat mask;
                matchTime = FindFeaturePointsAndMatch(modelImage, observedImage,modelKeyPoints, observedKeyPoints, matches);

                mask = GetMaskFromMatches(matches);

                // Calculate score based on matches size
                // ---------------------------------------------->
                long score = 0;
                for (int i = 0; i < matches.Size; i++)
                {
                    if (mask.GetRawData(i)[0] == 0) continue;
                    foreach (var e in matches[i].ToArray())
                        ++score;
                }
                // <----------------------------------------------

                Mat homography = FindHomography(modelKeyPoints, observedKeyPoints, matches, mask);

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
                if (listener != null)
                    listener.OnPerspective(observedImage,modelImage,homography);

                result_.pts = pts;
#if NETFX_CORE
           Point[] points = Extensions.ConvertAll<PointF, Point>(pts, Point.Round);
#else
                Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
#endif
                using (VectorOfPoint vp = new VectorOfPoint(points))
                {
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

                    result_.Angle = Angel;

                    #region draw the projected region on the image
                    //Draw the matched keypoints
                    if (listener != null)
                        listener.OnMatched(modelImage.ToImage<Bgr,byte>(), modelKeyPoints, observedImage.ToImage<Bgr,byte>(),vp,observedKeyPoints,matches, mask,matchTime,Angel);
                    #endregion draw the projected region on the image

                }
                return result_;
            }
        }

        private long FindFeaturePointsAndMatch(Mat modelImage, Mat observedImage, VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints,VectorOfVectorOfDMatch matches)
        {
            //using (UMat uModelImage = modelImage.GetUMat(AccessType.ReadWrite))
            //using (UMat uObservedImage = observedImage.GetUMat(AccessType.ReadWrite))
            //{
            //}
            Stopwatch watch = Stopwatch.StartNew();

            Mat modelDescriptors = new Mat();
            impl.DetectFeatures(modelImage, null, modelKeyPoints, modelDescriptors, false);
            
            Mat observedDescriptors = new Mat();
            impl.DetectFeatures(observedImage, null, observedKeyPoints, observedDescriptors, false);

            impl.MatchFeatures(modelDescriptors,observedDescriptors,modelKeyPoints,observedKeyPoints,matches);

            return watch.ElapsedMilliseconds;
        }

        private Mat FindHomography(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, Mat mask)
        {
            int nonZeroCount = CvInvoke.CountNonZero(mask);
            if (nonZeroCount >= 4)
            {
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                {
                    Mat homography= Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
                    if(homography == null)
                        throw new Exception("Matrix can not be recoverd.");

                    return homography;
                }
            }

            throw new Exception("No enough non-zero element.(>=4)");
        }

        private Mat GetMaskFromMatches(VectorOfVectorOfDMatch matches)
        {
            var mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));
            Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);
            return mask;
        }

        public void setListener(PuzzleRecognizerListener listener)
        {
            this.listener = listener;
        }
    }
}
