using AceNetFrameWork.ace;
using LOLServer.dao.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LOLServer.cache.impl
{
   public class AccountCache:IAccountCache
    {
       Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();

       Dictionary<string, Account> accMap = new Dictionary<string, Account>();

       private int atomicId = 0;
       
        public bool hasAccount(string account)
        {

            return accMap.ContainsKey(account);
        }

        public bool match(string account, string password)
        {
            if (!hasAccount(account)) return false;

           return accMap[account].password == password;
        }

        public bool isOnline(string account)
        {
           return onlineAccMap.ContainsValue(account);
        }

        public int getId(AceNetFrameWork.ace.UserToken token)
        {
            if (!onlineAccMap.ContainsKey(token)) return -1;
           return accMap[onlineAccMap[token]].id;
        }

        public void online(AceNetFrameWork.ace.UserToken token, string account)
        {
            onlineAccMap.Add(token, account);
        }

        public void offline(AceNetFrameWork.ace.UserToken token)
        {
            if (onlineAccMap.ContainsKey(token)) onlineAccMap.Remove(token);
        }


        public void add(string account, string password)
        {
            Account acc=new Account();
            acc.account=account;
            acc.password=password;
            acc.id=atomicId;
            accMap.Add(account,acc);
            atomicId++;          
        }
    }
}
