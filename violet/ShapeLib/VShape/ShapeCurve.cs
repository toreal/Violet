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
    class ShapeCurve : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Curve";

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.curve.png");
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
                BezierSegment bezier = new BezierSegment();
                bezier.Point3 = data.controlBtn3;
                PathFigure figure = new PathFigure();
                figure.StartPoint = data.controlBtn1;
                bezier.Point1 = figure.StartPoint;
                bezier.Point2 = bezier.Point3;
                figure.Segments.Add(bezier);
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(figure);
                Path myPath = new System.Windows.Shapes.Path();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.

                myPath.Stroke = new SolidColorBrush(Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                myPath.StrokeThickness = data.state.strokeT;
                /*myPath.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                myPath.MouseEnter += data.myLine_MouseEnter;
                myPath.MouseLeave += data.myLine_MouseLeave; */
                myPath.Data = geometry;
                shapeLib.Data.mygrid.Children.Add(myPath);
                gv.baseShape.Add(myPath);
            }
            else
            {
                Path myPath = (Path)gv.baseShape[0];// =(Line) currPath.getDrawShape();
                PathGeometry geometry = (PathGeometry)myPath.Data;
                geometry.Figures[0].StartPoint = data.controlBtn1;
                BezierSegment bs = (BezierSegment)geometry.Figures[0].Segments[0];
                bs.Point1 = data.controlBtn2;
                bs.Point2 = data.controlBtn3;
                bs.Point3 = data.controlBtn4;
            }
        }

        public override void DisplayControlPoints(gView gv, gPath data)
        {
            if (gv.controlShape.Count == 0)
            {
                BezierSegment bezier = new BezierSegment();
                bezier.Point3 = data.controlBtn3;
                PathFigure figure = new PathFigure();
                figure.StartPoint = data.controlBtn1;
                bezier.Point1 = figure.StartPoint;
                bezier.Point2 = bezier.Point3;
                figure.Segments.Add(bezier);
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(figure);
                Path myPath = new System.Windows.Shapes.Path();
                myPath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 255));
                myPath.StrokeThickness = data.state.strokeT;
                /* myPath.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
                 myPath.MouseEnter += data.myLine_MouseEnter;
                 myPath.MouseLeave += data.myLine_MouseLeave;  */
                myPath.Data = geometry;
                shapeLib.Data.mygrid.Children.Add(myPath);
                gv.controlShape.Add(myPath);

            }

            else
            {
                Path myPath = (Path)gv.controlShape[0];// =(Line) currPath.getDrawShape();
                PathGeometry geometry = (PathGeometry)myPath.Data;
                geometry.Figures[0].StartPoint = data.controlBtn1;
                BezierSegment bs = (BezierSegment)geometry.Figures[0].Segments[0];
                bs.Point1 = data.controlBtn2;
                bs.Point2 = data.controlBtn3;
                bs.Point3 = data.controlBtn4;
            }

        }
    }
}