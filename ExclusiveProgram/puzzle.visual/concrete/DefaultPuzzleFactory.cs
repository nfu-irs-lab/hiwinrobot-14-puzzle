using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class DefaultPuzzleFactory : IPuzzleFactory
    {
        private IPuzzleRecognizer recognizer;
        private IPuzzleLocator locator;
        private IPuzzleCorrector corrector;
        private readonly int threshold;


        public DefaultPuzzleFactory(int threshold,IPuzzleLocator locator,IPuzzleRecognizer recognizer, IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.threshold = threshold;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<Puzzle_sturct> Execute(Image<Bgr, byte> input)
        {
            var image = input.Clone();
            var dst = ColorImagePreprocess(image);
            var bin = BinaryImagePreprocess(dst);


            List<LocationResult> dataList = locator.Locate(bin);

            List<CorrectedPuzzleArgument> arguments = new List<CorrectedPuzzleArgument>(); 

            foreach (LocationResult location in dataList)
            {
                var i = corrector.Correct(image, location);
                CorrectedPuzzleArgument argument = new CorrectedPuzzleArgument();
                argument.Coordinate = location.Coordinate;
                argument.Angle = location.Angle;
                argument.image= i.Clone().Mat;
                i.Dispose();

                argument.Size = location.Size;
                arguments.Add(argument);
            }

            image.Dispose();
            return recognizer.Recognize(arguments);
        }
        private Image<Bgr,byte> ColorImagePreprocess(Image<Bgr,byte> image)
        {
            var dst = new Image<Bgr, byte>(image.Size);
            VisualSystem.WhiteBalance(image,dst);
            VisualSystem.ExtendColor(dst,dst);
            CvInvoke.MedianBlur(dst, dst, 27);
            return dst;
        }
        private Image<Gray,byte> BinaryImagePreprocess(Image<Bgr,byte> image)
        {
            var bin=new Image<Gray, byte>(image.Size);
            CvInvoke.CvtColor(image, bin, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(bin, bin,threshold, 255, ThresholdType.Binary);
            return bin;
        }
    }
}
