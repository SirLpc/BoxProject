using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLServer.tool
{
   public class EventUtil
   {
       Mutex tex = new Mutex();
       private static EventUtil util;

       public static EventUtil Instance { get { if (util == null)util = new EventUtil(); return util; } }
       public delegate void SelectStart(List<int> teamOne,List<int> teamTwo);
       //战前选择触发
       public SelectStart selectStart;

       public delegate void SelectDestory(int value);

       public SelectDestory selectDestory;

       public delegate void FightStart(SelectModel[] one,SelectModel[] two);

       public FightStart fightStart;

       public delegate void FightDestory(int area);

       public FightDestory fightDestory;
    }
}
