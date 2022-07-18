using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.framework.utils
{
    public interface IPuzzlePreProcessImpl 
    {
        void Preprocess(Image<Bgr, byte> input, Image<Bgr, byte> output);
    }
    public interface IPuzzleGrayConversionImpl 
    {
        void ConvertToGray(Image<Bgr, byte> input, Image<Gray, byte> output);
    }

    public interface IPuzzleThresholdImpl 
    {
        void Threshold(Image<Gray, byte> input, Image<Gray, byte> output);
    }

    public interface IPuzzleBinaryPreprocessImpl 
    {
        void BinaryPreprocess(Image<Gray, byte> input, Image<Gray, byte> output);
    }
}
