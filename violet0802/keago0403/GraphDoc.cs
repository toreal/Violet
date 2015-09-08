using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keago0403
{
    public class GraphDoc
    {
        public ArrayList PathList = new ArrayList();

        public int  selIndex =-1;
        public int node=0;
        public int mx;
        public int my;
        public bool bmove;
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
        public int x1;
        public int y1;

        public int x2;
        public int y2;

        public gPro state;
    }
}
