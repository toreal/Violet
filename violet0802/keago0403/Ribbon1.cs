﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools.Word;

namespace keago0403
{
    public partial class Ribbon1
    {
        Form1 f;
        void Check()
        {
            if (f == null)
            {
                f = new Form1();
                int formHeight = (int)(f.setFormSize("h")/1.7);
                int formWidth = (int)(f.setFormSize("w")/1.7);
                f.Size = new System.Drawing.Size(formWidth,formHeight);
                f.TopMost = true;
                f.drawBackgroundLine();
                f.Disposed += new EventHandler(f_Disposed);
            }
        }
        void f_Disposed(object sender, EventArgs e)
        {
            f = null;
        }

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            Check();
        }

        private void circle_btn_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(1);
            f.Show();
        }

        private void rectangle_btn_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(2);
            f.Show();
        }

        private void line_btn_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(3);
            f.Show();
        }
        private void Curve_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(4);
            f.Show();
        }
        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.ClearDrawing();
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
        private void px1_Click(object sender, RibbonControlEventArgs e)
        {
            f.setStrokeType(1);
        }

        private void px3_Click(object sender, RibbonControlEventArgs e)
        {
            f.setStrokeType(3);
        }

        private void px5_Click(object sender, RibbonControlEventArgs e)
        {
            f.setStrokeType(5);
        }

        private void selBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(5);
            f.Show();
        }
    }
}