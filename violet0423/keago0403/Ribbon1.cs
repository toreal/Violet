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

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void red_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("red");
        }

        private void button7_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("orange");
        }

        private void yellow_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("yellow");
        }

        private void green_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("green");
        }

        private void black_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("black");
        }

        private void blue_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("blue");
        }

        private void white_btn_Click(object sender, RibbonControlEventArgs e)
        {
            f.setColorType("white");
        }
    }
}
