using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
   public class Protocol
    {
       /// <summary>
       /// 用户
       /// </summary>
       public const int TYPE_USER = 0;
       /// <summary>
       /// 队伍
       /// </summary>
       public const int TYPE_TEAM = 1;
       /// <summary>
       /// 匹配队列
       /// </summary>
       public const int TYPE_MATCH = 2;
       /// <summary>
       /// 战斗
       /// </summary>
       public const int TYPE_FIGHT = 3;
       /// <summary>
       /// 登录
       /// </summary>
       public const int TYPE_LOGIN = 4;
       /// <summary>
       /// 对阵选择
       /// </summary>
       public const int TYPE_SELECT = 5;
    }
}
