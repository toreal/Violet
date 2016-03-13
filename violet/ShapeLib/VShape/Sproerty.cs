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
            ui.label = "red";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "green";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Blue";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Black";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "White";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Yellow";
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonButton;
            ui.label = "Orange";
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
                         case "red":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "orange":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 165;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "yellow":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 230;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "green":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 128;
                             shapeLib.Data.colorB = 0;
                             break;
<<<<<<< HEAD
                         case "blue":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 128;
                             break;
                         case "black":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "white":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 255;
                             shapeLib.Data.colorB = 255;
                             break;
                         case "violet":
                             shapeLib.Data.colorR = 138;
                             shapeLib.Data.colorG = 43;
                             shapeLib.Data.colorB = 226;
                             break;
                         case "gray":
                             shapeLib.Data.colorR = 128;
                             shapeLib.Data.colorG = 128;
                             shapeLib.Data.colorB = 128;
                             break;

=======

                         case "Blue":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 255;

                             break;

                         case "Black":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;

                             break;

                         case "White":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 255;
                             shapeLib.Data.colorB = 255;

                             break;

                         case "Yellow":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 217;
                             shapeLib.Data.colorB = 0;

                             break;

                         case "Orange":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 116;
                             shapeLib.Data.colorB = 21;

                             break;
>>>>>>> 25195234a8e156595b1eff3fbc802d98e51a6cbc
                     }
        
             }
        }
        
    }
}
