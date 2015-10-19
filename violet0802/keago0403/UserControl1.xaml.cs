using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;
using System.Xml.Serialization;

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

        public int drawtype = 1;
        public String colortype = "black";
        public int lineSpace = 9;
        public GraphDoc gdc = new GraphDoc();
        public RUse ru = new RUse();
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
        gPath tempGPath;
        Ellipse myEllipse;
        Rectangle myRect;
        Line myLine;
        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();

        bool bfirst = true;
        bool bmousedown = false;
        bool bhave = false;

        public void ClearDrawing()
        {
            mygrid.Children.Clear();
            gdc.sroot.PathList.Clear();
        }
        public void hideBackLine()
        {
            myBackground.Children.Clear();
        }
        public void drawBackLine(double w, double h, double opac)
        {
            int i;
            int height = (int)h;
            int width = (int)w;
            int tempStroke = strokeT;
            strokeT = 1;
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
                myLine.Opacity = opac;
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
                myLine.Opacity = opac;
                myBackground.Children.Add(myLine);
            }
            strokeT = tempStroke;
        }
        public void stroke(int stroketype)
        {
            switch (stroketype)
            {
                case 1:
                    strokeT = 1;
                    break;
                case 3:
                    strokeT = 3;
                    break;
                case 5:
                    strokeT = 5;
                    break;
                case 10:        //可考慮拿掉,有點太粗了
                    strokeT = 8;
                    break;
            }
        }
        public void color(String CName)
        {
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
        private void myControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pStart = e.GetPosition(myControl);
            tempGPath = new gPath();
            //ru.Sel = gdc.sroot.PathList.Count;
            if (drawtype == 5)
            {
                if (gdc.selIndex < 0)
                {
                    gdc.selIndex = gdc.sroot.PathList.Count - 1;
                    myChange.Opacity = .0;
                }
                else
                {
                    ru = gdc.checkOut(pStart);
                    if (ru.Node >= 0 && ru.Sel >= 0)
                    {
                        tempGPath = gdc.sroot.PathList[ru.Sel];
                        pStart = ru.Point;
                        gdc.node = ru.Node;
                        bhave = true;
                        bmousedown = true;
                    }
                }
            }
            else
            {
                gdc.selIndex = -1;
            }
            //bmousedown = true;
            //Debug.WriteLine("true");
        }

        public void drawCurve(Point point0, Point point1, Point point2, Point point3)
        {
            if (bfirst)
            {
                bfirst = false;
                bezier = new BezierSegment();
                bezier.Point3 = point3;
                figure = new PathFigure();
                figure.StartPoint = point0;
                bezier.Point1 = point1;
                bezier.Point2 = point2;
                figure.Segments.Add(bezier);
                geometry = new PathGeometry();
                geometry.Figures.Add(figure);
                myPath = new System.Windows.Shapes.Path();
                myPath.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myPath.StrokeThickness = strokeT;
                myPath.Data = geometry;
                if (bmousedown)
                {
                    myChange.Children.Add(myPath);
                }
                else
                {
                    mygrid.Children.Add(myPath);
                }
            }
        }
        //曲線曲線曲線曲線曲線曲線曲線曲線曲線曲線曲線
        
        public void drawCurve(int xStart, int yStart, int xEnd, int yEnd)
        {
            if (bfirst)
            {
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
                Status = "rest";
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
                if (bmousedown)
                {
                    myChange.Children.Add(myLine);
                }
                else
                {
                    mygrid.Children.Add(myLine);
                }
            }
            else
            {
                myLine.X2 = xEnd;
                myLine.Y2 = yEnd;
            }
        }

        //正方形正方形正方形正方形正方形正方形
        void drawRect(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        {
            if (bfirst)
            {
                Status = "rest";
                bfirst = false;
                myRect = new Rectangle();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(bfill, colorR, colorG, colorB);
                myRect.Fill = mySolidColorBrush;
                myRect.StrokeThickness = strokeT;
                myRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myRect.Width = Math.Abs(xEnd - xStart);
                myRect.Height = Math.Abs(yEnd - yStart);
                myRect.Margin = new Thickness(xStart, yStart, 0, 0);
                if (bmousedown)
                {
                    myChange.Children.Add(myRect);
                }
                else
                {
                    mygrid.Children.Add(myRect);
                }
            }
            else
            {
                myRect.Width = Math.Abs(xEnd - xStart);
                myRect.Height = Math.Abs(yEnd - yStart);
                myRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }

        //圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形圓形
        void drawEllipse(int xStart, int yStart, int xEnd, int yEnd)
        {
            if (bfirst)
            {
                Status = "rest";
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

                myEllipse.Width = Math.Abs(xEnd - xStart);
                myEllipse.Height = Math.Abs(yEnd - yStart);
                myEllipse.Margin = new Thickness(xStart, yStart, 0, 0);

                // Add the Ellipse to the StackPanel.
                if (bmousedown)
                {
                    myChange.Children.Add(myEllipse);
                }
                else
                {
                    mygrid.Children.Add(myEllipse);
                }
            }
            else
            {
                myEllipse.Width = Math.Abs(xEnd - xStart);
                myEllipse.Height = Math.Abs(yEnd - yStart);

                myEllipse.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }

        private void myControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //bmousedown = false;
            //Debug.WriteLine("false");
            pEnd = e.GetPosition(myControl);
            double tempX, tempY;
            double px = pStart.X;
            double py = pStart.Y;
            double ex = pEnd.X;
            double ey = pEnd.Y;

            //tempGPath = new gPath();
            if (drawtype != 3 && ex < px)
            {
                tempX = ex;
                ex = px;
                px = tempX;
            }
            if (drawtype != 3 && ey < py)
            {
                tempY = ey;
                ey = py;
                py = tempY;
            }
            if (px % lineSpace != 0)
                px = lineSpace * Math.Round((px / lineSpace), 0);
            if (py % lineSpace != 0)
                py = lineSpace * Math.Round((py / lineSpace), 0);
            if (ex % lineSpace != 0)
                ex = lineSpace * Math.Round((ex / lineSpace), 0);
            if (ey % lineSpace != 0)
                ey = lineSpace * Math.Round((ey / lineSpace), 0);

            remGPath(px, py, ex, ey);

            if (drawtype <= 4 && Status.Equals("rest"))
            {
                gdc.writeIn(tempGPath, 0);
            }
            if (bhave && ru.Sel >= 0)
            {
                if (new Point(ex, ey) != new Point(px, py))
                {
                    gdc.writeIn(tempGPath, 1);
                }
            }
            gdc.bmove = false;
            if (Status.Equals("rest"))
                reDraw(true);

            bfirst = true;
            bhave = false;
        }
        
        private void myControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!bhave) //if you can control an object
                {
                    pEnd = e.GetPosition(myControl);
                    double tempX, tempY;
                    double px = pStart.X;
                    double py = pStart.Y;
                    double ex = pEnd.X;
                    double ey = pEnd.Y;
                    if (drawtype < 3 && ex < px)
                    {
                        tempX = ex;
                        ex = px;
                        px = tempX;
                    }
                    if (drawtype < 3 && ey < py)
                    {
                        tempY = ey;
                        ey = py;
                        py = tempY;
                    }
                    if (px % lineSpace != 0)
                        px = lineSpace * Math.Round((px / lineSpace), 0);
                    if (py % lineSpace != 0)
                        py = lineSpace * Math.Round((py / lineSpace), 0);
                    if (ex % lineSpace != 0)
                        ex = lineSpace * Math.Round((ex / lineSpace), 0);
                    if (ey % lineSpace != 0)
                        ey = lineSpace * Math.Round((ey / lineSpace), 0);
                    switch (drawtype)
                    {
                        case 1:
                            drawEllipse((int)px, (int)py, (int)ex, (int)ey);
                            myEllipse.Opacity = 0.5;
                            break;
                        case 2:
                            drawRect((int)px, (int)py, (int)ex, (int)ey, 0);
                            myRect.Opacity = 0.5;
                            break;
                        case 3:
                            drawLine((int)px, (int)py, (int)ex, (int)ey);
                            myLine.Opacity = 0.5;
                            break;
                        case 4:
                            drawCurve((int)px, (int)py, (int)ex, (int)ey);
                            myPath.Opacity = 0.5;
                            break;
                    }
                }
                else
                {
                    pEnd = e.GetPosition(myControl);
                    double px = pStart.X;
                    double py = pStart.Y;
                    double ex = pEnd.X;
                    double ey = pEnd.Y;
                    
                    if (px % lineSpace != 0)
                        px = lineSpace * Math.Round((px / lineSpace), 0);
                    if (py % lineSpace != 0)
                        py = lineSpace * Math.Round((py / lineSpace), 0);
                    if (ex % lineSpace != 0)
                        ex = lineSpace * Math.Round((ex / lineSpace), 0);
                    if (ey % lineSpace != 0)
                        ey = lineSpace * Math.Round((ey / lineSpace), 0);
                    
                    gdc.bmove = true;
                    gdc.mx = (int)ex;
                    gdc.my = (int)ey;
                    reDraw(true);
                }
            }
        }

        private void remGPath(double px, double py, double ex, double ey)
        {
            tempGPath.state.colorB = colorB;
            tempGPath.state.colorG = colorG;
            tempGPath.state.colorR = colorR;
            tempGPath.state.strokeT = strokeT;
            tempGPath.ListPlace = ru.Sel;
            tempGPath.drawtype = drawtype;
            
            if (drawtype <= 3)
            {
                tempGPath.controlBtn1 = new Point(px, py);
                tempGPath.controlBtn4 = new Point(ex, ey);

                if (drawtype < 3)
                {
                    tempGPath.controlBtn2 = new Point(ex, py);
                    tempGPath.controlBtn3 = new Point(px, ey);
                }
            }
            if (drawtype == 4)
            {
                tempGPath.controlBtn1 = p0;
                tempGPath.controlBtn2 = p1;
                tempGPath.controlBtn3 = p2;
                tempGPath.controlBtn4 = p3;
            }
        }

        void reDraw(bool bfull)
        {
            if (bfull)
                mygrid.Children.Clear();
            // ClearDrawing();
            gPath p = new gPath();
            p = null;
            Point tempPoint;
            if (ru.Sel >= 0 && bhave)
            {
                p = (gPath)gdc.sroot.PathList[ru.Sel];
            }
            if (bfull)
            {
                foreach (gPath gpath in gdc.sroot.PathList)
                {
                    if (gpath != null && gpath != p)
                    {
                        drawGPath(gpath);
                    }
                }//end of for loop
            }
            if (p != null)
            {
                if (gdc.bmove)
                {
                    if (p.drawtype < 3)
                    {
                        if (gdc.node == 0)
                        {
                            if (p.controlBtn1.X > p.controlBtn2.X)
                            {
                                gdc.node = 1;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn1;
                                p.controlBtn1 = tempPoint;
                                tempPoint = p.controlBtn3;
                                p.controlBtn3 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else if (p.controlBtn1.Y > p.controlBtn3.Y)
                            {
                                gdc.node = 2;
                                tempPoint = p.controlBtn1;
                                p.controlBtn1 = p.controlBtn3;
                                p.controlBtn3 = tempPoint;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else
                            {
                                p.controlBtn1.X = gdc.mx;
                                p.controlBtn1.Y = gdc.my;
                                p.controlBtn2.Y = gdc.my;
                                p.controlBtn3.X = gdc.mx;
                            }
                        }
                        else if (gdc.node == 1)
                        {
                            if (p.controlBtn2.X < p.controlBtn1.X)
                            {
                                gdc.node = 0;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn1;
                                p.controlBtn1 = tempPoint;
                                tempPoint = p.controlBtn3;
                                p.controlBtn3 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else if (p.controlBtn2.Y > p.controlBtn4.Y)
                            {
                                gdc.node = 3;
                                tempPoint = p.controlBtn1;
                                p.controlBtn1 = p.controlBtn3;
                                p.controlBtn3 = tempPoint;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else
                            {
                                p.controlBtn2.X = gdc.mx;
                                p.controlBtn2.Y = gdc.my;
                                p.controlBtn1.Y = gdc.my;
                                p.controlBtn4.X = gdc.mx;
                            }
                        }
                        else if (gdc.node == 2)
                        {
                            if (p.controlBtn3.X > p.controlBtn4.X)
                            {
                                gdc.node = 3;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn1;
                                p.controlBtn1 = tempPoint;
                                tempPoint = p.controlBtn3;
                                p.controlBtn3 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else if (p.controlBtn3.Y < p.controlBtn1.Y)
                            {
                                gdc.node = 0;
                                tempPoint = p.controlBtn1;
                                p.controlBtn1 = p.controlBtn3;
                                p.controlBtn3 = tempPoint;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else
                            {
                                p.controlBtn3.X = gdc.mx;
                                p.controlBtn3.Y = gdc.my;
                                p.controlBtn1.X = gdc.mx;
                                p.controlBtn4.Y = gdc.my;
                            }
                        }
                        else
                        {
                            if (p.controlBtn4.X < p.controlBtn3.X)
                            {
                                gdc.node = 2;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn1;
                                p.controlBtn1 = tempPoint;
                                tempPoint = p.controlBtn3;
                                p.controlBtn3 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else if (p.controlBtn4.Y < p.controlBtn2.Y)
                            {
                                gdc.node = 1;
                                tempPoint = p.controlBtn1;
                                p.controlBtn1 = p.controlBtn3;
                                p.controlBtn3 = tempPoint;
                                tempPoint = p.controlBtn2;
                                p.controlBtn2 = p.controlBtn4;
                                p.controlBtn4 = tempPoint;
                            }
                            else
                            {
                                p.controlBtn4.X = gdc.mx;
                                p.controlBtn4.Y = gdc.my;
                                p.controlBtn2.X = gdc.mx;
                                p.controlBtn3.Y = gdc.my;
                            }
                        }
                    }
                    else if (p.drawtype == 3)
                    {
                        if (gdc.node == 0)
                        {
                            p.controlBtn1.X = gdc.mx;
                            p.controlBtn1.Y = gdc.my;
                        }
                        if (gdc.node == 3)
                        {
                            p.controlBtn4.X = gdc.mx;
                            p.controlBtn4.Y = gdc.my;
                        }
                    }
                    else if (p.drawtype == 4)
                    {
                        if (gdc.node == 0)
                        {
                            p.controlBtn1.X = gdc.mx;
                            p.controlBtn1.Y = gdc.my;
                        }
                        else if (gdc.node == 1)
                        {
                            p.controlBtn2.X = gdc.mx;
                            p.controlBtn2.Y = gdc.my;
                        }
                        else if (gdc.node == 2)
                        {
                            p.controlBtn3.X = gdc.mx;
                            p.controlBtn3.Y = gdc.my;
                        }
                        else
                        {
                            p.controlBtn4.X = gdc.mx;
                            p.controlBtn4.Y = gdc.my;
                        }
                    }
                }
                drawGPath(p);
                if (bhave)
                {
                    if (p.drawtype < 3)
                    {
                        byte tmpG = colorG;
                        colorG = 255;
                        drawRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                        bfirst = true;
                        drawRect((int)p.controlBtn2.X - 3, (int)p.controlBtn2.Y - 3, (int)p.controlBtn2.X + 3, (int)p.controlBtn2.Y + 3, 255);
                        bfirst = true;
                        drawRect((int)p.controlBtn3.X - 3, (int)p.controlBtn3.Y - 3, (int)p.controlBtn3.X + 3, (int)p.controlBtn3.Y + 3, 255);
                        bfirst = true;
                        drawRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                        bfirst = true;
                        colorG = tmpG;
                    }
                    else if (p.drawtype == 3)
                    {
                        byte tmpG = colorG;
                        colorG = 255;
                        drawRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                        bfirst = true;
                        drawRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                        bfirst = true;
                        colorG = tmpG;
                    }
                    else
                    {
                        byte tmpR = colorR;
                        byte tmpG = colorG;
                        drawLine((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn2.X, (int)p.controlBtn2.Y);
                        myLine.Opacity = 0.5;
                        bfirst = true;
                        drawLine((int)p.controlBtn3.X, (int)p.controlBtn3.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y);
                        myLine.Opacity = 0.5;
                        bfirst = true;
                        colorG = 255;
                        drawRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                        bfirst = true;
                        colorG = tmpG;
                        colorR = 255;
                        drawRect((int)p.controlBtn2.X - 3, (int)p.controlBtn2.Y - 3, (int)p.controlBtn2.X + 3, (int)p.controlBtn2.Y + 3, 255);
                        bfirst = true;
                        drawRect((int)p.controlBtn3.X - 3, (int)p.controlBtn3.Y - 3, (int)p.controlBtn3.X + 3, (int)p.controlBtn3.Y + 3, 255);
                        bfirst = true;
                        colorR = tmpR;
                        colorG = 255;
                        drawRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                        bfirst = true;
                        colorG = tmpG;
                    }
                }
            }
        }

        void drawGPath(gPath gpath)
        {
            colorR = gpath.state.colorR;
            colorG = gpath.state.colorG;
            colorB = gpath.state.colorB;
            strokeT = gpath.state.strokeT;
            bfirst = true;

            switch (gpath.drawtype)
            {
                case 1:
                    drawEllipse((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
                    if(bmousedown){
                        myEllipse.Opacity = 0.5;
                    }
                    else{
                        myEllipse.Opacity = 1;
                    }
                    break;
                case 2:
                    drawRect((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y, 0);
                    if (bmousedown)
                    {
                        myRect.Opacity = 0.5;
                    }
                    else
                    {
                        myRect.Opacity = 1;
                    }
                    break;
                case 3:
                    drawLine((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
                    if (bmousedown)
                    {
                        myLine.Opacity = 0.5;
                    }
                    else
                    {
                        myLine.Opacity = 1;
                    }
                    break;
                case 4:
                    drawCurve(gpath.controlBtn1, gpath.controlBtn2, gpath.controlBtn3, gpath.controlBtn4);
                    if (bmousedown)
                    {
                        myPath.Opacity = 0.5;
                    }
                    else
                    {
                        myPath.Opacity = 1;
                    }
                    break;
            }
            bfirst = true;
        }

        public void initpath(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SVGRoot));
            using (MemoryStream ms = new MemoryStream( System.Text.Encoding.UTF8.GetBytes(xml)))
            {

                gdc.sroot = (SVGRoot)serializer.Deserialize(XmlReader.Create(ms));

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

            using (MemoryStream stream = new MemoryStream())
            {

                XmlSerializer s = new XmlSerializer(typeof(SVGRoot));


             s.Serialize(XmlWriter.Create(stream), gdc.sroot);

                
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            StreamReader sr = new StreamReader(stream);
            string  myStr = sr.ReadToEnd();


            
            _utility.xml = myStr;
            }
            

            //  this.Dispose(true);
            //  this.Close();
           // _utility.TagName = "test";// cbxTagName.SelectedItem.ToString();
            Globals.ThisAddIn.AddPictureContentControl(_utility);
            ClearDrawing();
        }
    }
}