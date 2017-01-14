
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
       // Point pStart, pEnd; //滑鼠起點和終點
       // Point tempStart;
        Point p0, p1, p2, p3 = new Point(0, 0); //紀錄四個控制點使用
        BezierSegment bezier = new BezierSegment();
        PathFigure figure = new PathFigure();
        PathGeometry geometry = new PathGeometry();



        Geometry tempGeo;
        // gPath tempFPath; 
        // Ellipse myEllipse; //紀錄橢圓形
        // Rectangle myRect, cornerRect, sideRect;
        Line myLine;// controlLine; //紀錄直線、控制後的直線
       // Polygon myTri;
        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path(); //紀錄曲線
        System.Windows.Shapes.Path controlPath = new System.Windows.Shapes.Path(); //紀錄控制後的曲線


        bool bfirst = true; //是否為繪製新圖形
      //  bool bCanMove = false; //繪製時,滑鼠是否可以移動
       // bool bhave = false; //you have choose
       // bool gCanMove = false; //選取後是否可以移動
        bool bConThing = false; //是否有選取物件
    //  bool OnIt = false; //是否有滑入或滑出選取物件

        

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
        //換鼠標
        void Shapes_MouseEnter_Hands(object sender, MouseEventArgs e)
        {
           // OnIt = true;
            this.Cursor = System.Windows.Input.Cursors.Hand;
        }
        void Shapes_MouseEnter_SizeAll(object sender, MouseEventArgs e)
        {
        //    OnIt = true;
            this.Cursor = System.Windows.Input.Cursors.SizeAll;
        }
        void Shapes_MouseLeave(object sender, MouseEventArgs e)
        {
          //  OnIt = false;
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
                   shapeLib.Data.strokeT = 1;
                    break;
                case 3:
                    shapeLib.Data.strokeT = 3;
                    break;
                case 5:
                    shapeLib.Data.strokeT = 5;
                    break;
                case 8:        //可考慮拿掉,有點太粗了
                    shapeLib.Data.strokeT = 8;
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