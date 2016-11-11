using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using AceNetFrameWork.ace;
using LOLServer.biz;
using LOLServer.dao.model;
using LOLServer.tool;

namespace LOLServer.logic.match
{
    public class MatchHandler :AbsOnceHandler, HanderInterface
    {
        IUserBiz userBiz = BizFactory.userBiz;

        //用户所在匹配房间
        ConcurrentDictionary<int, int> userMatch = new ConcurrentDictionary<int, int>();

        //回收重复利用已创建匹配队列资源
        ConcurrentQueue<MatchRoom> cache = new ConcurrentQueue<MatchRoom>();
        //当前处于匹配状态的房间
        ConcurrentDictionary<int, MatchRoom> nowRoom = new ConcurrentDictionary<int, MatchRoom>();
        //房间ID生成器
        AtomicInt roomId = new AtomicInt();

        public void MessageRecevie(AceNetFrameWork.ace.UserToken token, AceNetFrameWork.ace.auto.SocketModel message)
        {
            switch (message.command)
            {
                case MatchProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    leave(token);
                    break;
            }
        }

        private void leave(UserToken token)
        {
            User user = userBiz.get(token);
            if (user == null) return;
            if (!userMatch.ContainsKey(user.id)) {
                return;
            }
            int roomId= userMatch[user.id];
            if (nowRoom.ContainsKey(roomId)) {
                MatchRoom room= nowRoom[roomId];
                if (room.teamOne.Contains(user.id)) {
                    room.teamOne.Remove(user.id);
                }else
                if (room.teamTwo.Contains(user.id))
                {
                    room.teamTwo.Remove(user.id);
                }
                int ri;
                userMatch.TryRemove(user.id,out ri);
                if (room.teamOne.Count + room.teamTwo.Count == 0)
                {
                    MatchRoom r;
                    nowRoom.TryRemove(room.id, out r);
                    cache.Enqueue(room);
                }
            }

        }

        private void enter(UserToken token)
        {
            User user= userBiz.get(token);
            if (!userMatch.ContainsKey(user.id)){
                //判断当前 是否有已经在匹配的房间
                MatchRoom room=null;
                if (nowRoom.Count > 0)
                {
                    foreach (MatchRoom item in nowRoom.Values)
                    {
                        if (item.teamMax * 2 > item.teamOne.Count + item.teamTwo.Count) {
                            room = item;
                            if (room.teamOne.Count < item.teamMax)
                            {
                                room.teamOne.Add(user.id);
                                
                            }
                            else {
                                room.teamTwo.Add(user.id);
                            }
                            userMatch.TryAdd(user.id, room.id);
                            break;
                        }
                    }
                    if (room == null) {
                        //没有的话 判断有没有历史房间
                        if (cache.Count > 0)
                        {
                            cache.TryDequeue(out room);
                            room.teamOne.Add(user.id);
                            nowRoom.TryAdd(room.id, room);
                            userMatch.TryAdd(user.id, room.id);
                        }
                        else
                        {
                            //没有历史房间 果断创建新的
                            room = new MatchRoom();
                            room.id = roomId.getAndAdd();
                            room.teamOne.Add(user.id);
                            nowRoom.TryAdd(room.id, room);
                            userMatch.TryAdd(user.id, room.id);
                        }
                    }
                }
                else { 
                    //没有的话 判断有没有历史房间
                    if (cache.Count > 0)
                    {
                        cache.TryDequeue(out room);
                        room.teamOne.Add(user.id);
                        nowRoom.TryAdd(room.id, room);
                        userMatch.TryAdd(user.id, room.id);
                    }
                    else { 
                        //没有历史房间 果断创建新的
                       room= new MatchRoom();
                       room.id = roomId.getAndAdd();
                       room.teamOne.Add(user.id);
                       nowRoom.TryAdd(room.id, room);
                       userMatch.TryAdd(user.id, room.id);
                    }
                }
                //不管何种进入，满员就果断开之,并且将匹配对象放入缓存
                if (room.teamOne.Count == room.teamTwo.Count&&room.teamTwo.Count == room.teamMax)
                {
                    EventUtil.Instance.selectStart(room.teamOne, room.teamTwo);
                    foreach (int item in room.teamOne)
	                {
                        int i;
                        userMatch.TryRemove(item, out i);
	                }
                    foreach (int item in room.teamTwo)
                    {
                        int i;
                        userMatch.TryRemove(item, out i);
                    }
                    room.teamOne.Clear();
                    room.teamTwo.Clear();
                    MatchRoom r;
                    nowRoom.TryRemove(room.id,out r);
                    cache.Enqueue(room);
                }
            }
        }

        public void ClientClose(AceNetFrameWork.ace.UserToken token)
        {
            leave(token);
        }

        public override int getType()
        {
            return Protocol.TYPE_MATCH;
        }
    }
}
