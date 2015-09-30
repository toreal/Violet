using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace keago0403
{
    public class GraphDoc
    {
        public ArrayList PathList = new ArrayList();
        public Stack FullStack = new Stack();
        public int listIndex = -1; // The last seat in FullList array
        public int selIndex = -1; // The last seat in PathList array
        public int node = 0;
        public int mx;
        public int my;
        public bool bmove;

        public Boolean checkBtn(Point point, int sel) // check if there has same place
        {
            gPath p = (gPath)PathList[sel];
            if (point.X == p.controlBtn1.X || point.X == p.controlBtn2.X || point.X == p.controlBtn3.X || point.X == p.controlBtn4.X)
            {
                return false;
            }
            if (point.Y == p.controlBtn1.Y || point.Y == p.controlBtn2.Y || point.Y == p.controlBtn3.Y || point.Y == p.controlBtn4.Y)
            {
                return false;
            }
            return true;
        }
        public void checkIn() //make sure the PathList is update
        {
            //PathList.Add(FullList[]);
        }
        public RUse checkOut(Point downPlace) //check for the place you mouseDown have object
        {
            RUse r = new RUse();
            for (int i = 0; i < PathList.Count; i++)
            {
                gPath p = (gPath)PathList[i];
                if ((downPlace.X >= p.controlBtn1.X - 3) && (downPlace.X <= p.controlBtn1.X + 3) && (downPlace.Y >= p.controlBtn1.Y - 3) && (downPlace.Y <= p.controlBtn1.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 0;
                    break;
                }
                if ((downPlace.X >= p.controlBtn2.X - 3) && (downPlace.X <= p.controlBtn2.X + 3) && (downPlace.Y >= p.controlBtn2.Y - 3) && (downPlace.Y <= p.controlBtn2.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 1;
                    break;
                }
                if ((downPlace.X >= p.controlBtn3.X - 3) && (downPlace.X <= p.controlBtn3.X + 3) && (downPlace.Y >= p.controlBtn3.Y - 3) && (downPlace.Y <= p.controlBtn3.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 2;
                    break;
                }
                if ((downPlace.X >= p.controlBtn4.X - 3) && (downPlace.X <= p.controlBtn4.X + 3) && (downPlace.Y >= p.controlBtn4.Y - 3) && (downPlace.Y <= p.controlBtn4.Y + 3))
                {
                    r.Sel = i;
                    r.Node = 3;
                    break;
                }
            }
            return r;
        }
    }
    public struct gPro
    {
        public byte colorR ;
        public byte colorG ;
        public byte colorB ;
        public int strokeT ;
    }

    public class gPath
    {
        public int drawtype;
        public gPro state;

        public int x1;
        public int y1;

        public int x2;
        public int y2;

        public Point controlBtn1;
        public Point controlBtn2;
        public Point controlBtn3;
        public Point controlBtn4;
    }

    public class RUse
    {
        public int Sel = -1;
        public int Node = -1;
    }
}