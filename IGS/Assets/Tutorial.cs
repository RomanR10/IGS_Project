using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EaseTools;

public class Tutorial : MonoBehaviour {
    private GameObject tutorialCanvas;
    private GameObject tutorialSingle;

    public bool pauseGame = true;
    private bool single = false;


    // Use this for initialization
    void Start()
    {

        if (PlayerPrefs.GetInt("single") == 0)
            single = true;
        else if (PlayerPrefs.GetInt("single") == 1)
            single = false;

        tutorialCanvas = GameObject.Find("Tutorial_COOP");
        tutorialSingle = GameObject.Find("Tutorial_Single");

        Debug.Log(single);


        if (single)
        {
            tutorialCanvas.SetActive(false);

        }
        else
        {
            tutorialSingle.SetActive(false);
        }

        pauseGame = true;

        Time.timeScale = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pauseGame)
        {
            onClick();
        }
    }

    public void onClick()
    {
        //Debug.Log("Tutorial close: start game");
        Time.timeScale = 1;
        if (!single)
        {

            tutorialCanvas.SetActive(false);
        }
        if (single)
        {
            tutorialSingle.SetActive(false);

        }

        pauseGame = false;
    }
}
