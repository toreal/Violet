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

namespace WindowsFormsApplication1
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

        Point mStart;
        Point mMove;
        Point mEnd;
        bool flag = true;
        bool flag2 = false;
        Ellipse myEllipse;
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mStart = e.GetPosition(myStackPanel);
            flag2 = !flag2;
        }
        void draw(int x,int y,int w,int h)
        {
            if (flag)
            {
                flag = false;
                myEllipse = new Ellipse();

                // Create a red Ellipse.


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
                myStackPanel.Children.Add(myEllipse);
            }
            else
            {
                myEllipse.Width = w;
                myEllipse.Height = h;
                myEllipse.Margin = new Thickness(x, y, 0, 0);
            }
        }

        private void myStackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag2)
            {
                mMove = e.GetPosition(myStackPanel);
                int px = (int)mStart.X;
                int py = (int)mStart.Y;
                int w = Math.Abs((int)(mMove.X - mStart.X));
                int h = Math.Abs((int)(mMove.Y - mStart.Y));
                if(mMove.X < mStart.X)
                    px = (int)mMove.X;
                if(mMove.Y < mStart.Y)
                    py = (int)mMove.Y;
                draw(px, py, w, h);
            }
        }

        private void myStackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mEnd = e.GetPosition(myStackPanel);
            int px = (int)mStart.X;
            int py = (int)mStart.Y;
            int w = Math.Abs((int)(mMove.X - mStart.X));
            int h = Math.Abs((int)(mMove.Y - mStart.Y));
            if (mMove.X < mStart.X)
                px = (int)mMove.X;
            if (mMove.Y < mStart.Y)
                py = (int)mMove.Y;
            draw(px, py, w, h);
            flag = true;
            flag2 = false;
        }
    }
}
