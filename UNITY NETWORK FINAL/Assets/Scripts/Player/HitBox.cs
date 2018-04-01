using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitBox : NetworkBehaviour {

    // Methods called by the player's animations which determine if the player can attack and when the hitboxes are active 

    public GameObject player;
    

    public void CanAttack(int i)
    {
        if (player.GetComponent<Player>() != null)
        {
            if (i == 0)
            {
                player.GetComponent<Player>().canAttack = true;
            }
            else if(i == 1)
            {
                player.GetComponent<Player>().canAttack = false;
            }
        }
    }

    public void AttackStart()
    {
        if (player.GetComponent<Player>() != null)
        {
            if (isServer)
            {
                player.GetComponent<Player>().RpcAttack();
            }
            else
            {
                player.GetComponent<Player>().CmdAttack();
            }
        }
    }

    public void AttackFinish()
    {
        if (player.GetComponent<Player>() != null)
        {
            if (isServer)
            {
                player.GetComponent<Player>().RpcDone();
            }
            else
            {
                player.GetComponent<Player>().CmdDone();
            }
        }
    }
}
