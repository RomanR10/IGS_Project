﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EaseTools;

public class startGame : MonoBehaviour
{

    public Button singlePlayer;
    public Button coop;
    public Button startBut;

    public EaseUI singComp;
    public EaseUI coopComp;
    public EaseUI startButComp;

    private static bool single = false;

    private bool loadScene = false;

    void Start()
    {
        //Debug.Log(SceneManager.sceneCount);
        //singlePlayer.gameObject.SetActive(false);
        //coop.gameObject.SetActive(false);
        startButComp.MoveIn();
    }

    public void onClick()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        singComp.ScaleIn();
        coopComp.ScaleIn();

        //startButComp.MoveOut();
        startButComp.ScaleOut();
    }

    public void OnSingle() //awww
    {
        if (loadScene == false)
        {
            loadScene = true;
            single = true;
            singComp.ScaleOut();
            PlayerPrefs.SetInt("single", 0);

            LoadNewScene();
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);

        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        while (!async.isDone)
        {
            yield return null;
        }

    }

    public void OnCoop()
    {
        single = false;
        coopComp.ScaleOut();
        PlayerPrefs.SetInt("single", 1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
