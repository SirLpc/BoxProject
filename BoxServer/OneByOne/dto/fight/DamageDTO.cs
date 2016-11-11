using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class DamageDTO
    {
        /// <summary>
        ///  攻击者ID
        /// </summary>
        public int id;

        /// <summary>
        ///  技能ID
        /// </summary>
        public int skill;
        /// <summary>
        /// 双向不同格式
        /// CtoS 数组格式[[被攻击者ID]]
        /// StoC数组格式[[被攻击ID,伤害值，是否死亡（0死亡 1 未死亡）]]
        /// </summary>
        public int[][] targetDamage;
    }
}
