using AceNetFrameWork.ace;
using AceNetFrameWork.ace.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace LOLServer.logic
{
   public class AbsMulitHandler:AbsOnceHandler
    {
      public List<UserToken> list = new List<UserToken>();
       public void brocast(int command, object message)
       {
           brocast(getArea(), command, message);
       }

       public void exBrocast(UserToken token,int command, object message)
       {
           byte[] bs = MessageEncoding.Encode(createSocketModel(getType(), getArea(), command, message));
           bs = LengthEncoding.encode(bs);
           Console.WriteLine("有排除模式群发");
           //遍历当前模块所有用户 进行消息发送
           foreach (UserToken item in list)
           {
               if (item.Equals(token)) continue;
               byte[] truebs = new byte[bs.Length];
               Array.Copy(bs, 0, truebs, 0, bs.Length);
               item.write(truebs);
           }
       }

       public void brocast(int area, int command, object message)
       {
           brocast(getType(), getArea(), command, message);
       }

       public void brocast(int type,int area,int command,object message) {
           byte[] bs = MessageEncoding.Encode(createSocketModel(type, area, command, message));
           bs = LengthEncoding.encode(bs);
           Console.WriteLine("有群发");
           //遍历当前模块所有用户 进行消息发送
           foreach (UserToken item in list)
           {
               byte[] truebs = new byte[bs.Length];
               Array.Copy(bs, 0, truebs, 0, bs.Length);
               item.write(truebs);
           }
       }

       public bool enter(UserToken token)
       {
           if (list.Contains(token)) return false;
           list.Add(token);
           return true;
       }

       public bool entered(UserToken token) {
           return list.Contains(token);
       }

       public bool leave(UserToken token)
       {
           if (list.Contains(token))
           {
               list.Remove(token);
               return true;
           }
           return false;
       }
    }
}
