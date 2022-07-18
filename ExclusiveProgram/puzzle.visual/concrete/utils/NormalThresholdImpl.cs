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
    public class NormalThresoldImpl: IPuzzleThresholdImpl
    {
        private readonly int threshold;

        public NormalThresoldImpl(int threshold)
        {
            this.threshold = threshold;
        }

        public void Threshold(Image<Gray, byte> input, Image<Gray, byte> output)
        {
            CvInvoke.Threshold(input,output,threshold, 255, ThresholdType.Binary);
        }
    }
}
