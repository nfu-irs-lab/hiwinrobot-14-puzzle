using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
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
        private readonly Size minSize;
        private readonly Size maxSize;

        public PuzzleLocator(Size minSize, Size maxSize)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
        }


        public List<LocationResult> Locate(Image<Gray, byte> binaryImage,Image<Bgr,byte> rawImage)
        {
            
            List<LocationResult> puzzleDataList = new List<LocationResult>();
            //取得輪廓組套件
            VectorOfVectorOfPoint contours = findContours(binaryImage);

            var preview_image = rawImage.Clone();

            //尋遍輪廓組之單一輪廓
            for (int i = 0; i < contours.Size; i++)
            {
                //尋找單一輪廓之套件
                VectorOfPoint contour = contours[i];

                CvInvoke.Polylines(preview_image, contour, true, new MCvScalar(0, 0, 255),2);

                //多邊形逼近之套件
                VectorOfPoint approxContour = new VectorOfPoint();

                //將輪廓組用多邊形框選 「0.05」為可更改逼近值
                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.005, true);

                //畫出最小框選矩形，裁切用
                Rectangle BoundingBox_ = CvInvoke.BoundingRectangle(approxContour);
                //Rectangle BoundingBox_ = CvInvoke.BoundingRectangle(contour);

                //畫在圖片上
                CvInvoke.Rectangle(preview_image, BoundingBox_, new MCvScalar(255, 0, 0), 2);


                //框選輪廓最小矩形
                Rectangle rect = CvInvoke.BoundingRectangle(contour);


                //獲得最小旋轉矩形，取得角度用
                RotatedRect BoundingBox = CvInvoke.MinAreaRect(contour);

                Point[] points = Array.ConvertAll<PointF, Point>(BoundingBox.GetVertices(), Point.Round);
                CvInvoke.Polylines(preview_image, points, true, new MCvScalar(0, 255, 255),2);

                LocationResult puzzleData = new LocationResult();

                Point Position = getCentralPosition(puzzleData.Angle, contour);
                if (CheckDuplicatePuzzlePosition(puzzleDataList, Position) && CheckSize(rect, Position))
                {
                    puzzleData.Angle = getAngle(BoundingBox,rect);
                    puzzleData.Coordinate = Position;
                    puzzleData.Size = new Size(rect.Width, rect.Height);
                    puzzleData.ROI = getROI(puzzleData.Coordinate,puzzleData.Size,rawImage);
                    puzzleData.BinaryROI= getBinaryROI(puzzleData.Coordinate,puzzleData.Size,binaryImage);
                    puzzleDataList.Add(puzzleData);
                }

            }
            preview_image.Save("results\\contours.jpg");
            preview_image.Dispose();

            return puzzleDataList;
        }

        private Image<Bgr,byte> getROI(Point Coordinate,Size Size,Image<Bgr,byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Size.Width / 2.0f), (int)(Coordinate.Y - Size.Height / 2.0f), Size.Width, Size.Height);

            //將ROI選取區域使用Mat型式讀取
            return new Mat(input.Mat, rect).ToImage<Bgr, byte>();
        }
        private Image<Gray,byte> getBinaryROI(Point Coordinate,Size Size,Image<Gray,byte> input)
        {
            Rectangle rect = new Rectangle((int)(Coordinate.X - Size.Width / 2.0f), (int)(Coordinate.Y - Size.Height / 2.0f), Size.Width, Size.Height);

            //將ROI選取區域使用Mat型式讀取
            return new Mat(input.Mat, rect).ToImage<Gray, byte>();
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
        private float getAngle(RotatedRect BoundingBox, Rectangle rect)
        {
            //計算角度
            //詳情請詢問我

            ////畫出最小矩形
            //CvInvoke.Rectangle(Ori_img, rect, new MCvScalar(0, 0, 255));

            //畫出可旋轉矩形
            //CvInvoke.Polylines(Ori_img, Array.ConvertAll(BoundingBox.GetVertices(), Point.Round), true, new Bgr(Color.DeepPink).MCvScalar, 3);


            //return BoundingBox.Angle;
            
            float Angel = 0;
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
}
