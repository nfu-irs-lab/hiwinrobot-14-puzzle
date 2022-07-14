using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using ExclusiveProgram.puzzle.visual.framework.utils;
using ExclusiveProgram.threading;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class DefaultPuzzleFactory : IPuzzleFactory
    {
        private IPuzzleRecognizer recognizer;
        private IPuzzleLocator locator;
        private readonly IPuzzleResultMerger merger;
        private readonly IPuzzlePreProcessImpl preProcessImpl;
        private PuzzleFactoryListener listener;
        private readonly TaskFactory factory;
        private readonly CancellationTokenSource cts;

        public DefaultPuzzleFactory(IPuzzleLocator locator, IPuzzleRecognizer recognizer, IPuzzleResultMerger merger,IPuzzlePreProcessImpl preProcessImpl,int threadCount)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.merger = merger;
            this.preProcessImpl = preProcessImpl;

            // Create a scheduler that uses two threads.
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(threadCount);

            // Create a TaskFactory and pass it our custom scheduler.
            factory = new TaskFactory(lcts);
            cts = new CancellationTokenSource();
        }

        public List<Puzzle_sturct> Execute(Image<Bgr, byte> input)
        {
            if (!recognizer.ModelImagePreprocessIsDone())
                recognizer.PreprocessModelImage();

            Image<Bgr, byte> stage1= new Image<Bgr, byte>(input.Size);
            Image<Gray, byte> stage2= new Image<Gray, byte>(input.Size);
            preProcessImpl.Preprocess(input,stage1);
            preProcessImpl.ConvertToGray(stage1,stage2);
            preProcessImpl.Threshold(stage2,stage2);
            if (listener != null)
                listener.onPreprocessDone(stage2);

            List<LocationResult> dataList = locator.Locate(stage2,input);
            if (listener != null)
                listener.onLocated(dataList);


            List<Puzzle_sturct> results = new List<Puzzle_sturct>();


            List<Task> tasks = new List<Task>();
            foreach (LocationResult location in dataList)
            {
                Task task = factory.StartNew(() =>
                {
                    var recognized_result = recognizer.Recognize(location.ROI);
                    if (listener != null)
                        listener.onRecognized(recognized_result);

                    results.Add(merger.merge(location, location.ROI, recognized_result));
                },cts.Token);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            stage1.Dispose();
            stage2.Dispose();
            cts.Dispose();
            return results;
        }

        public void setListener(PuzzleFactoryListener listener)
        {
            this.listener = listener;
        }
    }

}
