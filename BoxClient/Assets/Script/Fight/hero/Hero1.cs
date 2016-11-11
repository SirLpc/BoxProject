#define USING_RPG 

using UnityEngine;
using System.Collections;
using Constans;

public class Hero1:PlayerCon,IHero {

    public GameObject effect;

    private GameObject[] targets;
    private Transform target;
    private int skillCode;

    public void attack(GameObject[] targets)
    {
        #if !USING_RPG
        combatController.NetWorkAttackGet();
#endif
        return;

        if (PlayerState.MOVING == myState)
        {
            agent.CompleteOffMeshLink(); ;
        }
        this.targets = targets;
        gameObject.transform.LookAt(targets[0].transform);
        myState = PlayerState.ATK;
        anim.SetInteger("state", 2);
    }

    public void attacked()
    {
        myState = PlayerState.ATK;
        foreach (GameObject item in targets)
        {
            GameObject go= (GameObject)Instantiate(effect, transform.position + new Vector3(0, 1),transform.rotation);
            go.GetComponent<TargetSkill>().setData(item,-1,Data.id);
        }
        myState = PlayerState.IDEL;
        anim.SetInteger("state", 0);
    }

    public void skill(int code, GameObject[] targets)
    {
        
    }

    public void skill(int code, Transform target)
    {
        
    }

    public void skilled()
    {
        
    }


    public OneByOne.FightPlayerModel getData()
    {
        return base.Data;
    }

    public void setData(OneByOne.FightPlayerModel model)
    {
        base.Data = model;
        #if !USING_RPG
        base.stateController.NetSpawn(model);
#endif
        myState = PlayerState.INIT;
        //在玩家点击开始前   隐藏不显示
        transform.position = Vector3.left * 999;
        IsSelf = model.id == GameData.user.id;
        SetSelfMark();
        if (IsSelf)
            PlayerCon.SelfPlayer = this;
    }

    private void SetSelfMark()
    {
        Color c = IsSelf ? Color.green : Color.red;
        GetComponent<Renderer>().material.color = c;
    }

    public GameObject getHpObj()
    {
        return hpObj;
    }


    public PlayerState getState()
    {
        return myState;
    }

    public void damage(int value)
    {
        base.damage(value);
    }
}
