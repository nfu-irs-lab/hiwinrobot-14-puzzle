using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;

namespace ExclusiveProgram
{
    public class VisualSystem
    {
        private VisualSystem()
        {

        }

        private static VideoCapture camera=new VideoCapture();

        public static void setCameraResolution(int resolution_width,int resolution_height)
        {
            camera.Set(CapProp.FrameWidth,resolution_width);
            camera.Set(CapProp.FrameHeight,resolution_height);
        }

        public static Image<Bgr,byte> CaptureImage()
        {
            Mat image = camera.QueryFrame();
            return image.Clone().ToImage<Bgr, byte>();
        }

        #region 圖片預處理

        #region 圖片二值化

        public static Image<Gray,byte> Binarization(Image<Gray,byte> image,int threshold)
        {
            Image<Gray,byte> dst=new Image<Gray,byte>(image.Width,image.Height);
            CvInvoke.Threshold(image, dst, threshold, 255, ThresholdType.Binary);
            return dst;
        }

        #endregion 圖片二值化


        public static Image<Gray,byte> BgrToGray(Image<Bgr,byte> image)
        {   
            Image<Gray,byte> dst=new Image<Gray,byte>(image.Width,image.Height);
            CvInvoke.CvtColor(image, dst, ColorConversion.Bgr2Gray);
            return dst;
        }


        #region 圖片顏色拓展

        public static Image<Bgr, byte> ExtendColor(Image<Bgr, byte> image)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(image.Size);

            int rowsCounts = image.Rows;
            int colsCounts = image.Cols;

            int min = 256, max = 0;

            for (int i = 0; i < rowsCounts; i++)
            {
                for (int j = 0; j < colsCounts; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte buffer;
                        buffer = image.Data[i, j, k];
                        if ((int)buffer < min)
                        { min = (int)buffer; }

                        if (buffer > max)
                        { max = buffer; }
                    }
                }
            }

            for (int i = 0; i < rowsCounts; i++)
            {
                for (int j = 0; j < colsCounts; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        double d = image.Data[i, j, k];
                        d = d - min;
                        d = d / max;
                        d = d * 255;
                        img.Data[i, j, k] = (byte)d;
                    }
                }
            }

            return img;
        }

        #endregion 圖片顏色拓展

        #region 白平衡

        /// <summary>
        /// 圖像白平衡
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image<Bgr, byte> WhiteBalance(Image<Bgr, byte> img)
        {
            int avgR = 0, avgG = 0, avgB = 0;
            int sumR = 0, sumG = 0, sumB = 0;
            for (int h = 0; h < img.Height; ++h)
            {
                for (int w = 0; w < img.Width; ++w)
                {
                    sumB += img.Data[h, w, 0];
                    sumG += img.Data[h, w, 1];
                    sumR += img.Data[h, w, 2];
                }
            }
            int size = img.Height * img.Width;
            avgB = sumB / size;
            avgG = sumG / size;
            avgR = sumR / size;
            double k = 0.299 * avgR + 0.587 * avgG + 0.114 * avgB;

            avgR = (sumR / size);
            avgG = (sumG / size);
            avgB = (sumB / size);

            double kr = k / avgR;
            double kg = k / avgG;
            double kb = k / avgB;
            double newB, newG, newR;
            for (int h = 0; h < img.Height; ++h)
            {
                for (int w = 0; w < img.Width; ++w)
                {
                    newB = img.Data[h, w, 0] * kb;
                    newG = img.Data[h, w, 1] * kg;
                    newR = img.Data[h, w, 2] * kr;

                    img.Data[h, w, 0] = (byte)(newB > 255 ? 255 : newB);
                    img.Data[h, w, 1] = (byte)(newG > 255 ? 255 : newG);
                    img.Data[h, w, 2] = (byte)(newR > 255 ? 255 : newR);
                }
            }

            return img;
        }

        #endregion 白平衡

        #endregion 圖片預處理
    }

}
