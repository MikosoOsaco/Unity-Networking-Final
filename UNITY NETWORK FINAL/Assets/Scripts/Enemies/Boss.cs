using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Boss : NetworkBehaviour
{
    // Boss Components
    private Animator anim;
    private NavMeshAgent agent;

    // Boss Timers
    private float actionCDset;
    private float actionCDtimer;

    private float runCDset;
    private float runCDtimer;

    // States
    enum BossAction
    {
        IDLE,
        CHASE,
        ATTACK,
        DEATH
    }

    // Boss Variables
    private BossAction currAction;
    private GameObject currTarget;
    private Vector3 targetPos;
    GameObject[] playerList;
    [SyncVar]
    public float health = 1;
    [HideInInspector]
    [SyncVar]
    public bool chasePlayer;
    [SyncVar]
    private Vector3 goal = Vector3.zero;
    private bool players = false;

    // Use this for initialization
    void Start()
    {
        chasePlayer = false;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currAction = BossAction.IDLE;

        actionCDset = 1f;
        actionCDtimer = actionCDset;

        runCDset = 2f;
        runCDtimer = runCDset;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (health <= 0)
        {
            chasePlayer = false;
            if (isServer)
            {
                RpcDie();
            }
            else {
                CmdDie();
            }
        }
        if (chasePlayer)
        {
            actionCDtimer -= dt;
            runCDtimer -= dt;
            if (runCDtimer <= 0)
            {
                runCDtimer = 0;
            }
            if (currAction == BossAction.IDLE)
            {
                if (actionCDtimer <= 0)
                {
                    actionCDtimer = 0;
                    MakeDecision();
                }
            }
            else if (currAction == BossAction.CHASE)
            {
                if (isServer)
                {
                    RpcChasePlayer(targetPos);
                }
                else
                {
                    CmdChasePlayer(targetPos);
                }

                if (agent.transform.position == agent.destination || runCDtimer <= 0)
                {
                    currAction = BossAction.ATTACK;
                }
            }

            if (currAction == BossAction.ATTACK)
            {
                if (isServer)
                {
                    RpcAttackPlayer(targetPos);
                }
                else
                {
                    CmdAttackPlayer(targetPos);
                }
            }

            if (isServer)
            {
                RpcUpdateAgent();
            }
            else
            {
                CmdUpdateAgent();
            }
        }
    }

    private void MakeDecision()
    {
        playerList = GameObject.FindGameObjectsWithTag("PlayerMain");

        int i = Random.Range(0, playerList.Length);

        if (playerList[i].GetComponent<Interact>().bossArea)
        {
            targetPos = playerList[i].transform.position;
        }
        else
        {
            targetPos = transform.position;
        }
        runCDtimer = runCDset;
        currAction = BossAction.CHASE;
    }
    
    public void AttackFinish()
    {
        actionCDtimer = actionCDset;
        currAction = BossAction.IDLE;
        anim.SetBool("StandAttack", false);
        agent.isStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            if(isServer)
            {
                RpcHit();
            }
            else
            {
                CmdHit();
            }
        }
    }

    [Command]
    public void CmdHit()
    {
        RpcHit();
    }

    [ClientRpc]
    public void RpcHit()
    {
        health--;
    }

    [Command]
    public void CmdDie()
    {
        anim.SetBool("StandAttack", false);
        anim.SetBool("IsRunning", false);
        agent.isStopped = true;
        anim.SetBool("Death", true);
        currAction = BossAction.DEATH;
        RpcDie();
    }

    [ClientRpc]
    public void RpcDie()
    {
        anim.SetBool("StandAttack", false);
        anim.SetBool("IsRunning", false);
        agent.isStopped = true;
        anim.SetBool("Death", true);
        currAction = BossAction.DEATH;
    }

    [Command]
    void CmdAttackPlayer(Vector3 pos)
    {
        RpcAttackPlayer(pos);
        anim.SetBool("IsRunning", false);
        anim.SetBool("StandAttack", true);
        agent.isStopped = true;
        targetPos = transform.position;
    }

    [ClientRpc]
    void RpcAttackPlayer(Vector3 pos)
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("StandAttack", true);
        agent.isStopped = true;
        targetPos = transform.position;
    }

    [Command]
    public void CmdChasePlayer(Vector3 pos)
    {
        RpcChasePlayer(pos);
        anim.SetBool("IsRunning", true);
        goal = pos;
    }

    [ClientRpc]
    public void RpcChasePlayer(Vector3 pos)
    {
        anim.SetBool("IsRunning", true);
        goal = pos;
    }

    [Command]
    void CmdUpdateAgent()
    {
        RpcUpdateAgent();
        agent.destination = goal;
    }

    [ClientRpc]
    public void RpcUpdateAgent()
    {
        agent.destination = goal;
    }
    
    public void SetChasePlayer()
    {
        chasePlayer = true;
    }
}
