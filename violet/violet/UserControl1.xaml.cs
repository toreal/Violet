
using ShapeLib.VShape;
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


namespace violet
{

    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        public UserControl1()
        {
            InitializeComponent();
            hiddenCanvas(); //一開始要將myControl畫布取消顯示

       //     shapeLib.initControl(mygrid, myControl);

        }
        //初始設定
        public int drawtype = 1;
        public String colortype = "black";
        public int lineSpace = 9;
        //public GraphDoc gdc = new GraphDoc();
        public checkHitDraw chd = new checkHitDraw();
        public RUse ru = new RUse();
        gPoint gp;
        byte colorR = 0;
        byte colorG = 0;
        byte colorB = 0;
        int strokeT = 3;

        
        String Status = "rest"; //繪製曲線時的狀態
        Point pStart, pEnd; //滑鼠起點和終點
        Point tempStart;
        Point p0, p1, p2, p3 = new Point(0, 0); //紀錄四個控制點使用
        BezierSegment bezier = new BezierSegment();
        PathFigure figure = new PathFigure();
        PathGeometry geometry = new PathGeometry();



        Geometry tempGeo;
        gPath tempFPath; 
        Ellipse myEllipse; //紀錄橢圓形
        Rectangle myRect, cornerRect, sideRect;
        Line myLine, controlLine; //紀錄直線、控制後的直線
        Polygon myTri;
        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path(); //紀錄曲線
        System.Windows.Shapes.Path controlPath = new System.Windows.Shapes.Path(); //紀錄控制後的曲線


        bool bfirst = true; //是否為繪製新圖形
        bool bCanMove = false; //繪製時,滑鼠是否可以移動
        bool bhave = false; //you have choose
        bool gCanMove = false; //選取後是否可以移動
        bool bConThing = false; //是否有選取物件
        bool OnIt = false; //是否有滑入或滑出選取物件

        

        //矯正滑鼠位置
        private Point correctPoint(Point p)
        {
            Point temp = p;
            double tempDX = temp.X % lineSpace;
            double tempDY = temp.Y % lineSpace;
            if (temp.X % lineSpace != 0)
            {
                temp.X = lineSpace * Math.Round((temp.X / lineSpace), 0);
            }
            if (temp.Y % lineSpace != 0)
            {
                temp.Y = lineSpace * Math.Round((temp.Y / lineSpace), 0);
            }
            return temp;
        }

        //隱藏myControl
        public void hiddenCanvas()
        {
            myControl.Visibility = Visibility.Hidden;
        }
        //顯示myControl
        void showCanvas()
        {
            myControl.Visibility = Visibility.Visible;
        }

        //繪製曲線
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
        //繪製直線
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
        //繪製矩形
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
        //繪製三角形
        void drawTri(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        {
            if (bfirst)
            {
                Status = "rest";
                bfirst = false;
                myTri = new Polygon();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(bfill, colorR, colorG, colorB);
                myTri.Fill = mySolidColorBrush;
                myTri.StrokeThickness = strokeT;
                myTri.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                myTri.Width = Math.Abs(xEnd - xStart);
                myTri.Height = Math.Abs(yEnd - yStart);
                myTri.Margin = new Thickness(xStart, yStart, 0, 0);

                mygrid.Children.Add(myTri);
            }
            else
            {
                myTri.Width = Math.Abs(xEnd - xStart);
                myTri.Height = Math.Abs(yEnd - yStart);
               myTri.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
        //繪製橢圓
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
       
        //刷新畫面時 用來重繪圖形
        //private void drawGPath(gPath gpath)
        //{
        //    colorR = gpath.state.colorR;
        //    colorG = gpath.state.colorG;
        //    colorB = gpath.state.colorB;
        //    strokeT = gpath.state.strokeT;
        //    bfirst = true;

        //    switch (gpath.drawtype)
        //    {
        //        case 1:
        //            reEllipse((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
        //            myEllipse.Opacity = 1;
        //            break;
        //        case 2:
        //            reRect((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y, 0);
        //            myRect.Opacity = 1;
        //            break;
        //        case 3:
        //            reLine((int)gpath.controlBtn1.X, (int)gpath.controlBtn1.Y, (int)gpath.controlBtn4.X, (int)gpath.controlBtn4.Y);
        //            myLine.Opacity = 1;
        //            break;
        //        case 4:
        //            reCurve(gpath.controlBtn1, gpath.controlBtn2, gpath.controlBtn3, gpath.controlBtn4);
        //            myPath.Opacity = 1;
        //            break;
        //    }
        //    bfirst = true;
        //}
        //重繪橢圓
        //private void reEllipse(int xStart, int yStart, int xEnd, int yEnd)
        //{
        //    if (bfirst)
        //    {
        //        bfirst = false;
        //        myEllipse = new Ellipse();
        //        //如果要繪製中心顏色，可開啟這段
        //        /*SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        //        mySolidColorBrush.Color = Color.FromArgb(0, 0, 0, 255);
        //        myEllipse.Fill = mySolidColorBrush;*/
        //        myEllipse.StrokeThickness = strokeT;
        //        myEllipse.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
        //        myEllipse.Width = Math.Abs(xEnd - xStart);
        //        myEllipse.Height = Math.Abs(yEnd - yStart);
        //        myEllipse.Margin = new Thickness(xStart, yStart, 0, 0);
        //        myEllipse.MouseLeftButtonDown += myEllipse_MouseLeftButtonDown;
        //        myEllipse.MouseEnter += Shapes_MouseEnter_Hands;
        //        myEllipse.MouseLeave += Shapes_MouseLeave;

        //        mygrid.Children.Add(myEllipse);
        //    }
        //}
        ////重繪矩形
        //private void reRect(int xStart, int yStart, int xEnd, int yEnd, byte bfill)
        //{
        //    if (bfirst)
        //    {
        //        bfirst = false;
        //        myRect = new Rectangle();
        //        //如果要繪製中心顏色，可開啟這段
        //        /*SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        //        mySolidColorBrush.Color = Color.FromArgb(bfill, colorR, colorG, colorB);
        //        myRect.Fill = mySolidColorBrush;*/
        //        myRect.StrokeThickness = strokeT;
        //        myRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
        //        myRect.Width = Math.Abs(xEnd - xStart);
        //        myRect.Height = Math.Abs(yEnd - yStart);
        //        myRect.Margin = new Thickness(xStart, yStart, 0, 0);
        //        myRect.MouseLeftButtonDown += myRect_MouseLeftButtonDown;
        //        myRect.MouseEnter += Shapes_MouseEnter_Hands;
        //        myRect.MouseLeave += Shapes_MouseLeave;

        //        mygrid.Children.Add(myRect);
        //    }
        //}
        ////重繪直線
        //private void reLine(int xStart, int yStart, int xEnd, int yEnd)
        //{
        //    if (bfirst)
        //    {
        //        bfirst = false;
        //        myLine = new Line();
        //        myLine.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
        //        myLine.X1 = xStart;
        //        myLine.X2 = xEnd;
        //        myLine.Y1 = yStart;
        //        myLine.Y2 = yEnd;
        //        myLine.HorizontalAlignment = HorizontalAlignment.Left;
        //        myLine.VerticalAlignment = VerticalAlignment.Center;
        //        myLine.StrokeThickness = strokeT;
        //        myLine.MouseLeftButtonDown += myLine_MouseLeftButtonDown;
        //        myLine.MouseEnter += Shapes_MouseEnter_Hands;
        //        myLine.MouseLeave += Shapes_MouseLeave;

        //        mygrid.Children.Add(myLine);
        //    }
        //}
        ////重繪曲線
        //private void reCurve(Point point0, Point point1, Point point2, Point point3)
        //{
        //    if (bfirst)
        //    {
        //        bfirst = false;
        //        bezier = new BezierSegment();
        //        bezier.Point3 = point3;
        //        figure = new PathFigure();
        //        figure.StartPoint = point0;
        //        bezier.Point1 = point1;
        //        bezier.Point2 = point2;
        //        figure.Segments.Add(bezier);
        //        geometry = new PathGeometry();
        //        geometry.Figures.Add(figure);
        //        myPath = new System.Windows.Shapes.Path();
        //        myPath.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
        //        myPath.StrokeThickness = strokeT;
        //        myPath.Data = geometry;
        //        myPath.MouseLeftButtonDown += myPath_MouseLeftButtonDown;
        //        myPath.MouseEnter += Shapes_MouseEnter_Hands;
        //        myPath.MouseLeave += Shapes_MouseLeave;

        //        mygrid.Children.Add(myPath);
        //    }
        //}

        /*--------------  圖形事件  --------------*/
        ////當左鍵點擊按到橢圓時
        //void myEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (drawtype == 5)
        //    {
        //        bConThing = true;
        //        gp = new gPoint();
        //        Ellipse tempEll = sender as Ellipse;
        //        double locLeft = tempEll.Margin.Left;
        //        double locTop = tempEll.Margin.Top;

        //        gp.point0 = new Point(tempEll.RenderedGeometry.Bounds.TopLeft.X - (tempEll.StrokeThickness / 2) + locLeft, tempEll.RenderedGeometry.Bounds.TopLeft.Y - (tempEll.StrokeThickness / 2) + locTop);
        //        gp.point1 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.TopRight.X + (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.TopRight.Y - (tempEll.StrokeThickness / 2));
        //        gp.point2 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.BottomLeft.X - (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.BottomLeft.Y + (tempEll.StrokeThickness / 2));
        //        gp.point3 = new Point(gp.point0.X + tempEll.RenderedGeometry.Bounds.BottomRight.X + (tempEll.StrokeThickness / 2), gp.point0.Y + tempEll.RenderedGeometry.Bounds.BottomRight.Y + (tempEll.StrokeThickness / 2));

        //        gp.mouseXY = correctPoint(e.GetPosition(mygrid));
        //        showCanvas();
        //        ru.Sel = chd.checkHitWhich(gdc.sroot.PathList ,gp, 1);
        //        greenDrawing();
        //        bhave = true;
        //    }
        //}
        ////當左鍵點擊按到矩形時
        //void myRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (drawtype == 5)
        //    {
        //        bConThing = true;
        //        gp = new gPoint();
        //        Rectangle tempRect = sender as Rectangle;
        //        double locLeft = tempRect.Margin.Left;
        //        double locTop = tempRect.Margin.Top;

        //        gp.point0 = new Point(tempRect.RenderedGeometry.Bounds.TopLeft.X - (tempRect.StrokeThickness / 2) + locLeft, tempRect.RenderedGeometry.Bounds.TopLeft.Y - (tempRect.StrokeThickness / 2) + locTop);
        //        gp.point1 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.TopRight.X + (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.TopRight.Y - (tempRect.StrokeThickness / 2));
        //        gp.point2 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.BottomLeft.X - (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.BottomLeft.Y + (tempRect.StrokeThickness / 2));
        //        gp.point3 = new Point(gp.point0.X + tempRect.RenderedGeometry.Bounds.BottomRight.X + (tempRect.StrokeThickness / 2), gp.point0.Y + tempRect.RenderedGeometry.Bounds.BottomRight.Y + (tempRect.StrokeThickness / 2));

        //        gp.mouseXY = correctPoint(e.GetPosition(mygrid));
        //        showCanvas();
        //        ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 2);
        //        greenDrawing();
        //        bhave = true;
        //    }
        //}
        ////當左鍵點擊按到直線時
        //void myLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (drawtype == 5)
        //    {
        //        bConThing = true;
        //        gp = new gPoint();
        //        Line tempLine = sender as Line;

        //        gp.point0 = new Point(tempLine.X1, tempLine.Y1);
        //        gp.point3 = new Point(tempLine.X2, tempLine.Y2);

        //        gp.mouseXY = correctPoint(e.GetPosition(mygrid));

        //        showCanvas();
        //        ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 3);
        //        greenDrawing();
        //        bhave = true;
        //    }
        //}
        ////當左鍵點擊按到曲線時
        //void myPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (drawtype == 5)
        //    {
        //        bConThing = true;
        //        gp = new gPoint();
        //        System.Windows.Shapes.Path tempPath = sender as System.Windows.Shapes.Path;
        //        String[] tmpStr = pathDataToPoint(tempPath.Data.ToString()).Split(',');
        //        double[] tmpDouStr = new double[tmpStr.Length];

        //        for (int i = 0; i < tmpStr.Length; i++)
        //        {
        //            tmpDouStr[i] = Convert.ToDouble(tmpStr[i]);
        //        }

        //        gp.point0 = new Point(tmpDouStr[0], tmpDouStr[1]);
        //        gp.point1 = new Point(tmpDouStr[2], tmpDouStr[3]);
        //        gp.point2 = new Point(tmpDouStr[4], tmpDouStr[5]);
        //        gp.point3 = new Point(tmpDouStr[6], tmpDouStr[7]);

        //        gp.mouseXY = correctPoint(e.GetPosition(mygrid));
        //        showCanvas();
        //        ru.Sel = chd.checkHitWhich(gdc.sroot.PathList, gp, 4);
        //        greenDrawing();
        //        bhave = true;
        //    }
        //}
        ////四個角落的控制點
        //void cornerRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ru.Node = chd.checkHitCorner(correctPoint(e.GetPosition(myControl)), gdc.sroot.PathList[ru.Sel]);
        //    gdc.node = ru.Node;
        //    gCanMove = true;
        //}
        ////控制邊框的範圍
        //void cornerRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (gCanMove)
        //    {
        //        gCanMove = false;
        //        pEnd = correctPoint(e.GetPosition(myControl));
        //        double tempX, tempY;
        //        double px = pStart.X;
        //        double py = pStart.Y;
        //        double ex = pEnd.X;
        //        double ey = pEnd.Y;

        //        if (drawtype != 3 && ex < px)
        //        {
        //            tempX = ex;
        //            ex = px;
        //            px = tempX;
        //        }
        //        if (drawtype != 3 && ey < py)
        //        {
        //            tempY = ey;
        //            ey = py;
        //            py = tempY;
        //        }

        //        remGPath(px, py, ex, ey);

        //        if (bhave && ru.Sel >= 0)
        //        {
        //            if (new Point(ex, ey) != new Point(px, py))
        //            {
        //                tempFPath.copyVal(gdc.sroot.PathList[ru.Sel]);
        //                gdc.writeIn(tempFPath, 1);
        //                gdc.Release();
        //            }
        //        }
        //        if (Status.Equals("rest"))
        //            reDraw(true);
        //    }
        //}
        ////當左鍵按下綠色邊框
        //void sideRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (chd.checkHitCenter(correctPoint(e.GetPosition(myControl)), gdc.sroot.PathList[ru.Sel]))
        //    {
        //        ru.Node = 4;
        //        gCanMove = true;
        //    }
        //}
        ////當左鍵放開綠色邊框
        //void sideRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (gCanMove)
        //    {
        //        gCanMove = false;
        //        pEnd = correctPoint(e.GetPosition(myControl));
        //        double px = pStart.X;
        //        double py = pStart.Y;
        //        double ex = pEnd.X;
        //        double ey = pEnd.Y;
        //        remGPath(px, py, ex, ey);
        //        if (bhave && ru.Sel >= 0)
        //        {
        //            if (new Point(ex, ey) != new Point(px, py))
        //            {
        //                tempFPath.copyVal(gdc.sroot.PathList[ru.Sel]);
        //                gdc.writeIn(tempFPath, 1);
        //                gdc.Release();

        //                if (Status.Equals("rest"))
        //                    reDraw(true);
        //            }
        //        }
        //    }
        //}
        ////當左鍵按下被選取的線條
        //void controlLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (chd.checkHitLine(correctPoint(e.GetPosition(myControl)), gdc.sroot.PathList[ru.Sel]))
        //    {
        //        gCanMove = true;
        //        ru.Node = 4;
        //    }
        //}
        ////當左鍵按下被選取的曲線
        //void controlPath_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    System.Windows.Shapes.Path p = sender as System.Windows.Shapes.Path;
        //    String tempStr = pathDataToPoint(p.Data.ToString());

        //    ru.Node = 4;
        //    gCanMove = true;
        //}
        //換鼠標
        void Shapes_MouseEnter_Hands(object sender, MouseEventArgs e)
        {
            OnIt = true;
            this.Cursor = System.Windows.Input.Cursors.Hand;
        }
        void Shapes_MouseEnter_SizeAll(object sender, MouseEventArgs e)
        {
            OnIt = true;
            this.Cursor = System.Windows.Input.Cursors.SizeAll;
        }
        void Shapes_MouseLeave(object sender, MouseEventArgs e)
        {
            OnIt = false;
            this.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /*--------------  鍵盤事件  --------------*/
        private void UserControl_KeyDown(object sender, KeyEventArgs e) //鍵盤按鍵按下
        {
          //  shapeLib.InsertText(e.Key.ToString());
          
           /* if (e.Key == Key.Delete)
            {
                foreach(gPath gp in shapeLib.Data.multiSelList)
                {
                    gp.isSel = false;
                    gp.IsDelete = true;

                }
               shapeLib.Data.multiSelList.Clear();
                
            }*/
          
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.C)
                {
                    shapeLib.copy();
                }
                else if (e.Key == Key.V)
                {
                    shapeLib.paste();
                                  }
            }
        }

        /*------------  選取邊線使用  ------------*/
        //繪製點選圖形的控制範圍
        void greenDrawing()
        {
            gPath p = (gPath)shapeLib.Data.gdc.sroot.PathList[ru.Sel];
            byte tmpR = colorR;
            byte tmpG = colorG;
            byte tmpB = colorB;
            int tmpS = strokeT;
            colorR = 0;
            colorG = 0;
            colorB = 0;
            strokeT = 2;
            if (p.drawtype == 1)
            {
                colorG = 255;
                greenFourSideRect((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y, 0);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 4, (int)p.controlBtn1.Y - 4, (int)p.controlBtn1.X + 4, (int)p.controlBtn1.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn2.X - 4, (int)p.controlBtn2.Y - 4, (int)p.controlBtn2.X + 4, (int)p.controlBtn2.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn3.X - 4, (int)p.controlBtn3.Y - 4, (int)p.controlBtn3.X + 4, (int)p.controlBtn3.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 4, (int)p.controlBtn4.Y - 4, (int)p.controlBtn4.X + 4, (int)p.controlBtn4.Y + 4, 255);
                bfirst = true;
            }
            else if (p.drawtype  == 2)
            {
                strokeT = p.state.strokeT;
                colorG = 255;
                greenFourSideRect((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y, 0);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 4, (int)p.controlBtn1.Y - 4, (int)p.controlBtn1.X + 4, (int)p.controlBtn1.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn2.X - 4, (int)p.controlBtn2.Y - 4, (int)p.controlBtn2.X + 4, (int)p.controlBtn2.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn3.X - 4, (int)p.controlBtn3.Y - 4, (int)p.controlBtn3.X + 4, (int)p.controlBtn3.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 4, (int)p.controlBtn4.Y - 4, (int)p.controlBtn4.X + 4, (int)p.controlBtn4.Y + 4, 255);
                bfirst = true;
                colorG = tmpG;
            }
            else if (p.drawtype == 3)
            {
                strokeT = p.state.strokeT;
                colorG = 255;
                greenLine((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 4, (int)p.controlBtn1.Y - 4, (int)p.controlBtn1.X + 4, (int)p.controlBtn1.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 4, (int)p.controlBtn4.Y - 4, (int)p.controlBtn4.X + 4, (int)p.controlBtn4.Y + 4, 255);
                bfirst = true;
            }
            else
            {
                drawLine((int)p.controlBtn1.X, (int)p.controlBtn1.Y, (int)p.controlBtn2.X, (int)p.controlBtn2.Y);
                myLine.Opacity = 0.2;
                bfirst = true;
                drawLine((int)p.controlBtn3.X, (int)p.controlBtn3.Y, (int)p.controlBtn4.X, (int)p.controlBtn4.Y);
                myLine.Opacity = 0.2;
                bfirst = true;
                strokeT = p.state.strokeT;
                colorG = 255;
                greenCurve(p.controlBtn1, p.controlBtn2, p.controlBtn3, p.controlBtn4);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn1.X - 4, (int)p.controlBtn1.Y - 4, (int)p.controlBtn1.X + 4, (int)p.controlBtn1.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn4.X - 4, (int)p.controlBtn4.Y - 4, (int)p.controlBtn4.X + 4, (int)p.controlBtn4.Y + 4, 255);
                bfirst = true;
                colorG = 0;
                colorR = 255;
                greenFourCornerRect((int)p.controlBtn2.X - 4, (int)p.controlBtn2.Y - 4, (int)p.controlBtn2.X + 4, (int)p.controlBtn2.Y + 4, 255);
                bfirst = true;
                greenFourCornerRect((int)p.controlBtn3.X - 4, (int)p.controlBtn3.Y - 4, (int)p.controlBtn3.X + 4, (int)p.controlBtn3.Y + 4, 255);
                bfirst = true;
            }
            colorR = tmpR;
            colorG = tmpG;
            colorB = tmpB;
            strokeT = tmpS;
        }
        //繪製選取圖形邊框的控制範圍
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
            //    sideRect.MouseLeftButtonDown += sideRect_MouseLeftButtonDown;
             //   sideRect.MouseLeftButtonUp += sideRect_MouseLeftButtonUp;
                sideRect.MouseEnter += Shapes_MouseEnter_SizeAll;
                sideRect.MouseLeave += Shapes_MouseLeave;

                myControl.Children.Add(sideRect);
            }
            else
            {
                sideRect.Width = Math.Abs(xEnd - xStart);
                sideRect.Height = Math.Abs(yEnd - yStart);
                sideRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
        //繪製選取圖形四個角落的控制點範圍
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
              //  cornerRect.MouseLeftButtonDown += cornerRect_MouseLeftButtonDown;
              //  cornerRect.MouseLeftButtonUp += cornerRect_MouseLeftButtonUp;
                cornerRect.MouseEnter += Shapes_MouseEnter_Hands;
                cornerRect.MouseLeave += Shapes_MouseLeave;

                myControl.Children.Add(cornerRect);
            }
            else
            {
                cornerRect.Width = Math.Abs(xEnd - xStart);
                cornerRect.Height = Math.Abs(yEnd - yStart);
                cornerRect.Margin = new Thickness(xStart, yStart, 0, 0);
            }
        }
        //繪製選取直線的控制範圍
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
              //  controlLine.MouseLeftButtonDown += controlLine_MouseLeftButtonDown;
              //  controlLine.MouseLeftButtonUp += myControl_MouseLeftButtonUp;
                controlLine.MouseEnter += Shapes_MouseEnter_SizeAll;
                controlLine.MouseLeave += Shapes_MouseLeave;

                myControl.Children.Add(controlLine);
            }
            else
            {
                controlLine.X2 = xEnd;
                controlLine.Y2 = yEnd;
            }
        }
        //繪製選取曲線的控制範圍
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
       //         controlPath.MouseLeftButtonDown += controlPath_MouseLeftButtonDown;
                controlPath.MouseEnter += Shapes_MouseEnter_SizeAll;
                controlPath.MouseLeave += Shapes_MouseLeave;

                myControl.Children.Add(controlPath);
            }
        }

        /*--------------  其他功能  --------------*/
        private void remGPath(double px, double py, double ex, double ey) //儲存新繪製的圖形資料
        {
            tempFPath.state.colorB = colorB;
            tempFPath.state.colorG = colorG;
            tempFPath.state.colorR = colorR;
            tempFPath.state.strokeT = strokeT;
            tempFPath.drawtype = drawtype;

            if (ru.Sel >= 0)
                tempFPath.ListPlace = ru.Sel;
            else
                tempFPath.ListPlace = shapeLib.Data.gdc.sroot.PathList.Count;

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
            }
        }
        //private void reDraw(bool bfull) //重新繪製畫布
        //{
        //    if (bfull)
        //    {
        //        mygrid.Children.Clear();
        //        myControl.Children.Clear();
        //    }
        //    gPath p = new gPath();
        //    p = null;
        //    Point tempPoint;
        //    if (ru.Sel >= 0 && bhave)
        //    {
        //        p = (gPath)gdc.sroot.PathList[ru.Sel];
        //    }
        //    if (bfull)
        //    {
        //        foreach (gPath gpath in gdc.sroot.PathList)
        //        {
        //            if (gpath != null && gpath != p)
        //            {
        //                drawGPath(gpath);
        //            }
        //        }
        //    }
        //    if (p != null) //改變控制點位置,建議更改成一個方法使用
        //    {
        //        if (gdc.bmove)
        //        {
        //            if (gdc.node >= 0)
        //            {
        //                if (p.drawtype < 3)
        //                {
        //                    if (gdc.node == 0)
        //                    {
        //                        if (p.controlBtn1.X > p.controlBtn2.X)
        //                        {
        //                            gdc.node = 1;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn1;
        //                            p.controlBtn1 = tempPoint;
        //                            tempPoint = p.controlBtn3;
        //                            p.controlBtn3 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else if (p.controlBtn1.Y > p.controlBtn3.Y)
        //                        {
        //                            gdc.node = 2;
        //                            tempPoint = p.controlBtn1;
        //                            p.controlBtn1 = p.controlBtn3;
        //                            p.controlBtn3 = tempPoint;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else
        //                        {
        //                            p.controlBtn1.X = gdc.mx;
        //                            p.controlBtn1.Y = gdc.my;
        //                            p.controlBtn2.Y = gdc.my;
        //                            p.controlBtn3.X = gdc.mx;
        //                        }
        //                    }
        //                    else if (gdc.node == 1)
        //                    {
        //                        if (p.controlBtn2.X < p.controlBtn1.X)
        //                        {
        //                            gdc.node = 0;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn1;
        //                            p.controlBtn1 = tempPoint;
        //                            tempPoint = p.controlBtn3;
        //                            p.controlBtn3 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else if (p.controlBtn2.Y > p.controlBtn4.Y)
        //                        {
        //                            gdc.node = 3;
        //                            tempPoint = p.controlBtn1;
        //                            p.controlBtn1 = p.controlBtn3;
        //                            p.controlBtn3 = tempPoint;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else
        //                        {
        //                            p.controlBtn2.X = gdc.mx;
        //                            p.controlBtn2.Y = gdc.my;
        //                            p.controlBtn1.Y = gdc.my;
        //                            p.controlBtn4.X = gdc.mx;
        //                        }
        //                    }
        //                    else if (gdc.node == 2)
        //                    {
        //                        if (p.controlBtn3.X > p.controlBtn4.X)
        //                        {
        //                            gdc.node = 3;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn1;
        //                            p.controlBtn1 = tempPoint;
        //                            tempPoint = p.controlBtn3;
        //                            p.controlBtn3 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else if (p.controlBtn3.Y < p.controlBtn1.Y)
        //                        {
        //                            gdc.node = 0;
        //                            tempPoint = p.controlBtn1;
        //                            p.controlBtn1 = p.controlBtn3;
        //                            p.controlBtn3 = tempPoint;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else
        //                        {
        //                            p.controlBtn3.X = gdc.mx;
        //                            p.controlBtn3.Y = gdc.my;
        //                            p.controlBtn1.X = gdc.mx;
        //                            p.controlBtn4.Y = gdc.my;
        //                        }
        //                    }
        //                    else if (gdc.node == 3)
        //                    {
        //                        if (p.controlBtn4.X < p.controlBtn3.X)
        //                        {
        //                            gdc.node = 2;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn1;
        //                            p.controlBtn1 = tempPoint;
        //                            tempPoint = p.controlBtn3;
        //                            p.controlBtn3 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else if (p.controlBtn4.Y < p.controlBtn2.Y)
        //                        {
        //                            gdc.node = 1;
        //                            tempPoint = p.controlBtn1;
        //                            p.controlBtn1 = p.controlBtn3;
        //                            p.controlBtn3 = tempPoint;
        //                            tempPoint = p.controlBtn2;
        //                            p.controlBtn2 = p.controlBtn4;
        //                            p.controlBtn4 = tempPoint;
        //                        }
        //                        else
        //                        {
        //                            p.controlBtn4.X = gdc.mx;
        //                            p.controlBtn4.Y = gdc.my;
        //                            p.controlBtn2.X = gdc.mx;
        //                            p.controlBtn3.Y = gdc.my;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        tempPoint = tempStart;
        //                        p.controlBtn1.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn1.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn2.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn2.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn3.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn3.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn4.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn4.Y += (gdc.my - tempPoint.Y);
        //                        tempStart = new Point(gdc.mx, gdc.my);
        //                    }
        //                }
        //                else if (p.drawtype == 3)
        //                {
        //                    if (gdc.node == 0)
        //                    {
        //                        p.controlBtn1.X = gdc.mx;
        //                        p.controlBtn1.Y = gdc.my;
        //                    }
        //                    if (gdc.node == 3)
        //                    {
        //                        p.controlBtn4.X = gdc.mx;
        //                        p.controlBtn4.Y = gdc.my;
        //                    }
        //                    if (gdc.node == 4)
        //                    {
        //                        tempPoint = tempStart;
        //                        p.controlBtn1.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn1.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn4.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn4.Y += (gdc.my - tempPoint.Y);
        //                        tempStart = new Point(gdc.mx, gdc.my);
        //                    }
        //                }
        //                else
        //                {
        //                    if (gdc.node == 0)
        //                    {
        //                        p.controlBtn1.X = gdc.mx;
        //                        p.controlBtn1.Y = gdc.my;
        //                    }
        //                    else if (gdc.node == 1)
        //                    {
        //                        p.controlBtn2.X = gdc.mx;
        //                        p.controlBtn2.Y = gdc.my;
        //                    }
        //                    else if (gdc.node == 2)
        //                    {
        //                        p.controlBtn3.X = gdc.mx;
        //                        p.controlBtn3.Y = gdc.my;
        //                    }
        //                    else if(gdc.node == 3)
        //                    {
        //                        p.controlBtn4.X = gdc.mx;
        //                        p.controlBtn4.Y = gdc.my;
        //                    }
        //                    else
        //                    {
        //                        tempPoint = tempStart;
        //                        p.controlBtn1.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn1.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn2.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn2.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn3.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn3.Y += (gdc.my - tempPoint.Y);
        //                        p.controlBtn4.X += (gdc.mx - tempPoint.X);
        //                        p.controlBtn4.Y += (gdc.my - tempPoint.Y);
        //                        tempStart = new Point(gdc.mx, gdc.my);
        //                    }
        //                }
        //            }
        //        }
        //        drawGPath(p);
        //        greenDrawing();
        //    }
        //}
        public void ClearBtnUse() // 清除畫布警告
        {
            if (MessageBox.Show("你確定要清除畫布嗎?    若要你的檔案將會全部遺失!", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                mygrid.Children.Clear();
                shapeLib.Data.gdc.sroot.PathList.Clear();
               shapeLib.Data.gdc.FullList.Clear();
               shapeLib.Data.gdc.UndoStack.Clear();
               shapeLib.Data.gdc.Release();
            }
        }
        public void ClearDrawing()  //清空資料區
        {
            mygrid.Children.Clear();
            shapeLib.Data.gdc.sroot.PathList.Clear();
        }
        public void hideBackLine() //背景格線取消
        {
            myBackground.Children.Clear();
        }
        public void drawBackLine(double w, double h, double opac) //畫背景格線
        {
            int i;
            int height = (int)h;
            int width = (int)w;
            int tempStroke = strokeT;
            byte tmpR = colorR;
            byte tmpG = colorG;
            byte tmpB = colorB;
            colorR = 0;
            colorG = 0;
            colorB = 0;
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
            colorR = tmpR;
            colorG = tmpG;
            colorB = tmpB;
            strokeT = tempStroke;
        }
        public void stroke(int stroketype) //線條粗細
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
        public void color(String CName) //使用顏色
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
                    shapeLib.Data.gdc.unDo();
                 //   reDraw(true);
                }
                if (Act == 1)
                {
                   shapeLib.Data.gdc.reDo();
                   // reDraw(true);
                }
            }
        }
        private String pathDataToPoint(String Data) //將Path.Data的值轉換成四個控制點,可以考慮換成其他判斷方式
        {
            String tempStr = Data;
            int tmpMSeat = tempStr.IndexOf("M");
            tempStr = tempStr.Remove(tmpMSeat, 1);

            String[] tempAry = tempStr.Split('C');
            tempStr = tempAry[0]+","+tempAry[1];
            //Debug.WriteLine(tempStr);
            return tempStr;
        }

        /*--------------  圖檔使用  --------------*/
        public void initpath(string xml) //匯入xml,轉換成圖片
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SVGRoot));
            using (MemoryStream ms = new MemoryStream( System.Text.Encoding.UTF8.GetBytes(xml)))
            {
                //要重新去記錄步驟,否則匯入後redo, undo 無法使用
                shapeLib.Data.gdc.sroot = (SVGRoot)serializer.Deserialize(XmlReader.Create(ms));
                //reDraw(true);
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e) //關閉時,轉成圖片
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

                s.Serialize(XmlWriter.Create(stream), shapeLib.Data.gdc.sroot);

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