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
    public class PuzzleResultMerger : IPuzzleResultMerger
    {
        public PuzzleResultMerger()
        {
        }

        public Puzzle_sturct merge(LocationResult locationResult, Image<Bgr, byte> correctedImage, RecognizeResult recognizeResult)
        {
            Puzzle_sturct register = new Puzzle_sturct();
            register.id= locationResult.id;
            register.coordinate = locationResult.Coordinate;
            register.position = recognizeResult.position;
            register.Angel = recognizeResult.Angle+locationResult.Angle;
            register.image = correctedImage;

            //register.puzzle_region = Puzzle[i].puzzle_region;
            //register.image.Save(path_image_folder + "test" + i.ToString() + "_" + register.position + ".jpg");

            return register;
        }
    }
}
