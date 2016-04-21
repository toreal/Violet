using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLib.VShape
{
    public class ShapeArrow : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Arrow";


            ui.image = new System.Drawing.Bitmap(@"icons\arrow.png");
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

                double x = data.controlBtn1.X;
                double y = data.controlBtn1.Y;
                double num3 = data.controlBtn4.X;
                double num4 = data.controlBtn4.Y;
                double num5 = Math.Atan2((double)(num3 - x), (double)(num4 - y));
                double num6 = 0.52359877559829882;
                double num7 = 8.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point(num3, num4);
                point1.X = num3 - ((double)(num7 * Math.Sin(num5 - num6)));
                point1.Y = num4 - ((double)(num7 * Math.Cos(num5 - num6)));
                point2.X = num3 - ((double)(num7 * Math.Sin(num5 + num6)));
                point2.Y = num4 - ((double)(num7 * Math.Cos(num5 + num6)));
                Polygon myLine = new Polygon();

                //        //如果要繪製中心顏色，可開啟這段
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(1, data.state.colorR, data.state.colorG, data.state.colorB);
                myLine.Fill = mySolidColorBrush;
                myLine.StrokeThickness = data.state.strokeT;

                myLine.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                //myLine.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myLine.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);              
                PointCollection Points = new PointCollection();
                Points.Add(data.controlBtn1);
                Points.Add(data.controlBtn4);   
                Points.Add(point1);
                Points.Add(point2);
                Points.Add(new System.Windows.Point(num3 , num4)); 

                myLine.Points = new PointCollection(Points);
                //  myLine.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myLine.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = shapeLib.Data.strokeT;
                myLine.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                //   myLine.MouseEnter += data.myLine_MouseEnter;
                //   myLine.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myLine);
                gv.baseShape.Add(myLine);
                // currPath.setDrawShape( myLine);

            }
            else
            {
                Polygon myLine = (Polygon)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                //myLine.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myLine.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                PointCollection Points = new PointCollection();
                double x = data.controlBtn1.X;
                double y = data.controlBtn1.Y;
                double num3 = data.controlBtn4.X;
                double num4 = data.controlBtn4.Y;
                double num5 = Math.Atan2((double)(num3 - x), (double)(num4 - y));
                double num6 = 0.52359877559829882;
                double num7 = 8.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point(num3, num4);
                point1.X = num3 - ((double)(num7 * Math.Sin(num5 - num6)));
                point1.Y = num4 - ((double)(num7 * Math.Cos(num5 - num6)));
                point2.X = num3 - ((double)(num7 * Math.Sin(num5 + num6)));
                point2.Y = num4 - ((double)(num7 * Math.Cos(num5 + num6)));

                Points.Add(data.controlBtn1);
                Points.Add(data.controlBtn4);                      
                Points.Add(point1);
                Points.Add(point2);
                Points.Add(new System.Windows.Point(num3, num4)); 



                myLine.Points = new PointCollection(Points);
                // myLine.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);



            }
        }

    }
}