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


            ui.image = new System.Drawing.Bitmap(ui.codebase+@"icons\arrow.png");
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

                double num1 = Math.Atan2((data.controlBtn4.X - data.controlBtn1.X), (data.controlBtn4.Y - data.controlBtn1.Y));
                double num2 = 0.5;
                double num3 = 6.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point();
                point1.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 - num2)));
                point1.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 - num2)));
                point2.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 + num2)));
                point2.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 + num2)));
                point3.X = point1.X - (point1.X - point2.X) / 2;
                point3.Y = point1.Y - (point1.Y - point2.Y) / 2;
                Polyline myArr = new Polyline();

                //        //如果要繪製中心顏色，可開啟這段
               
                myArr.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                //myArr.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myArr.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);              
                PointCollection Points = new PointCollection();
          
                Points.Add(data.controlBtn1);
                Points.Add(point3);
                Points.Add(point1);
                Points.Add(data.controlBtn4);
                Points.Add(point2);
                Points.Add(point3); 
    

                myArr.Points = new PointCollection(Points);
                //  myArr.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myArr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myArr.VerticalAlignment = VerticalAlignment.Center;
                myArr.StrokeThickness = shapeLib.Data.strokeT;
                myArr.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myArr.MouseEnter += data.myLine_MouseEnter;
                myArr.MouseLeave += data.myLine_MouseLeave;    
                //   myLine.MouseEnter += data.myLine_MouseEnter;
                //   myLine.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myArr);
                gv.baseShape.Add(myArr);
                // currPath.setDrawShape( myArr);

            }
            else
            {
                Polyline myArr = (Polyline)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                PointCollection Points = new PointCollection();            
                double num1 = Math.Atan2((data.controlBtn4.X - data.controlBtn1.X), (data.controlBtn4.Y - data.controlBtn1.Y));
                double num2 =0.5;
                double num3= 6.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point();
                point1.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 - num2)));
                point1.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 - num2)));
                point2.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 + num2)));
                point2.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 + num2)));
                point3.X = point1.X - (point1.X - point2.X) / 2;
                point3.Y = point1.Y - (point1.Y - point2.Y) / 2;
                
                Points.Add(data.controlBtn1);
                Points.Add(point3);              
                Points.Add(point1);
                Points.Add(data.controlBtn4);
                Points.Add(point2);
                Points.Add(point3); 
                myArr.Points = new PointCollection(Points);
                // myLine.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);



            }
        }

        public override void DisplayControlPoints(gView gv, gPath data)
        {
            if (gv.controlShape.Count == 0)
            {
                double num1 = Math.Atan2((data.controlBtn4.X - data.controlBtn1.X), (data.controlBtn4.Y - data.controlBtn1.Y));
                double num2 = 0.5;
                double num3 = 6.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point();
                point1.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 - num2)));
                point1.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 - num2)));
                point2.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 + num2)));
                point2.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 + num2)));
                point3.X = point1.X - (point1.X - point2.X) / 2;
                point3.Y = point1.Y - (point1.Y - point2.Y) / 2;
                Polyline myArr = new Polyline();

                //        //如果要繪製中心顏色，可開啟這段
                myArr.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 255));          
                //myArr.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                //myArr.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);              
                PointCollection Points = new PointCollection();

                Points.Add(data.controlBtn1);
                Points.Add(point3);
                Points.Add(point1);
                Points.Add(data.controlBtn4);
                Points.Add(point2);
                Points.Add(point3);


                myArr.Points = new PointCollection(Points);
                //  myArr.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myArr.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myArr.VerticalAlignment = VerticalAlignment.Center;
                myArr.StrokeThickness = shapeLib.Data.strokeT;
                myArr.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myArr.MouseEnter += data.myLine_MouseEnter;
                myArr.MouseLeave += data.myLine_MouseLeave;    
                //   myLine.MouseEnter += data.myLine_MouseEnter;
                //   myLine.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myArr);
                // currPath.setDrawShape( myArr);
                gv.controlShape.Add(myArr);

            }

            else
            {       
                Polyline myArr = (Polyline)gv.controlShape[0];// =(Line) currPath.getDrawShape();
                PointCollection Points = new PointCollection();
                double num1 = Math.Atan2((data.controlBtn4.X - data.controlBtn1.X), (data.controlBtn4.Y - data.controlBtn1.Y));
                double num2 = 0.5;
                double num3 = 6.0;
                Point point1 = new Point();
                Point point2 = new Point();
                Point point3 = new Point();
                point1.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 - num2)));
                point1.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 - num2)));
                point2.X = data.controlBtn4.X - ((num3 * Math.Sin(num1 + num2)));
                point2.Y = data.controlBtn4.Y - ((num3 * Math.Cos(num1 + num2)));
                point3.X = point1.X - (point1.X - point2.X) / 2;
                point3.Y = point1.Y - (point1.Y - point2.Y) / 2;

                Points.Add(data.controlBtn1);
                Points.Add(point3);
                Points.Add(point1);
                Points.Add(data.controlBtn4);
                Points.Add(point2);
                Points.Add(point3);
                myArr.Points = new PointCollection(Points);
            }

        }
    }
}