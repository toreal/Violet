using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ShapeLib.VShape
{
    class eraser : ShapePencil
    {
        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Pen";

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.eraser.png");
            ui.image = new System.Drawing.Bitmap(myStream);

            ui.belong = "Tools";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }

        double B0(double u)
        {
            return (1 - u) * (1 - u) * (1 - u) / 6;
        }

        double B1(double u)
        {
            return (3 * u * u * u - 6 * u * u + 4) / 6;
        }
        double B2(double u)
        {
            return (-3 * u * u * u + 3 * u * u + 3 * u + 1) / 6;
        }
        double B3(double u)
        {
            return u * u * u / 6;
        }

        //public void curve_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (shapeLib.Data.mClick >= 3)
        //    {
        //        shapeLib.Data.mClick = 0;
        //    }
        //    else if (shapeLib.Data.mClick == 0)
        //    {

        //    }

        //}

        Point[] myarr = new Point[6];

        int m = 6, extra, count = 0;
        int MAX_STEPS = 1000;


        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {

            if (count == 0)
            {
                myarr[3] = data.controlBtn1;
                Path myPath = new System.Windows.Shapes.Path();
                PathGeometry geometry = (PathGeometry)myPath.Data;
                shapeLib.Data.mygrid.Children.Add(myPath);
                gv.baseShape.Add(myPath);
                count++;
            }
            else
            {


                if (bfirst)
                {
                    shapeLib.Data.Status = "rest";
                    shapeLib.Data.bfirst = false;
                    BezierSegment bezier = new BezierSegment();
                    PathFigure figure = new PathFigure();



                    for (int a = 0; a < 4; a++)
                    {
                        if ((3 - a) > 0)
                        {
                            myarr[2 - a] = myarr[3 - a];
                        }
                        else
                            myarr[a] = data.controlBtn1;

                    }

                    for (int i = 0; i < m - 3; i++)
                    {
                        if (i == m - 4)
                        {
                            extra = 1;
                        }
                        else
                            extra = 0;
                        for (int j = 0; j < MAX_STEPS + extra; j++)
                        {

                            double u = j / MAX_STEPS;
                            double Qx = B0(u) * myarr[i].X +
                                    B1(u) * myarr[i + 1].X +
                                    B2(u) * myarr[i + 2].X +
                                    B3(u) * myarr[i + 3].X;

                            double Qy = B0(u) * myarr[i].Y +
                                    B1(u) * myarr[i + 1].Y +
                                    B2(u) * myarr[i + 2].Y +
                                    B3(u) * myarr[i + 3].Y;

                            myarr[i].X = Qx;
                            myarr[i].Y = Qy;

                        }

                    }
                    //figure.StartPoint = myarr[0];
                    //bezier.Point1 = myarr[1];
                    //bezier.Point2 = data.controlBtn4;
                    //bezier.Point3 = myarr[3];

                    figure.Segments.Add(bezier);
                    PathGeometry geometry = new PathGeometry();
                    geometry.Figures.Add(figure);

                    //myPath.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(curve_MouseLeftButtonDown);
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    // Describes the brush's color using RGB values. 
                    // Each value has a range of 0-255.

                    Path myPath = new System.Windows.Shapes.Path();
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
                    if (geometry != null)
                    {
                        BezierSegment bs = (BezierSegment)geometry.Figures[0].Segments[0];

                        geometry.Figures[0].StartPoint = myarr[0];
                        bs.Point1 = myarr[1];
                        bs.Point2 = data.controlBtn4;
                        bs.Point3 = myarr[3];
                    }


                }

            }

        }
    }

    //        public override void DisplayControlPoints(gView gv, gPath data)
    //        {
    //            if (gv.controlShape.Count == 0)
    //            {
    //                BezierSegment bezier = new BezierSegment();
    //                //bezier.Point3 = data.controlBtn3;
    //                PathFigure figure = new PathFigure();
    //                if (myarr[0] == myarr[1] && myarr[1] == myarr[2] && myarr[2] == myarr[3])
    //                {

    //                    myarr[0] = data.controlBtn1;
    //                    myarr[1] = data.controlBtn1;
    //                    myarr[2] = data.controlBtn4;
    //                    myarr[3] = data.controlBtn4;
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < 4; i++)
    //                    {
    //                        if ((3 - i) > 0)
    //                        {
    //                            myarr[2 - i] = myarr[3 - i];
    //                        }
    //                        else
    //                            myarr[i] = data.controlBtn1;

    //                    }
    //                    figure.StartPoint = myarr[0];
    //                    bezier.Point1 = myarr[1];
    //                    bezier.Point2 = data.controlBtn4;
    //                    bezier.Point3 = myarr[3];
    //                }  
    //                //figure.StartPoint = data.controlBtn1;
    //                //bezier.Point1 = figure.StartPoint;
    //                //bezier.Point2 = bezier.Point3;
    //                figure.Segments.Add(bezier);
    //                PathGeometry geometry = new PathGeometry();
    //                geometry.Figures.Add(figure);
    //                Path myPath = new System.Windows.Shapes.Path();
    //                myPath.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 255));
    //                myPath.StrokeThickness = data.state.strokeT;
    //                /* myPath.MouseLeftButtonDown += data.myLine_MouseLeftButtonDown;
    //                 myPath.MouseEnter += data.myLine_MouseEnter;
    //                 myPath.MouseLeave += data.myLine_MouseLeave;  */
    //                myPath.Data = geometry;
    //                shapeLib.Data.mygrid.Children.Add(myPath);
    //                gv.controlShape.Add(myPath);

    //            }

    //            else
    //            {
    //                Path myPath = (Path)gv.controlShape[0];// =(Line) currPath.getDrawShape();
    //                PathGeometry geometry = (PathGeometry)myPath.Data;
    //                geometry.Figures[0].StartPoint = data.controlBtn4;
    //                BezierSegment bs = (BezierSegment)geometry.Figures[0].Segments[0];
    //                //bs.Point1 = data.controlBtn2;
    //                //bs.Point2 = data.controlBtn3;
    //                //bs.Point3 = data.controlBtn4;
    //                bs.Point1 = myarr[1];
    //                bs.Point2 = data.controlBtn4;
    //                bs.Point3 = myarr[3]; 
    //            }

    //        }
}

