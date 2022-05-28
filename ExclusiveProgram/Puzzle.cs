using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram;
using Puzzle.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Puzzle.Framework
{
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
        List<PuzzleType> Recognize(List<Image<Bgr, Byte>> input);
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

        public DefaultPuzzleFactory(IPuzzleRecognizer recognizer,IPuzzleLocator locator,IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<PuzzlePiece> Execute(Image<Bgr, byte> input)
        {
            List<PuzzleData> raws=locator.Locate(VisualSystem.Binarization(VisualSystem.BgrToGray(input),100));
            List<Image<Bgr,Byte>> PuzzleImages=new List<Image<Bgr, byte>>();
            foreach(PuzzleData piece in raws)
            {
                PuzzleImages.Add(corrector.Correct(input, piece));
            }

            List<PuzzleType> types=recognizer.Recognize(PuzzleImages);
               
            List<PuzzlePiece> puzzles=new List<PuzzlePiece>();
            for(int i = 0; i < PuzzleImages.Count; i++)
            {
                puzzles.Add(new PuzzlePiece(types[i],raws[i].Angle,raws[i].Size,PuzzleImages[i]));
            }
            return puzzles;
        }
    }

    public class PuzzleLocator : IPuzzleLocator
    {

        private Mat[] Template_Puzzle = new Mat[35];
        
        //是否獲得HSV、ROI區域與是否開燈
        private bool GetWholePuzzle = false;

        public PuzzleLocator()
        {

        }


        public List<PuzzleData> Locate(Image<Gray, byte> input)
        {

            List<PuzzleData> puzzleDataList=new List<PuzzleData>();

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

                Point Position=getCentralPosition(puzzleData.Angle,contour);
                if (CheckDuplicatePuzzlePosition(puzzleDataList,Position)) { 
                    puzzleData.Angle = getAngle(BoundingBox, BoundingBox_, rect);
                    puzzleData.CentralPosition = Position;
                    puzzleData.Size = new Size(rect.Width, rect.Height);
                    puzzleDataList.Add(puzzleData);
                }

            }

            return puzzleDataList;
        }

        private Point getCentralPosition(float Angle,VectorOfPoint contour)
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
        private bool CheckDuplicatePuzzlePosition(List<PuzzleData> definedPuzzleDataList,Point currentPuzzlePosition)
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
        private float getAngle(RotatedRect BoundingBox,Rectangle BoundingBox_,Rectangle rect)
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


}
