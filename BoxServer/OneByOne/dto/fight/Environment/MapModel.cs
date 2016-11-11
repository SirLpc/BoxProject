using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneByOne
{
    [Serializable]
    public class MapModel
    {
        public int rolNum;

        public int colNum;

        public int style;

        public List<GridModel> grids;

    }
}
