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
        public Form1()
        {
            InitializeComponent();
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
