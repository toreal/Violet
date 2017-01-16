using ShapeLib.VShape;
//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools.Word;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;


namespace ShapeLib.VShape
{

    [XmlRoot(ElementName = "SVGRoot", Namespace = "")]
    public class SVGRoot
    {
         [XmlElement("PathList")]
        public List<gPath> PathList = new List<gPath>();
    }
    /// <summary>
    /// 記錄shape list,action data
    /// stack 記錄動作,每個動作(pointAry)包含,該圖是圖形的第幾個(Listplace),之前記錄是否己有相同圖是第幾個,目前圖存在記錄的第幾個
    ///
    /// </summary>
    public class GraphDoc
    {
       public SVGRoot sroot = new SVGRoot();
       public List<gView> shapeList = new List<gView>();
       public List<gPath> FullList = new List<gPath>(); //remember all action from grid
        public Stack UndoStack = new Stack();
        public Stack RedoStack = new Stack();
        public int selIndex = -1; // The last seat in PathList array
        public int node = 0;
        public int mx;
        public int my;
        public bool bmove;
      
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

        public void reDrawAll()
        {
            foreach (gPath gp in sroot.PathList)
            {
                if ( !gp.IsDelete )
                {
                    gp.redraw(1);

                }
            }

        }

        /// <summary>
        /// 維護 undo stack ,把目前狀態存起來.並清空redo stack,如果之前有undo 動作,是回覆到某一狀態,在此之後的動作都可清除
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Action"></param>
        public void writeIn(gPath Data, int Action)
        {


            saveState pa;
                
            int lens =RedoStack.Count;            
             RedoStack.Clear();
             FullList.RemoveRange(FullList.Count - lens, lens);

            

            if (Action == 0)//新增動作
            {
                gPath g = new gPath();
                //等一下會加入到list 中,所以count 正好為其在list  所在的位置
                pa = new saveState(Action, sroot.PathList.Count, (FullList.Count ));
                
                UndoStack.Push(pa);
                g.copyVal(Data);
                FullList.Add(Data);
                sroot.PathList.Add(g);
            }
            else  //修改動作(物件已存在)
            {                               
                pa = new saveState(Action, Data.ListPlace, FullList.Count );
                FullList.Add(Data);
                UndoStack.Push(pa);
            }
        }


        /// <summary>
        /// 重作到目前狀態
        ///  
        /// </summary>
        public void reDo()
        {
            if (RedoStack.Count > 0)
            {
                gPath tempPath = new gPath();
                saveState tempPA ;

                tempPA = (saveState)RedoStack.Pop();

                if (tempPA.currSate >= 0 && tempPA.currSate < FullList.Count)
                {
                    tempPath.copyVal(FullList[tempPA.currSate]);
                 

                    if (tempPA.Action == 0)
                        sroot.PathList[tempPA.GraphIndex].IsDelete = false;

                    sroot.PathList[tempPA.GraphIndex] = tempPath;
                    sroot.PathList[tempPA.GraphIndex].redraw(1);
                      
                }
                else
                {
                   // sroot.PathList.RemoveAt(tempPA.GraphIndex);
                }
                UndoStack.Push(tempPA);
                
            }
        }

        /// <summary>
        /// undo 回到前一狀態
        /// </summary>
        public void unDo()
        {
            //檢查是否有可undo 的事件
            if (UndoStack.Count > 0)
            {
                gPath tempPath = new gPath();
                saveState tempPA ;
                shapeLib.Data.mClick = 0;

                tempPA = (saveState)UndoStack.Pop();

                if (tempPA.currSate >= 0 && tempPA.currSate < FullList.Count)
                {
                    if ( tempPA.Action ==0)
                    {

                        sroot.PathList[tempPA.GraphIndex].IsDelete = true;
 
                    }else
                    {
                        //找出前一個state 
                        int i;
                        for ( i = tempPA.currSate-1; i >=0 ; i --)
                        {
                           if (  FullList[i].ListPlace == tempPA.GraphIndex)
                           {
                               tempPath.copyVal(FullList[i]);
                               sroot.PathList[tempPA.GraphIndex] = tempPath;
                               sroot.PathList[tempPA.GraphIndex].redraw(1);
                               break;
 
                           }
                        }
                        if ( i < 0 ) //something wrong
                        {
                            Debug.WriteLine("something wrong");
                        }

                    }

                }
                else
                {

                    //something wrong
                    Debug.WriteLine("something wrong");

                    //sroot.PathList.RemoveAt(tempPA.GraphIndex);
                }
                //將該事件放入redo stack
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
        public String Text = "";
        bool _isSel;
        public List<Point> pList = new List<Point>();
        public bool isSel
        {
            get {             
                return _isSel; }
            set {
                
                if ( value != _isSel)
                {
                    _isSel = value;
                    if (!value)
                        redraw(2);
                    else
                        redraw(1);
                }
                                
                 }
        }
        private int shapeIndex=-1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remove"> -1 : 移除, 0: 運動中畫, 1: 正式畫 , 2: 重新加入</param>
        public void redraw( int removetype)
        {
            gView gv = null;
            Boolean bfirst = false;

            if (shapeIndex < 0)
            {
                gv = new gView();
                shapeIndex = shapeLib.Data.gdc.shapeList.Count;
                shapeLib.Data.gdc.shapeList.Add(gv);
                bfirst = true;
            }
            else
                gv = shapeLib.Data.gdc.shapeList[shapeIndex];


            if (isSel)
            {
                foreach (Shape sp in gv.baseShape)
                shapeLib.Data.mygrid.Children.Remove(sp);
                shapeLib.SupportedShape(null)[drawtype].DisplayControlPoints(gv, this);
                
            }
            else
            {
                foreach (Shape sp in gv.controlShape)
                    shapeLib.Data.mygrid.Children.Remove(sp);
                gv.controlShape.Clear();

                switch (removetype)
                {
                    case -1:
                        foreach (Shape sp in gv.baseShape)
                            shapeLib.Data.mygrid.Children.Remove(sp);
                        break;


                    case 2:
                        foreach (Shape sp in gv.baseShape)
                            shapeLib.Data.mygrid.Children.Add(sp);
                        shapeLib.SupportedShape(null)[drawtype].DrawShape(gv, this, bfirst);
                        break;
                    case 0:
                    case 1:
                        shapeLib.SupportedShape(null)[drawtype].DrawShape(gv, this, bfirst);

                        break;

                }
            }
        }
        
          // public Shape getDrawShape()
          //  {
          //      if (shapeIndex >= 0 && shapeIndex < shapeLib.Data.gdc.shapeList.Count)
          //      {
          //          Shape ishape = shapeLib.Data.gdc.shapeList[shapeIndex];
          //          return ishape;
          //      }
          //      return null;

          //  }

          //public  void setDrawShape(Shape value)
          //  {
          //      shapeLib.Data.gdc.shapeList.Add(value);
          //      shapeIndex = shapeLib.Data.gdc.shapeList.Count - 1;
          //  }
        

        private bool isdel = false;
        public bool IsDelete
        {
            get
            {


                return isdel;
                //throw new NotImplementedException();
            }
            set
            {
                if (value == true)
                {
                    this.redraw( -1 );
                 
                
                      
                }else
                {
                    if ( isdel)
                    {
                        this.redraw(2); 
                

                    }


                }
                isdel = true;


                // throw new NotImplementedException();
            }
        }
        public void myLine_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if ( isSel)
                shapeLib.Data.mygrid.Cursor = Cursors.SizeAll;
            else 
                shapeLib.Data.mygrid.Cursor= Cursors.Hand;
         //   throw new NotImplementedException();
        }

        public void myLine_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            shapeLib.Data.mygrid.Cursor = Cursors.Arrow;
       
        //    throw new NotImplementedException();
        }


       public   void myLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // if (shapeLib.Data.drawtype == 5)
            {
                //檢查是否有按下shift
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    shapeLib.Data.multiSelList.Add(this);

                }else
                {
                    foreach( gPath gp in shapeLib.Data.multiSelList )
                    {
                        gp.isSel = false;
                    }
                    if (shapeLib.Data.currShape != null && shapeLib.Data.currShape != this)
                    shapeLib.Data.currShape.isSel = false;
                    shapeLib.Data.multiSelList.Clear(); 
                    shapeLib.Data.multiSelList.Add(this);
                }

                shapeLib.Data.currShape = this;
                this.isSel = true;

                IInsertOP sh = shapeLib.SupportedShape(null)[this.drawtype];
                sh.MouseOP(1);

                e.Handled = true;
            }
            //throw new NotImplementedException();
        }


        public void copyVal(gPath obj)
        {

            drawtype = obj.drawtype;
            state = obj.state;
            ListPlace = obj.ListPlace;
            shapeIndex = obj.shapeIndex;

            controlBtn1 = obj.controlBtn1;
            controlBtn2 = obj.controlBtn2;
            controlBtn3 = obj.controlBtn3;
            controlBtn4 = obj.controlBtn4;

            foreach(Point p in obj.pList)
            {
                pList.Add(p);
            }
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
    }
    /// <summary>
    /// 為了維護undo redo, 系統任何操作必需把狀態存起來,
    /// </summary>
    public class saveState
    {
        //0: insert, 1:update, 2:delete
      public   int Action; 
    //目前圖形串列中的第幾個,為必免順序改變,凡加入的就一直存在(data list)
    public int GraphIndex;

    //操作前的狀態 fullList 中,操作前該物件所在位置,即最後一個狀態.至少會有新增的狀態.以GraphIndex 去找,其實可以不用記錄
    //int preSate;

    //操作後的狀態 fullList 中,剛更改的狀態
    public int currSate;

      
  
        public saveState()
        {
            Action = -1;
            GraphIndex = -1;
            currSate = -1; 
            //changeP = 0;
            //lastP = -1;
            //leastP = 0;

        }
        public saveState(int a, int b, int c)
        {
            Action = a;
            GraphIndex = b;
            currSate = c;
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