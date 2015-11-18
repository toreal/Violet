using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            myControl.Visibility = Visibility.Hidden;
        }

        public int drawtype = 1;
        public String colortype = "black";
        public int lineSpace = 9;
        public GraphDoc gdc = new GraphDoc();
        public checkHitDraw chd = new checkHitDraw();
        public RUse ru = new RUse();
        gPoint gp;
        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;
        int strokeT = 1;

        
        String Status = "rest";
        Point pStart,pEnd;
        Point tempStart;
        Point p0, p1, p2, p3 = new Point(0, 0);
        BezierSegment bezier = new BezierSegment();
        PathFigure figure = new PathFigure();
        PathGeometry geometry = new PathGeometry();
        Geometry tempGeo;
        gPath tempFPath;
        Ellipse myEllipse;
        Rectangle myRect, cornerRect, sideRect;
        Line myLine, controlLine;
        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
        System.Windows.Shapes.Path controlPath = new System.Windows.Shapes.Path();

        bool bfirst = true;
        bool bCanMove = false; //you can do mouseEvent
        bool bhave = false; //you have choose
        bool bConThing = false;

        /*--------------  滑鼠事件  --------------*/
        private void mygrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pStart = correctPoint(e.GetPosition(mygrid));
            tempFPath = new gPath();
            tempStart = pStart;
            bCanMove = true;
            if (drawtype == 5)
            {
                if (gdc.selIndex < 0)
                {
                    gdc.selIndex = gdc.sroot.PathList.Count - 1;
                }
                /*else
                {
                    ru = gdc.checkOut(pStart);
                    if (ru.Sel >= 0)
                    {
                        gdc.node = ru.Node;
                        
                    }
                    else
                    {
                        gdc.clearMaskNum();
                    }
                }*/
            }
            else
            {
                gdc.selIndex = -1;
                gdc.clearMaskNum();
            }
        }
        private void myControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pStart = correctPoint(e.GetPosition(myControl));
            tempFPath = new gPath();
            bCanMove = true;
            tempStart = pStart;
            int tempDraw = gdc.sroot.PathList[ru.Sel].drawtype;
            if (tempDraw < 3)
            {
                if (pStart.X < gdc.sroot.PathList[ru.Sel].controlBtn1.X || pStart.X > gdc.sroot.PathList[ru.Sel].controlBtn2.X || pStart.Y < gdc.sroot.PathList[ru.Sel].controlBtn1.Y || pStart.Y > gdc.sroot.PathList[ru.Sel].controlBtn3.Y)
                {
                    myControl.Visibility = Visibility.Hidden;
                    ru.Node = -1;
                    gdc.bmove = false;
                    bfirst = true;
                    bhave = false;
                    bConThing = false;
                }
            }
            if (tempDraw == 3)
            {
                if (!chd.checkHitLine(pStart, gdc.sroot.PathList[ru.Sel]))
                {
                    myControl.Visibility = Visibility.Hidden;
                    ru.Node = -1;
                    gdc.bmove = false;
                    bfirst = true;
                    bhave = false;
                    bConThing = false;
                }
            }
            if (tempDraw == 4)
            {
                if (!chd.checkHitCurve(tempGeo, gdc.sroot.PathList[ru.Sel]))
                {
                    myControl.Visibility = Visibility.Hidden;
                    ru.Node = -1;
                    gdc.bmove = false;
                    bfirst = true;
                    bhave = false;
                    bConThing = false;
                }
            }
            
            
            if (ru.Sel >= 0)
            {
                gdc.node = ru.Node;
            }
        }
        private void mygrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (bCanMove)
            {
                //bmousedown = false;
                //Debug.WriteLine("false");
                pEnd = correctPoint(e.GetPosition(mygrid));
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

                remGPath(px, py, ex, ey);

                if (drawtype <= 4 && Status.Equals("rest"))
                {
                    gdc.writeIn(tempFPath, 0);
                    gdc.Release();
                }
                gdc.bmove = false;
                if (Status.Equals("rest"))
                    reDraw(true);

                bfirst = true;
                bhave = false;
            }
        }
        private void mygrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!bhave) //if you can control an object
                {
                    pEnd = correctPoint(e.GetPosition(mygrid));
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
            }
        }
        private void myControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (ru.Node > -1)
                {
                    pEnd = correctPoint(e.GetPosition(myControl));
                    double px = pStart.X;
                    double py = pStart.Y;
                    double ex = pEnd.X;
                    double ey = pEnd.Y;

                    gdc.bmove = true;
                    gdc.mx = (int)ex;
                    gdc.my = (int)ey;
                    reDraw(true);
                }
            }
        }

        /*------------  矯正滑鼠位置  ------------*/
        private Point correctPoint(Point p)
        {
            Point temp = p;
            if (temp.X % lineSpace != 0)
                temp.X = lineSpace * Math.Round((temp.X / lineSpace), 0);
            if (temp.Y % lineSpace != 0)
                temp.Y = lineSpace * Math.Round((temp.Y / lineSpace), 0);

            return temp;
        }

        /*-------  myControl控制面板呼叫  --------*/
        public void hiddenCanvas()
        {
            myControl.Visibility = Visibility.Hidden;
        }
        void showCanvas()
        {
            myControl.Visibility = Visibility.Visible;
        }

        /*--------------  圖形繪製  --------------*/
        void drawCurve(int xStart, int yStart, int xEnd, int yEnd)
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
                    tempGeo = geometry;
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
                mygrid.Children.Add(myLine);
            }
            else
            {
                myLine.X2 = xEnd;
                myLine.Y2 = yEnd;
            }
        }
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

                mygrid.Children.Add(myRect);
            }
            else
            {
                myRect.Width = Math.Abs(xEnd - xStart);
                myRect.Height = Math.Abs(yEnd - yStart);
                myRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
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

                mygrid.Children.Add(myEllipse);
            }
            else
            {
                myEllipse.Width = Math.Abs(xEnd - xStart);
                myEllipse.Height = Math.Abs(yEnd - yStart);

                myEllipse.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }

        /*--------------  重繪使用  --------------*/
        private void drawGPath(gPath gpath)
        {
            colorR = gpath.state.colorR;
            colorG = gpath.state.colorG;
            colorB = gpath.state.colorB;
            strokeT = gpath.state.strokeT;
            bfirst = true;

            switch (gpath.drawtype)
            {
                case 1:
                    reEllipse((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
                    myEllipse.Opacity = 1;
                    break;
                case 2:
                    reRect((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y, 0);
                    myRect.Opacity = 1;
                    break;
                case 3:
                    reLine((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
                    myLine.Opacity = 1;
                    break;
                case 4:
                    reCurve(gpath.controlBtn1, gpath.controlBtn2, gpath.controlBtn3, gpath.controlBtn4);
                    myPath.Opacity = 1;
                    break;
            }
            bfirst = true;
        }
        private void reEllipse(int xStart, int yStart, int xEnd, int yEnd)
        {
            if (bfirst)
            {
                bfirst = false;
                myEllipse = new Ellipse();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(0, 0, 0, 255);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = strokeT;
                myEllipse.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myEllipse.Width = Math.Abs(xEnd - xStart);
                myEllipse.Height = Math.Abs(yEnd - yStart);
                myEllipse.Margin = new Thickness(xStart, yStart, 0, 0);
                myEllipse.MouseLeftButtonDown += myEllipse_MouseLeftButtonDown;

                mygrid.Children.Add(myEllipse);
            }
        }
        private void reRect(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        {
            if (bfirst)
            {
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
                myRect.MouseLeftButtonDown += myRect_MouseLeftButtonDown;

                mygrid.Children.Add(myRect);
            }
        }
        private void reLine(int xStart, int yStart, int xEnd, int yEnd)
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
                myLine.MouseLeftButtonDown += myLine_MouseLeftButtonDown;

                mygrid.Children.Add(myLine);
            }
        }
        private void reCurve(Point point0, Point point1, Point point2, Point point3)
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
                myPath.MouseLeftButtonDown += myPath_MouseLeftButtonDown;

                mygrid.Children.Add(myPath);
            }
        }

        /*--------------  圖形事件  --------------*/
        void myEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawtype == 5)
            {
                bConThing = true;
                gp = new gPoint();
                Ellipse tempEll = sender as Ellipse;
                double locLeft = tempEll.Margin.Left;
                double locTop = tempEll.Margin.Top;

                gp.point0 = new Point(tempEll.RenderedGeometry.Bounds.TopLeft.X - (tempEll.StrokeThickness / 2) + locLeft, tempEll.RenderedGeometry.Bounds.TopLeft.Y - (tempEll.StrokeThickness / 2) + locTop);
                gp.point1 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.TopRight.X + (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.TopRight.Y - (tempEll.StrokeThickness / 2));
                gp.point2 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.BottomLeft.X - (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.BottomLeft.Y + (tempEll.StrokeThickness / 2));
                gp.point3 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.BottomRight.X + (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.BottomRight.Y + (tempEll.StrokeThickness / 2));

                gp.mouseXY = new Point(e.GetPosition(mygrid).X, e.GetPosition(mygrid).Y);
                //Debug.WriteLine(gp.mouseXY.X + ", " + gp.mouseXY.Y + "......" + chd.checkHitEllipse(gp));
                if (chd.checkHitEllipse(gp))
                {
                    showCanvas();
                    ru.Sel = chd.checkHitWhich(gdc.sroot.PathList ,gp, 1);
                    greenDrawing();
                    bhave = true;
                }
            }
        }
        void myRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawtype == 5)
            {
                bConThing = true;
                gp = new gPoint();
                Rectangle tempRect = sender as Rectangle;
                double locLeft = tempRect.Margin.Left;
                double locTop = tempRect.Margin.Top;

                gp.point0 = new Point(tempRect.RenderedGeometry.Bounds.TopLeft.X - (tempRect.StrokeThickness / 2) + locLeft, tempRect.RenderedGeometry.Bounds.TopLeft.Y - (tempRect.StrokeThickness / 2) + locTop);
                gp.point1 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.TopRight.X + (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.TopRight.Y - (tempRect.StrokeThickness / 2));
                gp.point2 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.BottomLeft.X - (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.BottomLeft.Y + (tempRect.StrokeThickness / 2));
                gp.point3 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.BottomRight.X + (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.BottomRight.Y + (tempRect.StrokeThickness / 2));

                gp.mouseXY = new Point(e.GetPosition(mygrid).X, e.GetPosition(mygrid).Y);
                //Debug.WriteLine(gp.mouseXY.X + ", " + gp.mouseXY.Y + "......" + chd.checkHitEllipse(gp));
                if (chd.checkHitRect(gp))
                {
                    showCanvas();
                    ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 2);
                    greenDrawing();
                    bhave = true;
                }
            }
        }
        void myLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bConThing = true;
            gp = new gPoint();
            Line tempLine = sender as Line;

            gp.point0 = new Point(tempLine.X1, tempLine.Y1);
            gp.point3 = new Point(tempLine.X2, tempLine.Y2);

            gp.mouseXY = new Point(e.GetPosition(mygrid).X,e.GetPosition(mygrid).Y);

            showCanvas();
            ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 3);
            greenDrawing();
            bhave = true;
        }
        void myPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bConThing = true;

            gp = new gPoint();
            System.Windows.Shapes.Path tempPath = sender as System.Windows.Shapes.Path;
            gp.geo = tempPath.Data;
            gp.mouseXY = new Point(e.GetPosition(mygrid).X, e.GetPosition(mygrid).Y);
            //Debug.WriteLine(gp.geo);
            showCanvas();
            ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 4);
            greenDrawing();
            bhave = true;
        }
        void cornerRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ru.Node = chd.checkHitCorner(e.GetPosition(myControl), gdc.sroot.PathList[ru.Sel]);
            gdc.node = ru.Node;
        }
        void cornerRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (bCanMove)
            {
                //bmousedown = false;
                //Debug.WriteLine("false");
                pEnd = correctPoint(e.GetPosition(myControl));
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

                remGPath(px, py, ex, ey);

                if (bhave && ru.Sel >= 0)
                {
                    if (new Point(ex, ey) != new Point(px, py))
                    {
                        tempFPath.copyVal(gdc.sroot.PathList[ru.Sel]);
                        gdc.writeIn(tempFPath, 1);
                        gdc.Release();
                    }
                }
                if (Status.Equals("rest"))
                    reDraw(true);
            }
        }
        void sideRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chd.checkHitCenter(correctPoint(e.GetPosition(myControl)), gdc.sroot.PathList[ru.Sel]))
                ru.Node = 4;
        }
        void sideRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (bCanMove)
            {
                //bmousedown = false;
                //Debug.WriteLine("false");
                pEnd = correctPoint(e.GetPosition(myControl));
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

                remGPath(px, py, ex, ey);

                if (bhave && ru.Sel >= 0)
                {
                    if (new Point(ex, ey) != new Point(px, py))
                    {
                        tempFPath.copyVal(gdc.sroot.PathList[ru.Sel]);
                        gdc.writeIn(tempFPath, 1);
                        gdc.Release();
                    }
                }
                if (Status.Equals("rest"))
                    reDraw(true);
            }
        }
        void controlLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chd.checkHitLine(correctPoint(e.GetPosition(myControl)), gdc.sroot.PathList[ru.Sel]))
                ru.Node = 4;
        }
        void controlPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ru.Node = 4;
        }

        /*------------  選取邊線使用  ------------*/
        void greenDrawing()
        {
            gPath p = (gPath)gdc.sroot.PathList[ru.Sel];
            byte tmpR = colorR;
            byte tmpG = colorG;
            byte tmpB = colorB;
            int tmpS = strokeT;
            colorR = 0;
            colorG = 0;
            colorB = 0;
            strokeT = 1;
            if (p.drawtype < 3)
            {
                colorG = 255;
                greenFourSideRect((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y, 0);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn2.X - 3, (int)p.controlBtn2.Y - 3, (int)p.controlBtn2.X + 3, (int)p.controlBtn2.Y + 3, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn3.X - 3, (int)p.controlBtn3.Y - 3, (int)p.controlBtn3.X + 3, (int)p.controlBtn3.Y + 3, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                bfirst = true;
                colorG = tmpG;
            }
            else if (p.drawtype == 3)
            {
                colorG = 255;
                greenLine((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                bfirst = true;
                colorG = tmpG;
            }
            else
            {
                greenLine((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn2.X, (int)p.controlBtn2.Y);
                myLine.Opacity = 0.5;
                bfirst = true;
                greenLine((int)p.controlBtn3.X, (int)p.controlBtn3.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y);
                myLine.Opacity = 0.5;
                bfirst = true;
                colorG = 255;
                greenFourCornerRect((int)p.controlBtn1.X - 3, (int)p.controlBtn1.Y - 3, (int)p.controlBtn1.X + 3, (int)p.controlBtn1.Y + 3, 255);
                bfirst = true;
                colorG = tmpG;
                colorR = 255;
                greenFourCornerRect((int)p.controlBtn2.X - 3, (int)p.controlBtn2.Y - 3, (int)p.controlBtn2.X + 3, (int)p.controlBtn2.Y + 3, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn3.X - 3, (int)p.controlBtn3.Y - 3, (int)p.controlBtn3.X + 3, (int)p.controlBtn3.Y + 3, 255);
                bfirst = true;
                colorR = tmpR;
                colorG = 255;
                greenFourCornerRect((int)p.controlBtn4.X - 3, (int)p.controlBtn4.Y - 3, (int)p.controlBtn4.X + 3, (int)p.controlBtn4.Y + 3, 255);
                bfirst = true;
                greenCurve(p.controlBtn1, p.controlBtn2, p.controlBtn3, p.controlBtn4);
                colorG = tmpG;
            }
            strokeT = tmpS;
        }
        private void greenFourSideRect(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        {
            if (bfirst)
            {
                Status = "rest";
                bfirst = false;
                sideRect = new Rectangle();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(bfill, colorR, colorG, colorB);
                sideRect.Fill = mySolidColorBrush;
                sideRect.StrokeThickness = strokeT;
                sideRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                sideRect.Width = Math.Abs(xEnd - xStart);
                sideRect.Height = Math.Abs(yEnd - yStart);
                sideRect.Margin = new Thickness(xStart, yStart, 0, 0);
                sideRect.MouseLeftButtonDown += sideRect_MouseLeftButtonDown;
                sideRect.MouseLeftButtonUp += sideRect_MouseLeftButtonUp;

                myControl.Children.Add(sideRect);
            }
            else
            {
                sideRect.Width = Math.Abs(xEnd - xStart);
                sideRect.Height = Math.Abs(yEnd - yStart);
                sideRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
        private void greenFourCornerRect(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        {
            if (bfirst)
            {
                Status = "rest";
                bfirst = false;
                cornerRect = new Rectangle();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(bfill, colorR, colorG, colorB);
                cornerRect.Fill = mySolidColorBrush;
                cornerRect.StrokeThickness = strokeT;
                cornerRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                cornerRect.Width = Math.Abs(xEnd - xStart);
                cornerRect.Height = Math.Abs(yEnd - yStart);
                cornerRect.Margin = new Thickness(xStart, yStart, 0, 0);
                cornerRect.MouseLeftButtonDown += cornerRect_MouseLeftButtonDown;
                cornerRect.MouseLeftButtonUp += cornerRect_MouseLeftButtonUp;

                myControl.Children.Add(cornerRect);
            }
            else
            {
                cornerRect.Width = Math.Abs(xEnd - xStart);
                cornerRect.Height = Math.Abs(yEnd - yStart);
                cornerRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
        private void greenLine(int xStart, int yStart, int xEnd, int yEnd)
        {
            if (bfirst)
            {
                Status = "rest";
                bfirst = false;
                controlLine = new Line();
                controlLine.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                controlLine.X1 = xStart;
                controlLine.X2 = xEnd;
                controlLine.Y1 = yStart;
                controlLine.Y2 = yEnd;
                controlLine.HorizontalAlignment = HorizontalAlignment.Left;
                controlLine.VerticalAlignment = VerticalAlignment.Center;
                controlLine.StrokeThickness = strokeT;
                controlLine.MouseLeftButtonDown += controlLine_MouseLeftButtonDown;

                myControl.Children.Add(controlLine);
            }
            else
            {
                controlLine.X2 = xEnd;
                controlLine.Y2 = yEnd;
            }
        }
        private void greenCurve(Point point0, Point point1, Point point2, Point point3)
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
                controlPath = new System.Windows.Shapes.Path();
                controlPath.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                controlPath.StrokeThickness = strokeT;
                controlPath.Data = geometry;
                controlPath.MouseLeftButtonDown += controlPath_MouseLeftButtonDown;

                myControl.Children.Add(controlPath);
            }
        }

        /*--------------  其他功能  --------------*/
        private void remGPath(double px, double py, double ex, double ey)
        {
            tempFPath.state.colorB = colorB;
            tempFPath.state.colorG = colorG;
            tempFPath.state.colorR = colorR;
            tempFPath.state.strokeT = strokeT;
            tempFPath.drawtype = drawtype;

            if (ru.Sel >= 0)
                tempFPath.ListPlace = ru.Sel;
            else
                tempFPath.ListPlace = gdc.sroot.PathList.Count;

            if (drawtype <= 3)
            {
                tempFPath.controlBtn1 = new Point(px, py);
                tempFPath.controlBtn4 = new Point(ex, ey);

                if (drawtype < 3)
                {
                    tempFPath.controlBtn2 = new Point(ex, py);
                    tempFPath.controlBtn3 = new Point(px, ey);
                }
            }
            if (drawtype == 4)
            {
                tempFPath.controlBtn1 = p0;
                tempFPath.controlBtn2 = p1;
                tempFPath.controlBtn3 = p2;
                tempFPath.controlBtn4 = p3;
                tempFPath.geo = tempGeo;
            }
        }
        private void reDraw(bool bfull)
        {
            if (bfull)
            {
                mygrid.Children.Clear();
                myControl.Children.Clear();
            }
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
                    if (gdc.node >= 0)
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
                            else if (gdc.node == 3)
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
                            else
                            {
                                tempPoint = tempStart;
                                p.controlBtn1.X += (gdc.mx - tempPoint.X);
                                p.controlBtn1.Y += (gdc.my - tempPoint.Y);
                                p.controlBtn2.X += (gdc.mx - tempPoint.X);
                                p.controlBtn2.Y += (gdc.my - tempPoint.Y);
                                p.controlBtn3.X += (gdc.mx - tempPoint.X);
                                p.controlBtn3.Y += (gdc.my - tempPoint.Y);
                                p.controlBtn4.X += (gdc.mx - tempPoint.X);
                                p.controlBtn4.Y += (gdc.my - tempPoint.Y);
                                tempStart = new Point(gdc.mx, gdc.my);
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
                            if (gdc.node == 4)
                            {
                                tempPoint = tempStart;
                                p.controlBtn1.X += (gdc.mx - tempPoint.X);
                                p.controlBtn1.Y += (gdc.my - tempPoint.Y);
                                p.controlBtn4.X += (gdc.mx - tempPoint.X);
                                p.controlBtn4.Y += (gdc.my - tempPoint.Y);
                                tempStart = new Point(gdc.mx, gdc.my);
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
                }
                drawGPath(p);
                greenDrawing();
            }
        }
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
                case 8:        //可考慮拿掉,有點太粗了
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
                case "violet":
                    colorR = 138;
                    colorG = 43;
                    colorB = 226;
                    break;
                case "gray":
                    colorR = 128;
                    colorG = 128;
                    colorB = 128;
                    break;
            }
        }
        public void RUdo(int Act)  //redo undo used
        {
            if (!bConThing)
            {
                if (Act == 0)
                {
                    gdc.reDo();
                    reDraw(true);
                }
                if (Act == 1)
                {
                    gdc.unDo();
                    reDraw(true);
                }
            }
        }

        /*--------------  圖檔使用  --------------*/
        public void initpath(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SVGRoot));
            using (MemoryStream ms = new MemoryStream( System.Text.Encoding.UTF8.GetBytes(xml)))
            {
                gdc.sroot = (SVGRoot)serializer.Deserialize(XmlReader.Create(ms));
                reDraw(true);
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            myControl.Children.Clear();
            int margin = (int)mygrid.Margin.Left;
            int width = (int)mygrid.ActualWidth + (int)mygrid.Margin.Left + (int)mygrid.Margin.Right;
            int height = (int)mygrid.ActualHeight + (int)mygrid.Margin.Top + (int)mygrid.Margin.Bottom;
            mygrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255));

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
                string myStr = sr.ReadToEnd();

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