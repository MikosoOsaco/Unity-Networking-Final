using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {

    // PLAYER PROPERTIES
    [Header("Player Properties")]
    public float maxHealth = 100.0f;
    public float health = 0.0f; 
    public float moveSpeed = 300.0f;
    public float damageToPlayer = 15.0f;
    public float armor = 0;

    // PLAYER STATUS CHECKS
    private bool dead = false;
    private bool canMove = true;
    private bool win = false;
    [HideInInspector]
    public bool canAttack = true;

    // COMBO VARIABLES
    public float comboCoolDown = 1.0f;
    private float comboTimer;
    private int comboNumber = 0;

    // PLAYER COMPONENTS
    [Header("Player Components")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject hitBox;
    public GameObject playerCamera;
    public GameObject model;
    public GameObject inventory;
    public Animator animator;    

    // Use this for initialization
    void Start () {
        // Check to see if this is the local player
        if (!isLocalPlayer)
        {
            playerCamera.SetActive(false);            
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponents<AudioSource>()[0].enabled = false;
            gameObject.GetComponents<AudioSource>()[1].enabled = false;
            inventory.SetActive(false);
            enabled = false;
        } 
        // Make sure players can't bump into each other
        Physics.IgnoreLayerCollision(8, 8, true);
        // Set player variables
        comboTimer = comboCoolDown;
        health = maxHealth;
        hitBox.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // Only do this if the player is not dead
        if (!dead)
        {
            float dt = Time.fixedDeltaTime;
        
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float speed = 0;
        
            // Make sure speed is always a positive number
            if (h < 0)
            {
                speed += -h;
            }
            else
            {
                speed += h;
            }
            if (v < 0)
            {
                speed += -v;
            }
            else
            {
                speed += v;
            }
            if (speed > 0.3)
            {
                animator.SetBool("Run", true);
            }
            else if (speed < 0.3)
            {
                animator.SetBool("Run", false);
            }
            
            // Set velocity
            if (canMove)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(v * moveSpeed * dt, GetComponent<Rigidbody>().velocity.y, -h * moveSpeed * dt);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            }

            // Rotate in direction of movement
            Vector3 movement = new Vector3(v, 0.0f, -h);
            if (movement != Vector3.zero)
            {
                model.transform.rotation = Quaternion.LookRotation(movement);
            }
        }
    }

    void Update()
    {
        // If player wins or is dead, space will return them to the main menu
        if (win || dead)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Menu");
            }
        }

        // Only do this if the player isn't dead and hasn't won
        if (!dead && !win)
        {
            float dt = Time.deltaTime;

            // Update combo timer
            comboTimer -= dt;
            if (comboTimer <= 0)
            {
                comboNumber = 0;
                canMove = true;
            }

            if (health <= 0)
            {
                if (isServer)
                {
                    RpcDie();
                }
                else
                {
                    CmdDie();
                }
                health = 0;
            }

            // Make sure that while the player can't move the run animation isn't playing
            if (!canMove)
            {
                animator.SetBool("Run", false);
            }
                        
            /*
            // Combo strings work by setting a timer and as long as the timer is above 0 you can go to the next attack by incrementing comboNumber
            */

            // LIGHT COMBO STRING
            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
            {
                canMove = false;
                if (comboTimer < 0 && comboNumber == 0)
                {
                    comboTimer = comboCoolDown;
                    // animator.SetTrigger("Hikick");
                    // for some reason the animator doesnt work if you want to network triggers so you have to use the network animator
                    GetComponent<NetworkAnimator>().SetTrigger("Jab");
                    GetComponent<NetworkAnimator>().animator.ResetTrigger("Jab");
                    comboNumber++;
                }
                else if (comboTimer > 0)
                {
                    if (comboNumber == 1)
                    {
                        comboTimer = comboCoolDown + 0.5f;
                        GetComponent<NetworkAnimator>().SetTrigger("Hikick");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("Hikick");
                        comboNumber++;
                    }
                    else if (comboNumber == 2)
                    {
                        comboTimer = comboCoolDown + 0.7f;
                        GetComponent<NetworkAnimator>().SetTrigger("Spinkick");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("Spinkick");
                        comboNumber = 0;
                    }
                }
            }

            // HEAVY COMBO STRING
            if (Input.GetKeyDown(KeyCode.Mouse1) && canAttack)
            {
                canMove = false;
                if (comboTimer < 0 && comboNumber == 0)
                {
                    comboTimer = comboCoolDown;
                    GetComponent<NetworkAnimator>().SetTrigger("Rising_P");
                    GetComponent<NetworkAnimator>().animator.ResetTrigger("Rising_P");
                    comboNumber++;
                }
                else if (comboTimer > 0)
                {
                    if (comboNumber == 1)
                    {
                        comboTimer = comboCoolDown + 0.5f;
                        GetComponent<NetworkAnimator>().SetTrigger("Spinkick");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("Spinkick");
                        comboNumber++;
                    }
                    else if (comboNumber == 2)
                    {
                        comboTimer = comboCoolDown + 0.7f;
                        GetComponent<NetworkAnimator>().SetTrigger("Hikick");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("Hikick");
                        comboNumber = 0;
                    }
                }
            }

            // SPECIAL COMBO STRING
            if (Input.GetKeyDown(KeyCode.Mouse2) && canAttack)
            {
                canMove = false;
                if (comboTimer < 0 && comboNumber == 0)
                {
                    comboTimer = comboCoolDown;
                    GetComponent<NetworkAnimator>().SetTrigger("Spinkick");
                    GetComponent<NetworkAnimator>().animator.ResetTrigger("Spinkick");
                    comboNumber++;
                }
                else if (comboTimer > 0)
                {
                    if (comboNumber == 1)
                    {
                        comboTimer = comboCoolDown + 0.5f;
                        GetComponent<NetworkAnimator>().SetTrigger("ScrewK");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("ScrewK");
                        comboNumber++;
                    }
                    else if (comboNumber == 2)
                    {
                        comboTimer = comboCoolDown + 0.7f;
                        GetComponent<NetworkAnimator>().SetTrigger("SAMK");
                        GetComponent<NetworkAnimator>().animator.ResetTrigger("SAMK");
                        comboNumber = 0;
                    }
                }
            }
        }
        // Update damage value based on armor value
        if (armor == 0)
        {
            damageToPlayer = 10;
        }

        if (armor == 100)
        {
            damageToPlayer = 8;
        }

        if (armor == 200)
        {
            damageToPlayer = 6;
        }

        if (armor == 300)
        {
            damageToPlayer = 4;
        }

        if (armor == 400)
        {
            damageToPlayer = 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Lose health when you enter an enemy collider
        if (other.tag == "Enemy")
        {
            health -= damageToPlayer;
        }

        // Win if you enter an area tagged as finish
        if(other.tag == "Finish")
        {
            winScreen.SetActive(true);
            win = true;
            canMove = false;
        }
    }

    // Player dies
    [Command]
    public void CmdDie()
    {
        RpcDie();
    }

    [ClientRpc]
    public void RpcDie()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Dead", true);
        dead = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        model.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        loseScreen.SetActive(true);
    }

    // Attack() and Done() are called by the animations and determine when the hitboxes for the player are active
    [Command]
    public void CmdAttack()
    {
        RpcAttack();
    }

    [ClientRpc]
    public void RpcAttack()
    {
        hitBox.GetComponent<BoxCollider>().enabled = true;
    }

    [Command]
    public void CmdDone()
    {
        RpcDone();
    }

    [ClientRpc]
    public void RpcDone()
    {
        hitBox.GetComponent<BoxCollider>().enabled = false;
    }
}
