﻿using System;
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
using System.Threading;
namespace InkTestWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //Set this to the name of the style
        MainWindow mainWindow = null;
        
        public Window1(MainWindow w)
        {
            InitializeComponent();
            mainWindow = w;
        }

        public void Move(string style)
        {
            mainWindow.style = style;
            this.Close();
        }


        private void Click(object sender, MouseButtonEventArgs e)
        {
            Image i = (Image)sender;
            string src = i.Source.ToString();
            string[] temp = src.Split(@"/".ToCharArray());
            src = temp.Last();
            src = src.Split(".".ToCharArray())[0];
            Move(src);
        }
    }
}
