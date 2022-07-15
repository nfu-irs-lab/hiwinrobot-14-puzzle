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
    public class TPuzzlePreProcessImpl : IPuzzlePreProcessImpl
    {
        private readonly int threshold;
        private readonly double green_weight;
        private readonly double red_weight;
        private readonly double blue_weight;

        public TPuzzlePreProcessImpl(int threshold,double green_weight)
        {
            this.threshold = threshold;
            this.green_weight = green_weight;
            this.red_weight = 0.2989 + (green_weight / 2);
            this.blue_weight = 0.144+ (green_weight / 2);
        }

        public void ConvertToGray(Image<Bgr, byte> input, Image<Gray, byte> output)
        {

            for (int i = 0; i < input.Rows; i++)
            {
                for (int j = 0; j < input.Cols; j++)
                {
                    var R = input.Data[i,j,2];
                    var G = input.Data[i,j,1];
                    var B = input.Data[i,j,0];
                    var scale = (red_weight * R + green_weight * G + blue_weight * B);
                    if(scale>=threshold)
                        output.Data[i,j,0] = 255;
                    else
                        output.Data[i,j,0] = 0;
                }
            }
            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(4, 4), new Point(-1, -1));
            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(output,output,Struct_element,new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(output,output,Struct_element,new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));
        }

        public void Preprocess(Image<Bgr, byte> input, Image<Bgr, byte> output)
        {
            VisualSystem.WhiteBalance(input,output);
            VisualSystem.ExtendColor(output,output);
        }

        public void Threshold(Image<Gray, byte> input, Image<Gray, byte> output)
        {
            output = input;
        }
    }
}
