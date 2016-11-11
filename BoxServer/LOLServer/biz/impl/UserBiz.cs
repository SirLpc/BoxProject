using AceNetFrameWork.ace;
using LOLServer.dao.model;
using LOLServer.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz.impl
{
   public class UserBiz:IUserBiz
    {
       Dictionary<int, User> idToUser = new Dictionary<int, User>();
       Dictionary<int, int> accToUid = new Dictionary<int, int>();

       Dictionary<int, UserToken> idToToken = new Dictionary<int, UserToken>();
       Dictionary<UserToken, int> tokenToId = new Dictionary<UserToken, int>();

       AtomicInt id = new AtomicInt();

       IAccountBiz accountBiz = BizFactory.accountBiz;

        public bool create(AceNetFrameWork.ace.UserToken token,string name)
        {
            int acc= accountBiz.get(token);
            if (acc == -1) return false;
            User user= get(token);
            if (user != null) return false;
            //创建角色
            user = new User(name, id.getAndAdd(), acc);
            accToUid.Add(acc, user.id);
            idToUser.Add(user.id, user);
            return true;
        }

        public dao.model.User get(AceNetFrameWork.ace.UserToken token)
        {

            if (tokenToId.ContainsKey(token)) return idToUser[tokenToId[token]];
            return null;
        }



        public bool hasHero(int userId, int heroCode)
        {
            return idToUser[userId].heroList.Contains(heroCode);
        }

        public void addHero(int userId, int heroCode)
        {
            idToUser[userId].heroList.Add(heroCode);
        }

        public void addResult(int userId, int value)
        {
            User user= idToUser[userId];
            switch (value) { 
                case 1:
                    user.winCount += 1;
                    break;
                case 2:
                    user.loseCount += 1;
                    break;
                case 3:
                    user.ranCount += 1;
                    break;
            }
        }

        public User get(int id)
        {
            return idToUser[id];
        }

        public User online(UserToken token)
        {
            if (get(token) != null) return get(token);
            User user=getByAccount(token);
            if (user == null) return null;
            idToToken.Add(user.id, token);
            tokenToId.Add(token, user.id);
            return get(token);
        }

        public void offline(UserToken token)
        {
            if (tokenToId.ContainsKey(token))
            {
                if (idToToken.ContainsKey(tokenToId[token])) {
                    idToToken.Remove(tokenToId[token]);
                }
                tokenToId.Remove(token);
            }
        }


        public User getByAccount(UserToken token)
        {
            int acc= accountBiz.get(token);
            if (acc == -1) return null;
            if (!accToUid.ContainsKey(acc)) return null;
            return idToUser[accToUid[acc]];
        }


        public UserToken getToken(int id)
        {
            if (idToToken.ContainsKey(id))
                return idToToken[id];
            return null;
        }
    }
}
