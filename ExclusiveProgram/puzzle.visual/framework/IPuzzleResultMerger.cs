using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework
{
    public interface IPuzzleResultMerger
    {
        Puzzle_sturct merge(LocationResult locationResult,RecognizeResult recognizeResult);
    }
}
