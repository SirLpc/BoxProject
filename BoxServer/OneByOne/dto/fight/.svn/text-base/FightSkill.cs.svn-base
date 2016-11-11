using Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    /// <summary>
    /// 战斗技能对象
    /// </summary>
    [Serializable]
   public class FightSkill
    {
       public int id;//技能ID
       public int level;//技能当前等级 初始化为0级
       public int nextLevel;//学习等级
       public int time;//冷却时间
       public int mp;//耗蓝
       public float range;//攻击范围
       public string name;
       public string info;
       public SkillTarget target;//技能目标类型
       public SkillType type;//技能是放类型

       public FightSkill() { }

       public FightSkill(int id, int level, int nextLevel, int time, int mp, float range, string name, string info, SkillTarget target,SkillType type)
       { 
            this.id=id;
            this.level = level;
            this.nextLevel = nextLevel;
            this.time = time;
            this.mp = mp;
            this.range = range;
            this.name = name;
            this.info = info;
            this.target = target;
            this.type = type;
       }
    }
}
