using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class UserDTO
   {
       public string name;//用户昵称
       public int id;//用户ID
       public int level;//用户等级
       public int winCount;//胜利场次
       public int loseCount;//失败场次
       public int ranCount;//逃跑场次
       public List<int> heroList;//拥有英雄列表

       public UserDTO() { }
       public UserDTO(string name,int id,int level,int win,int lose,int ran,List<int> heros) {
           this.id = id;
           this.name = name;
           this.winCount = win;
           this.loseCount = lose;
           this.ranCount = ran;
           this.heroList = heros;
       }

    }
}
