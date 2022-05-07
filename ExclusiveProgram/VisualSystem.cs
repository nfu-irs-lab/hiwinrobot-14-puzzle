using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace Puzzle.Framework
{
    public class RawPuzzle
    {
        public Point Position{ set; get; }
        public double Angle_degree{ set; get; }
        public Size Size { set; get; }

    }
    public enum PuzzleShape
    {

    }

    public enum PuzzleContent
    {

    }

    public class PuzzlePiece
    {
        public PuzzleType type {  get; }
        public Image<Bgr,Byte> Image {  get; }

        public double Angle_degree{ set; get; }
        public Size Size { set; get; }

        public PuzzlePiece(PuzzleType type,double angle,Size size,Image<Bgr,Byte> Image)
        {
            this.type = type;
            this.Image = Image;
            this.Angle_degree = angle;
            this.Size = size;
        }
    }

    public enum PuzzleType
    {
        
    }
    public interface IPuzzleLocator
    {
        List<RawPuzzle> Locate(Image<Bgr, Byte> input);
    }

    public interface IPuzzleCorrector
    {
        Image<Bgr,Byte> Correct(Image<Bgr, Byte> input,RawPuzzle raw);
    }

    public interface IPuzzleRecognizer
    {
        List<PuzzleType> Recognize(List<Image<Bgr, Byte>> input);
    }

    public interface IPuzzleFactory
    {
        List<PuzzlePiece> Execute(Image<Bgr, Byte> input);
    }

    public class DefaultPuzzleFactory : IPuzzleFactory
    {
        private IPuzzleRecognizer recognizer;
        private IPuzzleLocator locator;
        private IPuzzleCorrector corrector;

        public DefaultPuzzleFactory(IPuzzleRecognizer recognizer,IPuzzleLocator locator,IPuzzleCorrector corrector)
        {
            this.recognizer = recognizer;
            this.locator = locator;
            this.corrector = corrector;
        }
        public List<PuzzlePiece> Execute(Image<Bgr, byte> input)
        {
            List<RawPuzzle> raws=locator.Locate(input);
            List<Image<Bgr,Byte>> PuzzleImages=new List<Image<Bgr, byte>>();
            foreach(RawPuzzle piece in raws)
            {
                PuzzleImages.Add(corrector.Correct(input, piece));
            }

            List<PuzzleType> types=recognizer.Recognize(PuzzleImages);
               
            List<PuzzlePiece> puzzles=new List<PuzzlePiece>();
            for(int i = 0; i < PuzzleImages.Count; i++)
            {
                puzzles.Add(new PuzzlePiece(types[i],raws[i].Angle_degree,raws[i].Size,PuzzleImages[i]));
            }
            return puzzles;
        }
    }

}
