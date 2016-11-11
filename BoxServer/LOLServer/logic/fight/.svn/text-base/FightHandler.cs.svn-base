using LOLServer.tool;
using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LOLServer.logic.fight;
using LOLServer.biz;
using LOLServer.dao.model;

namespace LOLServer.logic
{
   public class FightHandler:HanderInterface

    {
       IUserBiz userBiz = BizFactory.userBiz;

       private static FightHandler handler;

       public static FightHandler Instace { get { if (handler == null)handler = new FightHandler(); return handler; } }
       //玩家所在战场映射
       public ConcurrentDictionary<int, int> userFight = new ConcurrentDictionary<int, int>();
       //战场映射表
       private ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();

       //重复利用已创建战场对象
       ConcurrentQueue<FightRoom> cache = new ConcurrentQueue<FightRoom>();

       AtomicInt roomId = new AtomicInt();
       public FightHandler() {

           EventUtil.Instance.fightStart = fightStart;
           EventUtil.Instance.fightDestory = destoryFight;
       }

       void fightStart(SelectModel[] one, SelectModel[] two)
       {
           FightRoom room;
           if (cache.Count > 0)
           {
               cache.TryDequeue(out room);
           }
           else {
               room = new FightRoom();
               room.setArea(roomId.getAndAdd());
           }
           foreach (SelectModel item in one)
           {
               userFight.TryAdd(item.userId, room.getArea());
           }
           foreach (SelectModel item in two)
           {
               userFight.TryAdd(item.userId, room.getArea());
           }
           roomMap.TryAdd(room.getArea(), room);
           room.init(one,two);
           
       }

       void destoryFight(int area) {
           Console.WriteLine("yooo~~~~destory fight");
           FightRoom room;
           if (roomMap.TryRemove(area, out room)) {
               foreach (int item in room.teamOne.Keys)
               {
                   int i;
                   userFight.TryRemove(item, out i);
               }
               foreach (int item in room.teamTwo.Keys)
               {
                   int i;
                   userFight.TryRemove(item, out i);
               }
               cache.Enqueue(room);
           }
       }
        public void MessageRecevie(AceNetFrameWork.ace.UserToken token, AceNetFrameWork.ace.auto.SocketModel message)
        {
            User user = userBiz.get(token);
            if (user == null) return;
            if (userFight.ContainsKey(user.id))
            {
                roomMap[userFight[user.id]].MessageRecevie(token,message);
            }
        }

        public void ClientClose(AceNetFrameWork.ace.UserToken token)
        {
            User user= userBiz.get(token);
            if(user==null)return;
            if (userFight.ContainsKey(user.id)) {
                if (roomMap.ContainsKey(userFight[user.id]))
                roomMap[userFight[user.id]].ClientClose(token);
            }
        }
    }
}
