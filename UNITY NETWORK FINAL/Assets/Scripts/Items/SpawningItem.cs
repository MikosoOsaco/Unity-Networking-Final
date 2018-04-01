using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningItem : MonoBehaviour {

    [Header("Item Options")]
    public Transform spawnPoint;
    public GameObject[] inventoryItems;    

    private bool inRange = false;
    private bool spawn = false;
    private bool opened = false;

    public GameObject effect;
    public Transform effectSpawnPoint;

    private float lifeTime = 10f;

	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space) && inRange)
        {
            spawn = true;
                
        }

        if (spawn && !opened)
        {
            SpawnItem();
            opened = true;
        }        
	}

    void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    public void SpawnItem()
    {
        GameObject inventoryItem = inventoryItems[Random.Range(0, inventoryItems.Length)];

        Instantiate(inventoryItem, spawnPoint.position, spawnPoint.rotation);
        Instantiate(effect, effectSpawnPoint.position, effectSpawnPoint.rotation);
    }
}
