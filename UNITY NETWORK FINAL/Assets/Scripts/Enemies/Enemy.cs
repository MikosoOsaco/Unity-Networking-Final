using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    [Header("Enemy Properties")]
    [SyncVar]
    public float health = 3;
       
    // Update is called once per frame
    void Update()
    {
        // Check health
        if (health <= 0)
        {
            CmdDie();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Get hit if enter collider tagged as PlayerWeapon
        if (other.tag == "PlayerWeapon")
        {
            CmdHit();
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
        RpcDie();
    }

    [ClientRpc]
    public void RpcDie()
    {
        transform.parent.gameObject.GetComponent<Skeleton>().audioSource.volume = 0.0f;
        Destroy(transform.parent.gameObject);
    }
}