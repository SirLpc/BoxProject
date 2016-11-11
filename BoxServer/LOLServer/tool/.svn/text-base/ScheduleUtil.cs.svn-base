using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace LOLServer.tool
{
    public delegate void TimedEvent();
    public class ScheduleUtil
    {

        private static ScheduleUtil util;

        public static ScheduleUtil Instance { get { if (util == null)util = new ScheduleUtil(); return util; } }

        Timer timer;


        //等待执行任务表
        private Dictionary<int, TaskModel> mission = new Dictionary<int, TaskModel>();
        //任务ID自增
        private AtomicInt mId = new AtomicInt();
        //任务移除列表
        private List<int> removeList = new List<int>();

        ScheduleUtil()
        {
            timer = new Timer(200);
            timer.Elapsed += callback;
            timer.Start();
        }

        void callback(object sender, ElapsedEventArgs e)
        {
            lock (mission)
            {
                lock (removeList)
                {
                    foreach (int l in removeList)
                    {
                        mission.Remove(l);
                    }
                    removeList.Clear();
                    foreach (TaskModel model in mission.Values)
                    {
                        if (model.getRunTime() <= DateTime.Now.Ticks)
                        {
                            model.run();
                            removeList.Add(model.getId());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 对外调用 单位为毫秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public int schedule(TimedEvent task, long delay)
        {
            return schedulemms(task, delay * 1000 * 1000);
        }

        /**
         内部转微秒
         */
        private int schedulemms(TimedEvent task, long delay)
        {
            lock (mission)
            {
                int id = mId.getAndAdd();
                TaskModel model = new TaskModel(id, task, DateTime.Now.Ticks + delay);
                mission.Add(id, model);
                return id;
            }
        }

        public void removeMission(int id)
        {
            lock (removeList)
            {
                removeList.Add(id);
            }

        }


        public int schedule(TimedEvent task, DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return schedulemms(task, t);
        }


        public int timeSchedule(TimedEvent task, long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return schedulemms(task, t);
        }
    }
}
