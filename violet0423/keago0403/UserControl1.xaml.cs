using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

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

        public int drawtype=1;
        public String  colortype = "White";

        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;

        Point pStart;
        Point pEnd;

        Ellipse myEllipse;
        Rectangle myRect;
        Line myLine;

        bool bfirst = true;
        bool bmousedown = false;

        public void ClearDrawing()
        {
            mygrid.Children.Clear();
        }
        public void color(String CName) {
            colortype = CName;
            switch (colortype)
            {
                case "red":
                    colorR = 255;
                    colorG = 0;
                    colorB = 0;
                    break;
                case "orange":
                    colorR = 255;
                    colorG = 165;
                    colorB = 0;
                    break;
                case "yellow":
                    colorR = 255;
                    colorG = 230;
                    colorB = 0;
                    break;
                case "green":
                    colorR = 0;
                    colorG = 128;
                    colorB = 0;
                    break;
                case "blue":
                    colorR = 0;
                    colorG = 0;
                    colorB = 128;
                    break;
                case "black":
                    colorR = 0;
                    colorG = 0;
                    colorB = 0;
                    break;
                case "white":
                    colorR = 255;
                    colorG = 255;
                    colorB = 255;
                    break;

            }
        }



        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pStart = e.GetPosition(mygrid);

            bmousedown = true;
        }
        //線線線線線線線線線線線線線線線
        void drawLine(int xStart, int yStart, int xEnd, int yEnd)
        {
            if (bfirst)
            {
                bfirst = false;
                myLine = new Line();
                myLine.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myLine.X1 = xStart;
                myLine.X2 = xEnd;
                myLine.Y1 = yStart;
                myLine.Y2 = yEnd;
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 2;
                mygrid.Children.Add(myLine);

            }
            else
            {
                myLine.X2 = xEnd;
                myLine.Y2 = yEnd;
            }
        }

        //正方形正方形正方形正方形正方形正方形
        void drawRect(int x, int y, int w, int h)
        {
            if (bfirst)
            {
                bfirst = false;
                myRect = new Rectangle();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb( 0, colorR, colorG, colorB);
                myRect.Fill = mySolidColorBrush;
                myRect.StrokeThickness = 2;
                myRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                
                myRect.Width = w;
                myRect.Height = h;
                myRect.Margin = new Thickness(x, y, 0, 0);

                mygrid.Children.Add(myRect);
            }
            else {
                myRect.Width = w;
                myRect.Height = h;
                myRect.Margin = new Thickness(x, y, 0, 0);

            }
        }

        //圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形
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
                mySolidColorBrush.Color = Color.FromArgb(0, 0, 0, 255);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = 2;
                myEllipse.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));

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
            int ex = (int)pEnd.X;
            int ey = (int)pEnd.Y;
            int w = Math.Abs((int)(pEnd.X - pStart.X));
            int h = Math.Abs((int)(pEnd.Y - pStart.Y));
            if (pEnd.X < pStart.X)
                px = (int)pEnd.X;
            if (pEnd.Y < pStart.Y)
                py = (int)pEnd.Y;


            switch (drawtype)
            {
                case 1:
                    drawEllipse(px, py, w, h);
                    myEllipse.Opacity = 1;
                    break;
                case 2:
                    drawRect(px, py, w, h);
                    myRect.Opacity = 1;
                    break;
                case 3:
                    drawLine(px, py, ex, ey);
                    myLine.Opacity = 1;
                    break;
              



            }

            bfirst = true;
            bmousedown = false;
            

        }

        private void mygrid_MouseMove(object sender, MouseEventArgs e)
        {

            if (bmousedown)
            {
                pEnd = e.GetPosition(mygrid);
                int px = (int)pStart.X;
                int py = (int)pStart.Y;
                int ex = (int)pEnd.X;
                int ey = (int)pEnd.Y;
                int w = Math.Abs((int)(pEnd.X - pStart.X));
                int h = Math.Abs((int)(pEnd.Y - pStart.Y));
                if (pEnd.X < pStart.X)
                    px = (int)pEnd.X;
                if (pEnd.Y < pStart.Y)
                    py = (int)pEnd.Y;

                switch (drawtype)
                {
                    case 1:
                        drawEllipse(px, py, w, h);
                        myEllipse.Opacity = 0.5;
                        break;
                    case 2:
                        drawRect(px, py, w, h);
                        myRect.Opacity = 0.5;
                        break;
                    case 3:
                        drawLine(px, py, ex, ey);
                        myLine.Opacity = 0.5;
                        break;

                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            int margin = (int)mygrid.Margin.Left;
            int width = (int)mygrid.ActualWidth + (int)mygrid.Margin.Left + (int)mygrid.Margin.Right;
            int height = (int)mygrid.ActualHeight +
                (int)mygrid.Margin.Top + (int)mygrid.Margin.Bottom;

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(mygrid);
                Rect r = new Rect(new System.Windows.Point(0, 0), new System.Windows.Size(width, height));

                vb.Stretch = Stretch.UniformToFill;
                dc.DrawRectangle(vb, null, r);

            }
            rtb.Render(dv);
            //save the ink to a memory stream
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            byte[] bitmapBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                //get the bitmap bytes from the memory stream
                ms.Position = 0;
                bitmapBytes = ms.ToArray();
            }

            Utility _utility = new Utility();

            _utility.BitmapBytes = bitmapBytes;

          //  this.Dispose(true);
          //  this.Close();
            _utility.TagName = "test";// cbxTagName.SelectedItem.ToString();
            Globals.ThisAddIn.AddPictureContentControl(_utility);



        }


    }
}
