using AceNetFrameWork.ace;
using AceNetFrameWork.ace.auto;
using LOLServer.biz;
using LOLServer.dao.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.logic
{
   public class AbsOnceHandler
    {
       IUserBiz userBiz = BizFactory.userBiz;

       private int type;
       private int area;

       public void setArea(int area) {
           this.area = area;
       }

       public void setType(int type) {
           this.type = type;
       }

       public User getUser(int id)
       {
           return userBiz.get(id);
       }

       public User getUser(UserToken token)
       {
           return userBiz.get(token);
       }

       public int getUserId(UserToken token) {
           User user = getUser(token);
           if (user == null) return -1;
           return user.id;
       }

       public int getArea() {
           return area;
       }

       public virtual int getType() {
           return type;
       }

       public void write(UserToken token, int command)
       {
           write(token, getType(), getArea(), command, null);
       }

       public void write(UserToken token, int command, object message) {
           write(token, getType(), getArea(), command, message);
       }

       public void write(UserToken token, int area, int command, object message)
       {
           write(token, getType(), area, command, message);
       }

       

       public void write(UserToken token, int type, int area, int command, object message) {
           byte[] bs= MessageEncoding.Encode(createSocketModel(type, area, command, message));
           bs=LengthEncoding.encode(bs);
           Console.WriteLine("有消息发出");
           token.write(bs);
       }

       public void write(int id, int command)
       {
           write(id, getType(), getArea(), command, null);
       }

       public void write(int id, int command, object message)
       {
           write(id, getType(), getArea(), command, message);
       }

       public void write(int id, int area, int command, object message)
       {
           write(id, getType(), area, command, message);
       }



       public void write(int id, int type, int area, int command, object message)
       {
           UserToken token = userBiz.getToken(id);
           if (token == null) return;
           Console.WriteLine("有消息发出");
           byte[] bs = MessageEncoding.Encode(createSocketModel(type, area, command, message));
           bs = LengthEncoding.encode(bs);
           token.write(bs);
       }

       public void writeToUsers(int[] users, int type, int area, int command, object message) {
           byte[] bs = MessageEncoding.Encode(createSocketModel(type, area, command, message));
           bs = LengthEncoding.encode(bs);
           foreach (int item in users)
           {
               UserToken token = userBiz.getToken(item);
               if (token != null) {
                   Console.WriteLine("群发给指定ID");
                   byte[] truebs = new byte[bs.Length];
                   Array.Copy(bs, 0, truebs, 0, bs.Length);
                   token.write(truebs);
               }
           }
       }

       public SocketModel createSocketModel(int type, int area, int command, object message) {
           return new SocketModel(type, area, command, message);
       }
    }
}
