using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OpenBossDoors : NetworkBehaviour {

    [Header("Door Options")]
    public Transform door, doorOpen;
    public float openSpeed = 5F;                
    public GameObject boss;

    [HideInInspector]
    [SyncVar]
    public bool _open = false;

    void Update()
    {
        if (boss.GetComponent<Boss>().health < 1f)
        {
            _open = true;
        }

        if (_open)
        {
            if (isServer)
            {
                RpcDoorClicked(doorOpen.transform.rotation);
            }
            else
            {
                CmdDoorClicked();
            }
        }
    }

    [Command]
    public void CmdDoorClicked()
    {
        RpcDoorClicked(doorOpen.transform.rotation);
    }

    [ClientRpc]
    public void RpcDoorClicked(Quaternion toRot)
    {
        if (door.transform.rotation != toRot)
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, toRot, Time.deltaTime * openSpeed);
        }
    }


    public void Open()
    {
        _open = true;
    }
}
