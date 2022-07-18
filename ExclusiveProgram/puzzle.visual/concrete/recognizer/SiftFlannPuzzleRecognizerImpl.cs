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
    public class SiftFlannPuzzleRecognizerImpl: PuzzleRecognizerImpl
    {
        private readonly Feature2D featureDetector;

        public SiftFlannPuzzleRecognizerImpl()
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
            // KdTree for faster results / less accuracy
            using (var ip = new Emgu.CV.Flann.KdTreeIndexParams())
            using (var sp = new SearchParams())
            using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
            {
                matcher.Add(modelDescriptors);
                matcher.KnnMatch(observedDescriptors, matches, k, null);
            }

        }
    }

}
