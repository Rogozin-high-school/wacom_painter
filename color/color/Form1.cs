using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace color
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ColorPiker.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimationGB.Animate(ColorPiker, AnimationGB.Effect.Slide, 150, 180);
        }
    }
}
