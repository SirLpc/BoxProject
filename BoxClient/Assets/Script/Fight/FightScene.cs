﻿using UnityEngine;
using System.Collections;
using OneByOne;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Utility;

public class FightScene : MonoBehaviour {
    [HideInInspector]
    public int myTeam=0;
    [HideInInspector]
    public GameObject myHero;

    public static FightPlayerModel player;

    [SerializeField]
    private MapView map; 

    [Header("--Old--")]

    public Image myHead;

    public GameObject mainCamera;

    public Transform par;

    public RectTransform HPCanvas;

    public FightSkillGrid[] skillGrids;

    [SerializeField]
    private SkillTip skillTipBase;

    public static SkillTip skillTip;


    IEnumerator Start()
    {
        yield return null;

        //Screen.fullScreen = true;
        GameData.Par = HPCanvas;
        skillTip = skillTipBase;
        NetWorkScript.Instance.write(Protocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
    }

    /// <summary>
    /// map, skill, and so on..
    /// </summary>
    public void initUI()
    {
        return;

        //initSkill();
    }

    private void initSkill()
    {
        int i = 0;
        foreach (FightSkill item in player.skills)
        {
            skillGrids[i].setIcon(player.heroId, item);
            i++;
        }
    }

    public void refreshUI()
    {
        return;

        //refreshSkill();
    }
    private void refreshSkill()
    {
        int i = 0;
        foreach (FightSkill item in player.skills)
        {

            if (item.nextLevel <= player.level)
            {
                if (player.free > 0)
                {
                    skillGrids[i].btnState(true);
                }
                else
                {
                    skillGrids[i].btnState(false);
                }
            }
            else
            {
                skillGrids[i].btnState(false);
            }
            skillGrids[i].skillChange(item);
            skillGrids[i].setMask(0);

            i++;
        }
    }

    public void initMap(MapModel mapModel)
    {
        map.InitMap(mapModel);
    }

    public void sceneLefttClick(Vector2 position) {
        Debug.Log("zou键点击屏幕" + position);
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 300);
        for (int i = hits.Length-1; i >=0; i--)
        {
            RaycastHit hit=hits[i];

            if(hit.collider.CompareTag(Tags.TerrianLayer))
            {
                if (PlayerCon.SelfPlayer.myState == Constans.PlayerState.MOVING ||
                    PlayerCon.SelfPlayer.myState == Constans.PlayerState.DEAD)
                    return;

                GridView gv = hit.collider.GetComponent<GridView>();
                if(gv && gv.GridModel.state == Constans.GridState.EMPTY)
                {
                    MoveDTO m = new MoveDTO();
                    m.x = gv.GridModel.col;
                    m.y = 1;
                    m.z = gv.GridModel.row;
                    NetWorkScript.Instance.write(Protocol.TYPE_FIGHT, 0, FightProtocol.MOVE_CREQ, m);
                    PlayerCon.SelfPlayer.ReadyMove();
                    return;
                }
            }

            if ((hit.collider.tag == "enemyHero" || hit.collider.tag == "enemysoldier" || hit.collider.tag == "neutral") && hit.collider.gameObject.layer == LayerMask.NameToLayer("visible"))
            {
                FightPlayerModel m= hit.transform.gameObject.GetComponent<PlayerCon>().Data;
                if (Vector3.Distance(myHero.transform.position, hit.transform.position) <= player.range)
                {
                    NetWorkScript.Instance.write(Protocol.TYPE_FIGHT, 0, FightProtocol.ATTACK_CREQ, new int[] { m.id });
                    return;
                }
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                MoveDTO m=new MoveDTO();
                m.x=hit.point.x;m.y=hit.point.y;m.z=hit.point.z;
                NetWorkScript.Instance.write(Protocol.TYPE_FIGHT, 0, FightProtocol.MOVE_CREQ, m);
                return;
            }
        }
    }

    public void sceneRightClick(Vector2 position)
    {
        Debug.Log("you键点击屏幕" + position);
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            Debug.Log(hit.transform.gameObject.layer);
        }
    }

    private int cameraState = 0;

   public void cameraMove(int value)
    {
        cameraState = value;
        switch (cameraState)
        {
            case 0:
                
                break;
            case 1:
                if (mainCamera.transform.position.z < 150)
                    mainCamera.transform.Translate(Vector3.forward);
                
                break;
            case 2:
                if (mainCamera.transform.position.z > 10)
                    mainCamera.transform.Translate(Vector3.back);
                
                break;
            case 3:
                if (mainCamera.transform.position.x >10)
                mainCamera.transform.Translate(Vector3.left);
                break;
            case 4:
                if (mainCamera.transform.position.x <145)
                mainCamera.transform.Translate(Vector3.right); break;

        }
    }

    public void InitFollowCamera(GameObject myPlayer)
    {
        GetComponent<SmoothFollow>().InitFollow(myPlayer.transform, Camera.main.transform);
    }

    public void cameraReset() {
       mainCamera.transform.position= myHero.transform.position+new Vector3(-6, 8, 0);
    }

    void OnDestroy()
    {
        //Screen.SetResolution(1024, 768, false);
    }

    void OnApplicationQuit()
    {
        OnDestroy();
    }
}
