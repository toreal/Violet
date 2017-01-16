using Microsoft.Office.Tools.Ribbon;
using ShapeLib.VShape;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
//using System.Drawing;
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


namespace ShapeLib.VShape
{


    public enum shapeUIType
    {
        RibbonBigButton,
        RibbonSmallButton,
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
        public string belong; //屬於某一Group 或 menu

    }

    public class ShapeObj : IShapeUI, IDrawing, IUpdateOP, IInsertOP
    {

        protected gPath currPath;

        public virtual System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonGroup;
            ui.label = "Shapes";
            ret.Add(ui);

            ui = new shapeUI();
            ui.label = "Line";
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.line.png");
            ui.image = new System.Drawing.Bitmap(myStream);



            ui.click = this.btn_Click;
            ui.belong = "Shapes";
            ret.Add(ui);

            return ret;
        }

        public void btn_Click(object sender, RibbonControlEventArgs e)
        {

            MouseOP(0);
        }

        public void MouseOP(int ntype)
        {
            IForm f = null;

            if (shapeLib.Data.view != null)
            {
                f = shapeLib.Data.view();

                f.Show();
                shapeLib.Data.mygrid = f.drawControl;
                shapeLib.Data.Root = f.getRoot;
            }

            if (shapeLib.Data.mygrid != null)
            {
                IList<ShapeObj> ret = shapeLib.SupportedShape(null);

                foreach (ShapeObj obj in ret)
                {
                    shapeLib.Data.mygrid.MouseUp -= obj.MouseUpInsert;
                    shapeLib.Data.mygrid.MouseMove -= obj.MouseMoveInsert;
                    shapeLib.Data.mygrid.MouseDown -= obj.MouseDownInsert;

                    shapeLib.Data.mygrid.MouseUp -= obj.MouseDownUpdate;
                    shapeLib.Data.mygrid.MouseMove -= obj.MouseMoveUpdate;
                    shapeLib.Data.mygrid.MouseDown -= obj.MouseUpUpdate;

                    shapeLib.Data.Root.KeyDown -= obj.FormKeyDown;
                    shapeLib.Data.Root.KeyDown += obj.FormKeyDown;

                }

                if (ntype == 0)
                {
                    shapeLib.Data.mygrid.MouseUp += this.MouseUpInsert;
                    shapeLib.Data.mygrid.MouseMove += this.MouseMoveInsert;
                    shapeLib.Data.mygrid.MouseDown += this.MouseDownInsert;


                }
                else
                {

                    shapeLib.Data.mygrid.MouseLeftButtonUp += this.MouseUpUpdate;
                    shapeLib.Data.mygrid.MouseMove += this.MouseMoveUpdate;
                    shapeLib.Data.mygrid.MouseLeftButtonDown += this.MouseDownUpdate;

                }

            }





            //  System.Windows.Forms.MessageBox.Show("Clicked");

        }

        //從xml 生成data
        public ShapeObj Create(string svg)
        {
            throw new NotImplementedException();
        }

        public void changeProperty(string prop, string value)
        {
            throw new NotImplementedException();
        }

        //存檔時存生xml 
        public string SVGString()
        {
            throw new NotImplementedException();
        }

        //依data 繪製,如果是第一次畫要新建shape, 更新的話只要更新最後一點
        public virtual void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)

            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;

                Line myLine = new Line();
                myLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));

                myLine.X1 = data.controlBtn1.X;
                myLine.Y1 = data.controlBtn1.Y;
                myLine.X2 = data.controlBtn4.X;
                myLine.Y2 = data.controlBtn4.Y;
                myLine.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = shapeLib.Data.strokeT;
                myLine.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myLine.MouseEnter += data.myLine_MouseEnter;
                myLine.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myLine);
                gv.baseShape.Add(myLine);
                // currPath.setDrawShape( myLine);

            }
            else
            {
                Line myLine = (Line)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                myLine.X2 = data.controlBtn4.X;
                myLine.Y2 = data.controlBtn4.Y;
            }


            //   throw new NotImplementedException();
        }





        public virtual void DisplayControlPoints(gView gv, gPath data)
        {
            if (gv.controlShape.Count == 0)
            {
                Line myLine = new Line();
                myLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                myLine.X1 = data.controlBtn1.X;
                myLine.Y1 = data.controlBtn1.Y;
                myLine.X2 = data.controlBtn4.X;
                myLine.Y2 = data.controlBtn4.Y;
                myLine.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = shapeLib.Data.strokeT;
                myLine.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myLine.MouseEnter += data.myLine_MouseEnter;
                myLine.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myLine);
                gv.controlShape.Add(myLine);


            }
            else
            {
                Line myLine = (Line)gv.controlShape[0];// =(Line) currPath.getDrawShape();

                myLine.X1 = data.controlBtn1.X;
                myLine.Y1 = data.controlBtn1.Y;
                myLine.X2 = data.controlBtn4.X;
                myLine.Y2 = data.controlBtn4.Y;

            }


            // throw new NotImplementedException();
        }

        public void MouseDownUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //int tempDraw = shapeLib.Data.gdc.sroot.PathList[shapeLib.Data.ru.Sel].drawtype;
            //if (tempDraw == 3)
            //{
            //    shapeLib.Data.pStart = e.GetPosition(shapeLib.Data.myControl);
            //}
            //else
            //{
            //    shapeLib.Data.pStart = correctPoint(e.GetPosition(shapeLib.Data.myControl));
            //}

            //currPath = new gPath();
            //shapeLib.Data.tempStart = shapeLib.Data.pStart;

            //if (!shapeLib.Data.gCanMove && !shapeLib.Data.OnIt)
            //{
            //    //hiddenCanvas();
            //    shapeLib.Data.ru.Sel = -1;
            //    shapeLib.Data.ru.Node = -1;
            //    shapeLib.Data.bConThing = false;
            //    shapeLib.Data.gdc.bmove = false;
            //    shapeLib.Data.bfirst = true;
            //    shapeLib.Data.bhave = false;
            //    shapeLib.Data.OnIt = false;
            //}
            //if (shapeLib.Data.ru.Sel >= 0)
            //{
            //    shapeLib.Data.gdc.node = shapeLib.Data.ru.Node;
            //}
            //throw new NotImplementedException();
        }

        public void MouseUpUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void MouseMoveUpdate(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //  throw new NotImplementedException();
        }


        public virtual void MouseDownInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

            shapeLib.Data.pStart = correctPoint(e.GetPosition(mygrid));

            if ((this.GetType() != typeof(ShapeCurve) && this.GetType() != typeof(ShapePencil)) || shapeLib.Data.mClick == 0)
            {

                currPath = new gPath();
                currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);
            }
            shapeLib.Data.tempStart = shapeLib.Data.pStart;
            shapeLib.Data.bCanMove = true;

        }

        public virtual void MouseUpInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

                if (px == ex && py == ey) //click
                {
                    if (this.GetType() == typeof(ShapePencil))
                    {
                        currPath.pList.Add(new Point(ex, ey));

                    }

                    //
                    Debug.WriteLine("click");
                    remGPath(px, py, ex, ey);
                    currPath.redraw(1);

                    shapeLib.Data.mClick++;

                    if (this.GetType() == typeof(ShapeCurve) && shapeLib.Data.mClick >= 3)
                    {
                        currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);//line,在shaplib 中的位置
                        shapeLib.Data.gdc.writeIn(currPath, 0);
                        shapeLib.Data.gdc.Release();
                        shapeLib.Data.mClick = 0;
                    }


                    foreach (gPath gp in shapeLib.Data.multiSelList)
                    {
                        gp.isSel = false;
                    }
                    if (shapeLib.Data.currShape != null)
                        shapeLib.Data.currShape.isSel = false;

                    shapeLib.Data.multiSelList.Clear();
                    return;
                }
                if (this.GetType() == typeof(ShapeRectangle))
                {
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
                }
                if (this.GetType() == typeof(ShapeCircle))
                {
                    if (ex < px)
                    {
                        tempX = ex;
                        ex = px;
                        px = tempX;
                    }
                    if (ey < py)
                    {
                        tempY = ey;
                        ey = py;
                        py = tempY;
                    }
                }
                if (this.GetType() == typeof(ShapeTriangle))
                {
                    if (ex < px)
                    {
                        tempX = px;
                        px = ex;
                        ex = tempX;
                    }
                    if (ex > px)
                    {
                        tempX = ex;
                        ex = px;
                        ex = tempX;
                    }
                }
                remGPath(px, py, ex, ey);

                if (this.GetType() == typeof(ShapeCurve))
                {
                    shapeLib.Data.mClick++;
                    if (shapeLib.Data.mClick >= 3)
                        shapeLib.Data.mClick = 0;

                }
                // || shapeLib.Data.mClick >=2 )
                {
                    currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);//line,在shaplib 中的位置
                    shapeLib.Data.gdc.writeIn(currPath, 0);
                    shapeLib.Data.gdc.Release();
                }
                shapeLib.Data.gdc.bmove = false;
                if (shapeLib.Data.Status.Equals("rest"))
                    currPath.redraw(1);

                shapeLib.Data.bfirst = true;
                shapeLib.Data.bhave = false;
            }
            //throw new NotImplementedException();
        }

        public virtual void MouseMoveInsert(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                //   if (!shapeLib.Data.bhave) //if you can control an object
                {
                    shapeLib.Data.pEnd = correctPoint(e.GetPosition(mygrid));
                    double tempX, tempY;
                    double px = shapeLib.Data.pStart.X;
                    double py = shapeLib.Data.pStart.Y;
                    double ex = shapeLib.Data.pEnd.X;
                    double ey = shapeLib.Data.pEnd.Y;


                    if (this.GetType() == typeof(ShapeRectangle))
                    {
                        if (ex < px)
                        {
                            tempX = ex;
                            ex = px;
                            px = tempX;
                        }
                        if (ey < py)
                        {
                            tempY = ey;
                            ey = py;
                            py = tempY;
                        }
                    }
                    if (this.GetType() == typeof(ShapeCircle))
                    {
                        if (ex < px)
                        {
                            tempX = ex;
                            ex = px;
                            px = tempX;
                        }
                        if (ey < py)
                        {
                            tempY = ey;
                            ey = py;
                            py = tempY;
                        }
                    }
                    if (this.GetType() == typeof(ShapeTriangle))
                    {
                        if (ex < px)
                        {
                            tempX = px;
                            px = ex;
                            ex = tempX;
                        }
                        if (ex > px)
                        {
                            tempX = ex;
                            ex = px;
                            ex = tempX;
                        }
                    }
                    remGPath(px, py, ex, ey);
                    currPath.redraw(0);

                }
            }
            else
            {
                if (this.GetType() == typeof(ShapePencil))
                {

                }


            }

            //throw new NotImplementedException();
        }


        /*--------------  其他功能  --------------*/
        protected void remGPath(double px, double py, double ex, double ey) //儲存新繪製的圖形資料
        {
            currPath.state.colorB = shapeLib.Data.colorB;
            currPath.state.colorG = shapeLib.Data.colorG;
            currPath.state.colorR = shapeLib.Data.colorR;
            currPath.state.strokeT = shapeLib.Data.strokeT;
            currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);

            if (shapeLib.Data.ru.Sel >= 0)
                currPath.ListPlace = shapeLib.Data.ru.Sel;
            else
                currPath.ListPlace = shapeLib.Data.gdc.sroot.PathList.Count;

            if (this.GetType() == typeof(ShapeCurve))
            {

                switch (shapeLib.Data.mClick)
                {
                    case 0:
                        currPath.controlBtn1 = new System.Windows.Point(px, py);
                        currPath.controlBtn4 = new System.Windows.Point(ex, ey);

                        currPath.controlBtn2 = currPath.controlBtn1;
                        currPath.controlBtn3 = currPath.controlBtn4;

                        break;
                    case 1:
                        currPath.controlBtn2 = new System.Windows.Point(ex, ey);
                        break;
                    case 2:
                        currPath.controlBtn3 = new System.Windows.Point(ex, ey);

                        break;


                }

                Debug.WriteLine(currPath.controlBtn1);
                Debug.WriteLine(currPath.controlBtn2);
                Debug.WriteLine(currPath.controlBtn3);
                Debug.WriteLine(currPath.controlBtn4);
                Debug.WriteLine(px.ToString() + "," + py.ToString() + "_______________________" + shapeLib.Data.mClick.ToString());
            }
            else
            {
                currPath.controlBtn1 = new System.Windows.Point(px, py);
                currPath.controlBtn4 = new System.Windows.Point(ex, ey);
                currPath.controlBtn2 = new System.Windows.Point(ex, py);
                currPath.controlBtn3 = new System.Windows.Point(px, ey);



            }

        }


        protected System.Windows.Point correctPoint(System.Windows.Point p)
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

        public virtual void FormKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

    }
}