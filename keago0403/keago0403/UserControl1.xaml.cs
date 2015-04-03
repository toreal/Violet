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

namespace keago0403
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        Point pStart;
        Point pEnd;

        Ellipse myEllipse;

        bool bfirst = true;
        bool bmousedown = false;


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pStart = e.GetPosition(mygrid);

            bmousedown = true;
        }


        void drawEllipse(int x, int y, int w, int h)
        {

            if (bfirst)
            {
                bfirst = false;
                myEllipse = new Ellipse();

                // Create a SolidColorBrush with a red color to fill the 
                // Ellipse with.
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = 2;
                myEllipse.Stroke = Brushes.Black;

                // Set the width and height of the Ellipse.
                myEllipse.Width = w;
                myEllipse.Height = h;
                myEllipse.Margin = new Thickness(x, y, 0, 0);

                // Add the Ellipse to the StackPanel.

                mygrid.Children.Add(myEllipse);

            }
            else
            {

                myEllipse.Width = w;
                myEllipse.Height = h;
                myEllipse.Margin = new Thickness(x, y, 0, 0);

            }


        }

        private void mygrid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            pEnd = e.GetPosition(mygrid);


            int px = (int)pStart.X;
            int py = (int)pStart.Y;
            int w = Math.Abs((int)(pEnd.X - pStart.X));
            int h = Math.Abs((int)(pEnd.Y - pStart.Y));
            if (pEnd.X < pStart.X)
                px = (int)pEnd.X;
            if (pEnd.Y < pStart.Y)
                py = (int)pEnd.Y;


            drawEllipse(px, py, w, h);

            bfirst = true;
            bmousedown = false;
            myEllipse.Opacity = 1;

        }

        private void mygrid_MouseMove(object sender, MouseEventArgs e)
        {

            if (bmousedown)
            {
                pEnd = e.GetPosition(mygrid);
                int px = (int)pStart.X;
                int py = (int)pStart.Y;
                int w = Math.Abs((int)(pEnd.X - pStart.X));
                int h = Math.Abs((int)(pEnd.Y - pStart.Y));
                if (pEnd.X < pStart.X)
                    px = (int)pEnd.X;
                if (pEnd.Y < pStart.Y)
                    py = (int)pEnd.Y;
                drawEllipse(px, py, w, h);
                myEllipse.Opacity = 0.5;
                // drawEllipse((int)pStart.X, (int)pStart.Y, (int)(pEnd.X - pStart.X), (int)(pEnd.Y - pStart.Y), 1);
            }
        }


    }
}
