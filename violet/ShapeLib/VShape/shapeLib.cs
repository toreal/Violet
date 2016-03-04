using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace violet.VShape
{
    static public class shapeLib
    {
        /// <summary>
        /// define supported shape
        /// </summary>
        /// <returns></returns>
        static public IList<ShapeObj> SupportedShape(Form myview)
        {
            if (myview != null)
            Data.view = myview;
         
           
            return ret;
        }

        static public void initControl(Canvas grid ,Canvas control)
        {
            Data.mygrid = grid;
            Data.myControl = control;
        }

        static public  GModel Data = new GModel();
        static IList<ShapeObj> ret = new[] { new ShapeObj() , new ShapeCircle()};

        static public void undo()
        {

        }

        static public void redo()
        {

        }

    }

    public class GModel
    {
        public Form view;
        public int lineSpace = 9;
        public int drawtype = 1;
        public String colortype = "black";
        public GraphDoc gdc = new GraphDoc();
        public Canvas mygrid;
        public Canvas myControl;
        public checkHitDraw chd = new checkHitDraw();
        public RUse ru = new RUse();
        public gPoint gp;
     //   public gPath tempFPath;
        public byte colorR = 0;
        public byte colorG = 0;
        public byte colorB = 0;
        public int strokeT = 3;


        public String Status = "rest"; //繪製曲線時的狀態
        public Point pStart, pEnd; //滑鼠起點和終點
        public Point tempStart;

        public bool bfirst = true; //是否為繪製新圖形
        public bool bCanMove = false; //繪製時,滑鼠是否可以移動
        public bool bhave = false; //you have choose
        public bool gCanMove = false; //選取後是否可以移動
        public bool bConThing = false; //是否有選取物件
        public bool OnIt = false; //是否有滑入或滑出選取物件

        public gPath currShape;
    }
}
