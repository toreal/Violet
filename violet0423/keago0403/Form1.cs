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
        public void drawBackgroundLine()
        {
            userControl11.drawLine(Swidth, Sheight);
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
        public void setDrawType(int ntype)
        {
            userControl11.drawtype = ntype;
        }
        public void setColorType(String colorName)
        {
            userControl11.color(colorName);
        }
        public void ClearDrawing()
        {
            userControl11.ClearDrawing();
        }
        public void setStrokeType(int ntype)
        {
            userControl11.stroke(ntype);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
