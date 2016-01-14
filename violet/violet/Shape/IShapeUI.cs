using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace violet.Shape
{
    interface IShapeUI
    {
        ArrayList getMenuItem();
        ShapeObj Create(String svg); 
        void changeProperty(String prop);
        String SVGString(); //for save
        Boolean IsDelete{ get;set;}


    }
}
