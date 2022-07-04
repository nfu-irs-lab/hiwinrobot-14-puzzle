using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{

    public interface PuzzleCorrectorListener
    {

        void onROIDetected(Image<Bgr, byte> result,LocationResult locationResult);
        void onBinarizationDone(Image<Gray, byte> result);
        void onPreprocessDone(Image<Gray, byte> result);
        
    }

    public interface IPuzzleCorrector
    {
        Image<Bgr,byte> Correct(Image<Bgr, byte> input,LocationResult raw);
        void setListener(PuzzleCorrectorListener listener);
    }
}
