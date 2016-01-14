using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace violet.Shape
{
    class ShapeObj:IShapeUI,IDrawing,IUpdateOP,IInsertOP
    {
        public System.Collections.ArrayList getMenuItem()
        {
            throw new NotImplementedException();
        }

        public ShapeObj Create(string svg)
        {
            throw new NotImplementedException();
        }

        public void changeProperty(string prop)
        {
            throw new NotImplementedException();
        }

        public string SVGString()
        {
            throw new NotImplementedException();
        }

        public bool IsDelete
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DrawShape()
        {
            throw new NotImplementedException();
        }

        public void DisplayControlPoints()
        {
            throw new NotImplementedException();
        }

        public void MouseDownUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseUpUpdate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveUpdate(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
