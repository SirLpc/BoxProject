using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class SelectRoomDTO
    {
       public SelectModel[] teamOne;
       public SelectModel[] teamTwo;


       public int inTeam(int id) {
           foreach (SelectModel item in teamOne)
           {
               if (item.userId == id) return 1;
           }
           foreach (SelectModel item in teamTwo)
           {
               if (item.userId == id) return 2;
           }
           return -1;
       }
    }
}
