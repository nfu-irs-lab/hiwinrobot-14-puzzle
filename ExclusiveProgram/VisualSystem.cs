using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExclusiveProgram
{
    public class VisualSystem
    {
        private VisualSystem()
        {

        }

        #region 圖片預處理

        #region 直方圖均衡化
        public static void HistogramEqualization(Image<Bgr, byte> image,Image<Bgr,byte> outImage)
        {
            VectorOfMat channels = new VectorOfMat();
            CvInvoke.Split(image, channels); 
            CvInvoke.EqualizeHist(channels[0], channels[0]);
            CvInvoke.EqualizeHist(channels[1], channels[1]);
            CvInvoke.EqualizeHist(channels[2], channels[2]);
            CvInvoke.Merge(channels, outImage); 
        }

        #endregion 直方圖均衡化
        #region 圖片顏色拓展

        public static void ExtendColor(Image<Bgr, byte> image,Image<Bgr,byte> outImage)
        {
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
                        outImage.Data[i, j, k] = (byte)d;
                    }
                }
            }
        }

        #endregion 圖片顏色拓展

        #region 白平衡

        /// <summary>
        /// 圖像白平衡
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static void WhiteBalance(Image<Bgr, byte> img,Image<Bgr,byte> outImage)
        {

            VectorOfMat channels= new VectorOfMat();
            CvInvoke.Split(img,channels);

            var avgB = CvInvoke.Mean(channels[0]).V0;
            var avgG = CvInvoke.Mean(channels[1]).V0;
            var avgR = CvInvoke.Mean(channels[2]).V0;
            double k = 0.299 * avgR + 0.587 * avgG + 0.114 * avgB;

            Task[] tasks=new Task[3];
            tasks[0]=Task.Factory.StartNew(() =>
            {
                double kb = k / avgB;
                for (int h = 0; h < img.Height; ++h)
                {
                    for (int w = 0; w < img.Width; ++w)
                    {
                        var newB = img.Data[h, w, 0] * kb;
                        outImage.Data[h, w, 0] = (byte)(newB > 255 ? 255 : newB);
                    }
                }
            });

            tasks[1]=Task.Factory.StartNew(() =>
            {
                double kg = k / avgG;
                for (int h = 0; h < img.Height; ++h)
                {
                    for (int w = 0; w < img.Width; ++w)
                    {
                        var newG = img.Data[h, w, 1] * kg;
                        outImage.Data[h, w, 1] = (byte)(newG > 255 ? 255 : newG);
                    }
                }
            });

            tasks[2]=Task.Factory.StartNew(() =>
            {
                double kr = k / avgR;
                for (int h = 0; h < img.Height; ++h)
                {
                    for (int w = 0; w < img.Width; ++w)
                    {
                        var newR = img.Data[h, w, 2] * kr;
                        outImage.Data[h, w, 2] = (byte)(newR > 255 ? 255 : newR);

                    }
                }
            });
            Task.WaitAll(tasks);
        }

        #endregion 白平衡

        #endregion 圖片預處理
    }

}
