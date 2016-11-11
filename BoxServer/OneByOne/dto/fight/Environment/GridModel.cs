using Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
    public class GridModel
    {
        public int row;
        public int col;
        public GridState state;
        public List<int> player;
        public int obstacle;
        public int trap;
    }
}
