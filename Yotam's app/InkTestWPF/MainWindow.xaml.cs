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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Ink;
using System.IO;
using System.Web;
using System.Net;
using System.Drawing.Imaging;

namespace InkTestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string style="Scream";

        //DO NOT TOUCH THIS VARIABLE
        HttpWebRequest requestX = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Sends the data to the Style Trasnfer Server and places the given image
        // On the inkCanvas
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                WebResponse resp = requestX.EndGetResponse(asynchronousResult);
                HttpWebResponse response = (HttpWebResponse)resp;
                Stream streamResponse = response.GetResponseStream();
                Stream stream = response.GetResponseStream();

                // saving image from stream to local file
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save("after.bmp", ImageFormat.Bmp);

                //Creating an imagebrush from the saved image
                // and setting it to the inkCanvas.Background
                ImageBrush ib = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("after.bmp", UriKind.Relative);
                bitmap.EndInit();
                ib.ImageSource = bitmap;
                //TODO : We need to set inkCanvas.Background to ib (ImageBrush)

                // Close the stream object
                streamResponse.Close();
                // Release the HttpWebResponse
                response.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void penButton_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.EditingMode == InkCanvasEditingMode.Ink)
            {
                TogglePenPicker();
            }
            else
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                penButton.Background = Brushes.RoyalBlue;
                eraseButton.Background = Brushes.SkyBlue;
                eraseStrokeButton.Background = Brushes.SkyBlue;
                HidePenPicker();
            }
        }

        void TogglePenPicker()
        {
            if (penPicker.Visibility == Visibility.Collapsed)
            {
                ShowPenPicker();
            }
            else
            {
                HidePenPicker();
            }
        }

        void ShowPenPicker()
        {
            var sb = FindResource("OpenInkPanel") as Storyboard;
            Storyboard.SetTarget(sb, penPicker);
            penPicker.Visibility = Visibility.Visible;
            sb.Begin();
        }

        void HidePenPicker()
        {
            var sb = FindResource("CloseInkPanel") as Storyboard;
            Storyboard.SetTarget(sb, penPicker);
            sb.Completed += (a, b) =>
            {
                penPicker.Visibility = Visibility.Collapsed;
            };
            sb.Begin();
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            eraseButton.Background = Brushes.RoyalBlue;
            penButton.Background = Brushes.SkyBlue;
            eraseStrokeButton.Background = Brushes.SkyBlue;
            HidePenPicker();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int objects = VisualTreeHelper.GetChildrenCount(penPicker);
            for (int i = 0; i < objects; i++)
            {
                Visual btn = (Visual)VisualTreeHelper.GetChild(penPicker, i);
                if (btn is Button)
                {
                    ((Button)btn).Click += colorButton_Click;
                }
            }
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)((Border)((Button)sender).Content).BorderBrush).Color;
            HidePenPicker();
        }

        private void inkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            HidePenPicker();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            inkCanvas.DefaultDrawingAttributes.Height = inkCanvas.DefaultDrawingAttributes.Width = penWidthSlider.Value;
        }

        private void eraseStrokeButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            eraseButton.Background = Brushes.SkyBlue;
            penButton.Background = Brushes.SkyBlue;
            eraseStrokeButton.Background = Brushes.RoyalBlue;
            HidePenPicker();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        private void SelectStyleButton(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1(this);
            w.ShowDialog();
            this.SetStyleLabel();
        }

        private void SetStyleLabel()
        {
            StyleLabel.Content = "Style : " + style;
        }

        private void PretifyButton(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            rtb.Render(inkCanvas);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            FileStream fs = File.Open(@"test.bmp", FileMode.Create);
            encoder.Save(fs);
            fs.Close();

            string file_path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "test.bmp";
            string current_style = "wave";

            string json_data = "{path:" + file_path + ",style:" + current_style + "}";
            json_data = Base64Encode(json_data);

            //The real code
            //requestX = WebRequest.CreateHttp("http://localhost:9182?" + json_data);

            //Debug code
            //will set the inkCanvas to the google logo
            requestX = WebRequest.CreateHttp("https://pbs.twimg.com/profile_images/839721704163155970/LI_TRk1z.jpg");
            requestX.Method = "GET";
            requestX.BeginGetResponse(GetResponseCallback, requestX);
        }
    }
}
