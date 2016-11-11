using LOLServer.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.biz.impl
{
   public class AccountBiz:IAccountBiz
    {
       IAccountCache accountCache = CacheFactory.accountCache;

        public int login(AceNetFrameWork.ace.UserToken token, string account, string password)
       {
           if (account == null || password == null)
           {
               return -4;//输入不合法
           }
           if (!accountCache.hasAccount(account))
           {
               return -1;//帐号不存在
           }
           if (accountCache.isOnline(account)) {
               return -2;//帐号已登录
           }
           if (!accountCache.match(account, password)) {
               return -3;//帐号密码不匹配 简称--密码错误
           }
           
           accountCache.online(token, account);
            return 0;
        }

        public int create(AceNetFrameWork.ace.UserToken token, string account, string password)
        {
            if (account == null || password == null) {
                return -2;//输入不合法
            }
            if (accountCache.hasAccount(account)) {
                return -1;//帐号存在
            }
            accountCache.add(account, password);//写入新帐号
            return 0;
        }

        public void colse(AceNetFrameWork.ace.UserToken token)
        {
            accountCache.offline(token);
        }

       public int get(AceNetFrameWork.ace.UserToken token)
        {
           return accountCache.getId(token);
        }
    }
}
