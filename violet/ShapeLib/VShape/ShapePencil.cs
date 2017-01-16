using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    
    public class ShapePencil : ShapeObj
    {
        
        public override System.Collections.ArrayList getMenuItem()
        {
            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonGroup;
            ui.label = "Tools";
            ret.Add(ui);
            shapeUI pui = new shapeUI();
            pui.label = "Pencil";
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.pencil.png");
            pui.image = new System.Drawing.Bitmap(myStream);

            
            pui.click = btn_Click;
            pui.belong = "Tools";
            ret.Add(pui);
            
            return ret;
        }



        double B0(double u)
        {
            double ret = (1 - u) * (1 - u) * (1 - u) / 6.0;
            return ret;
        }

        double B1(double u)
        {
            double ret = (3 * u * u * u - 6 * u * u + 4.0) / 6.0;
            return ret;
        }
        double B2(double u)
        {
            double ret = (-3 * u * u * u + 3 * u * u + 3 * u + 1.0) / 6.0;
            return ret;
        }
        double B3(double u)
        {
            double ret = u * u * u / 6.0;
            return ret;
        }


    //    Point[] plist ;

      //  ArrayList addlist = new ArrayList();

        //ArrayList list = new ArrayList();

        int extra ;
    	byte r, g, b;

           public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)
            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;
                Path p = buildShape(data);

                r = data.state.colorR;
                g = data.state.colorG;
                b = data.state.colorB;

                p.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r,g,b));
                p.StrokeThickness = shapeLib.Data.strokeT;
               // p.StrokeEndLineCap = PenLineCap.Round;
               // p.StrokeStartLineCap = PenLineCap.Flat;
                shapeLib.Data.mygrid.Children.Add(p);
                gv.baseShape.Add(p);

                Debug.WriteLine("draw pencil");
        
            }
            else
            {
                Path myLine = (Path)gv.baseShape[0];

                buildShape(data, myLine);

            }
            

            //shapeLib.Data.mygrid.MouseDown += new System.Windows.Input.MouseButtonEventHandler(LeftButtonDown);
        }
        private  Path buildShape(gPath data ,Path old=null)
        {
            Path ret = old;

            if (old == null)
            {
                ret = new Path();
            }
            int m = data.pList.Count;
            Point[]    plist = new Point[m + 3];
            data.pList.CopyTo(plist, 1);
            plist[0] = plist[1] ;
            plist[m + 1] = plist[m+2]= data.controlBtn4;
            m = m +3;

            int MAX_STEPS = 10;

            PathFigure myPathFigure = new PathFigure();
          
          
            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
           
            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            ret.Data = myPathGeometry;

            bool bstart = true;
            for (int i = 0; i < m - 3; i++)
            {
                Point p = (Point)plist[i];
                Point p1 = (Point)plist[i + 1];
                Point p2 = (Point)plist[i + 2];
                Point p3 = (Point)plist[i + 3];


                if (i == m - 4)
                {
                    extra = 1;
                }
             
               // sx = p.X;
               // sy = p.Y;

                for (int j = 0; j < MAX_STEPS + extra; j++)
                {

                    double u = j * 1.0 / MAX_STEPS;
                    double Qx = B0(u) * p.X +
                                     B1(u) * p1.X +
                                     B2(u) * p2.X +
                                     B3(u) * p3.X;

                    double Qy = B0(u) * p.Y +
                                   B1(u) * p1.Y +
                                   B2(u) * p2.Y +
                                   B3(u) * p3.Y;

                    if ( bstart)
                    {
                        bstart = false;
                        myPathFigure.StartPoint = new System.Windows.Point((int)Qx, (int)Qy);


                    }
                    else
                    {
                        LineSegment lineseg = new LineSegment();
                        lineseg.Point = new System.Windows.Point((int)Qx, (int)Qy);
                        myPathSegmentCollection.Add(lineseg);

                    }


                }

            }

            return ret;
        }



        public override void MouseDownInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
           // if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                shapeLib.Data.gdc.writeIn(currPath, 0);
                shapeLib.Data.gdc.Release();
                shapeLib.Data.mClick = 0;
                return;

            }


            Canvas mygrid = shapeLib.Data.mygrid;

            shapeLib.Data.pStart = e.GetPosition(mygrid);

            if ( shapeLib.Data.mClick == 0)
            {

                currPath = new gPath();
                currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);
                currPath.pList.Add(shapeLib.Data.pStart);
            }
            shapeLib.Data.tempStart = shapeLib.Data.pStart;
            shapeLib.Data.bCanMove = true;

        }

        public override void MouseUpInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

        

            if (shapeLib.Data.bCanMove && e.ChangedButton== MouseButton.Left)
            {
                shapeLib.Data.pEnd = e.GetPosition(mygrid);
                double tempX, tempY;
                double px = shapeLib.Data.pStart.X;
                double py = shapeLib.Data.pStart.Y;
                double ex = shapeLib.Data.pEnd.X;
                double ey = shapeLib.Data.pEnd.Y;

               // if (px == ex && py == ey) //click
                {
                        currPath.pList.Add(new Point(ex, ey));

                    //
                    Debug.WriteLine("click");
                    remGPath(px, py, ex, ey);
                    currPath.redraw(1);

                    shapeLib.Data.mClick++;

                    //if (this.GetType() == typeof(ShapeCurve) && shapeLib.Data.mClick >= 3)
                    //{
                    //    currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);//line,在shaplib 中的位置
                    //    shapeLib.Data.gdc.writeIn(currPath, 0);
                    //    shapeLib.Data.gdc.Release();
                    //    shapeLib.Data.mClick = 0;
                    //}


                    foreach (gPath gp in shapeLib.Data.multiSelList)
                    {
                        gp.isSel = false;
                    }
                    if (shapeLib.Data.currShape != null)
                        shapeLib.Data.currShape.isSel = false;

                    shapeLib.Data.multiSelList.Clear();
                    return;
                }
                //remGPath(px, py, ex, ey);
                //{
                //    currPath.drawtype = shapeLib.SupportedShape(null).IndexOf(this);//line,在shaplib 中的位置
                //    shapeLib.Data.gdc.writeIn(currPath, 0);
                //    shapeLib.Data.gdc.Release();
                //}
                //shapeLib.Data.gdc.bmove = false;
                //if (shapeLib.Data.Status.Equals("rest"))
                //    currPath.redraw(1);

                //shapeLib.Data.bfirst = true;
                //shapeLib.Data.bhave = false;
            }
            //throw new NotImplementedException();
        }

        public override void MouseMoveInsert(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas mygrid = shapeLib.Data.mygrid;

           

            if ( currPath!= null && shapeLib.Data.mClick > 0)
            {
                //   if (!shapeLib.Data.bhave) //if you can control an object
                {
                    shapeLib.Data.pEnd =e.GetPosition(mygrid);
                    double tempX, tempY;
                    double px = shapeLib.Data.pStart.X;
                    double py = shapeLib.Data.pStart.Y;
                    double ex = shapeLib.Data.pEnd.X;
                    double ey = shapeLib.Data.pEnd.Y;

                    remGPath(px, py, ex, ey);
                    currPath.redraw(0);

                }
            }

            
  
            //throw new NotImplementedException();
        }


    }
}