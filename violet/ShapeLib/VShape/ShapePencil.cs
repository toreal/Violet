using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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


        ArrayList plist = new ArrayList();
        //ArrayList list = new ArrayList();

        int extra ;
        int sx;
        int sy;

           public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)
            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;
                sx = (int)data.controlBtn1.X;
                sy = (int)data.controlBtn1.Y;
                plist.Add(new System.Drawing.Point(sx, sy));
                shapeLib.Data.mygrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LeftButtonDown);


            }
            
            //shapeLib.Data.mygrid.MouseDown += new System.Windows.Input.MouseButtonEventHandler(LeftButtonDown);
        }
        public void DrawPoint(int x, int y, int idx)
        {

            if (idx != 0)
            {
                Line myLine = new Line();
                myLine.X1 = sx;
                myLine.Y1 = sy;
                myLine.X2 = x;
                myLine.Y2 = y;
                myLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(shapeLib.Data.colorR, shapeLib.Data.colorG, shapeLib.Data.colorB));
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = shapeLib.Data.strokeT;

                shapeLib.Data.mygrid.Children.Add(myLine);
            }
            //gv.baseShape.Add(myLine);
            sx = x;
            sy = y;
        }
        public void redraw()
        {
            int m = plist.Count;
            int MAX_STEPS = 10;
     
            for (int i = 0; i < m - 3; i++)
            {
                System.Drawing.Point p = (System.Drawing.Point)plist[i];
                System.Drawing.Point p1 = (System.Drawing.Point)plist[i + 1];
                System.Drawing.Point p2 = (System.Drawing.Point)plist[i + 2];
                System.Drawing.Point p3 = (System.Drawing.Point)plist[i + 3];


                if (i == m - 4)
                {
                    extra = 1;
                }
             
                sx = p.X;
                sy = p.Y;

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

                    DrawPoint((int)Qx, (int)Qy, j);

                }

            }

         
        }
        public void LeftButtonDown(object sender,EventArgs e)
        {

            for (int i = 0; i < plist.Count; i++) {
                if(plist.Count>4) plist.RemoveAt(i); 
                    
            }
                redraw();

        }
     
        
    }
}