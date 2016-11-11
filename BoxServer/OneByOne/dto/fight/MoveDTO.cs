using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class MoveDTO
    {
       public int userId;
        /// <summary>
        /// col
        /// </summary>
       public float x;
        /// <summary>
        /// 0 == INIT, 1 == IDEL, 2 == MOVING
        /// </summary>
       public float y;
        /// <summary>
        /// row
        /// </summary>
       public float z;
    }
}
