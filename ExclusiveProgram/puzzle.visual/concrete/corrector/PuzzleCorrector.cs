using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class PuzzleCorrector : IPuzzleCorrector
    {
        private readonly Bgr backgroundColor;
        private PuzzleCorrectorListener listener;
        private readonly IPuzzlePreProcessImpl preProcessImpl;
        private readonly IPuzzleGrayConversionImpl grayConversionImpl;
        private readonly IPuzzleThresholdImpl thresholdImpl;
        private readonly IPuzzleBinaryPreprocessImpl binaryPreprocessImpl;

        public PuzzleCorrector(Color backgroundColor, IPuzzlePreProcessImpl preProcessImpl, locator.NormalPreprocessImpl preprocessImpl, IPuzzleGrayConversionImpl grayConversionImpl, IPuzzleThresholdImpl thresholdImpl,IPuzzleBinaryPreprocessImpl binaryPreprocessImpl)
        {
            this.backgroundColor = new Bgr(backgroundColor);
            this.preProcessImpl = preProcessImpl;
            this.grayConversionImpl = grayConversionImpl;
            this.thresholdImpl = thresholdImpl;
            this.binaryPreprocessImpl = binaryPreprocessImpl;
        }
        public Image<Bgr, byte> Correct(Image<Bgr, byte> ROI, double Angle)
        {
            //獲得矩形長寬
            int Rotate_newImg_x = ROI.Width,
                Rotate_newImg_y = ROI.Height;

            //計算對角線，為了旋轉時圖片不轉出邊界
            double Lenght = Math.Sqrt((Rotate_newImg_x * Rotate_newImg_x) + (Rotate_newImg_y * Rotate_newImg_y));

            //創造 【邊長為對角線長】(後續處理方便)的擴大版圖片
            Image<Bgr, byte> new_img = new Image<Bgr, byte>((int)Lenght, (int)Lenght,backgroundColor);

            //ROI設定，須為→(中心-(邊長/2))，為了將圖片放到中央
            new_img.ROI = new Rectangle(new Point(((int)Lenght - Rotate_newImg_x) / 2, ((int)Lenght - Rotate_newImg_y) / 2), new Size(Rotate_newImg_x, Rotate_newImg_y));

            //將圖片複製到圖片中央
            ROI.CopyTo(new_img);

            //取消ROI設定
            new_img.ROI = Rectangle.Empty;

            //將圖片旋轉(矯正[rectify]用)，旋轉出邊界顏色使用ROI_HSV值轉RGB(後續處理方便)
            new_img = new_img.Rotate(Angle, backgroundColor);

            Image<Gray, byte> Out = new Image<Gray, byte>(new_img.Size);

            grayConversionImpl.ConvertToGray(new_img, Out);
            thresholdImpl.Threshold(Out, Out);
            binaryPreprocessImpl.BinaryPreprocess(Out, Out);
            
            if (listener != null)
                listener.onPreprocessDone(Out);

            //new_ing._EqualizeHist();
            //進一步縮小圖片
            //尋找輪廓組函式
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            //尋找輪廓函式

            CvInvoke.FindContours(Out, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            int max_size=-1;
            Rectangle max_size_rectangle=new Rectangle(new Point(-1,-1),new Size(0,0));
            //尋遍輪廓組
            for (int test = 0; test < contours.Size; test++)
            {
                var contour = contours[test];

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
            return new_img_Save.ToImage<Bgr,byte>();
        }

        public void setListener(PuzzleCorrectorListener listener)
        {
            this.listener=listener; 
        }

    }
}
