using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLServer.tool
{
   public class AtomicInt
    {
       int value;
       Mutex tex = new Mutex();


       public void reset()
       {
           lock (this)
           {
               tex.WaitOne();
               value=0;
               tex.ReleaseMutex();
           }
       }

       public AtomicInt()
       {
           value = 0;
       }

       public AtomicInt(int value)
       {
           this.value = value;

       }

       public int getAndAdd()
       {
           lock (this)
           {
               tex.WaitOne();
               value++;
               tex.ReleaseMutex();
               return value;
           }
       }

       public int getAndReduce()
       {
           lock (this)
           {
               tex.WaitOne();

               value--;
               tex.ReleaseMutex();
               return value;
           }
       }

       public int get()
       {
           return value;
       }

    }
}
