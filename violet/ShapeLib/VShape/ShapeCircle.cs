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
    public class ShapeCircle : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Circle";


            ui.image = new System.Drawing.Bitmap(ui.codebase+@"icons\circle.png");
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
                Ellipse  myEllipse = new Ellipse();
                // Create a SolidColorBrush with a red color to fill the 
                // Ellipse with.
              
                myEllipse.Stroke = new SolidColorBrush(Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                // Set the width and height of the Ellipse.
                myEllipse.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myEllipse.Height = Math.Abs(data.controlBtn4.Y- data.controlBtn1.Y);
                myEllipse.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
                myEllipse.StrokeThickness = shapeLib.Data.strokeT;
                myEllipse.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myEllipse.MouseEnter += data.myLine_MouseEnter;
                myEllipse.MouseLeave += data.myLine_MouseLeave; 
                shapeLib.Data.mygrid.Children.Add(myEllipse);
                gv.baseShape.Add(myEllipse);
            }
            else
            {
                Ellipse myEllipse = (Ellipse)gv.baseShape[0];
                myEllipse.Width = Math.Abs(data.controlBtn4.X - data.controlBtn1.X);
                myEllipse.Height = Math.Abs(data.controlBtn4.Y - data.controlBtn1.Y);
                myEllipse.Margin = new Thickness(data.controlBtn1.X, data.controlBtn1.Y, 0, 0);
            }
        }

        
    }
}
