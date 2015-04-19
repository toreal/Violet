using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keago0403
{
    class Utility
    {
        byte[] bitmapBytes;

        public byte[] BitmapBytes
        {
            get { return bitmapBytes; }
            set { bitmapBytes = value; }
        }

        string tagName;

        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        List<string> picControlName;

        public List<string> PicControlName
        {
            get { return picControlName; }
            set { picControlName = value; }
        }
    }
}
