﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeLib.VShape
{
    class ShapeCurve : ShapeObj
    {


        public override System.Collections.ArrayList getMenuItem()
        {

            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "Curve";


            ui.image = new Bitmap(@"icons\curve.png");
            ui.belong = "Shapes";
            //      ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }



    }
}