using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InkTestWPF
{
    /// <summary>
    /// Interaction logic for ShowImage.xaml
    /// </summary>
    public partial class ShowImage : Window
    {
        public ShowImage(string file_path)
        {
            InitializeComponent();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "after.bmp", UriKind.Absolute);
            //bi3.UriSource = new Uri(file_path, UriKind.Absolute);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;

            MessageBox.Show(bi3.ToString());
        }
    }
}
