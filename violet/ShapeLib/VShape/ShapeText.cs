using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace ShapeLib.VShape
{

    public class ShapeText : ShapeObj
    {
        
        public String txt;
        public System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
        
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
        //public override void FormKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    txt += e.Key.ToString();

        //}
        public void textBox_KeyDown(object sender,System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt = textBox.Text;
                //textBox.Visible = false;
            }
           
        }
        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)
            {
                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;
                if (txt == null) {
                System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
                textBox.Height = 25;
                textBox.Width = 100;
                textBox.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                Canvas.SetLeft(textBox, data.controlBtn1.X);
                Canvas.SetTop(textBox, data.controlBtn1.Y);
                shapeLib.Data.mygrid.Children.Add(textBox);
                textBox.KeyUp += new System.Windows.Input.KeyEventHandler(textBox_KeyDown);
                }
                else {     
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 50;
                textBlock.Width = 200;
                textBlock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(data.state.colorR, data.state.colorG, data.state.colorB));
                Canvas.SetLeft(textBlock, data.controlBtn1.X);
                Canvas.SetTop(textBlock, data.controlBtn1.Y+25);
                textBlock.Text = txt;
                shapeLib.Data.mygrid.Children.Add(textBlock);
                txt = null;
                } 
            }
          
        }
       
       
       
       

    }
}
       

    
