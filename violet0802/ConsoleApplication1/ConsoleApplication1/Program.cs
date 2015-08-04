using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private int point;

        public int Line{
            get
            {
                return point;
            }
            set
            {
                point = value;
            }
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("e: " + Line);
            Console.Read();
        }
    }
}
