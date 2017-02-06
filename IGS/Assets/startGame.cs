using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{

    public Button singlePlayer;
    public Button coop;
    public Button startBut;

    private static bool single = false;

    void Start()
    {
        //Debug.Log(SceneManager.sceneCount);
        singlePlayer.gameObject.SetActive(false);
        coop.gameObject.SetActive(false);
    }

    public void onClick()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        singlePlayer.gameObject.SetActive(true);
        coop.gameObject.SetActive(true);
        startBut.gameObject.SetActive(false);
    }

    public void OnSingle() //awww
    {
        single = true;
        PlayerPrefs.SetInt("single", 0);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void OnCoop()
    {
        single = false;
        PlayerPrefs.SetInt("single", 1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
