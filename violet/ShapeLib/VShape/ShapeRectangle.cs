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

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.rectangle.png");
            ui.image = new System.Drawing.Bitmap(myStream);

            
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
              
                   //如果要繪製中心顏色，可開啟這段                     
                myRect.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));             
                myRect.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myRect.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myRect.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myRect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myRect.VerticalAlignment = VerticalAlignment.Center;
                myRect.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myRect.MouseEnter += data.myLine_MouseEnter;
                myRect.MouseLeave += data.myLine_MouseLeave;
                myRect.StrokeThickness = shapeLib.Data.strokeT;
                myRect.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                shapeLib.Data.mygrid.Children.Add(myRect);
                gv.baseShape.Add(myRect);
 

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

        public override  void DisplayControlPoints(gView gv, gPath data)
        {
            if (gv.controlShape.Count == 0)
            {
                Rectangle myRect = new Rectangle();
                myRect.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));                          
                myRect.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myRect.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myRect.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myRect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                myRect.VerticalAlignment = VerticalAlignment.Center;
                myRect.StrokeThickness = shapeLib.Data.strokeT;
                myRect.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myRect.MouseEnter += data.myLine_MouseEnter;
                myRect.MouseLeave += data.myLine_MouseLeave;
                shapeLib.Data.mygrid.Children.Add(myRect);
                gv.controlShape.Add(myRect);
               

            }
            else
            {
                Rectangle myRect = (Rectangle)gv.controlShape[0];
                myRect.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myRect.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myRect.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
 
            }
        }

    }
}