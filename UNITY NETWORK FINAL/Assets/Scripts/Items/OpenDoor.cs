using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OpenDoor : NetworkBehaviour
{
    [Header("Door Options")]
    public Transform door, doorOpen;
    public float openSpeed = 5F;

    [HideInInspector]
    [SyncVar]
    public bool _open = false;	                    

    void Update()
    {
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