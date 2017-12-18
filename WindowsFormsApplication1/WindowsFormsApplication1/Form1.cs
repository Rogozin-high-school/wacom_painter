using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Bitmap b;
        const int width = 1366;
        const int height = 768;
        byte[] arr = new byte[1366*768*3];
        int pointSize = 0;
        Dictionary<string, Color> colors = new Dictionary<string, Color>();
        public string style="";
        private void setColors()
        {
            colors["red"] = Color.Red;
            colors["green"] = Color.Green;
        }
        public Form1()
        {
            InitializeComponent();
            pictureBox1 = pictureBox2;
            setColors();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if(this.style=="")
            {
                imageSelection isS = new imageSelection();
                this.Hide();
                isS.ShowDialog();
                this.Close();
            }
            MessageBox.Show(style);
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    if (timer1.Enabled)
                    {
                        int x = Cursor.Position.X - this.Location.X - pictureBox1.Location.X - 8;
                        int y = Cursor.Position.Y - this.Location.Y - pictureBox1.Location.Y - 30;
                        print(x, y, pointSize, b, colors["red"]);//change "red" to selected color
                        Invoke((MethodInvoker)delegate ()
                        {
                            pictureBox1.Image = b;
                        });
                        //Thread.Sleep(1);
                    }
                }
            });
            t.Start();
            this.BackColor = Color.White;
            b = new Bitmap(pictureBox1.Size.Width,pictureBox1.Size.Height);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                    b.SetPixel(i, j, Color.LightGray);
            pictureBox1.Image = b;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            timer1.Interval = 1;
            //t.Start();
        }
      


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
            /*print(e.X, e.Y, pointSize, b, colors["red"]);//change "red" to selected color
            pictureBox1.Image = b;*/
        }
        public static void printPlus(int x,int y,int r,Bitmap b,Color c)
        {
            for(int i=0;i<r;i++)
            {
                b.SetPixel(x - i, y,c);
                b.SetPixel(x + i, y, c);
                b.SetPixel(x, y - i, c);
                b.SetPixel(x, y + i, c);
            }
        }
        public static void print(int x,int y,int r,Bitmap b,Color c)
        {
            Point s = new Point(x - r, y - r);
            for(int i=-r;i<r;i++)
            {
                for(int j=-r;j<r;j++)
                {
                    if (distance((double)i,(double)j)<=r)
                    {
                        try
                        {
                            b.SetPixel(x + i, y + j, c);
                        }
                        catch{ }
                    }
                }
            }
        }
        public static double distance(double x,double y)
        {
            x = Math.Pow(x, 2.0);
            y = Math.Pow(y, 2.0);
            return Math.Sqrt(x+y);
        }
        public static void printX(int x,int y,int r,Bitmap b,Color c)
        {
            if (r == 0)
                return;
            for (int i = 0; i < r; i++)
                b.SetPixel(x - i, y - i, c);
            for (int i = 0; i < r; i++)
                b.SetPixel(x - i, y + i, c);
            for (int i = 0; i < r; i++)
                b.SetPixel(x + i, y - i,c);
            for (int i = 0; i < r; i++)
                b.SetPixel(x + i, y + i, c);
            printX(x, y, r - 1,b,c);
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //MessageBox.Show(trackBar1.Value.ToString());
            pointSize = trackBar1.Value+1;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Point p = System.Windows.Forms.Cursor.Position;
            /*int x = Cursor.Position.X-this.Location.X - pictureBox1.Location.X-8;
            int y = Cursor.Position.Y - this.Location.Y - pictureBox1.Location.Y-30;
            print(x,y, pointSize, b, colors["red"]);//change "red" to selected color
            pictureBox1.Image = b;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageConverter ic = new ImageConverter();
            byte[] arr = (byte[])ic.ConvertTo(b, typeof(byte[]));
            string base64String = Convert.ToBase64String(arr);
            richTextBox1.Text = base64String;
        }
    }
}
