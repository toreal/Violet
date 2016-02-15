using Microsoft.Office.Tools.Ribbon;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace violet.Shape
{


    public enum shapeUIType
    {
        RibbonButton,
        RibbonMenu,
        RibbonGroup
    }

   //public  delegate void mouseClick(object sender, RibbonControlEventArgs e);
    public class shapeUI
    {
        public shapeUIType uitype;
        public System.Drawing.Image image;
        public String label;
        public ArrayList items;
        public RibbonControlEventHandler click;
        public string belong;
    }

    public class ShapeObj:IShapeUI,IDrawing,IUpdateOP,IInsertOP
    {

        gPath tempFPath;
        int drawtype=3;
        System.Windows.Point p0, p1, p2, p3 = new System.Windows.Point(0, 0); //紀錄四個控制點使用
        Line myLine;
        int xStart;
        int yStart;
        int xEnd;
        int yEnd;

        public System.Collections.ArrayList getMenuItem()
        {
           
            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "test";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }

        void btn_Click(object sender, RibbonControlEventArgs e)
        {

            if (shapeLib.Data.mygrid!= null)
            {
                IList<ShapeObj> ret = shapeLib.SupportedShape(null);

                foreach(ShapeObj obj in ret)
                {
                    shapeLib.Data.mygrid.MouseLeftButtonUp -= obj.MouseUpInsert;
                    shapeLib.Data.mygrid.MouseMove -= this.MouseMoveInsert;
                    shapeLib.Data.mygrid.MouseLeftButtonDown -= this.MouseDownInsert;
                
                }

                shapeLib.Data.mygrid.MouseLeftButtonUp += this.MouseUpInsert;
                shapeLib.Data.mygrid.MouseMove += this.MouseMoveInsert;
                shapeLib.Data.mygrid.MouseLeftButtonDown += this.MouseDownInsert;

            }


          if ( shapeLib.Data.view != null)
          {
              shapeLib.Data.view.Show();
          }

            
            System.Windows.Forms.MessageBox.Show("Clicked");
            
        }

        public ShapeObj Create(string svg)
        {
            throw new NotImplementedException();
        }

        public void changeProperty(string prop)
        {
            throw new NotImplementedException();
        }

        public string SVGString()
        {
            throw new NotImplementedException();
        }

        public bool IsDelete
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DrawShape()
        {
            if (shapeLib.Data.bfirst)
            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;
                myLine = new Line();
                myLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(shapeLib.Data.colorR, shapeLib.Data.colorG, shapeLib.Data.colorB));
                myLine.X1 = xStart;
                myLine.X2 = xEnd;
                myLine.Y1 = yStart;
                myLine.Y2 = yEnd;
                myLine.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = shapeLib.Data.strokeT;
                shapeLib.Data.mygrid.Children.Add(myLine);
            }
            else
            {
                myLine.X2 = xEnd;
                myLine.Y2 = yEnd;
            }


         //   throw new NotImplementedException();
        }

        public void DisplayControlPoints()
        {
            throw new NotImplementedException();
        }

        public void MouseDownUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int tempDraw = shapeLib.Data.gdc.sroot.PathList[shapeLib.Data.ru.Sel].drawtype;
            if (tempDraw == 3)
            {
                shapeLib.Data.pStart = e.GetPosition(shapeLib.Data.myControl);
            }
            else
            {
                shapeLib.Data.pStart = correctPoint(e.GetPosition(shapeLib.Data.myControl));
            }

            tempFPath = new gPath();
            shapeLib.Data.tempStart = shapeLib.Data.pStart;

            if (!shapeLib.Data.gCanMove && !shapeLib.Data.OnIt)
            {
                //hiddenCanvas();
                shapeLib.Data.ru.Sel = -1;
                shapeLib.Data.ru.Node = -1;
                shapeLib.Data.bConThing = false;
                shapeLib.Data.gdc.bmove = false;
                shapeLib.Data.bfirst = true;
                shapeLib.Data.bhave = false;
                shapeLib.Data.OnIt = false;
            }
            if (shapeLib.Data.ru.Sel >= 0)
            {
                shapeLib.Data.gdc.node = shapeLib.Data.ru.Node;
            }
            //throw new NotImplementedException();
        }

        public void MouseUpUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveUpdate(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void MouseDownInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

            shapeLib.Data.pStart = correctPoint(e.GetPosition(mygrid));
            tempFPath = new gPath();
            shapeLib.Data.tempStart = shapeLib.Data.pStart;
            shapeLib.Data.bCanMove = true;
            if (shapeLib.Data.drawtype == 5)
            {
                if (shapeLib.Data.gdc.selIndex < 0)
                {
                    shapeLib.Data.gdc.selIndex = shapeLib.Data.gdc.sroot.PathList.Count - 1;
                }
            }
            else
            {
                shapeLib.Data.gdc.selIndex = -1;
            }

           // throw new NotImplementedException();
        }

        public void MouseUpInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

            if (shapeLib.Data.bCanMove)
            {
                shapeLib.Data.pEnd = correctPoint(e.GetPosition(mygrid));
                double tempX, tempY;
                double px = shapeLib.Data.pStart.X;
                double py = shapeLib.Data.pStart.Y;
                double ex = shapeLib.Data.pEnd.X;
                double ey = shapeLib.Data.pEnd.Y;

                if (shapeLib.Data.drawtype != 3 && ex < px)
                {
                    tempX = ex;
                    ex = px;
                    px = tempX;
                }
                if (shapeLib.Data.drawtype != 3 && ey < py)
                {
                    tempY = ey;
                    ey = py;
                    py = tempY;
                }

                remGPath(px, py, ex, ey);

                if (shapeLib.Data.drawtype <= 4 && shapeLib.Data.Status.Equals("rest"))
                {
                    shapeLib.Data.gdc.writeIn(tempFPath, 0);
                    shapeLib.Data.gdc.Release();
                }
                shapeLib.Data.gdc.bmove = false;
                if (shapeLib.Data.Status.Equals("rest"))
                    DrawShape();

                shapeLib.Data.bfirst = true;
                shapeLib.Data.bhave = false;
            }
            //throw new NotImplementedException();
        }

        public void MouseMoveInsert(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (!shapeLib.Data.bhave) //if you can control an object
                {
                    shapeLib.Data.pEnd = correctPoint(e.GetPosition(mygrid));
                    double tempX, tempY;
                    double px = shapeLib.Data.pStart.X;
                    double py = shapeLib.Data.pStart.Y;
                    double ex = shapeLib.Data.pEnd.X;
                    double ey = shapeLib.Data.pEnd.Y;
                    if (shapeLib.Data.drawtype < 3 && ex < px)
                    {
                        tempX = ex;
                        ex = px;
                        px = tempX;
                    }
                    if (shapeLib.Data.drawtype < 3 && ey < py)
                    {
                        tempY = ey;
                        ey = py;
                        py = tempY;
                    }

                     xStart=(int)px;
                     yStart = (int)py;
                     xEnd = (int)ex;
                     yEnd = (int)ey;
                     DrawShape();
                    //switch (shapeLib.Data.drawtype)
                    //{
                    //    case 1:
                    //        mygrid.drawEllipse((int)px, (int)py, (int)ex, (int)ey);
                    //        myEllipse.Opacity = 0.5;
                    //        break;
                    //    case 2:
                    //        mygrid.drawRect((int)px, (int)py, (int)ex, (int)ey, 0);
                    //        myRect.Opacity = 0.5;
                    //        break;
                    //    case 3:
                    //        mygrid.drawLine((int)px, (int)py, (int)ex, (int)ey);
                    //        myLine.Opacity = 0.5;
                    //        break;
                    //    case 4:
                    //        mygrid.drawCurve((int)px, (int)py, (int)ex, (int)ey);
                    //        myPath.Opacity = 0.5;
                    //        break;
                    //}
                }
            }

            //throw new NotImplementedException();
        }


        /*--------------  其他功能  --------------*/
        private void remGPath(double px, double py, double ex, double ey) //儲存新繪製的圖形資料
        {
            tempFPath.state.colorB = shapeLib.Data.colorB;
            tempFPath.state.colorG = shapeLib.Data.colorG;
            tempFPath.state.colorR = shapeLib.Data.colorR;
            tempFPath.state.strokeT = shapeLib.Data.strokeT;
            tempFPath.drawtype = drawtype;

            if (shapeLib.Data.ru.Sel >= 0)
                tempFPath.ListPlace = shapeLib.Data.ru.Sel;
            else
                tempFPath.ListPlace = shapeLib.Data.gdc.sroot.PathList.Count;

            if (drawtype <= 3)
            {
                tempFPath.controlBtn1 = new System.Windows.Point(px, py);
                tempFPath.controlBtn4 = new System.Windows.Point(ex, ey);

                if (drawtype < 3)
                {
                    tempFPath.controlBtn2 = new System.Windows.Point(ex, py);
                    tempFPath.controlBtn3 = new System.Windows.Point(px, ey);
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
      

        private System.Windows.Point correctPoint(System.Windows.Point p)
        {
            int lineSpace = shapeLib.Data.lineSpace;
            System.Windows.Point temp = p;
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


    }
}
