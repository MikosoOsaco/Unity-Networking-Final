using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour {

    public GameObject lobby;

    public void GoToMenu()
    {
        Destroy(lobby);
        SceneManager.LoadScene("Menu");
    }
}
