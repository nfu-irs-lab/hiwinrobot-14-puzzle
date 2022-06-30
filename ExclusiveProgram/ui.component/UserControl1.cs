using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExclusiveProgram.ui.component
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void setImage(Bitmap bitmap)
        { 
            pictureBox1.Image = bitmap;
        }
        public void setLabel(double angle,Point point)
        { 
            label1.Text=angle.ToString();
            label2.Text=point.ToString();   
        }
    }

}
