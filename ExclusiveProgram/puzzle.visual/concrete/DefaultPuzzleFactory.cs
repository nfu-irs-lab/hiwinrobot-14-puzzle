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
        private readonly IPuzzleResultMerger merger;
        private PuzzleFactoryListener listener;

        public DefaultPuzzleFactory(IPuzzleLocator locator,IPuzzleRecognizer recognizer, IPuzzleCorrector corrector,IPuzzleResultMerger merger)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.corrector = corrector;
            this.merger = merger;
        }
        public List<Puzzle_sturct> Execute(Image<Bgr, byte> input)
        {
            var image = input.Clone();
            List<LocationResult> dataList = locator.Locate(image);
            if(listener != null)
                listener.onLocated(dataList);


            List<Puzzle_sturct> results = new List<Puzzle_sturct>();
            foreach (LocationResult location in dataList)
            {
                var correctedImage = corrector.Correct(image, location);
                if (listener != null)
                    listener.onCorrected(correctedImage);

                var recognized_result=recognizer.Recognize(correctedImage);
                if (listener != null)
                    listener.onRecognized(recognized_result);

                results.Add(merger.merge(location,correctedImage,recognized_result));
            }

            image.Dispose();
            return results;
        }

        public void setListener(PuzzleFactoryListener listener)
        {
            this.listener = listener;
        }
    }
}
