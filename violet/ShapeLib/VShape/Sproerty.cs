using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeLib.VShape;
using System.Collections;
using Microsoft.Office.Tools.Ribbon;

namespace ShapeLib.VShape
{
    class Sproerty: ShapeObj
    {
        public override System.Collections.ArrayList getMenuItem()
        {
            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonGroup;
            ui.label = "Colors";
            ret.Add(ui);


            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Red";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Green";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);


            return ret;
        }

         void btn_Click(object sender, RibbonControlEventArgs e)
        {
            RibbonButton btn = sender as RibbonButton;
             if ( btn!= null)
             {
                     switch(btn.Label)
                     {
                         case "Red":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;
                             break;

                         case "Green":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 255;
                             shapeLib.Data.colorB = 0;
                          
                             break;
                     }
        
             }
        }
        
    }
}
