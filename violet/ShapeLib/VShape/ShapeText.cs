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

        String txt;
        System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
        double x, y;
        byte r, g, b;
        Boolean last = false;
        public override System.Collections.ArrayList getMenuItem()
        {
            ArrayList ret = new ArrayList();
            shapeUI ui = new shapeUI();
            ui.label = "Text";
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream myStream = myAssembly.GetManifestResourceStream("ShapeLib.icons.text.png");
            ui.image = new System.Drawing.Bitmap(myStream);


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

        public override void MouseDownInsert(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //public void LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.MouseDownInsert(sender, e);
            finish();

        }
        public override void finish()
        {

            int scale = shapeLib.Data.strokeT;
            txt = textBox.Text ;
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
            textBlock.Height = textBox.Height;
            textBlock.Width = textBox.Width;
            textBlock.FontSize = 10 + scale * 4;

            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);

           while (txt.Contains(@"\u" ))
            {
                int ind = txt.IndexOf(@"\u");
                string s= txt.Substring(ind, 6);
                byte[] bytes = new byte[2];
                bytes[1] = Convert.ToByte(s.Substring(2, 2), 16);
                bytes[0] = Convert.ToByte(s.Substring(4, 2), 16);
                string result = Encoding.Unicode.GetString(bytes);
                txt=txt.Replace(s, result);




            }
            textBlock.Text = txt;
            shapeLib.Data.mygrid.Children.Add(textBlock);
            textBox.Text = null;
            txt = null;
            shapeLib.Data.mygrid.Children.Remove(textBox);
            last = true;
            shapeLib.Data.Root.Focus();
            

        }
        //public void RightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    shapeLib.Data.mygrid.Children.Remove(textBox);

        //}

        public override void DrawShape(gView gv, gPath data, Boolean bfirst)
        {
            if (bfirst)
            {

                shapeLib.Data.Status = "rest";
                shapeLib.Data.bfirst = false;

                if (txt == null)
                {
                    x = data.controlBtn1.X;
                    y = data.controlBtn1.Y;
                    r = data.state.colorR;
                    g = data.state.colorG;
                    b = data.state.colorB;
                    textBox.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
                    Canvas.SetLeft(textBox, x);
                    Canvas.SetTop(textBox, y);
                    textBox.FontSize = 10 + shapeLib.Data.strokeT * 4;
                    shapeLib.Data.mygrid.Children.Add(textBox);
                    textBox.Focus();
                    textBox.AcceptsReturn = true;
                    textBox.AcceptsTab = true;
                    txt = textBox.Text;
                    last = false;
                   // shapeLib.Data.mygrid.MouseDown += this.MouseDownInsert;
                        //new System.Windows.Input.MouseButtonEventHandler(LeftButtonDown);

                }
                else if (!last)
                {
                    txt = null;
                    textBox.Focus();

                }
            }else


                txt = null;
           

        }
    }

}
