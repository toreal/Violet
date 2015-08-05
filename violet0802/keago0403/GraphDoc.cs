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

        public ArrayList Properties = new ArrayList();


        public int  selIndex =-1;

       


    }

    public class gPro
    {

        public byte colorR = 0;
        public byte colorG = 0;
        public byte colorB = 0;
        public int strokeT = 1;


    }

    public class gPath
    {
        public int drawtype;
        public int x1;
        public int y1;

        public int x2;
        public int y2;

    }


}
