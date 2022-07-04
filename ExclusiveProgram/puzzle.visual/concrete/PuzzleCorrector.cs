using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class PuzzleCorrector : IPuzzleCorrector
    {
        private readonly int thereshold;
        private readonly Bgr backgroundColor;
        private PuzzleCorrectorListener listener;

        public PuzzleCorrector(int Thereshold,Color backgroundColor)
        {
            thereshold = Thereshold;
            this.backgroundColor = new Bgr(backgroundColor);
        }
        int id;
        public Image<Bgr, byte> Correct(Image<Bgr, byte> input, LocationResult raw)
        {
            Rectangle rect = new Rectangle((int)(raw.Coordinate.X - raw.Size.Width / 2.0f), (int)(raw.Coordinate.Y - raw.Size.Height / 2.0f), raw.Size.Width, raw.Size.Height);

            Mat Ori_img = VisualSystem.Image2Mat<Bgr>(input);

            //將ROI選取區域使用Mat型式讀取
            Image<Bgr, byte> Copy_ = new Mat(Ori_img, rect).ToImage<Bgr, byte>();
            if (listener != null)
            {
                listener.onROIDetected(Copy_,raw);
            }

            //獲得矩形長寬
            int Rotate_newImg_x = rect.Width,
                Rotate_newImg_y = rect.Height;

            //計算對角線，為了旋轉時圖片不轉出邊界
            double Lenght = Math.Sqrt((Rotate_newImg_x * Rotate_newImg_x) + (Rotate_newImg_y * Rotate_newImg_y));

            //創造 【邊長為對角線長】(後續處理方便)的擴大版圖片
            Image<Bgr, byte> new_img = new Image<Bgr, byte>((int)Lenght, (int)Lenght,backgroundColor);

            //ROI設定，須為→(中心-(邊長/2))，為了將圖片放到中央
            new_img.ROI = new Rectangle(new Point(((int)Lenght - Rotate_newImg_x) / 2, ((int)Lenght - Rotate_newImg_y) / 2), new Size(Rotate_newImg_x, Rotate_newImg_y));


            //將圖片複製到圖片中央
            Copy_.CopyTo(new_img);

            //取消ROI設定
            new_img.ROI = Rectangle.Empty;

            //將圖片旋轉(矯正[rectify]用)，旋轉出邊界顏色使用ROI_HSV值轉RGB(後續處理方便)
            new_img = new_img.Rotate(raw.Angle, backgroundColor);

            //new_ing._EqualizeHist();
            //進一步縮小圖片
            //尋找輪廓組函式
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            //尋找輪廓函式
            VectorOfPoint contour = new VectorOfPoint();

            Image<Gray, byte> Out = new Image<Gray, byte>(new_img.Size);

            CvInvoke.CvtColor(new_img, Out,ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(Out, Out, thereshold,255,ThresholdType.Binary);
            /*
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
            */
            if(listener!=null)
                listener.onBinarizationDone(Out);

            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));

            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(Out, Out, Struct_element, new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(Out, Out, Struct_element, new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));

            if(listener!=null)
                listener.onPreprocessDone(Out);

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

        public void setListener(PuzzleCorrectorListener listener)
        {
            this.listener=listener; 
        }

    }
}
