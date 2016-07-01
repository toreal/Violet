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
    class ShaperightTriangle : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Right Triangle";

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.right-triangle.png");
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

                Polyline myTri = new Polyline();

                //        //如果要繪製中心顏色，可開啟這段
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                myTri.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));               
                PointCollection Points = new PointCollection();
                Points.Add(data.controlBtn1);
                Points.Add(data.controlBtn3);
                Points.Add(data.controlBtn4);
                Points.Add(data.controlBtn1);
                

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
                Points.Add(data.controlBtn1);
                Points.Add(data.controlBtn3);
                Points.Add(data.controlBtn4);
                Points.Add(data.controlBtn1);
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
                myTri.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 255));          
                PointCollection Points = new PointCollection();
                Points.Add(data.controlBtn1);
                Points.Add (data.controlBtn3);
                Points.Add(data.controlBtn4);
                Points.Add(data.controlBtn1);
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
                Points.Add(data.controlBtn1);
                Points.Add(data.controlBtn3);
                Points.Add(data.controlBtn4);
                Points.Add(data.controlBtn1);

                myTri.Points = new PointCollection(Points);
            }
        
        }
    }
}