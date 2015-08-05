using System;
using System.Collections;
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

   class myAddFunction
   {
       public Point start;
       public Point end;
       public int numCode;

       public void AddLine(int startX, int startY, int endX, int endY, int numCode)
       {
           this.start = new Point(startX, startY);
           this.end = new Point(endX, endY);
           this.numCode = numCode;
       }
   }

    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        
        public UserControl1()
        {
            InitializeComponent();
        }

        myAddFunction maf;
        ArrayList objList = new ArrayList();

        public int drawtype=1;
        public String  colortype = "black";
        public int lineSpace = 25;
        public GraphDoc gdc = new GraphDoc();
        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;
        int strokeT = 1;

        String Status = "rest";
        Point pStart;
        Point pEnd;
        Point p0, p1, p2, p3 = new Point(0, 0);
        BezierSegment bezier = new BezierSegment();
        PathFigure figure = new PathFigure();
        PathGeometry geometry = new PathGeometry();

        Ellipse myEllipse;
        Rectangle myRect;
        Line myLine;
        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();

        bool bfirst = true;
        bool bmousedown = false;

        public void ClearDrawing()
        {
            mygrid.Children.Clear();
        }
        public void drawLine(double w, double h)
        {
            int i;
            int height = (int)h;
            int width = (int)w;
            for (i = 0; i <= height; i += lineSpace)
            {
                myLine = new Line();
                myLine.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myLine.X1 = 0;
                myLine.X2 = width;
                myLine.Y1 = i;
                myLine.Y2 = i;
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = strokeT;
                myLine.Opacity = 0.2;
                myBackground.Children.Add(myLine);
            }
            for (i = 0; i <= width; i += lineSpace)
            {
                myLine = new Line();
                myLine.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myLine.X1 = i;
                myLine.X2 = i;
                myLine.Y1 = 0;
                myLine.Y2 = height;
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = strokeT;
                myLine.Opacity = 0.2;
                myBackground.Children.Add(myLine);
            }
        }
        public void stroke(int stroketype)
        {
            switch (stroketype){
                case 1:
                    strokeT = 1;
                    break;
                case 3:
                    strokeT = 3;
                    break;
                case 5:
                    strokeT = 5;
                    break;
                case 10:
                    strokeT = 8;
                    break;
            }
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
        /*void saveFunction(LineGeometry lg)
        {
            objList.Add(lg);
            showArrayList();
        }*/
        /*void showArrayList()
        {
            Console.WriteLine(objList.Count);
        }*/
        //曲線曲線曲線曲線曲線曲線曲線曲線曲線曲線曲線
        public void Curve(int xStart, int yStart, int xEnd, int yEnd)
        {
            if(bfirst){
                if (Status.Equals("rest"))
                {
                    bfirst = false;
                    bezier = new BezierSegment();
                    bezier.Point3 = new Point(xEnd, yEnd);
                    figure = new PathFigure();
                    figure.StartPoint = new Point(xStart, yStart);
                    bezier.Point1 = figure.StartPoint;
                    bezier.Point2 = bezier.Point3;
                    p0 = figure.StartPoint;
                    p1 = bezier.Point1;
                    figure.Segments.Add(bezier);
                    geometry = new PathGeometry();
                    geometry.Figures.Add(figure);
                    myPath = new System.Windows.Shapes.Path();
                    myPath.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                    myPath.StrokeThickness = strokeT;
                    myPath.Data = geometry;
                    mygrid.Children.Add(myPath);
                    Status = "work1";
                }
                else if (Status.Equals("work1"))
                {
                    bfirst = false;
                    mygrid.Children.Remove(myPath);
                    bezier.Point1 = new Point(xEnd, yEnd);
                    bezier.Point3 = p3;
                    figure = new PathFigure();
                    figure.StartPoint = p0;
                    figure.Segments.Add(bezier);
                    geometry = new PathGeometry();
                    geometry.Figures.Add(figure);
                    myPath.Data = geometry;
                    mygrid.Children.Add(myPath);
                    Status = "work2";
                }
                else if (Status.Equals("work2"))
                {
                    bfirst = false;
                    mygrid.Children.Remove(myPath);
                    bezier.Point2 = new Point(xEnd, yEnd);
                    bezier.Point1 = p1;
                    bezier.Point3 = p3;
                    figure = new PathFigure();
                    figure.StartPoint = p0;
                    figure.Segments.Add(bezier);
                    geometry = new PathGeometry();
                    geometry.Figures.Add(figure);
                    myPath.Data = geometry;
                    mygrid.Children.Add(myPath);
                    Status = "rest";
                }
            }
            else
            {
                if (Status.Equals("work1"))
                {
                    bezier.Point3 = new Point(xEnd, yEnd);
                    p3 = bezier.Point3;
                    bezier.Point2 = bezier.Point3;
                    p2 = bezier.Point2;
                }
                else if (Status.Equals("work2"))
                {
                    bezier.Point1 = new Point(xEnd, yEnd);
                    p1 = bezier.Point1;
                }
                else if (Status.Equals("rest"))
                {
                    bezier.Point2 = new Point(xEnd, yEnd);
                    p2 = bezier.Point2;
                }
            }
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
                myLine.StrokeThickness = strokeT;
                mygrid.Children.Add(myLine);
            }
            else
            {
                myLine.X2 = xEnd;
                myLine.Y2 = yEnd;

                //myline mline = new myline();
                
                //mline.a = new Point()
                //objList.Add(mline);

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
                myRect.StrokeThickness = strokeT;
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
                myEllipse.StrokeThickness = strokeT;
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
            int w = Math.Abs(ex-px);
            int h = Math.Abs(ey-py);
            if (pEnd.X < pStart.X)
                px = (int)pEnd.X;
            if (pEnd.Y < pStart.Y)
                py = (int)pEnd.Y;
            if (w % 25 != 0)
                w = 25 * (int)(w / 25);
            if (h % 25 != 0)
                h = 25 * (int)(h / 25);
            if (px % 25 != 0)
                px = 25 * (int)(px / 25);
            if (py % 25 != 0)
                py = 25 * (int)(py / 25);
            if (ex % 25 != 0)
                ex = 25 * (int)(ex / 25);
            if (ey % 25 != 0)
                ey = 25 * (int)(ey / 25);
            maf = new myAddFunction();

            gPath gp = new gPath();
            gp.state.colorB = colorB;
            gp.state.colorG = colorG;
            gp.state.colorR = colorR;
            gp.state.strokeT = strokeT;
            gp.drawtype = drawtype;
            gp.x1 = px;
            gp.y1 = py;
            gp.x2 = ex;
            gp.y2 = ey;

            if (drawtype  <=2) // 應修改成一致
            {
                gp.x2 = w;
                gp.y2 = h;


            }

            if (drawtype == 3)
            { 
                gp.y1 = (int)pStart.Y;

                if (gp.y1 % 25 != 0)
                    gp.y1 = 25 * (int)(gp.y1 / 25);
            
    
            }
            gdc.PathList.Add(gp);

            if (true) //新舊碼切換(暫時)
            {
                           reDraw();
            }else
            { 

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
                    maf.AddLine(px, py, ex, ey, objList.Count);
                    objList.Add(maf);
                    break;
                case 4:
                    Curve(px, py, ex, ey);
                    myPath.Opacity = 1;
                    break;
            }

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
                if (w % 25 != 0)
                    w = 25 * (int)(w / 25);
                if (h % 25 != 0)
                    h = 25 * (int)(h / 25);
                if (px % 25 != 0)
                    px = 25 * (int)(px / 25);
                if (py % 25 != 0)
                    py = 25 * (int)(py / 25);
                if (ex % 25 != 0)
                    ex = 25 * (int)(ex / 25);
                if (ey % 25 != 0)
                    ey = 25 * (int)(ey / 25);
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
                    case 4:
                        Curve(px, py, ex, ey);
                        myPath.Opacity = 0.5;
                        break;
                }
            }
        }

        void reDraw()
        {

            ClearDrawing();

            foreach ( gPath gpath in gdc.PathList )
            {

                if ( gpath != null )
                {
                    
                      colorR = gpath.state.colorR;
                      colorG = gpath.state.colorG;
                      colorB = gpath.state.colorB;
                      strokeT = gpath.state.strokeT;

                      
                      switch (gpath.drawtype)
                      {
                          case 1:
                              drawEllipse(gpath.x1, gpath.y1, gpath.x2, gpath.y2);
                              myEllipse.Opacity = 1;
                              break;
                          case 2:
                              drawRect(gpath.x1, gpath.y1, gpath.x2, gpath.y2);
                              myRect.Opacity = 1;
                              break;
                          case 3:
                              drawLine(gpath.x1, gpath.y1, gpath.x2, gpath.y2);
                              myLine.Opacity = 1;
                              //maf.AddLine(px, py, ex, ey, objList.Count);
                              //objList.Add(maf);
                              break;
                          case 4:
                              Curve(gpath.x1, gpath.y1, gpath.x2, gpath.y2);
                              myPath.Opacity = 1;
                              break;
                      }


                      bfirst = true;
                }

            }


        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            int margin = (int)mygrid.Margin.Left;
            int width = (int)mygrid.ActualWidth + (int)mygrid.Margin.Left + (int)mygrid.Margin.Right;
            int height = (int)mygrid.ActualHeight +
                (int)mygrid.Margin.Top + (int)mygrid.Margin.Bottom;
            mygrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                Rect r = new Rect(new System.Windows.Point(0, 0), new System.Windows.Size(width, height));

                dc.DrawRectangle(new SolidColorBrush(Colors.White), new Pen(), r);
                VisualBrush vb = new VisualBrush(mygrid);

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
            ClearDrawing();
        }
        
        
    }
}
