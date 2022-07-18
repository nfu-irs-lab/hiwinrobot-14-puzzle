using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{
    public interface IPuzzleResultMerger
    {
        Puzzle_sturct merge(LocationResult locationResult,Image<Bgr,byte> correctedImage,RecognizeResult recognizeResult);
    }
}
