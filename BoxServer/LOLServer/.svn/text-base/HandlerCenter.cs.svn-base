using LOLServer.logic;
using LOLServer.logic.login;
using LOLServer.logic.match;
using LOLServer.logic.selecthero;
using LOLServer.logic.user;
using LOLServer.tool;
using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AceNetFrameWork.ace.auto
{
    public class HandlerCenter:AbsHandlerCenter
    {
        private HanderInterface user;
        private HanderInterface login;
        private HanderInterface match;
        private HanderInterface select;
        private HanderInterface fight;
        public HandlerCenter() {
            user = new UserHandler();
            login = new LoginHandler();
            match = new MatchHandler();
            select = new SelectHandler();
            fight = FightHandler.Instace;
        }

        public override void MessageReceive(UserToken token, object message)
        {
            Console.WriteLine("有消息到达"+(message as SocketModel).ts());
            SocketModel model=message as SocketModel;
            switch (model.type) { 
                case Protocol.TYPE_USER:
                    user.MessageRecevie(token, model);
                    break;
                case Protocol.TYPE_TEAM:
                    break;
                case Protocol.TYPE_MATCH:
                    match.MessageRecevie(token, model);
                    break;
                case Protocol.TYPE_FIGHT:
                    fight.MessageRecevie(token, model);
                    break;
                case Protocol.TYPE_LOGIN:
                    login.MessageRecevie(token, model);
                    break;
                case Protocol.TYPE_SELECT:
                    select.MessageRecevie(token, model);
                    break;
            }
           
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接"+token.connectSocket.AddressFamily.ToString());
           
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开" +"---error:"+error);
            fight.ClientClose(token);
            match.ClientClose(token);
            select.ClientClose(token);
            login.ClientClose(token);
            user.ClientClose(token);
        }
    }
}
