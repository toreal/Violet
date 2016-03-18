using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeLib.VShape;
using System.Collections;
using Microsoft.Office.Tools.Ribbon;
using System.Drawing;

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
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Red";
            ui.image = new Bitmap(@"icons\red.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Green";
            ui.image = new Bitmap(@"icons\green.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Blue";
            ui.image = new Bitmap(@"icons\blue.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Black";
            ui.image = new Bitmap(@"icons\black.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "White";
            ui.image = new Bitmap(@"icons\white1.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Yellow";
            ui.image = new Bitmap(@"icons\yellow.png");
            ui.click = btn_Click;
            ui.belong = "Colors";
            ret.Add(ui);

            ui = new shapeUI();
            ui.uitype = shapeUIType.RibbonSmallButton;
            ui.label = "Orange";
            ui.image = new Bitmap(@"icons\orange.png");
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
                 String txt = btn.Label;

                 if (String.IsNullOrEmpty(txt))
                     txt = btn.Name;

                     switch(txt)
                     {
                         case "Red":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "Orange":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 165;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "Yellow":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 230;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "Green":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 128;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "Blue":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 128;
                             break;
                         case "Black":
                             shapeLib.Data.colorR = 0;
                             shapeLib.Data.colorG = 0;
                             shapeLib.Data.colorB = 0;
                             break;
                         case "white":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 255;
                             shapeLib.Data.colorB = 255;
                             break;
                         case "Violet":
                             shapeLib.Data.colorR = 138;
                             shapeLib.Data.colorG = 43;
                             shapeLib.Data.colorB = 226;
                             break;
                         case "Gray":
                             shapeLib.Data.colorR = 128;
                             shapeLib.Data.colorG = 128;
                             shapeLib.Data.colorB = 128;
                             break;
break;

                         case "White":
                             shapeLib.Data.colorR = 255;
                             shapeLib.Data.colorG = 255;
                             shapeLib.Data.colorB = 255;

                             break;

                     }
        
             }
        }
        
    }
}
