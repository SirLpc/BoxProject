using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
    public class FightPlayerModel : AbsFightModel
    {
       public string name;//玩家名称
       public int heroId;//英雄ID
       public int free;//潜能点
       public int level;//英雄等级
       public int exp;//英雄经验       
       public float speed;//英雄移动速度
       public float aspeed;//英雄攻击速度
       public float range;//攻击距离
       public int money;//当前金钱
       public int[] equs;//当前物品栏
       public FightSkill[] skills;//英雄技能


       public int skillLevel(int code) {
           foreach (FightSkill item in skills)
           {
               if (item.id == code) return item.level;
           }
           return -1;
       }
    }
}
