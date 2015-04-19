using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools.Word;

namespace keago0403
{
    public partial class Ribbon1
    {
        Form1 f = new Form1();

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            f.TopMost = true;
        }

        private void circle_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setDrawType(1);
            f.Show();
        }

        private void rectangle_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setDrawType(2);
            f.Show();
        }

        private void line_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setDrawType(3);
            f.Show();
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            f.ClearDrawing();
            f.Show();
      

        }
    }
}
