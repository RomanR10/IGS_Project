using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EaseTools;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject modMenuObj;

    private Tutorial tut;

    public bool pause = false;
    public bool moving = false;

    public EaseUI easeUIComponent;
    public EaseUI modMenuComp;
    public EaseUI MenuComp;


    void Start()
    {
        //pauseMenu.SetActive(false);
        tut = GetComponent<Tutorial>();
    }

    void Update()
    {

        //Debug.Log(tut.pauseGame);

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && pause && !tut.pauseGame && !moving)
        {
            //Time.timeScale = 1;
            //pauseMenu.SetActive(false);


            if (pause && !moving)
            {
                easeUIComponent.MoveOut();
                moving = true;
            }

            StartCoroutine("wait", .75f);
            pause = false;

        }

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !pause && !tut.pauseGame && !moving)
        {
            //
            //pauseMenu.SetActive(true);
            
            if (!pause && !moving)
            {
                easeUIComponent.MoveIn();
                moving = true;
            }

            StartCoroutine("wait2", .75f);
            pause = true;
        }

        //Debug.Log(pause);
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moving = false;
        //Time.timeScale = 1;
    }

    IEnumerator wait2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moving = false;
        //Time.timeScale = 0;
    }


    public void onMenu()
    {
        MenuComp.ScaleOut();
        SceneManager.LoadSceneAsync(0 , LoadSceneMode.Single);
    }

    public void onMod()
    {
        modMenuObj.SetActive(true);
        modMenuComp.MoveIn();
    }

    public void onExit()
    {
        //pauseMenu.SetActive(false);
        easeUIComponent.MoveOut();
        Time.timeScale = 1;
    }
}
