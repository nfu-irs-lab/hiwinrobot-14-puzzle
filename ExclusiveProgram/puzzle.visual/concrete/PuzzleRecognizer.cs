using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
/*
 * https://www.796t.com/post/OXRpbnE=.html
 * https://www.twblogs.net/a/5cb5e690bd9eee0eff45642b
 */
namespace ExclusiveProgram.puzzle.visual.concrete
{

    public class PuzzleRecognizer : IPuzzleRecognizer
    {

        private Image<Bgr, byte> preprocessModelImage = null;
        private readonly PuzzleRecognizerImpl impl;
        private readonly IPuzzlePreProcessImpl puzzlePreProcessImpl;
        private readonly Image<Bgr, byte> modelImage;
        private readonly double uniquenessThreshold;
        private PuzzleRecognizerListener listener;


        public PuzzleRecognizer(Image<Bgr, byte> modelImage, double uniquenessThreshold, PuzzleRecognizerImpl impl, IPuzzlePreProcessImpl puzzlePreProcessImpl)
        {
            this.modelImage = modelImage;
            this.uniquenessThreshold = uniquenessThreshold;
            this.impl = impl;
            this.puzzlePreProcessImpl = puzzlePreProcessImpl;
        }

        public void PreprocessModelImage()
        {
            preprocessModelImage = new Image<Bgr, byte>(modelImage.Size);
            if(puzzlePreProcessImpl!=null)
                puzzlePreProcessImpl.Preprocess(modelImage,preprocessModelImage);
        }

        public bool ModelImagePreprocessIsDone()
        {
            return preprocessModelImage != null;
        }

        public RecognizeResult Recognize(Image<Bgr, byte> image)
        {
            Image<Bgr, byte> observedImage = image.Clone();

            VisualSystem.ExtendColor(observedImage, observedImage);
            //VisualSystem.WhiteBalance(observedImage,observedImage);

            long matchTime;

            RecognizeResult result = new RecognizeResult();

            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            Mat mask;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                MatchFeaturePointsAndFindMask(observedImage.Mat, preprocessModelImage.Mat, out modelKeyPoints, out observedKeyPoints, matches, out mask, out matchTime);

                if (listener != null)
                    listener.OnMatched(preprocessModelImage, modelKeyPoints, observedImage, observedKeyPoints, matches, mask, matchTime);

                Mat homography = FindHomography(modelKeyPoints, observedKeyPoints, matches, mask);

                result.Angle = -Math.Atan2(GetDoubleValue(homography, 0, 1), GetDoubleValue(homography, 0, 0)) * 180 / Math.PI;

                Mat invert_homography = homography.Clone();
                CvInvoke.Invert(invert_homography, invert_homography, DecompMethod.Svd);
                var invert_perspectiveImage = new Mat(preprocessModelImage.Size, preprocessModelImage.Mat.Depth, 3);
                CvInvoke.WarpPerspective(observedImage, invert_perspectiveImage, invert_homography, preprocessModelImage.Size);

                Rectangle rect = new Rectangle(Point.Empty, observedImage.Size);

                /*
                 0     左下
                 1     右下
                 2     右上
                 3     左上
                */
                PointF[] points_on_observedImage = new PointF[]
                {
                      new PointF(rect.Left, rect.Bottom),
                      new PointF(rect.Right, rect.Bottom),
                      new PointF(rect.Right, rect.Top),
                      new PointF(rect.Left, rect.Top)
                };

                var point_on_modelImage = CvInvoke.PerspectiveTransform(points_on_observedImage, invert_homography);
                Point[] points = Array.ConvertAll<PointF, Point>(point_on_modelImage, Point.Round);
                int x_P = (int)Math.Abs(point_on_modelImage[2].X + point_on_modelImage[3].X) / 2;
                int y_P = (int)Math.Abs(point_on_modelImage[0].Y + point_on_modelImage[3].Y) / 2;

                /*
                if (Math.Abs(result.Angle)>=90)
                {
                    x_P = (int)Math.Abs(point_on_modelImage[3].X + point_on_modelImage[0].X) / 2;
                    y_P = (int)Math.Abs(point_on_modelImage[1].Y + point_on_modelImage[0].Y) / 2;
                }
                */


                int x = (int)x_P / (modelImage.Width / 7);
                int y = (int)y_P / (modelImage.Height / 5);

                if (x >= 7)
                    x = 6;
                if (y >= 5)
                { y = 4; }

                result.position = y + "" + x.ToString();

                if (listener != null)
                    listener.OnPerspective(invert_perspectiveImage.ToImage<Bgr,byte>(), result.position);
            }
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
        private void MatchFeaturePointsAndFindMask(Mat observedImage, Mat modelImage, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out long matchTime)
        {
            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();
            matchTime = FindFeaturePointsAndMatch(modelImage, observedImage, modelKeyPoints, observedKeyPoints, matches);
            mask = GetMaskFromMatches(matches);
        }

        private long FindFeaturePointsAndMatch(Mat modelImage, Mat observedImage, VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches)
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

            impl.MatchFeatures(modelDescriptors, observedDescriptors, modelKeyPoints, observedKeyPoints, matches);

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
                    Mat homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
                    if (homography == null)
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
