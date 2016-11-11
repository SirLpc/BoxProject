using AceNetFrameWork.ace;
using AceNetFrameWork.ace.auto;
using LOLServer.biz;
using LOLServer.tool;
using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LOLServer.logic.login
{
   public class LoginHandler:AbsOnceHandler,HanderInterface
    {
       private IAccountBiz accountBiz=BizFactory.accountBiz;

        public void MessageRecevie(UserToken token, SocketModel message)
        {
            switch (message.command) { 
                case LoginProtocol.CREATE_CREQ:
                    create(token,message.message);
                    break;
                case LoginProtocol.LOGIN_CREQ:
                    login(token, message.message);
                    break;
            }
        }

        private void create(UserToken token, object message) {
             AccountDTO dto=message as AccountDTO;
            ExecutorPool.Instance.execute(delegate() {
                write(token, LoginProtocol.CREATE_SRES, accountBiz.create(token, dto.account, dto.password));
            });
        }

        private void login(UserToken token, object message) {
            AccountDTO dto = message as AccountDTO;
            ExecutorPool.Instance.execute(delegate()
            {
                write(token, LoginProtocol.LOGIN_SRES, accountBiz.login(token, dto.account, dto.password));
            });
        }

        public void ClientClose(UserToken token)
        {
            ExecutorPool.Instance.execute(delegate()
            {
                accountBiz.colse(token);
            });
        }

       public override int getType(){
           return Protocol.TYPE_LOGIN;
       }
    }
}
