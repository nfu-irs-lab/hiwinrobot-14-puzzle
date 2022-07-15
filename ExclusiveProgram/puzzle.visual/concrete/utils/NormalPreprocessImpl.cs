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
    public class NormalPreprocessImpl : IPuzzlePreProcessImpl
    {
        public void Preprocess(Image<Bgr, byte> input, Image<Bgr, byte> output)
        {
            VisualSystem.WhiteBalance(input,output);
            VisualSystem.ExtendColor(output,output);
        }
    }
}
