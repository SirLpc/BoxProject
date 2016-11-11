using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ObstacleView : MonoBehaviour
{

    private static GameObject _obstaclePref;
    public static GameObject ObstaclePref {
        get {
            if (_obstaclePref == null)
                _obstaclePref = Resources.Load<GameObject>(Tags.ObstaclePath);
            return _obstaclePref;
        }
    }

    public static void CreateObstacle(Transform parent)
    {
        var tr = Instantiate(ObstaclePref).transform;
        tr.SetParent(parent, false);
        tr.GetComponent<ObstacleView>().InitObstacle();
    }

    private void InitObstacle()
    {
        transform.localPosition = Vector3.up * .5f;
    }
}

