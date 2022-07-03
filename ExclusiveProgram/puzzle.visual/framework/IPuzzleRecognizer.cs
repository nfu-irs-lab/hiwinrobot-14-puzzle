using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.framework
{
    public struct CorrectedPuzzleArgument
    {
        public Point Coordinate;
        public double Angle;
        public Size Size;
        public Image<Bgr,byte> image;
    }

    public struct result
    {
        public double Angel;
        public Image<Bgr,byte> image;
        public PointF[] pts;
    };

    public struct Puzzle_sturct
    {
        public double Angel;
        public Point coordinate;
        public Image<Bgr,byte> image;
        public string position;
    };

    public interface IPuzzleRecognizer
    {
        List<Puzzle_sturct> Recognize(List<CorrectedPuzzleArgument> input);
    }

}
