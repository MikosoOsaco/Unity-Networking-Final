using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Skeleton : NetworkBehaviour
{
    // SKELETON STATUS
    bool attacking = false;
    bool chasing = false;
    bool dead = false;

    // SKELETON AI VARIABLES
    [HideInInspector]
    [SyncVar]
    private Vector3 goal = Vector3.zero;
    [HideInInspector]
    [SyncVar]
    private Vector3 startPos = Vector3.zero;

    // SKELETON MUSIC VARIABLES
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public float musicVol = 0;    

    // SKELETON ANIMATOR
    Animator animator;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        goal = startPos;
        audioSource = new AudioSource();
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Update navmesh agent
        if (!attacking)
        {
            if (isServer)
            {
                RpcUpdateAgent();
            }
            else
            {
                CmdUpdateAgent();
            }
        }

        // Update music
        // If chasing player raise volume
        if (audioSource != null)
        {
            audioSource.volume = musicVol;
            if (chasing)
            {
                musicVol += 0.15f * Time.deltaTime;
                if (musicVol >= 0.5f)
                {
                    musicVol = 0.5f;
                }
            }
            else
            {
                musicVol -= 0.15f * Time.deltaTime;
                if (musicVol <= 0.0f)
                {
                    musicVol = 0.0f;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Chase player when in range
        if (other.tag == "PlayerMain" && GetComponent<Collider>().GetType() == typeof(CapsuleCollider))
        {
            CmdChasePlayer(other.transform.position);
            if (other.gameObject.tag == "PlayerMain")
            {
                if (other.gameObject.GetComponent<AudioSource>() != null)
                {
                    audioSource = other.gameObject.GetComponent<AudioSource>();
                }
                chasing = true;
            }   
        }
    }

    [Command]
    void CmdChasePlayer(Vector3 pos)
    {
        RpcChasePlayer(pos);
        if (animator != null)
            animator.SetBool("enterChaseArea", true);
        goal = pos;
    }

    [ClientRpc]
    void RpcChasePlayer(Vector3 pos)
    {
        if (animator != null)
            animator.SetBool("enterChaseArea", true);
        goal = pos;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Attack player when in range
        if (collision.gameObject.tag == "PlayerMain")
        {
            CmdAttackPlayer();
        }
    }

    [Command]
    void CmdAttackPlayer()
    {
        RpcAttackPlayer();
        if (animator != null)
            animator.SetBool("enterAttackArea", true);
        attacking = true;
    }

    [ClientRpc]
    void RpcAttackPlayer()
    {
        if (animator != null)
            animator.SetBool("enterAttackArea", true);
        attacking = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        CmdStopAttackPlayer();
    }

    [Command]
    void CmdStopAttackPlayer()
    {
        RpcStopAttackPlayer();
        if (animator != null)
            animator.SetBool("enterAttackArea", false);
        attacking = false;
    }

    [ClientRpc]
    void RpcStopAttackPlayer()
    {
        if (animator != null)
            animator.SetBool("enterAttackArea", false);
        attacking = false;
    }


    private void OnTriggerExit(Collider other)
    {
        CmdStopChasePlayer(startPos);
        if (other.gameObject.tag == "PlayerMain")
        {
            chasing = false;
        }
    }

    [Command]
    void CmdStopChasePlayer(Vector3 pos)
    {
        RpcStopChasePlayer(pos);
        goal = pos;
    }

    [ClientRpc]
    void RpcStopChasePlayer(Vector3 pos)
    {
        goal = pos;
    }

    [Command]
    void CmdUpdateAgent()
    {
        RpcUpdateAgent();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal;

        if (agent.transform.position == agent.destination)
        {
            if (animator != null)
                animator.SetBool("enterChaseArea", false);
        }
    }

    [ClientRpc]
    public void RpcUpdateAgent()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal;

        if (agent.transform.position == agent.destination)
        {
            if (animator != null)
                animator.SetBool("enterChaseArea", false);
        }
    }
}