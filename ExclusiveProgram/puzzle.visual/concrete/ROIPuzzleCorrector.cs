using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ExclusiveProgram.puzzle.visual.framework;
using System;
using System.Drawing;

namespace ExclusiveProgram.puzzle.visual.concrete
{
    public class ROIPuzzleCorrector : IPuzzleCorrector
    {
        private PuzzleCorrectorListener listener;

        public Image<Bgr, byte> Correct(Image<Bgr, byte> input, LocationResult raw)
        {
            Rectangle rect = new Rectangle((int)(raw.Coordinate.X - raw.Size.Width / 2.0f), (int)(raw.Coordinate.Y - raw.Size.Height / 2.0f), raw.Size.Width, raw.Size.Height);

            Mat Ori_img = VisualSystem.Image2Mat<Bgr>(input);

            //將ROI選取區域使用Mat型式讀取
            Image<Bgr, byte> Copy_ = new Mat(Ori_img, rect).ToImage<Bgr, byte>();
            
            if (listener != null)
            {
                listener.onROIDetected(Copy_,raw);
            }

            return Copy_;
        }

        public void setListener(PuzzleCorrectorListener listener)
        {
            this.listener=listener; 
        }

    }
}
