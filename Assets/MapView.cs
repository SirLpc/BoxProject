using UnityEngine;
using System.Collections;
using OneByOne;
using System.Collections.Generic;

public class MapView : MonoBehaviour
{
    [SerializeField]
    private Transform _mapParent;

    private static MapModel map;
    private static List<GridView> gridList = new List<GridView>();

    public void InitMap(MapModel mapModel)
    {
        map = mapModel;

        for (int i = 0; i < map.grids.Count; i++)
        {
            var g = map.grids[i];
            var gv = GridView.CreateGrid(g, _mapParent);
            if(!gridList.Contains(gv))
                gridList.Add(gv);
        }
    }

    public static GridView GetGrid(MoveDTO move)
    {
        var col_x = (int)move.x;
        var row_z = (int)move.z;
        var map = gridList.Find(g => g.GridModel.col == col_x && g.GridModel.row == row_z);
        return map;
    }

}
