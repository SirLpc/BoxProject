using Constans;
using OneByOne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts
{
    public interface ISkill
    {
        /// <summary>
        /// 造成伤害
        /// </summary>
        /// <param name="level">技能等级</param>
        /// <param name="atkPlayer">攻击者</param>
        /// <param name="beAtk">被攻击者</param>
        /// <param name="atkType">攻击者类型</param>
        /// <param name="beAtkType">被攻击者类型</param>
        /// <param name="isfriend">是否友方单位</param>
        /// <param name="damages">伤害列表</param>
        void damage(int level,ref AbsFightModel atkPlayer,ref AbsFightModel beAtk, FightModelType atkType, FightModelType beAtkType, bool isfriend,ref List<int[]> damages);
    }
}
