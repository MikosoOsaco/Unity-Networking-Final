using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ActivateChest : NetworkBehaviour {

	public Transform lid, lidOpen, lidClose;	// Lid, Lid open rotation, Lid close rotation
	public float openSpeed = 5F;                // Opening speed

    


    [HideInInspector]
    [SyncVar]
    public bool _open = false;					        // Is the chest opened
    public bool canClose = false;                       // Can the chest be closed

    GameObject player;

    void Update () {
        /*
        if (player != null)
        {

            if (player.GetComponent<Player>().pressedSpace && canClose)
            {
                _open = true;
            }
        }

        if ( _open && canClose && NetworkServer.active)
        {
            RpcChestClicked(lidOpen.rotation);
        }
        */
        if(_open){
            if (isServer)
            {
                RpcChestClicked(lidOpen.rotation);
            }
            else
            {
                CmdChestClicked(lidOpen.rotation);
            }
        }

        /*if(_open){
			ChestClicked(lidOpen.rotation);
		}
		else{
			ChestClicked(lidClose.rotation);
		}*/


    }




    /*void OnMouseDown(){
		if(canClose) _open = !_open; else _open = true;
	}*/


    // Rotate the lid to the requested rotation
    [Command]
    public void CmdChestClicked(Quaternion toRot)
    {
        RpcChestClicked(toRot);
    }
    [ClientRpc]
    public void RpcChestClicked(Quaternion toRot){
		if(lid.rotation != toRot){
			lid.rotation = Quaternion.Lerp(lid.rotation, toRot, Time.deltaTime * openSpeed);
		}        
    }

     void OnTriggerEnter(Collider other)
     {
        // We set this variable to indicate that character is in trigger
        //canClose = true;
        //player = other.transform.parent.gameObject;
        ///Debug.Log("trigger entered");
        //_open = true;
    }

     void OnTriggerExit(Collider other)
     {
        
        //canClose = false;
        //_open = false;
        //Debug.Log("trigger exit");
    }

    public void Open()
    {
        _open = true;
    }

}
