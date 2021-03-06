﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class FightBuildModel:AbsFightModel
    {
       public int code;       
       public bool reBorn;//是否重生
       public int rebornTime;//重生时间
       public bool initiative;//是否攻击型建筑
       public bool infrared;//不可否认，这货字面意思是红外线，但是红外代表夜视，所以这里咱用来表示是否反隐吧
       public string name;//还是给个名字 用来区分下吧
       public FightBuildModel() { }
       public FightBuildModel(int id, int code, int hp, int hpMax, int atk, int def, bool reborn, int rebornTime, bool initiative, bool infrared, string name)
       {
           this.id = id;
           this.code = code;
           this.hp = hp; this.maxHp = hpMax;
           this.atk = atk;
           this.def = def;
           this.reBorn = reborn;
           this.rebornTime = rebornTime;
           this.initiative = initiative;
           this.infrared = infrared;
           this.name = name;
       
       }
    }
}
