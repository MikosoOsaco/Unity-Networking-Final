using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Interact : NetworkBehaviour {

    // This script contains code to activate anything the player can intereact with

    private GameObject[] prisonCellDoors;
    public GameObject boss;

    [SyncVar]
    public bool bossArea = false;

    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        prisonCellDoors = GameObject.FindGameObjectsWithTag("PrisonCellDoor");
        for (int i = 0; i < prisonCellDoors.Length; i++)
        {
            GameObject door = prisonCellDoors[i].gameObject;
            CmdOpenCells(door);
        }
    }

    void Update()
    {
        if (GameObject.FindWithTag("Boss") != null)
        {
            boss = GameObject.FindWithTag("Boss");
        }
    }

    [Command]
    void CmdOpen(GameObject go)
    {
        go.GetComponent<ActivateChest>().Open();
        go.GetComponent<TextToggle>().RpcOpen();
    }

    [Command]
    void CmdOpenCells(GameObject go)
    {
        go.GetComponent<OpenPrisonCell>().Open();
    }

    [Command]
    void CmdOpenDoor(GameObject go)
    {
        go.GetComponent<OpenDoor>().Open();
        go.GetComponent<TextToggle>().RpcOpen();
    }

    [Command]
    void CmdBridgeGap(GameObject go)
    {
        go.GetComponent<CloseTrap>().Open();
        go.GetComponent<TextToggle>().RpcOpen();
    }

    [Command]
    void CmdSetChase()
    {
        boss.GetComponent<Boss>().SetChasePlayer();
        bossArea = true;    
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Interactable")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject door = other.gameObject;
                CmdOpenDoor(door);
            }
        }

        if (other.tag == "Trap")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject trap = other.gameObject;
                CmdBridgeGap(trap);
            }
        }

        if (other.tag == "Chest")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject chest = other.gameObject;
                CmdOpen(chest);
            }
        }

        if (other.tag == "Teleporter")
        {
            transform.position = other.gameObject.GetComponentsInChildren<Transform>()[1].position;
            CmdSetChase();
            boss = GameObject.FindWithTag("Boss");
            if (isServer)
            {
                boss.GetComponent<Boss>().RpcChasePlayer(transform.position);
            }
            else
            {
                boss.GetComponent<Boss>().CmdChasePlayer(transform.position);
            }
        }
    }
}
