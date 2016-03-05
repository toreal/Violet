using ShapeLib.VShape;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace violet
{
    public partial class Form1 : Form,IForm 
    {
        double Swidth;
        double Sheight;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //繪製背景格線
        public void drawBackgroundLine(double Sop)
        {
            if (Sop != 0)
                userControl1.drawBackLine(Swidth, Sheight, Sop);
            else
                userControl1.hideBackLine();
        }
        //取得設定用的畫布大小
        public double setFormSize(String ctype)
        {
            if (ctype == "w")
            {
                Swidth = Screen.PrimaryScreen.Bounds.Width;
                return Swidth;
            }
            Sheight = Screen.PrimaryScreen.Bounds.Height;
            return Sheight;
        }
        //設定目前動作
        public void setDrawType(int ntype){
            userControl1.drawtype = ntype;
            if (ntype == 5)
                userControl1.hiddenCanvas();
        }
        //設定顏色
        public void setColorType(String colorName)
        {
            userControl1.color(colorName);
        }
        //清除畫布
        public void ClearDrawing()
        {
            userControl1.ClearBtnUse();
        }
        //設定線條粗細
        public void setStrokeType(int ntype)
        {
            userControl1.stroke(ntype);
        }
        //載入XML
        public void initpath(string xml)
        {
            userControl1.initpath(xml);
        }
        //選擇使用Redo或Undo 
        public void setAction(int act)
        {
            userControl1.RUdo(act);
        }

      

        public System.Windows.Controls.Canvas drawControl
        {
            get { return userControl1.mygrid; }
        }
    }
}
