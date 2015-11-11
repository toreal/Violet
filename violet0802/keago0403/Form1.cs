using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace keago0403
{
    public partial class Form1 : Form
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

        public void drawBackgroundLine(double Sop)
        {
            if (Sop != 0)
                userControl1.drawBackLine(Swidth, Sheight, Sop);
            else
                userControl1.hideBackLine();
        }
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
        public void setDrawType(int ntype){
            if (ntype == 5)
                userControl1.hiddenCanvas();
            userControl1.drawtype = ntype;
        }
        public void setColorType(String colorName)
        {
            userControl1.color(colorName);
        }
        public void ClearDrawing()
        {
            userControl1.ClearDrawing();
        }
        public void setStrokeType(int ntype)
        {
            userControl1.stroke(ntype);
        }

        public void initpath(string xml)
        {
            userControl1.initpath(xml);
        }
        public void setAction(int act)
        {
            userControl1.RUdo(act);
        }
    }
}
