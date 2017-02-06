using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject modMenuObj;

    private Tutorial tut;

    private bool pause = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        tut = GetComponent<Tutorial>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log("wtf is up");

        Debug.Log(tut.pauseGame);

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && pause && !tut.pauseGame)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            pause = false;

        }
        else if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !pause && !tut.pauseGame)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pause = true;
        }

        //Debug.Log(pause);
    }


    public void onMenu()
    {
        SceneManager.LoadSceneAsync(0 , LoadSceneMode.Single);
    }

    public void onMod()
    {
        modMenuObj.SetActive(true);
    }

    public void onExit()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
