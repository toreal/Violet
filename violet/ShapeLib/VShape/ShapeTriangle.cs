using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLib.VShape
{
    class ShapeTriangle : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Triangle";


            ui.image = new Bitmap(@"icons\triangle.png");
            ui.belong = "Shapes";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }

        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)
            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;

                Polyline myTri = new Polyline();

                //        //如果要繪製中心顏色，可開啟這段
                myTri.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                PointCollection Points = new PointCollection();
             
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X , data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn4.X , data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));

                myTri.Points = new PointCollection(Points);
                myTri.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myTri.VerticalAlignment = VerticalAlignment.Center;
                myTri.StrokeThickness = shapeLib.Data.strokeT;
                myTri.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myTri.MouseEnter += data.myLine_MouseEnter;
                myTri.MouseLeave += data.myLine_MouseLeave;               
                shapeLib.Data.mygrid.Children.Add(myTri);
                gv.baseShape.Add(myTri);

            }
            else
            {
                Polyline myTri = (Polyline)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                PointCollection Points = new PointCollection();
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn4.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                myTri.Points = new PointCollection(Points);

            }


            //   throw new NotImplementedException();
        }

        public override void DisplayControlPoints(gView gv, gPath data)
        {
            if (gv.controlShape.Count == 0)
            {
                Polyline myTri = new Polyline();

                //        //如果要繪製中心顏色，可開啟這段
                myTri.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 0));
                PointCollection Points = new PointCollection();
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn4.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                myTri.Points = new PointCollection(Points);
                myTri.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myTri.VerticalAlignment = VerticalAlignment.Center;
                myTri.StrokeThickness = shapeLib.Data.strokeT;
                myTri.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myTri.MouseEnter += data.myLine_MouseEnter;
                myTri.MouseLeave += data.myLine_MouseLeave;    
                shapeLib.Data.mygrid.Children.Add(myTri);
                gv.controlShape.Add(myTri);

            }

            else
            {
                Polyline myTri = (Polyline)gv.controlShape[0];// =(Line) currPath.getDrawShape();
                PointCollection Points = new PointCollection();
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn4.X, data.controlBtn4.Y));
                Points.Add(new System.Windows.Point(data.controlBtn1.X + (data.controlBtn4.X - data.controlBtn1.X) / 2, data.controlBtn1.Y));
                myTri.Points = new PointCollection(Points);
            }

        }
    }
}