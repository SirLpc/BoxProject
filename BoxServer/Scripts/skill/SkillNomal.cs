using Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public class SkillNomal:ISkill
    {

        public void damage(int level,ref OneByOne.AbsFightModel atkPlayer, ref OneByOne.AbsFightModel beAtk, FightModelType atkType, FightModelType beAtkType, bool isfriend, ref List<int[]> damages)
        {
            int value= atkPlayer.atk - beAtk.def;
            beAtk.hp= beAtk.hp - value <= 0 ? 0 : beAtk.hp - value;
            damages.Add(new int[] {beAtk.id,value,beAtk.hp==0?0:1});
        }
    }
}
