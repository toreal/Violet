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

                Polygon myTri = new Polygon();

                //        //如果要繪製中心顏色，可開啟這段
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(1, data.state.colorR, data.state.colorG, data.state.colorB);
                myTri.Fill = mySolidColorBrush;
                myTri.StrokeThickness = data.state.strokeT;

                myTri.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                //myTri.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myTri.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);              
                PointCollection Points = new PointCollection();
                Points.Insert(0,data.controlBtn1);
                Points.Insert(1, data.controlBtn3);
                Points.Add(data.controlBtn4);
                myTri.Points = new PointCollection(Points);
            //    myTri.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myTri.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myTri.VerticalAlignment = VerticalAlignment.Center;
                myTri.StrokeThickness = shapeLib.Data.strokeT;
                myTri.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
             //   myTri.MouseEnter += data.myLine_MouseEnter;
             //   myTri.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myTri);
                gv.baseShape.Add(myTri);
                // currPath.setDrawShape( myLine);

            }
            else
            {
                Polygon myTri = (Polygon)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                //myTri.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myTri.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                PointCollection Points = new PointCollection();
                Points.Insert(0, data.controlBtn1);
                Points.Insert(1, data.controlBtn3);
                Points.Add(data.controlBtn4);
                myTri.Points = new PointCollection(Points);
             // myTri.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                
            }


            //   throw new NotImplementedException();
        }


    }
}