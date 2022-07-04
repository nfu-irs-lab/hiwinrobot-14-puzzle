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


        public DefaultPuzzleFactory(IPuzzleLocator locator,IPuzzleRecognizer recognizer, IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<Puzzle_sturct> Execute(Image<Bgr, byte> input)
        {
            var image = input.Clone();
            List<LocationResult> dataList = locator.Locate(image);

            List<CorrectedPuzzleArgument> arguments = new List<CorrectedPuzzleArgument>();
            foreach (LocationResult location in dataList)
            {
                var correctedImage = corrector.Correct(image, location);
                CorrectedPuzzleArgument argument = new CorrectedPuzzleArgument();
                argument.Coordinate = location.Coordinate;
                argument.Angle = location.Angle;
                argument.image= correctedImage.Clone();
                correctedImage.Dispose();

                argument.Size = location.Size;
                arguments.Add(argument);
            }

            image.Dispose();
            return recognizer.Recognize(arguments);
        }
    }
}
