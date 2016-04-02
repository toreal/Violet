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
    class ShapeRectangle : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Rectangle";


            ui.image = new System.Drawing.Bitmap(@"icons\rectangle.png");
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

                Rectangle myRect = new Rectangle();
              
                //        //如果要繪製中心顏色，可開啟這段
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = Color.FromArgb(1, data.state.colorR, data.state.colorG, data.state.colorB);
                        myRect.Fill = mySolidColorBrush;
                        myRect.StrokeThickness = data.state.strokeT;
                //        myRect.Stroke = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                //        myRect.Width = Math.Abs(xEnd - xStart);
                //        myRect.Height = Math.Abs(yEnd - yStart);
                //        myRect.Margin = new Thickness(xStart, yStart, 0, 0);
                //        myRect.MouseLeftButtonDown += myRect_MouseLeftButtonDown;
                //        myRect.MouseEnter += Shapes_MouseEnter_Hands;
                //        myRect.MouseLeave += Shapes_MouseLeave;

                //        mygrid.Children.Add(myRect);


                myRect.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                //myLine.X1 = data.controlBtn1.X;
                //myLine.Y1 = data.controlBtn1.Y;
                //myLine.X2 = data.controlBtn4.X;
                //myLine.Y2 = data.controlBtn4.Y;
                myRect.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myRect.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myRect.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);

                myRect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myRect.VerticalAlignment = VerticalAlignment.Center;
                myRect.StrokeThickness = shapeLib.Data.strokeT;
                myRect.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
              //  myRect.MouseEnter += data.myLine_MouseEnter;
               // myRect.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myRect);
                gv.baseShape.Add(myRect);
                // currPath.setDrawShape( myLine);

            }
            else
            {
                Rectangle myRect = (Rectangle)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                myRect.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myRect.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myRect.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
 
            }


            //   throw new NotImplementedException();
        }



    }
}