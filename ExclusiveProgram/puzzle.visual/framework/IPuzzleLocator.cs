using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{
    
    public interface PuzzleLocatorListener
    {
        void onPreprocessDone(Image<Gray, byte> result);

        void onLocated(LocationResult result);
    }

    public struct LocationResult
    {
        public Point Coordinate;
        public double Angle;
        public Size Size;
        public Image<Bgr,byte> ROI;
    }

    public interface IPuzzleLocator
    {
        List<LocationResult> Locate(Image<Bgr, byte> input);
        void setListener(PuzzleLocatorListener listener);
    }
}
