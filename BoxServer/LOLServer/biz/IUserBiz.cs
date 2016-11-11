using AceNetFrameWork.ace;
using LOLServer.dao.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
   public interface IUserBiz
    {
        /// <summary>
        /// 创建用户角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool create(UserToken token,string name);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        User get(UserToken token);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"> 用户ID</param>
        /// <returns></returns>
        User get(int id);

        /// <summary>
        /// 用户是否拥有指定英雄
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="heroCode"></param>
        /// <returns></returns>

        bool hasHero(int userId, int heroCode);
        /// <summary>
        /// 给指定用户添加指定英雄
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="heroCode"></param>

        void addHero(int userId, int heroCode);
        /// <summary>
        /// 添加战斗结果
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="value">战斗结果 1 胜利 2 失败 3 逃跑</param>
        void addResult(int userId, int value);

        User online(UserToken token);

        void offline(UserToken token);
        User getByAccount(UserToken token);
        UserToken getToken(int id);
    }
}
