using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class AttackDTO
    {//这里是普通攻击--只能目标指向
       public int id;
       public int[] target;
    }
}
