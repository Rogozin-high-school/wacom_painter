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
namespace WindowsFormsApplication1
{
    public partial class imageSelection : Form
    {
        int pWidth;
        const int pageAmount =20;
        const int ROWLEN = 5;
        const string JSONPATH = "styles1.txt";
        List<PictureBox> pictures = new List<PictureBox>();
        List<Dictionary<string, string>> d = new List<Dictionary<string, string>>();
        int index = 0;
        public imageSelection()
        {
            InitializeComponent();
        }

        private void imageSelection_Load(object sender, EventArgs e)
        {
            Load1();
        }
        public void Load1()
        {
            pWidth = this.Width / ROWLEN;
            string cont = File.ReadAllText(JSONPATH);
            string[] temp = cont.Split("\r\n".ToCharArray());
            foreach (string item in temp)
            {
                //MessageBox.Show(item);
                Dictionary<string, string> td = new Dictionary<string, string>();
                try
                {
                    string[] t2 = item.Split(":".ToCharArray());
                    td["id"] = t2[0];
                    td["title"] = t2[1];
                    d.Add(td);
                }
                catch
                {
                    // MessageBox.Show(item);
                }
            }
            try
            {
                for (int i = 0; i < pageAmount; i++)
                {
                    string filePath = d[index]["id"];
                    Point p = new Point(3 + (pictures.Count % ROWLEN) * 70, 3 + (pictures.Count / ROWLEN) * 107);
                    PictureBox pb = new PictureBox();
                    pb.Name = filePath;
                    pb.Image = Image.FromFile("styles/" + filePath + ".jpg");
                    pb.Location = p;
                    pb.Size = new Size(62, 100);
                    pb.Click += (object s1, EventArgs e1) =>
                    {
                        Pclick(s1, e1);
                    };
                    pictures.Add(pb);
                    Controls.Add(pb);
                    /*Label l = new Label();
                    l.Text = d[index]["title"];
                    l.Name = l.Text;
                    l.Location = new Point(p.X, p.Y + 110);
                    l.Font = new Font(FontFamily.GenericMonospace, 9);
                    Controls.Add(l);*/
                    index++;
                }
            }
            catch
            {

            }
        }
        public void Pclick(object sender,EventArgs e)
        {
            PictureBox p1 = (PictureBox)sender;
            Form1 f = new Form1();
            this.Hide();
            f.style = p1.Name;
            f.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Controls.
            Load();*/
        }
    }
}
