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
        public int selIndex = -1; // The last seat in PathList array
        public int node = 0;
        public int mx;
        public int my;
        public bool bmove;

        public void writeIn(gPath Data, int Action)
        {
            pointAry pa = new pointAry();

            gPath path = new gPath();
            path.state.colorR = Data.state.colorR;
            path.state.colorG = Data.state.colorG;
            path.state.colorB = Data.state.colorB;
            path.state.strokeT = Data.state.strokeT;
            path.drawtype = Data.drawtype;
            path.controlBtn1 = Data.controlBtn1;
            path.controlBtn2 = Data.controlBtn2;
            path.controlBtn3 = Data.controlBtn3;
            path.controlBtn4 = Data.controlBtn4;

            if (Action == 0)
            {
                FullList.Add(path);
                pa = new pointAry(Data.ListPlace, -1, (FullList.Count - 1));
                FullStack.Push(pa);
                sroot.PathList.Add(path);
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
                FullList.Add(path);
                pa = new pointAry(Data.ListPlace, temp, (FullList.Count - 1));
                FullStack.Push(pa);
                //sroot.PathList.RemoveAt(Data.ListPlace);
                //sroot.PathList.Insert(Data.ListPlace, Data);
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
    }

}