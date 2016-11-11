using AceNetFrameWork.ace;
using LOLServer.dao.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.biz
{
   public interface IAccountBiz
    {
       /// <summary>
       /// 登录
       /// </summary>
       /// <param name="token">用户连接对象</param>
       /// <param name="account">帐号</param>
       /// <param name="password">密码</param>
       /// <returns>返回登录结果 成功返回0 失败返回错误代码</returns>
       int login(UserToken token,string account, string password);
       /// <summary>
       /// 创建
       /// </summary>
       /// <param name="token">用户连接对象</param>
       /// <param name="account">帐号</param>
       /// <param name="password">密码</param>
       /// <returns>返回创建结果代码</returns>
       int create(UserToken token, string account, string password);
       /// <summary>
       /// 断开
       /// </summary>
       /// <param name="token">用户连接对象</param>
       void colse(UserToken token);

       /// <summary>
       /// 获取当前连接的帐号id
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       int get(UserToken token);
    }
}
