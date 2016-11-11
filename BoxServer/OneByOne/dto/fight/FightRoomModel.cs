using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
   public class FightRoomModel
    {
        public FightPlayerModel[] teamOne;
        public FightPlayerModel[] teamTwo;

        //队伍建筑映射表
        public FightBuildModel[] teamOneBuildMap;
        public FightBuildModel[] teamTwoBuildMap;

        public MapModel map;
    }
}
