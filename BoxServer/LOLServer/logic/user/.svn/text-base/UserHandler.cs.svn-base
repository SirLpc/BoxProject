using AceNetFrameWork.ace;
using AceNetFrameWork.ace.auto;
using LOLServer.biz;
using LOLServer.dao.model;
using LOLServer.tool;
using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.logic.user
{
    public class UserHandler :AbsOnceHandler, HanderInterface
    {
        IUserBiz userBiz = BizFactory.userBiz;
        public void MessageRecevie(UserToken token, SocketModel message)
        {
            switch (message.command) { 
                case UserProtocol.CREATE_CREQ:
                    create(token, message.message as string);
                    break;
                case UserProtocol.GET_CREQ:
                    get(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    online(token);
                    break;
            }
        }

        private void online(UserToken token) {
            ExecutorPool.Instance.execute(delegate() {
                write(token, UserProtocol.ONELINE_SRES, convert(userBiz.online(token)));
            });
        }

        private void create(UserToken token,string name)
        {
            ExecutorPool.Instance.execute(delegate() {
               write(token,UserProtocol.CREATE_SRES, userBiz.create(token, name));
            });
        }

        private void get(UserToken token) {
            ExecutorPool.Instance.execute(delegate() {
                write(token, UserProtocol.GET_SRES, convert(userBiz.getByAccount(token)));
            });
        }

        UserDTO convert(User user)
        {
            if (user == null) return null;
            UserDTO dto=new UserDTO(user.name,user.id,user.level,user.winCount,user.loseCount,user.ranCount,user.heroList);
            return dto;

        }

        public void ClientClose(UserToken token)
        {
            ExecutorPool.Instance.execute(delegate()
            {
                userBiz.offline(token);
            });
        }
    }
}
