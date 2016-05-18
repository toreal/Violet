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

    public class ShapeText : ShapeObj
    {
        public String txt ;      
        public override System.Collections.ArrayList getMenuItem()
        {
            ArrayList ret = new ArrayList();
            shapeUI ui = new shapeUI();
            ui.label = "Text";

            ui.image = new System.Drawing.Bitmap(@"icons\text.png");
            ui.belong = "Shapes";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }

          public override void  FormKeyDown(object sender, System.Windows.Input.KeyEventArgs e){
              txt +=e.Key.ToString();
            
          }

    }
}