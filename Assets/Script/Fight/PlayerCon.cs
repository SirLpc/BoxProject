#define USING_RPG 

using UnityEngine;
using System.Collections;
using OneByOne;
using System;
using Constans;

public class PlayerCon : MonoBehaviour
{
    public static PlayerCon SelfPlayer { get; protected set; }

    public GameObject EyeRange;
    public GameObject hpObj;
#if !USING_RPG
    public MrpgcKeyboardMovementController moveController;
    public PlayerCombatController combatController;
    public PlayerStatController stateController;

    public enum PlayerState
    {
        IDLE, RUN, ATK, NOCON
    }
    protected PlayerState myState = PlayerState.IDLE;
#endif
    protected NavMeshAgent agent=null;

    protected Animator anim;

    public bool IsSelf { protected set; get; }

    private FightPlayerModel data;

    public PlayerState myState { get; protected set; }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

   public FightPlayerModel Data
    {
        get {return data;}
        set { data = value; }
    }

    public void ReadyMove()
    {
        myState = PlayerState.MOVING;
    }

    public void StartMove(MoveDTO dto)
    {
#if !USING_RPG
        moveController.NetWorkInputGet(dto);
#endif
        PlayerState ps = (PlayerState)dto.y;
        //Reset to IDEL state, not really move command!
        myState = ps;
        if (myState == PlayerState.IDEL)
            return;

        switch (myState)
        {
            case PlayerState.INIT:
            case PlayerState.IDEL:
            case PlayerState.MOVING:    /*之所以Moving还能移动，
                是因为，在本地点击之后，为了防止连点,已经设置为Moving状态了，
                但实际上还未收到服务器的真正移动通知*/
                var grid = MapView.GetGrid(dto);
                if (grid == null)
                    return;

                CompleteMove(grid, dto.userId);
                break;
            case PlayerState.ATK:
                break;
            case PlayerState.DAMAGE:
                break;
            case PlayerState.DEAD:
                break;
            default:
                break;
        }

       

        return;
        if (agent==null) agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();
        agent.SetDestination(new Vector3(dto.x, dto.y, dto.z));
        anim.SetInteger("state", 1);
    }

    private void CompleteMove(GridView grid, int userId)
    {
        switch (myState)
        {
            case PlayerState.INIT:
            case PlayerState.IDEL:
            case PlayerState.MOVING:
                var preGrid = transform.GetComponentInParent<GridView>();
                if (preGrid != null)
                {
                    if(preGrid.Equals(grid))
                        return;

                    preGrid.RemovePlayer(userId);
                    //todo mormal move
                    transform.SetParent(grid.transform);
                    transform.localPosition = Vector3.up * 1f;
                }
                else
                {
                    //todo init move
                    transform.SetParent(grid.transform);
                    transform.localPosition = Vector3.up * 1f;
                }
                grid.AddPlayer(userId);
                break;
        }
    }

    public void setTag(string tag)
    {
        gameObject.tag = tag;

        if(EyeRange)
            EyeRange.tag = tag;
    }

    // Update is called once per frame
    void Update()
    {
        hpChange();

        return;

        switch (myState)
        {
            case PlayerState.IDEL:
                break;
            case PlayerState.MOVING:
                if (agent.pathStatus==NavMeshPathStatus.PathComplete&& agent.remainingDistance <= 0 && !agent.pathPending)
                {
                    myState = PlayerState.IDEL;
                    anim.SetInteger("state", 0);
                }
                else
                {
                    if (agent.isOnOffMeshLink)
                    {
                        agent.CompleteOffMeshLink();
                    }
                }
                break;
            case PlayerState.INIT:
                break;
            case PlayerState.DEAD:
                break;
            default:
                break;
        }
    }

    void hpChange()
    {
        if (hpObj)
        {
            Vector3 tar = GameData.worldToTarget(transform.position);
            
            if (hpObj.transform.localPosition != tar)
            {
              
                hpObj.transform.localPosition = tar;
            }
        }
    }

    public void setHp(GameObject hp, Transform par)
    {
        hpObj = hp;
        hp.transform.parent = par;
        hp.transform.localScale = Vector3.one;
        hp.transform.localPosition = GameData.worldToTarget(transform.position);
    }

    protected void damage(int value) {
        data.hp -= value;
        if (data.hp < 0) data.hp = 0;
        if (data.hp > data.maxHp) data.hp = data.maxHp;

#if !USING_RPG
        stateController.TakeDamage(value);
#endif
        //hpObj.GetComponent<HpProcess>().hpChange((float)data.hp/data.maxHp);
    }
}
