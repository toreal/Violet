using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeLib.VShape
{
    interface IShapeUI
    {
        ArrayList getMenuItem();
        ShapeObj Create(String svg); 
        void changeProperty(String prop, string value);
     
        String SVGString(); //for save
     //   Boolean IsDelete{ get;set;}


    }
}
