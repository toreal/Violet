using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShapeLib.VShape
{
    public class PencilEraser : ShapeObj
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
            pui.image = new Bitmap(@"icons\pencil.png");
            pui.click = btn_Click;
            pui.belong = "Tools";
            ret.Add(pui);
            
            shapeUI eui = new shapeUI();
            eui.label = "Eraser";
            eui.image = new Bitmap(@"icons\eraser.png");
            eui.click = btn_Click;
            eui.belong = "Tools";
            ret.Add(eui);

            return ret;
        }  




    }
}