using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class PuzzleLocator : IPuzzleLocator
    {

        private Mat[] Template_Puzzle = new Mat[35];

        //是否獲得HSV、ROI區域與是否開燈
        private bool GetWholePuzzle = false;
        private PuzzleLocatorListener listener;
        private readonly Size minSize;
        private readonly Size maxSize;
        private readonly int threshold;

        public PuzzleLocator(int threshold,Size minSize, Size maxSize)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.threshold = threshold;
        }


        public List<LocationResult> Locate(Image<Bgr, byte> input)
        {
            var dst = ColorImagePreprocess(input);
            var bin = BinaryImagePreprocess(dst);

            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(bin, bin, Struct_element, new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(bin, bin, Struct_element, new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));

            if (listener != null)
                listener.onPreprocessDone(bin);

            List<LocationResult> puzzleDataList = new List<LocationResult>();

            //取得輪廓組套件
            VectorOfVectorOfPoint contours = findContours(bin);
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
                    puzzleData.ROI = getROI(puzzleData.Coordinate,puzzleData.Size,input);
                    puzzleDataList.Add(puzzleData);
                    if (listener != null)
                        listener.onLocated(puzzleData);
                }

            }

            return puzzleDataList;
        }

        private Image<Bgr,byte> getROI(Point Coordinate,Size Size,Image<Bgr,byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Size.Width / 2.0f), (int)(Coordinate.Y - Size.Height / 2.0f), Size.Width, Size.Height);

            //將ROI選取區域使用Mat型式讀取
            return new Mat(input.Mat, rect).ToImage<Bgr, byte>();
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

        public void setListener(PuzzleLocatorListener listener)
        {
            this.listener = listener;
        }
    }
}
