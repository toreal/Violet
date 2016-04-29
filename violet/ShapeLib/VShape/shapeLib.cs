using ShapeLib.VShape;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace ShapeLib.VShape
{
    
    static public class shapeLib
    {
        /// <summary>
        /// define supported shape
        /// </summary>
        /// <returns></returns>
        /// 
        
        static public IList<ShapeObj> SupportedShape(getForm myview)
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
        static IList<ShapeObj> ret = new[] { new ShapeObj(), new ShapeCircle(), new ShapeRectangle(), new ShapeCurve(), new ShapeTriangle(), new ShaperightTriangle(), new ShapeArrow(), new ShapeText(), new ShapePencil(), new eraser(), new Sproerty() };
        static public void InsertText(String txt)
        {
            if (shapeLib.Data.currShape != null)
            {
                shapeLib.Data.currShape.Text += txt;
                shapeLib.Data.currShape.redraw(1);

            }

        }
        static public void copy()
        {
         //   gPath tp = new gPath();

         //   tp.copyVal(shapeLib.Data.gdc.sroot.PathList[ru.Sel]);
            if (Data.multiSelList.Count > 0)
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer s = new XmlSerializer(typeof(List<gPath>));


                    s.Serialize(XmlWriter.Create(stream), Data.multiSelList);

                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    StreamReader sr = new StreamReader(stream);
                    string myStr = sr.ReadToEnd();

                    System.Windows.Clipboard.SetText(myStr);
                    shiftPos = 0;
                }
            }
        }
        
        static int shiftPos =0;
        static public void paste()
        {
            String str = System.Windows.Clipboard.GetText();

            shiftPos += 15;
            XmlSerializer serializer = new XmlSerializer(typeof(List<gPath>));
            using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(str)))
            {
                List<gPath> tpList = (List< gPath>) serializer.Deserialize(XmlReader.Create(ms));
                foreach (gPath tp in tpList)
                { 
                tp.ListPlace = shapeLib.Data.gdc.sroot.PathList.Count;
                if (shapeLib.Data.gdc.checkWhich(tp) != -1)
                {
                    tp.controlBtn1 = new Point(tp.controlBtn1.X + shiftPos, tp.controlBtn1.Y + shiftPos);
                    tp.controlBtn2 = new Point(tp.controlBtn2.X + shiftPos, tp.controlBtn2.Y + shiftPos);
                    tp.controlBtn3 = new Point(tp.controlBtn3.X + shiftPos, tp.controlBtn3.Y + shiftPos);
                    tp.controlBtn4 = new Point(tp.controlBtn4.X + shiftPos, tp.controlBtn4.Y + shiftPos);
                }
                tp.redraw(1);

                shapeLib.Data.gdc.writeIn(tp, 0);
                shapeLib.Data.gdc.Release();
                              }
                // reDraw(true);
            }

        }

    }

    public class GModel
    {
        public getForm view;
        public int lineSpace = 9;
        public int drawtype = 1;
        public String colortype = "black";
        public GraphDoc gdc = new GraphDoc();
        public Canvas mygrid;
        public Canvas myControl;
        
        public checkHitDraw chd = new checkHitDraw();
        public RUse ru = new RUse();
        public gPoint gp;
     //public System.Windows.Forms.Panel panel1;
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
        public int mClick = 0;
        public gPath currShape;
        public List<gPath> multiSelList = new List<gPath>();
    }
}
