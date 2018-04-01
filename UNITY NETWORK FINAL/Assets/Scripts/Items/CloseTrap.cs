using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CloseTrap : NetworkBehaviour
{
    [Header("Trap Options")]
    public GameObject pit, pitOpen;
    public float openSpeed = 5F;               
    public GameObject chest;

    [HideInInspector]
    [SyncVar]
    public bool _open = true;                   
    
    void Update()
    {
        if (chest.GetComponent<ActivateChest>()._open == true)
        {
            _open = true;
        }

        if (_open )
        {
            if (isServer)
            {
                RpcPitClose(pitOpen.transform.rotation);
            }
            else
            {
                CmdPitClose();
            }
        }

    }
    
    [Command]
    public void CmdPitClose()
    {
        RpcPitClose(pitOpen.transform.rotation);
    }

    [ClientRpc]
    public void RpcPitClose(Quaternion toRot)
    {
        if (pit.transform.rotation != toRot)
        {
            pit.transform.rotation = Quaternion.Lerp(pit.transform.rotation, toRot, Time.deltaTime * openSpeed);
        }
    }

    public void Open()
    {
        _open = false;
    }
}
