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


            ui.image = new System.Drawing.Bitmap(@"icons\curve.png");
            ui.belong = "Shapes";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }




        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            BezierSegment bezier ;
            
            if (bfirst)
            {
                    bfirst = false;
                    bezier = new BezierSegment();
                    bezier.Point3 = data.controlBtn3;
                    PathFigure figure = new PathFigure();
                    figure.StartPoint = data.controlBtn1;
                    bezier.Point1 = figure.StartPoint;
                    bezier.Point2 = bezier.Point3;
                    figure.Segments.Add(bezier);
                    PathGeometry geometry = new PathGeometry();
                    geometry.Figures.Add(figure);
                    Path  myPath = new System.Windows.Shapes.Path();
                    myPath.Stroke = new SolidColorBrush(Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                    myPath.StrokeThickness = data.state.strokeT;
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


    }
}