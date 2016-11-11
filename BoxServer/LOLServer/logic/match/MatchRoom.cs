using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.logic.match
{
   public class MatchRoom
    {
       public int id;
       public int teamMax = 1;

       public List<int> teamOne = new List<int>();

       public List<int> teamTwo = new List<int>();
    }
}
