using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;
using System;
using System.Collections;
using System.Collections.Generic;
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
       public  SVGRoot sroot = new SVGRoot();
       public List<gPath> FullList = new List<gPath>();
        public Stack FullStack = new Stack();
        public Stack TempStack = new Stack();
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
                //sroot.PathList.RemoveAt(Data.ListPlace);
                //sroot.PathList.Insert(Data.ListPlace, FullList[FullList.Count - 1]);
            }
        }
        

        /*public Boolean checkBtn(System.Windows.Point point, int sel) // check if there has same place
        {
            gPath p = (gPath)sroot.PathList[sel];
            if (point.X == p.controlBtn1.X || point.X == p.controlBtn2.X || point.X == p.controlBtn3.X || point.X == p.controlBtn4.X)
            {
                return false;
            }
            if (point.Y == p.controlBtn1.Y || point.Y == p.controlBtn2.Y || point.Y == p.controlBtn3.Y || point.Y == p.controlBtn4.Y)
            {
                return false;
            }
            return true;
        }*/

        public RUse checkOut(System.Windows.Point downPlace) //check for the place you mouseDown have object
        {
            RUse r = new RUse();
            for (int i = 0; i < sroot.PathList.Count; i++)
            {
                gPath p = (gPath)sroot.PathList[i];
                if ((downPlace.X >= p.controlBtn1.X - 3) && (downPlace.X <= p.controlBtn1.X + 3) && (downPlace.Y >= p.controlBtn1.Y - 3) && (downPlace.Y <= p.controlBtn1.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 0;
                    r.Point = p.controlBtn4;
                    break;
                }
                if ((downPlace.X >= p.controlBtn2.X - 3) && (downPlace.X <= p.controlBtn2.X + 3) && (downPlace.Y >= p.controlBtn2.Y - 3) && (downPlace.Y <= p.controlBtn2.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 1;
                    r.Point = p.controlBtn3;
                    break;
                }
                if ((downPlace.X >= p.controlBtn3.X - 3) && (downPlace.X <= p.controlBtn3.X + 3) && (downPlace.Y >= p.controlBtn3.Y - 3) && (downPlace.Y <= p.controlBtn3.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 2;
                    r.Point = p.controlBtn2;
                    break;
                }
                if ((downPlace.X >= p.controlBtn4.X - 3) && (downPlace.X <= p.controlBtn4.X + 3) && (downPlace.Y >= p.controlBtn4.Y - 3) && (downPlace.Y <= p.controlBtn4.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 3;
                    r.Point = p.controlBtn1;
                    break;
                }
            }
            return r;
        }

        public void addContent(Document doc)
        {

            object missing = Type.Missing;

            ContentControl cc =doc.ContentControls.Add(WdContentControlType.wdContentControlPicture,
                                                                       ref missing);

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


        public void copyVal(gPath obj)
        {

            drawtype = obj.drawtype;
            state = obj.state;
            ListPlace = obj.ListPlace;

            controlBtn1 = obj.controlBtn1;
            controlBtn2 = obj.controlBtn2;
            controlBtn3 = obj.controlBtn3;
            controlBtn4 = obj.controlBtn4;


        }

    }

    public class RUse
    {
        public int Sel = -1;
        public int Node = -1;
        public System.Windows.Point Point;
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
        /*public void copyPA(pointAry pa)
        {
            changeP = pa.changeP;
            lastP = pa.lastP;
            leastP = pa.leastP;
        }*/
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

}