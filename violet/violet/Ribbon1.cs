using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools.Word;
using System.Collections;
using ShapeLib.VShape;


namespace violet
{
    public partial class Ribbon1
    {
        public Form1 f;
        double backGroundColor = 0.2;
        IForm  Check()
        {
            //確認畫布是否有開啟中
            if (f == null)
            {
                f = new Form1();
                backGroundColor = 0.2;
                int formHeight = (int)(f.setFormSize("h")/1.7);
                int formWidth = (int)(f.setFormSize("w")/1.7);
                f.Size = new System.Drawing.Size(formWidth,formHeight);
                f.TopMost = true;
                f.drawBackgroundLine(backGroundColor);
                f.Disposed += new EventHandler(f_Disposed);
            }
            return (IForm)f;
        }
        //create form


        public void Construt(ShapeObj obj )
        {
            ArrayList list = obj.getMenuItem();
          //  RibbonGroup group3 = this.Factory.CreateRibbonGroup();
           // RibbonTab tab = Factory.CreateRibbonTab();

            foreach (shapeUI ui in list)
            {
                switch(ui.uitype)
                {
                    case shapeUIType.RibbonButton:
                        RibbonButton uiobj =  this.Factory.CreateRibbonButton();
                        uiobj.Click+=(RibbonControlEventHandler) ui.click;
                        uiobj.Label = ui.label;
                        uiobj.Image = ui.image;
                        uiobj.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;

                        addByName(uiobj, ui.belong);
                        break;
                    case shapeUIType.RibbonGroup:
                        RibbonGroup uig = this.Factory.CreateRibbonGroup();
                        uig.Label = ui.label;
                        tab1.Groups.Add(uig);

                        break;
                    case shapeUIType.RibbonMenu:
                        RibbonMenu rim = Factory.CreateRibbonMenu();
                        rim.Label = ui.label;
                        rim.Image = ui.image;
                        addByName(rim, ui.belong);
                        break;
                }

                
                    
            }

            //tab.Groups.Add(group3);
            //Tabs.Add(tab);
                
        }


        void addByName(RibbonControl uiobj, String name)
        {
            if (uiobj != null)
            {
                foreach (RibbonGroup gr in  tab1.Groups )
                {
                    if ( gr.Label == name)
                    {
                        gr.SuspendLayout();
                        gr.Items.Add(uiobj);
                        gr.ResumeLayout(false);
                        gr.PerformLayout();
                        break;
                    }
                    
                    
                    foreach(RibbonControl rc in gr.Items  )
                    {
                        if ( rc is RibbonMenu )
                        {
                            RibbonMenu rm = (RibbonMenu)rc;
                            if (rm.Label == name)
                            {
                                rm.SuspendLayout();
                                rm.Items.Add(uiobj);
                                rm.ResumeLayout(false);
                                rm.PerformLayout();
                                break;
                            }
                        }

                    }
                }

                
            }

        }


        void f_Disposed(object sender, EventArgs e)
        {
            f = null;
        }

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            int i = 0;
            //Check();
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
        private void clear_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
            {
                Check();
                f.TopMost = false;
                f.ClearDrawing();
                f.TopMost = true;
            }
        }
        private void bgp_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
            {
                if (backGroundColor != 0)
                {
                    backGroundColor = 0;
                    f.drawBackgroundLine(backGroundColor);
                }
                else
                {
                    backGroundColor = 0.2;
                    f.drawBackgroundLine(backGroundColor);
                }
            }
        }

        private void red_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("red");
        }
        private void button7_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("orange");
        }
        private void yellow_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("yellow");
        }
        private void green_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("green");
        }
        private void black_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("black");
        }
        private void blue_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("blue");
        }
        private void violet_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("violet");
        }
        private void gray_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("gray");
        }
        private void white_btn_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setColorType("white");
        }
        private void px1_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setStrokeType(1);
        }
        private void px3_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setStrokeType(3);
        }
        private void px5_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setStrokeType(5);
        }
        private void px8_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setStrokeType(8);
        }
        //selection mode
        private void selBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Check();
            f.setDrawType(5);

           shapeLib.Data.drawtype = 5;

        }
        private void redo_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setAction(1);
        }
        private void undo_Click(object sender, RibbonControlEventArgs e)
        {
            if (f != null)
                f.setAction(0);
        }
        //載入圖片
        public void initPath(string xml)
        {
            Check();
            f.initpath(xml);
            f.setDrawType(5);
            f.Show();
        }
    }
}
