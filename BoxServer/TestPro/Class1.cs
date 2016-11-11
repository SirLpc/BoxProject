using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPro
{
   public class Class1
    {
      string s;
      static Class1() {
          Program.test.Add(1, new TestPro.Class1("hehe"));
          Console.WriteLine("fuck");
       }

      Class1(string s) {
          this.s = s;
      }
     public void what() {
          Console.WriteLine(s);
      }
    }
}
