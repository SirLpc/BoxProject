using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OneByOne;
using UnityEngine;

public class GridView : MonoBehaviour
{
    private static GameObject _gridPref = null;
    private static GameObject GridPref
    {
        get
        {
            if (_gridPref == null)
                _gridPref = Resources.Load<GameObject>(Tags.GridPath);
            return _gridPref;
        }
    }

    private GridModel _gridModel;
    public GridModel GridModel {
        get {
            return _gridModel;
        }
    }

    public static GridView CreateGrid(GridModel gridModel, Transform parent)
    {
        var tr = Instantiate(GridPref).transform;
        tr.SetParent(parent, false);
        tr.localPosition = new Vector3(gridModel.col, 0, gridModel.row);
        var gv = tr.GetComponent<GridView>();
        gv.InitGrid(gridModel);
        return gv;
    }

    public void RemovePlayer(int userId)
    {
        _gridModel.player.Remove(userId);
        _gridModel.state = Constans.GridState.EMPTY;
    }

    public void AddPlayer(int userId)
    {
        _gridModel.player.Add(userId);
        _gridModel.state = Constans.GridState.STAND;
    }

    private void InitGrid(GridModel gridModel)
    {
        _gridModel = gridModel;
        if (_gridModel.player == null)
            _gridModel.player = new List<int>();

        if (_gridModel.state == Constans.GridState.OBSTACLE)
            SetObstacle();
    }

    private void SetObstacle()
    {
        ObstacleView.CreateObstacle(transform);
    }


}

