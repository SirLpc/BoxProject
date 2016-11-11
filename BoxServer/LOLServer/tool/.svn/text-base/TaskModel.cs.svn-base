using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.tool
{
   public class TaskModel
   {// 任务
       private TimedEvent execut;
       // 执行时间
       private long runTime;
       //任务ID
       private int id;

       public TaskModel(int id, TimedEvent execut, long runTime)
       {
           this.id = id;
           this.execut = execut;
           this.runTime = runTime;
       }

       public void run()
       {
           execut();
       }

       public TimedEvent getExecut()
       {
           return execut;
       }

       public void setExecut(TimedEvent execut)
       {
           this.execut = execut;
       }

       public int getId()
       {
           return id;
       }

       public void setId(int id)
       {
           this.id = id;
       }

       public long getRunTime()
       {
           return runTime;
       }

       public void setRunTime(long runTime)
       {
           this.runTime = runTime;
       }
    }
}
