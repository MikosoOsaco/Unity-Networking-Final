using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OpenPrisonCell : NetworkBehaviour {

    [Header("Cell Options")]
    public GameObject childDoor;
    public float openSpeed = 0.001f;                

    [HideInInspector]
    [SyncVar]
    public bool _open = false;

    // Update is called once per frame
    void Update()
    {
        if (_open)
        {
            if (isServer)
            {
                RpcGateOpen(new Quaternion(0, 1, 0, -70));
            }
            else
            {
                CmdGateOpen();
            }
        }
    }

    [Command]
    public void CmdGateOpen()
    {
        RpcGateOpen(new Quaternion(0, 1, 0, -70));
    }

    [ClientRpc]
    public void RpcGateOpen(Quaternion toRot)
    {
        if (childDoor.transform.rotation != toRot)
        {
            childDoor.transform.rotation = Quaternion.Lerp(childDoor.transform.rotation, toRot, Time.deltaTime * openSpeed);
        }
    }

    public void Open()
    {
        _open = true;
    }
}
