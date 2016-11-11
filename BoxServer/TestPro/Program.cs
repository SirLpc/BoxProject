using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPro
{
    class Program
    {
       public static Dictionary<int, Class1> test = new Dictionary<int, Class1>();
        static void Main(string[] args)
        {
            test[1].what();
            Console.ReadKey();
        }
    }
}
