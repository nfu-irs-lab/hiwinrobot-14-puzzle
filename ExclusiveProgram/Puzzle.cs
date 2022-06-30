using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram;
using Puzzle.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Puzzle.Framework
{


    public struct result
    {
        public double Angel;
        public Mat image;
        public PointF[] pts;
    };

    public struct Puzzle_sturct
    {
        public double Angel;
        public Point coordinate;
        public Mat image;
        public string position;
    };

    public struct LocationResult
    {
        public Point Coordinate;
        public double Angle;
        public Size Size;
    }

    public struct CorrectedPuzzleArgument
    {
        public Point Coordinate;
        public double Angle;
        public Size Size;
        public Mat image;
    }


    public class PuzzlePiece
    {
        public PuzzleType type {  get; }
        public Image<Bgr,Byte> Image {  get; }

        public double Angle_degree{ set; get; }
        public Size Size { set; get; }

        public PuzzlePiece(PuzzleType type,double angle,Size size,Image<Bgr,Byte> Image)
        {
            this.type = type;
            this.Image = Image;
            this.Angle_degree = angle;
            this.Size = size;
        }
    }

    public enum PuzzleType
    {
        
    }
    public interface IPuzzleLocator
    {
        List<LocationResult> Locate(Image<Gray, Byte> input);
    }

    public interface IPuzzleCorrector
    {
        Image<Bgr,Byte> Correct(Image<Bgr, Byte> input,LocationResult raw);
    }

    public interface IPuzzleRecognizer
    {
        List<Puzzle_sturct> Recognize(List<CorrectedPuzzleArgument> input);
    }

    public interface IPuzzleFactory
    {
        List<Puzzle_sturct> Execute(Image<Bgr, Byte> input);
    }

}
namespace Puzzle.Concrete
{

    public class DefaultPuzzleFactory : IPuzzleFactory
    {
        private IPuzzleRecognizer recognizer;
        private IPuzzleLocator locator;
        private IPuzzleCorrector corrector;
        private readonly int threshold;


        public DefaultPuzzleFactory(int threshold,IPuzzleLocator locator,IPuzzleRecognizer recognizer, IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.threshold = threshold;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<Puzzle_sturct> Execute(Image<Bgr, byte> input)
        {
            var image = input.Clone();
            var dst = ColorImagePreprocess(image);
            var bin = BinaryImagePreprocess(dst);


            List<LocationResult> dataList = locator.Locate(bin);

            List<CorrectedPuzzleArgument> arguments = new List<CorrectedPuzzleArgument>(); 

            foreach (LocationResult location in dataList)
            {
                var i = corrector.Correct(image, location);
                CorrectedPuzzleArgument argument = new CorrectedPuzzleArgument();
                argument.Coordinate = location.Coordinate;
                argument.Angle = location.Angle;
                argument.image= i.Clone().Mat;
                i.Dispose();

                argument.Size = location.Size;
                arguments.Add(argument);
            }

            image.Dispose();
            return recognizer.Recognize(arguments);
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
            CvInvoke.Threshold(bin, bin,threshold, 255, ThresholdType.Binary);
            return bin;
        }
    }

    public class PuzzleLocator : IPuzzleLocator
    {

        private Mat[] Template_Puzzle = new Mat[35];

        //是否獲得HSV、ROI區域與是否開燈
        private bool GetWholePuzzle = false;
        private readonly Size minSize;
        private readonly Size maxSize;

        public PuzzleLocator(Size minSize, Size maxSize)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
        }


        public List<LocationResult> Locate(Image<Gray, byte> input)
        {

            List<LocationResult> puzzleDataList = new List<LocationResult>();

            //取得輪廓組套件
            VectorOfVectorOfPoint contours = findContours(input);
            //尋遍輪廓組之單一輪廓
            for (int i = 0; i < contours.Size; i++)
            {
                //尋找單一輪廓之套件
                VectorOfPoint contour = contours[i];

                //多邊形逼近之套件
                VectorOfPoint approxContour = new VectorOfPoint();

                //將輪廓組用多邊形框選 「0.05」為可更改逼近值
                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.005, true);

                //畫出最小框選矩形，裁切用
                Rectangle BoundingBox_ = CvInvoke.BoundingRectangle(approxContour);
                //Rectangle BoundingBox_ = CvInvoke.BoundingRectangle(contour);

                //畫在圖片上
                //CvInvoke.Rectangle(Ori_img, BoundingBox_, new MCvScalar(255, 0, 255, 255), 2);

                //框選輪廓最小矩形
                Rectangle rect = CvInvoke.BoundingRectangle(contour);


                //獲得最小旋轉矩形，取得角度用
                RotatedRect BoundingBox = CvInvoke.MinAreaRect(contour);

                LocationResult puzzleData = new LocationResult();

                Point Position = getCentralPosition(puzzleData.Angle, contour);
                if (CheckDuplicatePuzzlePosition(puzzleDataList, Position) && CheckSize(rect, Position))
                {
                    puzzleData.Angle = getAngle(BoundingBox, BoundingBox_, rect);
                    puzzleData.Coordinate = Position;
                    puzzleData.Size = new Size(rect.Width, rect.Height);
                    puzzleDataList.Add(puzzleData);
                }

            }

            return puzzleDataList;
        }

        private bool CheckSize(Rectangle rect, Point Position)
        {
            if (rect.Size.Width < minSize.Width || rect.Size.Height < minSize.Height)
                return false;

            if (rect.Size.Width > maxSize.Width || rect.Size.Height > maxSize.Height)
                return false;

            if (Position.X - rect.Width / 2.0f < 0)
                return false;

            if (Position.Y - rect.Height / 2.0f < 0)
                return false;

            return true;
        }

        private Point getCentralPosition(double Angle, VectorOfPoint contour)
        {
            //畫出最小外切圓，獲得圓心用
            CircleF Puzzle_circle = CvInvoke.MinEnclosingCircle(contour);
            return new Point((int)Puzzle_circle.Center.X, (int)Puzzle_circle.Center.Y);
        }

        private VectorOfVectorOfPoint findContours(Image<Gray, byte> image)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(image, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            return contours;
        }

        #region 檢查重複項

        /// <summary>
        /// 檢查重複項
        /// </summary>
        /// <param name="currentPuzzlePosition"></param>
        /// <returns></returns>
        private bool CheckDuplicatePuzzlePosition(List<LocationResult> definedPuzzleDataList, Point currentPuzzlePosition)
        {
            bool Answer = true;
            for (int i = 0; i < definedPuzzleDataList.Count; i++)
            {
                int x_up = definedPuzzleDataList[i].Coordinate.X + 10,
                    x_down = definedPuzzleDataList[i].Coordinate.X - 10,
                    y_up = definedPuzzleDataList[i].Coordinate.Y + 10,
                    y_down = definedPuzzleDataList[i].Coordinate.Y - 10;

                if (currentPuzzlePosition.X < x_up && currentPuzzlePosition.X > x_down && currentPuzzlePosition.Y < y_up && currentPuzzlePosition.Y > y_down)
                    Answer = false;
            }
            return Answer;
        }

        #endregion 檢查重複項
        private float getAngle(RotatedRect BoundingBox, Rectangle BoundingBox_, Rectangle rect)
        {

            float Angel = 0;
            ////畫出最小矩形
            //CvInvoke.Rectangle(Ori_img, rect, new MCvScalar(0, 0, 255));

            //畫出可旋轉矩形
            //CvInvoke.Polylines(Ori_img, Array.ConvertAll(BoundingBox.GetVertices(), Point.Round), true, new Bgr(Color.DeepPink).MCvScalar, 3);

            //計算角度
            //詳情請詢問我
            if (rect.Size.Width < rect.Size.Height)
            {
                Angel = -(BoundingBox.Angle + 90);
            }
            else
            {
                Angel = -BoundingBox.Angle;
            }


            return Angel;
        }
    }

    public class PuzzleCorrector : IPuzzleCorrector
    {
        private readonly int thereshold;
        public PuzzleCorrector(int Thereshold)
        {
            thereshold = Thereshold;
        }

        public Image<Bgr, byte> Correct(Image<Bgr, byte> input, LocationResult raw)
        {
            Rectangle rect = new Rectangle((int)(raw.Coordinate.X - raw.Size.Width / 2.0f), (int)(raw.Coordinate.Y - raw.Size.Height / 2.0f), raw.Size.Width, raw.Size.Height);

            Mat Ori_img = VisualSystem.Image2Mat<Bgr>(input);

            //將ROI選取區域使用Mat型式讀取
            Image<Bgr, byte> Copy_ = new Mat(Ori_img, rect).ToImage<Bgr, byte>();

            //獲得矩形長寬
            int Rotate_newImg_x = rect.Width,
                Rotate_newImg_y = rect.Height;

            //計算對角線，為了旋轉時圖片不轉出邊界
            double Lenght = Math.Sqrt((Rotate_newImg_x * Rotate_newImg_x) + (Rotate_newImg_y * Rotate_newImg_y));

            //創造 【邊長為對角線長】(後續處理方便)的擴大版圖片
            Image<Bgr, byte> new_img = new Image<Bgr, byte>((int)Lenght, (int)Lenght, new Bgr(Color.Green));

            //ROI設定，須為→(中心-(邊長/2))，為了將圖片放到中央
            new_img.ROI = new Rectangle(new Point(((int)Lenght - Rotate_newImg_x) / 2, ((int)Lenght - Rotate_newImg_y) / 2), new Size(Rotate_newImg_x, Rotate_newImg_y));


            //將圖片複製到圖片中央
            Copy_.CopyTo(new_img);

            //取消ROI設定
            new_img.ROI = Rectangle.Empty;

            //將圖片旋轉(矯正[rectify]用)，旋轉出邊界顏色使用ROI_HSV值轉RGB(後續處理方便)
            new_img = new_img.Rotate(raw.Angle, new Bgr(Color.Green));

            //new_ing._EqualizeHist();
            //進一步縮小圖片
            //尋找輪廓組函式
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            //尋找輪廓函式
            VectorOfPoint contour = new VectorOfPoint();

            Image<Gray, byte> Out = new Image<Gray, byte>(new_img.Size);


            for (int i = 0; i < new_img.Rows; i++)
            {
                for (int j = 0; j < new_img.Cols; j++)
                {
                    int Red = (int)new_img.Data[i, j, 2],
                    Blue = (int)new_img.Data[i, j, 0],
                        threshold = Red + Blue;
                    if (threshold > this.thereshold)
                        Out.Data[i, j, 0] = 255;
                    else
                        Out.Data[i, j, 0] = 0;
                }
            }
            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));

            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(Out, Out, Struct_element, new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(Out, Out, Struct_element, new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));
            //尋找輪廓
            CvInvoke.FindContours(Out, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            int max_size=-1;
            Rectangle max_size_rectangle=new Rectangle(new Point(-1,-1),new Size(0,0));
            //尋遍輪廓組
            for (int test = 0; test < contours.Size; test++)
            {
                contour = contours[test];

                //以最小矩形框選
                Rectangle new_ing_Rectangle = CvInvoke.BoundingRectangle(contour);
                ////畫出最小矩形
                //CvInvoke.Rectangle(OutA, new_ing_Rectangle, new MCvScalar(255, 255, 255));
                int current = new_ing_Rectangle.Width * new_ing_Rectangle.Height;
                if (current > max_size)
                {
                    max_size = current;
                    max_size_rectangle= new_ing_Rectangle;
                }
            }
            if (max_size_rectangle.Width*max_size_rectangle.Height==0)
                throw new Exception("Max Size rectangle not found.");


            Mat new_img_Save = new Mat(new_img.Mat, max_size_rectangle);
            



            //儲存圖片
            //new_img_Save.Save(@"C:\Users\HIWIN\Desktop\第十三屆上銀程式\ming\顏色辨別(HSV)\test" + num.ToString() + ".jpg");
            return VisualSystem.Mat2Image<Bgr>(new_img_Save);
        }

    }

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
                puzzles.Add(doSingleRecognize(readOnlyModleImage.Clone().Mat,argument.image.Clone(), argument.Angle, argument.Coordinate));
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
            result Answer= Draw(register_Puzzle.Mat,modelImage, out matchTime, out score);

            //-------------------------------------------------------------------------------------
            /*
            if (Answer.image == null || Answer.pts == null)
            {
                Check_Error_Index.Add(i);
                continue;
            }
            */
            

            int x_P = (int)Math.Abs(Answer.pts[2].X + Answer.pts[3].X) / 2;
            int y_P = (int)Math.Abs(Answer.pts[0].Y + Answer.pts[3].Y) / 2;

            if (Math.Abs(Answer.Angel) == 90)
            {
                x_P = (int)Math.Abs(Answer.pts[3].X + Answer.pts[0].X) / 2;
                y_P = (int)Math.Abs(Answer.pts[1].Y + Answer.pts[0].Y) / 2;
            }

            int x = (int)x_P / (modelImage.Width / 7);
            int y = (int)y_P / (modelImage.Height / 5);

            if (x >= 7)
            { x = 6; }
            if (y >= 5)
            { y = 4; }

            Puzzle_sturct register = new Puzzle_sturct();
            register.image = register_Puzzle.Clone().Mat;

            register.coordinate = coordinate;
            register.position = y.ToString() + x.ToString();
            register.Angel = Answer.Angel + angle;

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


        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        private result Draw(Mat observedImage,Mat modelImage, out long matchTime, out long score)
        {
            result result_ = new result();
            Mat homography;
            VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint();
            VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, out matchTime, modelKeyPoints, observedKeyPoints, matches,
                   out mask, out homography, out score);

                //Draw the matched keypoints
                Mat result = new Mat();
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                   matches, result, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), mask);

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
                        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
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

                        CvInvoke.PutText(result, string.Format("{0}ms , {1:0.00}", matchTime, Slope), new Point(1, 50), FontFace.HersheySimplex, 1, new MCvScalar(100, 100, 255), 3, LineType.FourConnected);
                        result_.Angel = Angel;
                        result_.image = result;
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
                using (var ip = new Emgu.CV.Flann.KdTreeIndexParams())
                using (var sp = new SearchParams())
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
