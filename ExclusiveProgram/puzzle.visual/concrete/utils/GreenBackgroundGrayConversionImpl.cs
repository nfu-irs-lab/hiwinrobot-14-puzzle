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
    public class GreenBackgroundGrayConversionImpl : IPuzzleGrayConversionImpl
    {
        private readonly double green_weight;
        private readonly double red_weight;
        private readonly double blue_weight;

        public GreenBackgroundGrayConversionImpl(double green_weight)
        {
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

                    output.Data[i,j,0] = (byte)scale;
                }
            }
        }
    }
}
