using LOLServer.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LOLServer.biz;
using LOLServer.dao.model;

namespace LOLServer.logic.selecthero
{
   public class SelectHandler:HanderInterface
    {
       //选择房间ID生成器
       AtomicInt roomId = new AtomicInt();

       private ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();
       //玩家与房间ID映射
       private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
       //已创建房间留着备用
       private ConcurrentQueue<SelectRoom> cache = new ConcurrentQueue<SelectRoom>();

       private IUserBiz userBiz = BizFactory.userBiz;

       public SelectHandler() {
           EventUtil.Instance.selectStart = selectStart;
           EventUtil.Instance.selectDestory = selectDestory;
       }

       public void selectStart(List<int> teamOne, List<int> teamTwo)
       {
           SelectRoom room;
           if (cache.Count > 0)
           {
               cache.TryDequeue(out room);
           }
           else {
               room = new SelectRoom();
           }
           room.setData(teamOne, teamTwo);
           room.setArea(roomId.getAndAdd());
           foreach (int item in teamOne)
           {
               userRoom.TryAdd(item, room.getArea());
           }
           foreach (int item in teamTwo)
           {
               userRoom.TryAdd(item, room.getArea());
           }
           roomMap.TryAdd(room.getArea(), room);
       }

       public void selectDestory(int id) { 
           SelectRoom room;
           if (roomMap.TryRemove(id, out room)) {
               foreach (int item in room.teamOneMap.Keys)
               {
                   int i;
                   userRoom.TryRemove(item,out i);
               }
               foreach (int item in room.teamTwoMap.Keys)
               {
                   int i;
                   userRoom.TryRemove(item, out i);
               }
               cache.Enqueue(room);
           }
       }

    public void MessageRecevie(AceNetFrameWork.ace.UserToken token, AceNetFrameWork.ace.auto.SocketModel message)
    {
        User user=userBiz.get(token);
        if(user==null)return;
        if (userRoom.ContainsKey(user.id)) {
            int roomId = userRoom[user.id];
            if (roomMap.ContainsKey(roomId)) {
                roomMap[roomId].MessageRecevie(token, message);
            }
        }
    }

    public void ClientClose(AceNetFrameWork.ace.UserToken token)
    {
        User user = userBiz.get(token);
        if (user == null) return;
        if (userRoom.ContainsKey(user.id))
        {
             int roomId;
            userRoom.TryRemove(user.id, out roomId);           
            if (roomMap.ContainsKey(roomId))
            {
                roomMap[roomId].ClientClose(token);
            }
        }
    }
}
}
