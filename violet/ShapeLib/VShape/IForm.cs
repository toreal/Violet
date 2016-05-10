using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;




namespace ShapeLib.VShape
{
    public delegate IForm getForm(); 

    public interface  IForm
    {
        Canvas drawControl { get; }
        System.Windows.Controls.UserControl getRoot { get; }
        void Show();

    }
}
