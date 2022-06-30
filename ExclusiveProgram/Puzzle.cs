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

    public class PuzzleData
    {
        public Point CentralPosition{ set; get; }
        public float Angle{ set; get; }
        public Size Size { set; get; }

    }
    public enum PuzzleShape
    {

    }

    public enum PuzzleContent
    {

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
        List<PuzzleData> Locate(Image<Gray, Byte> input);
    }

    public interface IPuzzleCorrector
    {
        Image<Bgr,Byte> Correct(Image<Bgr, Byte> input,PuzzleData raw);
    }

    public interface IPuzzleRecognizer
    {
        List<result> Recognize(List<Image<Bgr, Byte>> input);
    }

    public interface IPuzzleFactory
    {
        List<PuzzlePiece> Execute(Image<Bgr, Byte> input);
    }

}
namespace Puzzle.Concrete
{

    public class DefaultPuzzleFactory : IPuzzleFactory
    {
        private IPuzzleRecognizer recognizer;
        private IPuzzleLocator locator;
        private IPuzzleCorrector corrector;

        public DefaultPuzzleFactory(IPuzzleRecognizer recognizer, IPuzzleLocator locator, IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<PuzzlePiece> Execute(Image<Bgr, byte> input)
        {
            /*
            List<PuzzleData> raws = locator.Locate(VisualSystem.Binarization(VisualSystem.BgrToGray(input), 100));
            List<Image<Bgr, Byte>> PuzzleImages = new List<Image<Bgr, byte>>();
            foreach (PuzzleData piece in raws)
            {
                PuzzleImages.Add(corrector.Correct(input, piece));
            }

            List<PuzzleType> types = recognizer.Recognize(PuzzleImages);

            List<PuzzlePiece> puzzles = new List<PuzzlePiece>();
            for (int i = 0; i < PuzzleImages.Count; i++)
            {
                puzzles.Add(new PuzzlePiece(types[i], raws[i].Angle, raws[i].Size, PuzzleImages[i]));
            }
            return puzzles;
            */
            return null;
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


        public List<PuzzleData> Locate(Image<Gray, byte> input)
        {

            List<PuzzleData> puzzleDataList = new List<PuzzleData>();

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

                PuzzleData puzzleData = new PuzzleData();

                Point Position = getCentralPosition(puzzleData.Angle, contour);
                if (CheckDuplicatePuzzlePosition(puzzleDataList, Position) && CheckSize(rect, Position))
                {
                    puzzleData.Angle = getAngle(BoundingBox, BoundingBox_, rect);
                    puzzleData.CentralPosition = Position;
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

        private Point getCentralPosition(float Angle, VectorOfPoint contour)
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
        private bool CheckDuplicatePuzzlePosition(List<PuzzleData> definedPuzzleDataList, Point currentPuzzlePosition)
        {
            bool Answer = true;
            for (int i = 0; i < definedPuzzleDataList.Count; i++)
            {
                int x_up = definedPuzzleDataList[i].CentralPosition.X + 10,
                    x_down = definedPuzzleDataList[i].CentralPosition.X - 10,
                    y_up = definedPuzzleDataList[i].CentralPosition.Y + 10,
                    y_down = definedPuzzleDataList[i].CentralPosition.Y - 10;

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

        public Image<Bgr, byte> Correct(Image<Bgr, byte> input, PuzzleData raw)
        {
            Rectangle rect = new Rectangle((int)(raw.CentralPosition.X - raw.Size.Width / 2.0f), (int)(raw.CentralPosition.Y - raw.Size.Height / 2.0f), raw.Size.Width, raw.Size.Height);

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
            CvInvoke.Imshow("Aaa", Out);
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
}
