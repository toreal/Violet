using Microsoft.Office.Tools.Ribbon;
using Microsoft.Windows.Controls.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace violet.Shape
{


    public enum shapeUIType
    {
        RibbonButton,
        RibbonMenu,
        RibbonGroup
    }

   public  delegate void mouseClick(object sender, RibbonControlEventArgs e);
    public class shapeUI
    {
        public shapeUIType uitype;
        public Image image;
        public String label;
        public ArrayList items;
        public RibbonControlEventHandler click;
        public string belong;
    }

    public class ShapeObj:IShapeUI,IDrawing,IUpdateOP,IInsertOP
    {
        public System.Collections.ArrayList getMenuItem()
        {
           
            ArrayList ret = new ArrayList();

            shapeUI ui = new shapeUI();
            ui.label = "test";
            ui.click = this.btn_Click;
            ret.Add(ui);

            return ret;
            //throw new NotImplementedException();
        }

      public   void btn_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Clicked");
            //throw new NotImplementedException();
        }

        public ShapeObj Create(string svg)
        {
            throw new NotImplementedException();
        }

        public void changeProperty(string prop)
        {
            throw new NotImplementedException();
        }

        public string SVGString()
        {
            throw new NotImplementedException();
        }

        public bool IsDelete
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DrawShape()
        {
            throw new NotImplementedException();
        }

        public void DisplayControlPoints()
        {
            throw new NotImplementedException();
        }

        public void MouseDownUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseUpUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveUpdate(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void MouseDownInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseUpInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveInsert(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
