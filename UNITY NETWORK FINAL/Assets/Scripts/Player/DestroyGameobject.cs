using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DestroyGameobject : MonoBehaviour {

    // This script contains code for when an item is removed from the inventory

    public GameObject playerStats;
    public GameObject main;

    // Use this for initialization
    void Start()
    {
        playerStats.GetComponent<ItemPickUp>();
        main.GetComponent<Player>();
    }

    public void DestroyThisObject()
    {
        if (transform.parent.CompareTag("Axe"))
        {
            playerStats.GetComponent<ItemPickUp>().atk -= 150;

        } else if (transform.parent.CompareTag("Shield"))
        {
            playerStats.GetComponent<ItemPickUp>().def -= 100;
            main.GetComponent<Player>().armor -= 100;

        } else if (transform.parent.CompareTag("Shield2"))
        {
            playerStats.GetComponent<ItemPickUp>().hlt -= 200;
        }
        Destroy(transform.parent.gameObject);
    }
}
