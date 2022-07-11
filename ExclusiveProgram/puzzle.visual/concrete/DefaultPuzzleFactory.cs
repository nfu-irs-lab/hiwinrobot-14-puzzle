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

            List<Task> tasks = new List<Task>();

            foreach (LocationResult location in dataList)
            {
                Task task=Task.Factory.StartNew(() => { 
                    var correctedImage = corrector.Correct(image, location);
                    if (listener != null)
                        listener.onCorrected(correctedImage);

                    var recognized_result=recognizer.Recognize(correctedImage);
                    if (listener != null)
                        listener.onRecognized(recognized_result);

                    results.Add(merger.merge(location,correctedImage,recognized_result));
                    image.Dispose();
                });
                tasks.Add(task);
            }
            int completed_task_count= 0;
            int monitor= 0;
            bool[] record=new bool[tasks.Count];
            while (completed_task_count<tasks.Count)
            {
                if (tasks[monitor].IsCompleted && record[monitor] == false)
                {
                    completed_task_count++;
                    record[monitor]=true;
                }
                if (monitor + 1 == tasks.Count)
                    monitor = 0;
                else
                    monitor++;

            }

            return results;
        }

        public void setListener(PuzzleFactoryListener listener)
        {
            this.listener = listener;
        }
    }
}
