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
using System.Runtime.InteropServices;
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

        private Image<Bgr,byte> preprocessModelImage=null;
        private readonly PuzzleRecognizerImpl impl;
        private readonly Image<Bgr, byte> modelImage;
        private readonly double uniquenessThreshold;
        private PuzzleRecognizerListener listener;


        public PuzzleRecognizer(Image<Bgr,byte> modelImage,double uniquenessThreshold,PuzzleRecognizerImpl impl)
        {
            this.modelImage = modelImage;
            this.uniquenessThreshold = uniquenessThreshold;
            this.impl = impl;
        }

        public void PreprocessModelImage()
        {
            preprocessModelImage = new Image<Bgr, byte>(modelImage.Size);
            VisualSystem.ExtendColor(modelImage,preprocessModelImage);
            VisualSystem.WhiteBalance(preprocessModelImage,preprocessModelImage);
        }

        public bool ModelImagePreprocessIsDone()
        {
            return preprocessModelImage!=null;
        }

        public RecognizeResult Recognize(Image<Bgr,byte> image)
        {
            Image<Bgr,byte> observedImage = image.Clone();

            VisualSystem.ExtendColor(observedImage,observedImage);
            VisualSystem.WhiteBalance(observedImage,observedImage);

            long matchTime;

            RecognizeResult result = new RecognizeResult();

            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            Mat mask;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                MatchFeaturePoints(observedImage.Mat,preprocessModelImage.Mat,out modelKeyPoints,out observedKeyPoints,matches, out mask, out matchTime);
                Mat homography = FindHomography(modelKeyPoints, observedKeyPoints, matches, mask);

                //draw a rectangle along the projected model
                Rectangle rect = new Rectangle(Point.Empty, preprocessModelImage.Size);

                PointF[] pts = new PointF[]
                {
                      new PointF(rect.Left, rect.Bottom),
                      new PointF(rect.Right, rect.Bottom),
                      new PointF(rect.Right, rect.Top),
                      new PointF(rect.Left, rect.Top)
                };
                var prespective_pts = CvInvoke.PerspectiveTransform(pts, homography);
                if (listener != null)
                    listener.OnPerspective(observedImage.Mat, preprocessModelImage.Mat, homography, prespective_pts);

                result.pts = prespective_pts;
    #if NETFX_CORE
           Point[] points = Extensions.ConvertAll<PointF, Point>(pts, Point.Round);
    #else
                Point[] points = Array.ConvertAll<PointF, Point>(result.pts, Point.Round);
                
    #endif
                using (VectorOfPoint vp = new VectorOfPoint(points))
                {
                    double Slope = Math.Atan2(points[2].Y - points[3].Y, points[2].X - points[3].X) * (180 / Math.PI);
                    var a = GetDoubleValue(homography, 0, 1);
                    var b = GetDoubleValue(homography, 0, 0);
                    double Angle = - Math.Atan2(a,b) * 180 / Math.PI;

                    int w = Math.Abs(points[2].X - points[3].X);
                    int h = Math.Abs(points[0].Y - points[3].Y);

                    if (w < 50 && h < 50)
                    {
                        h = Math.Abs(points[3].X - points[0].X);
                        w = Math.Abs(points[1].Y - points[0].Y);
                    }

                    int width = preprocessModelImage.Width,
                        height = preprocessModelImage.Height;
                    //if (w >= modelImage.Width * 0.75 && h >= modelImage.Height * 0.75 && w < modelImage.Width * 1.3 && h < modelImage.Height * 1.3)
                    //{ Check_Image_Bool = true; }

                    result.Angle = Angle;

                    #region draw the projected region on the image
                    //Draw the matched keypoints
                    if (listener != null)
                        listener.OnMatched(preprocessModelImage, modelKeyPoints, observedImage, vp, observedKeyPoints, matches, mask, matchTime, Slope, Angle);
                    #endregion draw the projected region on the image

                }
            }
            int x_P = (int)Math.Abs(result.pts[2].X + result.pts[3].X) / 2;
            int y_P = (int)Math.Abs(result.pts[0].Y + result.pts[3].Y) / 2;

            if (Math.Abs(result.Angle) == 90)
            {
                x_P = (int)Math.Abs(result.pts[3].X + result.pts[0].X) / 2;
                y_P = (int)Math.Abs(result.pts[1].Y + result.pts[0].Y) / 2;
            }

            int x = (int)x_P / (preprocessModelImage.Width / 7);
            int y = (int)y_P / (preprocessModelImage.Height / 5);

            if (x >= 7)
            { x = 6; }
            if (y >= 5)
            { y = 4; }
            result.position=y.ToString() + x.ToString();

            return result; 
        }

        //四捨五入至想要位數
        private static double Round(double value, int d)
        {
            return Math.Round(value / Math.Pow(10, d)) * Math.Pow(10, d);
        }


        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        private void MatchFeaturePoints(Mat observedImage,Mat modelImage,out VectorOfKeyPoint modelKeyPoints,out VectorOfKeyPoint observedKeyPoints,VectorOfVectorOfDMatch matches,out Mat mask, out long matchTime)
        {
            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();
            matchTime = FindFeaturePointsAndMatch(modelImage,observedImage,modelKeyPoints,observedKeyPoints, matches); 
            mask = GetMaskFromMatches(matches);
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

        private double GetDoubleValue(Mat mat, int row, int col)
        {
            var value = new double[1];
            Marshal.Copy(mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, value, 0, 1);
            return value[0];
        }

        private void SetDoubleValue(Mat mat, int row, int col, double value)
        {
            var target = new[] { value };
            Marshal.Copy(target, 0, mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, 1);
        }

        public void setListener(PuzzleRecognizerListener listener)
        {
            this.listener = listener;
        }
    }
}
