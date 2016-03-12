using Microsoft.Office.Interop.Word;
using ShapeLib.VShape;
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



namespace violet
{

    //[XmlRoot(ElementName = "SVGRoot", Namespace = "")]
    //public class SVGRoot
    //{
    //     [XmlElement("PathList")]
    //    public List<gPath> PathList = new List<gPath>();
    //}
    /// <summary>
    /// 記錄shape list,action data
    /// stack 記錄動作,每個動作(pointAry)包含,該圖是圖形的第幾個(Listplace),之前記錄是否己有相同圖是第幾個,目前圖存在記錄的第幾個
    ///
    /// </summary>
    public class GraphDoc
    {
       public SVGRoot sroot = new SVGRoot();
       public List<gPath> FullList = new List<gPath>(); //remember all action from grid
        public Stack UndoStack = new Stack();
        public Stack RedoStack = new Stack();
        public int selIndex = -1; // The last seat in PathList array
        public int node = 0;
        public int mx;
        public int my;
        public bool bmove;

        public void writeIn(gPath Data, int Action)
        {
            pointAry pa;
            if (Action == 0)
            {
                gPath g = new gPath();
                FullList.Add(Data);
                pa = new pointAry(Data.ListPlace, -1, (FullList.Count - 1));
                UndoStack.Push(pa);
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
                UndoStack.Push(pa);
            }
        }

        public int checkWhich(gPath gp)
        {
            int whichOne = -1;
            for (int i = sroot.PathList.Count - 1; i >= 0; i--)
            {
                if (gp.drawtype < 3 || gp.drawtype == 4)
                {
                    if (sroot.PathList[i].controlBtn1 != gp.controlBtn1)
                        continue;
                    if (sroot.PathList[i].controlBtn2 != gp.controlBtn2)
                        continue;
                    if (sroot.PathList[i].controlBtn3 != gp.controlBtn3)
                        continue;
                    if (sroot.PathList[i].controlBtn4 != gp.controlBtn4)
                        continue;
                }
                if (gp.drawtype == 3)
                {
                    if (sroot.PathList[i].controlBtn1 != gp.controlBtn1)
                        continue;
                    if (sroot.PathList[i].controlBtn4 != gp.controlBtn4)
                        continue;
                }
                whichOne = i;
            }
            return whichOne;
        }
      
        /// <summary>
        /// 
        /// </summary>
        public void unDo()
        {
            if (RedoStack.Count > 0)
            {
                gPath tempPath = new gPath();
                pointAry tempPA = new pointAry();

                tempPA = (pointAry)RedoStack.Pop();

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
                UndoStack.Push(tempPA);
            }
        }

        public void reDo()
        {
            if (UndoStack.Count > 0)
            {
                gPath tempPath = new gPath();
                pointAry tempPA = new pointAry();

                tempPA = (pointAry)UndoStack.Pop();

                if (tempPA.lastPlace() >= 0)
                {
                    tempPath.copyVal(FullList[tempPA.lastPlace()]);
                    sroot.PathList[tempPA.changePlace()] = tempPath;
                }
                else
                {
                    sroot.PathList.RemoveAt(tempPA.changePlace());
                }
                RedoStack.Push(tempPA);
            }
        }

        public void Release()
        {
            this.RedoStack.Clear();
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
    //[Serializable]
    //public class gPath
    //{
    //    public int drawtype;
    //    public gPro state;
    //    public int ListPlace;
    //    public System.Windows.Point controlBtn1;
    //    public System.Windows.Point controlBtn2;
    //    public System.Windows.Point controlBtn3;
    //    public System.Windows.Point controlBtn4;

    //    public void copyVal(gPath obj)
    //    {

    //        drawtype = obj.drawtype;
    //        state = obj.state;
    //        ListPlace = obj.ListPlace;

    //        controlBtn1 = obj.controlBtn1;
    //        controlBtn2 = obj.controlBtn2;
    //        controlBtn3 = obj.controlBtn3;
    //        controlBtn4 = obj.controlBtn4;
    //    }

    //}

    public class RUse
    {
        public int Sel = -1;
        public int Node = -1;
        public System.Windows.Point Point;
    }
    //當mouse 點選之後,要查看mouse 是點選到那一個物件,所以把點選到的shape 的boundary 找出來依其boundary  去比對
    //但新的作法是否仍有需要呢?

    public class gPoint{
        public System.Windows.Point mouseXY;

        public System.Windows.Point point0;
        public System.Windows.Point point1;
        public System.Windows.Point point2;
        public System.Windows.Point point3;
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
                if (drawType < 3 || drawType == 4)
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

        public bool checkHitCurve(String Data, gPath p)
        {
            bool tf = true;
            String[] tmpStr = Data.Split(',');
            double[] tmpDouStr = new double[tmpStr.Length];
            for (int i = 0; i < tmpStr.Length; i++)
            {
                tmpDouStr[i] = Convert.ToDouble(tmpStr[i]);
            }
            System.Windows.Point tmpPoint0 = new System.Windows.Point(tmpDouStr[0], tmpDouStr[1]);
            System.Windows.Point tmpPoint1 = new System.Windows.Point(tmpDouStr[2], tmpDouStr[3]);
            System.Windows.Point tmpPoint2 = new System.Windows.Point(tmpDouStr[4], tmpDouStr[5]);
            System.Windows.Point tmpPoint3 = new System.Windows.Point(tmpDouStr[6], tmpDouStr[7]);

            if (!tmpPoint0.Equals(p.controlBtn1))
                tf = false;
            if (!tmpPoint1.Equals(p.controlBtn2))
                tf = false;
            if (!tmpPoint2.Equals(p.controlBtn3))
                tf = false;
            if (!tmpPoint3.Equals(p.controlBtn4))
                tf = false;
            return tf;
        }

        public int checkHitCorner(System.Windows.Point downPlace, gPath p)
        {
            int Node = -1;
            if ((downPlace.X >= p.controlBtn1.X - 4) && (downPlace.X <= p.controlBtn1.X + 4) && (downPlace.Y >= p.controlBtn1.Y - 4) && (downPlace.Y <= p.controlBtn1.Y + 4))
            {
                Node = 0;
            }
            if ((downPlace.X >= p.controlBtn2.X - 4) && (downPlace.X <= p.controlBtn2.X + 4) && (downPlace.Y >= p.controlBtn2.Y - 4) && (downPlace.Y <= p.controlBtn2.Y + 4))
            {
                Node = 1;
            }
            if ((downPlace.X >= p.controlBtn3.X - 4) && (downPlace.X <= p.controlBtn3.X + 4) && (downPlace.Y >= p.controlBtn3.Y - 4) && (downPlace.Y <= p.controlBtn3.Y + 4))
            {
                Node = 2;
            }
            if ((downPlace.X >= p.controlBtn4.X - 4) && (downPlace.X <= p.controlBtn4.X + 4) && (downPlace.Y >= p.controlBtn4.Y - 4) && (downPlace.Y <= p.controlBtn4.Y + 4))
            {
                Node = 3;
            }
            return Node;
        }

        public bool checkHitCenter(System.Windows.Point downPlace, gPath p)
        {
            bool tf = true;
            if (downPlace.X > p.controlBtn2.X || downPlace.X < p.controlBtn1.X)
                tf = false;
            if (downPlace.Y > p.controlBtn4.Y || downPlace.Y < p.controlBtn1.Y)
                tf = false;
            return tf;
        }
    }

}