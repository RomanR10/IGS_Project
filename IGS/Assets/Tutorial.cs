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

    public GameObject tutKeysObj;
    private GameObject rightKey;
    private GameObject leftKey;

    public GameObject tutKeysObj_1;
    private GameObject downKey;
    private bool deatachTut = false;

    // Use this for initialization
    void Start()
    {
        rightKey = GameObject.Find("Right_Key");
        rightKey.SetActive(false);
        leftKey = GameObject.Find("Left_Key");
        leftKey.SetActive(false);

        downKey = GameObject.Find("Down_Key");
        downKey.SetActive(false);

        if (PlayerPrefs.GetInt("single") == 0)
            single = true;
        else if (PlayerPrefs.GetInt("single") == 1)
            single = false;

        tutorialCanvas = GameObject.Find("Tutorial_COOP");
        tutorialSingle = GameObject.Find("Tutorial_Single");

        //Debug.Log(single);


        if (single)
        {
            tutorialCanvas.SetActive(false);

        }
        else
        {
            tutorialSingle.SetActive(false);
        }

        pauseGame = true;

        //Time.timeScale = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pauseGame)
        {
            onClick();
        }

        if((GameObject.Find("Player2").GetComponent<Rigidbody2D>().velocity.x > 25 || GameObject.Find("Player2").GetComponent<Rigidbody2D>().velocity.x < -25) && !deatachTut)
        {
            StartCoroutine("deatachTutorial");
            deatachTut = true;
        }
    }

    IEnumerator deatachTutorial()
    {
        downKey.SetActive(true);
        yield return new WaitForSeconds(3);
        downKey.SetActive(false);
    }

    public void onClick()
    {
        //Debug.Log("Tutorial close: start game");
        StartCoroutine("tutKeys");
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

    IEnumerator tutKeys()
    {
        yield return new WaitForSeconds(1f);

        rightKey.SetActive(true);

        yield return new WaitForSeconds(.75f);

        leftKey.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        rightKey.SetActive(false);

        yield return new WaitForSeconds(.75f);

        leftKey.SetActive(false);

    }
}
