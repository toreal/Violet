using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace violet.Shape
{
    interface IInsertOP
    {
        void MouseDownUpdate(object sender, MouseButtonEventArgs e);
        void MouseUpUpdate(object sender, MouseButtonEventArgs e);
        void MouseMoveUpdate(object sender, MouseEventArgs e);
    }
}
