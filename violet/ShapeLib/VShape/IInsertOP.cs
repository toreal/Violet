using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace violet.VShape
{
    interface IInsertOP
    {
        void MouseOP();
        void MouseDownInsert(object sender, MouseButtonEventArgs e);
        void MouseUpInsert(object sender, MouseButtonEventArgs e);
        void MouseMoveInsert(object sender, MouseEventArgs e);
    }
}
