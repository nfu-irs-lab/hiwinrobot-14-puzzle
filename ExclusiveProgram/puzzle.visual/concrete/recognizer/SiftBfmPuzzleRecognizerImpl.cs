using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class SiftBfmPuzzleRecognizerImpl: PuzzleRecognizerImpl
    {
        private readonly Feature2D featureDetector;

        public SiftBfmPuzzleRecognizerImpl()
        {
            featureDetector = new SIFT();
        }

        public void DetectFeatures(Mat image, object p, VectorOfKeyPoint keyPoints, Mat descriptors, bool v)
        {
            featureDetector.DetectAndCompute(image, null, keyPoints, descriptors,v);
        }

        public void MatchFeatures(Mat modelDescriptors, Mat observedDescriptors, VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints,VectorOfVectorOfDMatch matches)
        {
            int k = 2;
            using (DescriptorMatcher matcher = new BFMatcher(DistanceType.L2Sqr))
            {
                matcher.Add(modelDescriptors);
                matcher.KnnMatch(observedDescriptors, matches, k, null);
            }

        }
    }

}
