using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;


namespace keago0403
{

    [XmlRoot(ElementName = "SVGRoot", Namespace = "")]
    public class SVGRoot
    {
         [XmlElement("PathList")]
        public List<gPath> PathList = new List<gPath>();
    }
    
    public class GraphDoc
    {
       public SVGRoot sroot = new SVGRoot();
       public List<gPath> FullList = new List<gPath>();
        public Stack FullStack = new Stack();
        public Stack TempStack = new Stack();
        public int selIndex = -1; // The last seat in PathList array
        public int node = 0;
        public int mx;
        public int my;
        public bool bmove;
        private int maskNum = -1;

        public void writeIn(gPath Data, int Action)
        {
            pointAry pa;
            if (Action == 0)
            {
                gPath g = new gPath();
                FullList.Add(Data);
                pa = new pointAry(Data.ListPlace, -1, (FullList.Count - 1));
                FullStack.Push(pa);
                g.copyVal(Data);
                sroot.PathList.Add(g);
            }
            else if (Action == 1)
            {
                int temp = 0;
                for (int i = FullList.Count - 1; i >= 0; i--)
                {
                    if (FullList[i].ListPlace == Data.ListPlace)
                    {
                        temp = i;
                        break;
                    }
                }
                
                FullList.Add(Data);
                pa = new pointAry(Data.ListPlace, temp, (FullList.Count - 1));
                FullStack.Push(pa);
            }
        }

        private int checkCorner(System.Windows.Point downPlace, gPath p)//check if you click on the corner you choose
        {
            int Node = -1;
            if ((downPlace.X >= p.controlBtn1.X - 3) && (downPlace.X <= p.controlBtn1.X + 3) && (downPlace.Y >= p.controlBtn1.Y - 3) && (downPlace.Y <= p.controlBtn1.Y + 3))
            {
                Node = 0;
            }
            if ((downPlace.X >= p.controlBtn2.X - 3) && (downPlace.X <= p.controlBtn2.X + 3) && (downPlace.Y >= p.controlBtn2.Y - 3) && (downPlace.Y <= p.controlBtn2.Y + 3))
            {
                Node = 1;
            }
            if ((downPlace.X >= p.controlBtn3.X - 3) && (downPlace.X <= p.controlBtn3.X + 3) && (downPlace.Y >= p.controlBtn3.Y - 3) && (downPlace.Y <= p.controlBtn3.Y + 3))
            {
                Node = 2;
            }
            if ((downPlace.X >= p.controlBtn4.X - 3) && (downPlace.X <= p.controlBtn4.X + 3) && (downPlace.Y >= p.controlBtn4.Y - 3) && (downPlace.Y <= p.controlBtn4.Y + 3))
            {
                Node = 3;
            }
            return Node;
        }

        public RUse checkOut(System.Windows.Point downPlace, params System.Windows.Media.Geometry[] checkGeo) //check for the place you mouseDown have object
        {
            RUse r = new RUse();
            r.Sel = -1;
            int tempInt;
            for (int i = sroot.PathList.Count - 1; i >= 0; i--)
            {
                gPath p = (gPath)sroot.PathList[i];
                if (p.drawtype == 1)
                {
                    if (checkEllipse(downPlace, p))
                    {
                        r.Sel = i;
                        maskNum = i;
                        break;
                    }
                    if (i == maskNum)
                    {
                        if (maskRL(downPlace, p))
                        {
                            r.Sel = i;
                            tempInt = checkCorner(downPlace, p);
                            if (tempInt >= 0)
                            {
                                r.Node = tempInt;
                            }
                            else
                            {
                                r.Node = 4;
                            }
                            break;
                        }
                    }
                }
                if (p.drawtype == 2)
                {
                    if (checkRect(downPlace, p))
                    {
                        r.Sel = i;
                        tempInt = checkCorner(downPlace, p);
                        if (tempInt >= 0)
                        {
                            r.Node = tempInt;
                        }
                        maskNum = i;
                        break;
                    }
                    if( (i == maskNum) && maskRL(downPlace, p)){
                        r.Sel = i;
                        r.Node = 4;
                        break;
                    }
                }
                if (p.drawtype == 3)
                {
                    if (checkLine(downPlace, p))
                    {
                        r.Sel = i;
                        tempInt = checkCorner(downPlace, p);
                        if (tempInt >= 0)
                        {
                            r.Node = tempInt;
                        }
                        else if (i == maskNum)
                        {
                            r.Node = 4;
                        }
                        maskNum = i;
                        break;
                    }
                }
                if (p.drawtype == 4)
                {
                    
                    if (checkGeo.Length > 0)
                    {
                        System.Windows.Media.Geometry tempGeo = checkGeo[0];
                        if (checkCurve(downPlace, p, tempGeo))
                        {
                            r.Sel = i;
                            tempInt = checkCorner(downPlace, p);
                            if (tempInt >= 0)
                            {
                                r.Node = tempInt;
                            }
                            maskNum = i;
                            break;
                        }
                    }
                }
            }
            return r;
        }

        private bool maskRL(System.Windows.Point downPlace, gPath p)
        {
            bool tf = true;
            if (downPlace.X > p.controlBtn2.X + 3 || downPlace.X < p.controlBtn1.X - 3)
                tf = false;
            if (downPlace.Y > p.controlBtn4.Y + 3 || downPlace.Y < p.controlBtn1.Y - 3)
                tf = false;
            return tf;
        }

        public void clearMaskNum()
        {
            maskNum = -1;
        }

        private bool checkCurve(System.Windows.Point downPlace, gPath p, System.Windows.Media.Geometry Geo)
        {
            bool tf = false;
            if(p.geo == Geo)
                tf = true;
            return tf;
        }

        private bool checkEllipse(System.Windows.Point downPlace, gPath p)
        {
            //另類解法 - EGO
            /*double xR = Math.Abs(downPlace.X - center.X);
            double yR = Math.Abs(downPlace.Y - center.Y);
            double longToC = Math.Sqrt((Math.Pow((downPlace.X - center.X), 2)) + (Math.Pow((downPlace.Y - center.Y), 2)));

            double cos = (Math.Pow(xR, 2) + Math.Pow(longToC, 2) - Math.Pow(yR, 2)) / (2 * xR * longToC);
            double sin = yR / longToC;


            double sum = 0;

            double temp_x = center.X + xR * cos;
            double temp_y = center.Y + yR * sin;
            sum = Math.Sqrt((Math.Pow((downPlace.X - temp_x), 2)) + (Math.Pow((downPlace.Y - temp_y), 2)));
            System.Windows.Point Point = new System.Windows.Point(temp_x, temp_y);
            if (sum <= 3)
            {
                tf = true;
            }*/
            bool tf = false;
            double c_x = (p.controlBtn2.X - p.controlBtn1.X) / 2;
            double c_y = (p.controlBtn3.Y - p.controlBtn1.Y) / 2;
            System.Windows.Point center = new System.Windows.Point(p.controlBtn1.X + c_x, p.controlBtn1.Y + c_y);

            double simpleX = Math.Sqrt((1 - Math.Pow((downPlace.Y - center.Y), 2) / Math.Pow(c_y, 2)) * Math.Pow(c_x, 2));
            double simpleY = Math.Sqrt((1 - Math.Pow((downPlace.X - center.X), 2) / Math.Pow(c_x, 2)) * Math.Pow(c_y, 2));
            double higherPlaceX = center.X + simpleX;
            double lowerPlaceX = center.X - simpleX;
            double higherPlaceY = center.Y + simpleY;
            double lowerPlaceY = center.Y - simpleY;

            if (downPlace.X <= higherPlaceX + 3 && downPlace.X >= higherPlaceX - 3)
                tf = true;
            if (downPlace.X <= lowerPlaceX + 3 && downPlace.X >= lowerPlaceX - 3)
                tf = true;
            if (downPlace.Y <= higherPlaceY + 3 && downPlace.Y >= higherPlaceY - 3)
                tf = true;
            if (downPlace.Y <= lowerPlaceY + 3 && downPlace.Y >= lowerPlaceY - 3)
                tf = true;
            return tf;

        }

        private bool checkRect(System.Windows.Point downPlace, gPath p)
        {
            bool tf = false;
            if ((downPlace.X <= p.controlBtn2.X + 3 && downPlace.X >= p.controlBtn2.X - 3) || (downPlace.X >= p.controlBtn1.X - 3 && downPlace.X <= p.controlBtn1.X + 3))
            {
                if (downPlace.Y <= p.controlBtn3.Y && downPlace.Y >= p.controlBtn1.Y)
                {
                    tf = true;
                }
            }
            if ((downPlace.Y <= p.controlBtn3.Y + 3 && downPlace.Y >= p.controlBtn3.Y - 3) || (downPlace.Y >= p.controlBtn1.Y - 3 && downPlace.Y <= p.controlBtn1.Y + 3))
            {
                if (downPlace.X <= p.controlBtn2.X && downPlace.X >= p.controlBtn1.X)
                {
                    tf = true;
                }
            }
            return tf;
        }

        private bool checkLine(System.Windows.Point downPlace, gPath p)
        {
            bool tf = false;
            double m = (p.controlBtn4.Y - p.controlBtn1.Y) / (p.controlBtn4.X - p.controlBtn1.X);
            double xm = (downPlace.Y - p.controlBtn1.Y) / m + p.controlBtn1.X;
            double ym = (downPlace.X - p.controlBtn1.X) * m + p.controlBtn1.Y;
            if (downPlace.X >= xm - 3 && downPlace.X <= xm + 3)
                tf = true;
            if (downPlace.Y >= ym - 3 && downPlace.Y <= ym + 3)
                tf = true;
            return tf;
        }

        public void addContent(Document doc)
        {

            object missing = Type.Missing;

            ContentControl cc = doc.ContentControls.Add(WdContentControlType.wdContentControlPicture,ref missing);

        }

        public void unDo()
        {
            if (TempStack.Count > 0)
            {
                gPath tempPath = new gPath();
                pointAry tempPA = new pointAry();

                tempPA = (pointAry)TempStack.Pop();

                if (tempPA.leastPlace() >= 0)
                {
                    tempPath.copyVal(FullList[tempPA.leastPlace()]);
                    if ((sroot.PathList.Count - 1) < tempPA.changePlace())
                    {
                        sroot.PathList.Add(tempPath);
                    }
                    else
                    {
                        sroot.PathList[tempPA.changePlace()] = tempPath;
                    }
                }
                else
                {
                    sroot.PathList.RemoveAt(tempPA.changePlace());
                }
                FullStack.Push(tempPA);
            }
        }

        public void reDo()
        {
            if (FullStack.Count > 0)
            {
                gPath tempPath = new gPath();
                pointAry tempPA = new pointAry();

                tempPA = (pointAry)FullStack.Pop();

                if (tempPA.lastPlace() >= 0)
                {
                    tempPath.copyVal(FullList[tempPA.lastPlace()]);
                    sroot.PathList[tempPA.changePlace()] = tempPath;
                }
                else
                {
                    sroot.PathList.RemoveAt(tempPA.changePlace());
                }
                TempStack.Push(tempPA);
            }
        }

        public void Release()
        {
            this.TempStack.Clear();
        }
    }
    
    [Serializable]
    public struct gPro
    {
        public byte colorR ;
        public byte colorG ;
        public byte colorB ;
        public int strokeT ;
    }
    [Serializable]
    public class gPath
    {
        public int drawtype;
        public gPro state;
        public int ListPlace;
        public System.Windows.Point controlBtn1;
        public System.Windows.Point controlBtn2;
        public System.Windows.Point controlBtn3;
        public System.Windows.Point controlBtn4;
        public System.Windows.Media.Geometry geo;

        public void copyVal(gPath obj)
        {

            drawtype = obj.drawtype;
            state = obj.state;
            ListPlace = obj.ListPlace;

            controlBtn1 = obj.controlBtn1;
            controlBtn2 = obj.controlBtn2;
            controlBtn3 = obj.controlBtn3;
            controlBtn4 = obj.controlBtn4;
            geo = obj.geo;
        }

    }

    public class RUse
    {
        public int Sel = -1;
        public int Node = -1;
        public System.Windows.Point Point;
    }

    public class gPoint{
        public System.Windows.Point mouseXY;

        public System.Windows.Point point0;
        public System.Windows.Point point1;
        public System.Windows.Point point2;
        public System.Windows.Point point3;
        public System.Windows.Media.Geometry geo;
    }

    public class pointAry
    {
        private int leastP;
        private int lastP;
        private int changeP;
        public pointAry()
        {
            changeP = 0;
            lastP = -1;
            leastP = 0;

        }
        public pointAry(int a, int b, int c)
        {
            changeP = a;
            lastP = b;
            leastP = c;
        }
        public int changePlace()
        {
            return changeP;
        }
        public int lastPlace()
        {
            return lastP;
        }
        public int leastPlace()
        {
            return leastP;
        }
    }

    public class checkHitDraw
    {
        public int checkHitWhich(List<gPath> l, gPoint gp, int drawType)
        {
            int whichOne = -1;
            for (int i = l.Count - 1; i >= 0; i--)
            {
                if (drawType < 3)
                {
                    if (l[i].controlBtn1 != gp.point0)
                        continue;
                    if (l[i].controlBtn2 != gp.point1)
                        continue;
                    if (l[i].controlBtn3 != gp.point2)
                        continue;
                    if (l[i].controlBtn4 != gp.point3)
                        continue;
                }
                if (drawType == 3)
                {
                    if (l[i].drawtype != drawType)
                        continue;
                    if (l[i].controlBtn1 != gp.point0)
                        continue;
                    if (l[i].controlBtn4 != gp.point3)
                        continue;
                }
                if (drawType == 4)
                {
                    if (!l[i].geo.Bounds.Equals(gp.geo.Bounds))
                        continue;
                }
                whichOne = i;
            }
            return whichOne;
        }

        public bool checkHitEllipse(gPoint p)
        {
            bool tf = false;
            double c_x = (p.point1.X - p.point0.X) / 2;
            double c_y = (p.point2.Y - p.point0.Y) / 2;
            System.Windows.Point center = new System.Windows.Point(p.point0.X + c_x, p.point0.Y + c_y);

            double simpleX = Math.Sqrt((1 - Math.Pow((p.mouseXY.Y - center.Y), 2) / Math.Pow(c_y, 2)) * Math.Pow(c_x, 2));
            double simpleY = Math.Sqrt((1 - Math.Pow((p.mouseXY.X - center.X), 2) / Math.Pow(c_x, 2)) * Math.Pow(c_y, 2));
            double higherPlaceX = center.X + simpleX;
            double lowerPlaceX = center.X - simpleX;
            double higherPlaceY = center.Y + simpleY;
            double lowerPlaceY = center.Y - simpleY;

            if (p.mouseXY.X <= higherPlaceX && p.mouseXY.X >= higherPlaceX - 3)
                tf = true;
            if (p.mouseXY.X >= lowerPlaceX && p.mouseXY.X <= lowerPlaceX + 3)
                tf = true;
            if (p.mouseXY.Y <= higherPlaceY && p.mouseXY.Y >= higherPlaceY - 3)
                tf = true;
            if (p.mouseXY.Y >= lowerPlaceY && p.mouseXY.Y <= lowerPlaceY + 3)
                tf = true;

            return tf;
        }

        public bool checkHitRect(gPoint p)
        {
            bool tf = false;
            if ((p.mouseXY.X <= p.point1.X + 3 && p.mouseXY.X >= p.point1.X - 3) || (p.mouseXY.X >= p.point0.X - 3 && p.mouseXY.X <= p.point0.X + 3))
            {
                if (p.mouseXY.Y <= p.point2.Y && p.mouseXY.Y >= p.point0.Y)
                {
                    tf = true;
                }
            }
            if ((p.mouseXY.Y <= p.point2.Y + 3 && p.mouseXY.Y >= p.point2.Y - 3) || (p.mouseXY.Y >= p.point0.Y - 3 && p.mouseXY.Y <= p.point0.Y + 3))
            {
                if (p.mouseXY.X <= p.point1.X && p.mouseXY.X >= p.point0.X)
                {
                    tf = true;
                }
            }
            return tf;
        }

        public bool checkHitLine(System.Windows.Point downPlace, gPath p)
        {
            bool tf = false;
            double m = (p.controlBtn4.Y - p.controlBtn1.Y) / (p.controlBtn4.X - p.controlBtn1.X);
            double xm = (downPlace.Y - p.controlBtn1.Y) / m + p.controlBtn1.X;
            double ym = (downPlace.X - p.controlBtn1.X) * m + p.controlBtn1.Y;
            if (downPlace.X >= xm - 3 && downPlace.X <= xm + 3)
                tf = true;
            if (downPlace.Y >= ym - 3 && downPlace.Y <= ym + 3)
                tf = true;
            return tf;
        }

        public bool checkHitCurve(System.Windows.Media.Geometry geo, gPath p)
        {
            bool tf = false;
            System.Windows.Media.Geometry tmpGeo = p.geo;
            if (geo.Bounds.Equals(tmpGeo.Bounds))
                tf = true;
            return tf;
        }

        public int checkHitCorner(System.Windows.Point downPlace, gPath p)
        {
            int Node = -1;
            if ((downPlace.X >= p.controlBtn1.X - 3) && (downPlace.X <= p.controlBtn1.X + 3) && (downPlace.Y >= p.controlBtn1.Y - 3) && (downPlace.Y <= p.controlBtn1.Y + 3))
            {
                Node = 0;
            }
            if ((downPlace.X >= p.controlBtn2.X - 3) && (downPlace.X <= p.controlBtn2.X + 3) && (downPlace.Y >= p.controlBtn2.Y - 3) && (downPlace.Y <= p.controlBtn2.Y + 3))
            {
                Node = 1;
            }
            if ((downPlace.X >= p.controlBtn3.X - 3) && (downPlace.X <= p.controlBtn3.X + 3) && (downPlace.Y >= p.controlBtn3.Y - 3) && (downPlace.Y <= p.controlBtn3.Y + 3))
            {
                Node = 2;
            }
            if ((downPlace.X >= p.controlBtn4.X - 3) && (downPlace.X <= p.controlBtn4.X + 3) && (downPlace.Y >= p.controlBtn4.Y - 3) && (downPlace.Y <= p.controlBtn4.Y + 3))
            {
                Node = 3;
            }
            return Node;
        }

        public bool checkHitCenter(System.Windows.Point downPlace, gPath p)
        {
            bool tf = true;
            if (downPlace.X > p.controlBtn2.X + 3 || downPlace.X < p.controlBtn1.X - 3)
                tf = false;
            if (downPlace.Y > p.controlBtn4.Y + 3 || downPlace.Y < p.controlBtn1.Y - 3)
                tf = false;
            return tf;
        }
    }

}