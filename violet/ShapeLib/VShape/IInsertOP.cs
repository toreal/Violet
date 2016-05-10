using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ShapeLib.VShape
{
    interface IInsertOP
    {
        void MouseOP( int ntype );
        void MouseDownInsert(object sender, MouseButtonEventArgs e);
        void MouseUpInsert(object sender, MouseButtonEventArgs e);
        void MouseMoveInsert(object sender, MouseEventArgs e);
        void FormKeyDown(object sender, System.Windows.Input.KeyEventArgs e);
    }
}
