using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // PLAY BUTTONS
    [Header("Play Buttons")]
    public GameObject playButtons;
    public GameObject playButtonsStart;
    public GameObject playButtonsEnd;

    // NETWORK BUTTONS
    [Header("Network Button")]
    public GameObject netButtons;
    public GameObject netButtonsStart;
    public GameObject netButtonsEnd;

    public GameObject createLanButton;
    public GameObject joinLanButton;
    public GameObject cancalConnect;

    // CAMERA PROPERTIES
    [Header("Camera Properties")]
    public GameObject menuCamera;
    public GameObject cameraStart;
    public GameObject cameraEnd;

    // MENU
    [Header("Menu")]
    public GameObject menu;

    // MENU OPTIONS
    bool playPressed = false;
    float menuSpeed = 4000.0f;
    float cameraSpeed = 10.0f;
    bool lobbyOpened = false;
    float timer = 0.4f;

    // Use this for initialization
    void Start () {
        netButtons.transform.position = netButtonsEnd.transform.position;
        playButtons.transform.position = playButtonsEnd.transform.position;
        cancalConnect.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        // Move UI and camera positions
        // Depending on whether player was pressed the menu buttons will move to different positions
        if (!playPressed)
        {
            playButtons.transform.position += new Vector3(menuSpeed * dt, 0.0f, 0.0f);
            if (playButtons.transform.position.x > playButtonsStart.transform.position.x)
            {
                playButtons.transform.position = playButtonsStart.transform.position;
            }
            netButtons.transform.position += new Vector3(menuSpeed * dt, 0.0f, 0.0f);
            if (netButtons.transform.position.x > netButtonsEnd.transform.position.x)
            {
                netButtons.transform.position = netButtonsEnd.transform.position;
            }
            menuCamera.transform.position += new Vector3(0.0f, 0.0f, cameraSpeed * dt);
            if (menuCamera.transform.position.z > cameraStart.transform.position.z)
            {
                menuCamera.transform.position = new Vector3(menuCamera.transform.position.x, menuCamera.transform.position.y, cameraStart.transform.position.z);
            }
            menuCamera.transform.position += new Vector3(0.0f, cameraSpeed * dt, 0.0f);
            if (menuCamera.transform.position.y > cameraStart.transform.position.y)
            {
                menuCamera.transform.position = new Vector3(menuCamera.transform.position.x, cameraStart.transform.position.y, menuCamera.transform.position.z);
            }
        }
        else if (playPressed)
        {
            playButtons.transform.position += new Vector3(-menuSpeed * dt, 0.0f, 0.0f);
            if (playButtons.transform.position.x < playButtonsEnd.transform.position.x)
            {
                playButtons.transform.position = playButtonsEnd.transform.position;
            }
            netButtons.transform.position += new Vector3(-menuSpeed * dt, 0.0f, 0.0f);
            if (netButtons.transform.position.x < netButtonsStart.transform.position.x)
            {
                netButtons.transform.position = netButtonsStart.transform.position;
            }
            menuCamera.transform.position += new Vector3(0.0f, 0.0f, -cameraSpeed * dt);
            if (menuCamera.transform.position.z < cameraEnd.transform.position.z)
            {
                menuCamera.transform.position = new Vector3(menuCamera.transform.position.x, menuCamera.transform.position.y, cameraEnd.transform.position.z);
            }
            menuCamera.transform.position += new Vector3(0.0f, -cameraSpeed * dt, 0.0f);
            if (menuCamera.transform.position.y < cameraEnd.transform.position.y)
            {
                menuCamera.transform.position = new Vector3(menuCamera.transform.position.x, cameraEnd.transform.position.y, menuCamera.transform.position.z);
            }
        }

        // Load the lobby scene after a few moments 
        if (lobbyOpened)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public void Play()
    {
        playPressed = true;
    }

    public void Back()
    {
        playPressed = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CreateLANGame()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinLANGame()
    {
        createLanButton.GetComponent<Button>().interactable = false;
        joinLanButton.GetComponent<Button>().interactable = false;
        cancalConnect.SetActive(true);
        NetworkManager.singleton.StartClient();
    }

    public void CancelConnect()
    {
        createLanButton.GetComponent<Button>().interactable = true;
        joinLanButton.GetComponent<Button>().interactable = true;
        cancalConnect.SetActive(false);
        NetworkManager.singleton.StopClient();
    }

    public void OpenLobby()
    {
        menu.SetActive(false);
        playPressed = false;
        lobbyOpened = true;
    }
}
