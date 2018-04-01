using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextToggle : NetworkBehaviour {

    [Header("Text")]
    ActivateChest chest;
    public GameObject txt;

    [SyncVar]
    private bool opened = false;

    // Use this for initialization
    void Start () { 
        txt.SetActive(false);
	}
	
    [ClientRpc]
    public void RpcOpen()
    {
        opened = true;
        txt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!opened)
        {
            txt.SetActive(true);
        }        
    }

    void OnTriggerExit(Collider other)
    {
        txt.SetActive(false);
    }
}
