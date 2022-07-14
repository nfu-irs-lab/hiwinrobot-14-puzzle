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
    public class NormalPuzzlePreProcessImpl : IPuzzlePreProcessImpl
    {
        private readonly int threshold;

        public NormalPuzzlePreProcessImpl(int threshold)
        {
            this.threshold = threshold;
        }

        public void Preprocess(Image<Bgr, byte> input, Image<Gray, byte> output)
        {

            var dst = new Image<Bgr, byte>(input.Size);
            VisualSystem.WhiteBalance(input,dst);
            VisualSystem.ExtendColor(dst,dst);
            CvInvoke.MedianBlur(dst, dst, 27);
            CvInvoke.Threshold(dst, output, 255,threshold,ThresholdType.Binary);

            //定義結構元素
            Mat Struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            //Erode:侵蝕，Dilate:擴張
            CvInvoke.Dilate(output,output,Struct_element,new Point(1, 1), 6, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(output,output,Struct_element,new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0));
        }
    }
}
