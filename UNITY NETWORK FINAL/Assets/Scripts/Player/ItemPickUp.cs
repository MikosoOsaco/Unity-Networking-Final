using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

    [Header("Inventory Text")]
    public Text HP, Attack, Defence, Health, inventorySpace;

    [Header("Inventory Stats")]
    public int inv, atk, def, hlt;
    
    [Header("Inventory Components")]
    public GameObject player;
    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;  

    // PLAYER REFERENCE
    Player main;

    // Use this for initialization
    void Start()
    {
        inv = 0;
        def = 0;
        hlt = 0;
        main = GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        inv = inventoryPanel.transform.childCount;
        SetCountInv();

        HP.text = "HEALTH : " + player.GetComponent<Player>().health;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Set inventory item based on what item you collide with

        GameObject i;

        if (inv < 4) {
            if (collision.gameObject.tag == "Axe")
            {

                i = Instantiate(inventoryIcons[0]);
                i.transform.SetParent(inventoryPanel.transform);
                atk += 150;
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Shield")
            {
                i = Instantiate(inventoryIcons[1]);
                i.transform.SetParent(inventoryPanel.transform);
                def += 100;
                main.armor += 100;
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Shield2")
            {
                i = Instantiate(inventoryIcons[2]);
                i.transform.SetParent(inventoryPanel.transform);
                hlt += 200;
                main.health += 200.0f;
                Destroy(collision.gameObject);
            }
        }
    }

    void SetCountInv()
    {
        inventorySpace.text = " " + inv.ToString();
        Attack.text = "ATTACK + " + atk.ToString();
        Defence.text = "DEFENCE + " + def.ToString();
        Health.text = "MAX HEALTH + " + hlt.ToString();
    }
}
