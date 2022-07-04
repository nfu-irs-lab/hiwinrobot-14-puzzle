using Emgu.CV;
using Emgu.CV.Structure;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public interface PuzzleFactoryListener
    {
        void onLocated(List<LocationResult> results);
        void onCorrected(Image<Bgr,byte> result);
        void onRecognized(RecognizeResult result);
    }
    public interface IPuzzleFactory
    {
        List<Puzzle_sturct> Execute(Image<Bgr, byte> input);
        void setListener(PuzzleFactoryListener listener);
    }
}
