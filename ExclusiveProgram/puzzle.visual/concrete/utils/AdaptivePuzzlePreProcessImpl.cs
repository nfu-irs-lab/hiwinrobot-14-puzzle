using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete.locator
{
    public class AdaptivePuzzlePreProcessImpl : IPuzzlePreProcessImpl
    {
        private readonly double param1;
        private readonly int blockSize;
        public AdaptivePuzzlePreProcessImpl(int blockSize,double param1)
        {
            if (blockSize % 2 == 0)
                throw new ArgumentException("blockSize % 2 == 0");
            this.param1 = param1;
            this.blockSize = blockSize;
        }

        public void Preprocess(Image<Bgr, byte> input, Image<Bgr, byte> output)
        {
            VisualSystem.WhiteBalance(input,output);
            VisualSystem.ExtendColor(output,output);
            CvInvoke.MedianBlur(output, output, 27);
        }

        public void Threshold(Image<Gray, byte> input, Image<Gray, byte> output)
        {
            CvInvoke.AdaptiveThreshold(input,output, 255,AdaptiveThresholdType.MeanC, ThresholdType.Binary,blockSize,param1);
            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(4, 4), new Point(-1, -1));
            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(output,output,Struct_element,new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(output,output,Struct_element,new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));
        }

        public void ConvertToGray(Image<Bgr, byte> input, Image<Gray, byte> output)
        {
            CvInvoke.CvtColor(input, output, ColorConversion.Bgr2Gray);
        }
    }
}
