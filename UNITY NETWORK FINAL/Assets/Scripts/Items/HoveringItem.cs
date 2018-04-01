using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringItem : MonoBehaviour {
    
    private float verticalSpeed = 1f;
    private float amplitude = 0.2f;
    private float speed = 20f;

    private Vector3 tempPosition;
    private Vector3 startPosition;

	// Use this for initialization
	void Start () {
        tempPosition = transform.position;
        startPosition = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        tempPosition.y = (Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude) + startPosition.y;
        transform.position = tempPosition;
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);        
    }
}
