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

namespace InkTestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
