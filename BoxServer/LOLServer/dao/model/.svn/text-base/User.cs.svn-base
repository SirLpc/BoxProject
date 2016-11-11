using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.dao.model
{
   public class User
    {
       public string name;//用户昵称
       public int id;//用户ID
       public int accId;//用户帐号ID
       public int level=0;//用户等级
       public int winCount=0;//胜利场次
       public int loseCount=0;//失败场次
       public int ranCount=0;//逃跑场次
       public List<int> heroList = new List<int>();//拥有英雄列表

       public User() { }
       public User(string name,int id,int accId) {
           this.name = name;
           this.id = id;
           this.accId = accId;
           for (int i = 1; i <= 12; i++) {
               heroList.Add(i);
           }
       }
    }
}
