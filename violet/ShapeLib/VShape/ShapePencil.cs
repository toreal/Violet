using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLib.VShape
{
    
    public class ShapePencil : ShapeObj
    {
        
        public override System.Collections.ArrayList getMenuItem()
        {
            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonGroup;
            ui.label = "Tools";
            ret.Add(ui);
            shapeUI pui = new shapeUI();
            pui.label = "Pencil";
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.pencil.png");
            pui.image = new System.Drawing.Bitmap(myStream);

            
            pui.click = btn_Click;
            pui.belong = "Tools";
            ret.Add(pui);
            
            return ret;
        }
        
        
        
        private void panel1_mouseDown(object sender, MouseEventArgs e)
        {
            shapeLib.Data.bfirst = false;
        }
        private void panel1_mouseUp(object sender, MouseEventArgs e)
        {
            shapeLib.Data.bfirst = true;
            prex = null;
            prey = null;
        }
        int? prex = null;
        int? prey = null;
        //void panel1_mouseMove(object sender, MouseEventArgs e)
        //{
        //    if (canpaint)
        //    {
        //        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, shapeLib.Data.strokeT);
        //        g.DrawLine(pen, new System.Drawing.Point(prex ?? e.X, prey ?? e.Y), new System.Drawing.Point(e.X, e.Y));
        //        prex = e.X;
        //        prey = e.Y;
        //    }
        //}
        
        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            
            if (bfirst)
            {
                shapeLib.Data.bfirst = false;
                Form form = new Form();
                Graphics g = form.CreateGraphics();
                int ex = (int)(data.controlBtn1.X);
                int ey = (int)(data.controlBtn1.Y);
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, shapeLib.Data.strokeT);
                g.DrawLine(pen, new System.Drawing.Point(prex ?? ex, prey ?? ey), new System.Drawing.Point(ex, ey));
                prex = ex;
                prey = ey;

                //shapeLib.Data.mygrid.Children.Add(form);
                //gv.baseShape.Add(pen);
            }
           
        }

       

    }
}