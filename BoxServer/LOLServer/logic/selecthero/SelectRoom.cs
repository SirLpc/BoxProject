﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LOLServer.tool;
using OneByOne;
using AceNetFrameWork.ace;
using LOLServer.dao.model;

namespace LOLServer.logic.selecthero
{
    public class SelectRoom : AbsMulitHandler, HanderInterface
    {
        public int id;//房间ID

        public ConcurrentDictionary<int, SelectModel> teamOneMap = new ConcurrentDictionary<int, SelectModel>();

        public ConcurrentDictionary<int, SelectModel> teamTwoMap = new ConcurrentDictionary<int, SelectModel>();

        int inCount = 0;

        int missionId = -1;

        List<int> readyList = new List<int>();

        bool canSelectSameHero = true;

        //相当于类数据初始化
        public void setData(List<int> teamOne, List<int> teamTwo)
        {

            missionId = -1;
            inCount = 0;
            readyList.Clear();
            teamOneMap.Clear();
            foreach (int item in teamOne)
            {
                SelectModel sm = new SelectModel();
                sm.userId = item;
                sm.name = getUser(item).name;
                sm.heroId = -1;
                sm.entered = false;
                sm.ready = false;
                teamOneMap.TryAdd(item, sm);
            }

            teamTwoMap.Clear();
            foreach (int item in teamTwo)
            {
                SelectModel sm = new SelectModel();
                sm.userId = item;
                sm.name = getUser(item).name;
                sm.heroId = -1;
                sm.entered = false;
                sm.ready = false;
                teamTwoMap.TryAdd(item, sm);
            }
            writeToUsers(teamOne.ToArray(), Protocol.TYPE_MATCH, 0, MatchProtocol.ENTER_SELECT_BRO, getArea());
            writeToUsers(teamTwo.ToArray(), Protocol.TYPE_MATCH, 0, MatchProtocol.ENTER_SELECT_BRO, getArea());
            missionId = ScheduleUtil.Instance.schedule(delegate()
            {
                if (inCount < teamOneMap.Count + teamTwoMap.Count)
                {
                    brocast(SelectProtocol.ROOM_DESTORY_BRO, -1);
                    EventUtil.Instance.selectDestory(getArea());
                }
                else
                {
                    bool allSelect = true;
                    foreach (SelectModel item in teamOneMap.Values)
                    {
                        if (item.heroId == -1) {
                            allSelect = false;
                            break;
                        }
                    }
                    if (allSelect) {
                        foreach (SelectModel item in teamTwoMap.Values)
                        {
                            if (item.heroId == -1)
                            {
                                allSelect = false;
                                break;
                            }
                        }
                    }

                    //都已经进入了 判断是否都选择了角色 是则开始战斗 否则销毁
                    if (allSelect)
                    {
                        //全都准备好了 开始战斗吧
                        startFight();
                    }
                    else {
                        brocast(SelectProtocol.ROOM_DESTORY_BRO, -2);
                        EventUtil.Instance.selectDestory(getArea());
                    }
                }
            }, 60*1000);
        }


        public void MessageRecevie(AceNetFrameWork.ace.UserToken token, AceNetFrameWork.ace.auto.SocketModel message)
        {
            switch (message.command)
            {
                case SelectProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case SelectProtocol.SELECT_CREQ:
                    select(token, message.getMessage<int>());
                    break;
                case SelectProtocol.TALK_CREQ:
                    //预留 不做
                    break;
                case SelectProtocol.READY_CREQ:
                    ready(token);
                    break;
            }
        }

        private void ready(UserToken token)
        {
            int userId = getUserId(token);
            if (userId == -1) return;
            if (readyList.Contains(userId))
            {
                return;
            }
            SelectModel sm = null;
            if (teamOneMap.ContainsKey(userId))
            {
                sm = teamOneMap[userId];
            }
            else if (teamTwoMap.ContainsKey(userId))
            {
                sm = teamTwoMap[userId];
            }
            if (sm != null)
            {
                if (sm.heroId == -1)
                {
                    randomSelect(userId, sm);
                }
                sm.ready = true;
                brocast(SelectProtocol.READY_BRO, sm);
            }
            readyList.Add(userId);
            if (readyList.Count == teamOneMap.Count + teamTwoMap.Count)
            {
                //全都准备好了 开始战斗吧
                startFight();
            }
        }

        private void randomSelect(int userId,SelectModel model)
        {
            User user= getUser(userId);
            List<int> temp = new List<int>();
            temp.AddRange(user.heroList);
            if (teamOneMap.ContainsKey(userId))
            {
                foreach (SelectModel item in teamOneMap.Values)
                {
                    if (item.heroId != -1)
                    {
                        if (temp.Contains(item.heroId))
                        {
                            temp.Remove(item.heroId);
                        }
                    }
                }
            }
            else if (teamTwoMap.ContainsKey(userId))
            {
                foreach (SelectModel item in teamTwoMap.Values)
                {
                    if (item.heroId != -1)
                    {
                        if (temp.Contains(item.heroId))
                        {
                            temp.Remove(item.heroId);
                        }
                    }
                }
            }

            Random r = new Random();
            model.heroId = temp[r.Next(temp.Count)];
        }

        void startFight() {
            if (missionId != -1)
            {
                ScheduleUtil.Instance.removeMission(missionId);
            }
            EventUtil.Instance.fightStart(teamOneMap.Values.ToArray(),teamTwoMap.Values.ToArray());
            EventUtil.Instance.selectDestory(getArea());
        }

        private void select(UserToken token, int value)
        {
            User user = getUser(token);
            if (user == null) return;
            if (!user.heroList.Contains(value))
            {
                write(token, SelectProtocol.SELECT_SRES, null);
            }
            int userId = getUserId(token);
            SelectModel sm = null;
            if (teamOneMap.ContainsKey(userId))
            {
                foreach (SelectModel item in teamOneMap.Values)
                {
                    if (!canSelectSameHero && item.heroId == value) return;
                }
                sm = teamOneMap[userId];

            }
            else if (teamTwoMap.ContainsKey(userId))
            {
                foreach (SelectModel item in teamTwoMap.Values)
                {
                    if (!canSelectSameHero && item.heroId == value) return;
                }
                sm = teamTwoMap[userId];
            }
            if (sm != null)
            {
                sm.heroId = value;
                brocast(SelectProtocol.SELECT_BRO, sm);
            }
            
        }

        private new void enter(UserToken token)
        {
            if (base.enter(token))
            {
                inCount++;
            }
            SelectRoomDTO dto = new SelectRoomDTO();
            int userId = getUserId(token);
            if (teamOneMap.ContainsKey(userId))
            {
                teamOneMap[userId].entered = true;
            }
            else if (teamTwoMap.ContainsKey(userId))
            {
                teamTwoMap[userId].entered = true;
            }
            dto.teamOne = teamOneMap.Values.ToArray();
            dto.teamTwo = teamTwoMap.Values.ToArray();
            write(token, SelectProtocol.ENTER_SRES, dto);
            
            exBrocast(token,SelectProtocol.ENTER_BRO, userId);
            
        }

        public void ClientClose(AceNetFrameWork.ace.UserToken token)
        {
            leave(token);
            int userId = getUserId(token);
            if (teamOneMap.ContainsKey(userId) || teamTwoMap.ContainsKey(userId)) {
                if (missionId != -1) {
                    ScheduleUtil.Instance.removeMission(missionId);
                }
                brocast(SelectProtocol.ROOM_DESTORY_BRO, -3);
                EventUtil.Instance.selectDestory(getArea());
            }
        }

        public override int getType()
        {
            return Protocol.TYPE_SELECT;
        }
    }
}
