using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    private GameObject tutorialCanvas;
    private bool pauseGame = false;


    // Use this for initialization
    void Start () {

        tutorialCanvas = GameObject.Find("Tutorial");

        pauseGame = true;
        Time.timeScale = 0;
		
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pauseGame)
        {
            Debug.Log("Hello");
            onClick();
        }
    }
	
	public void onClick()
    {
        Debug.Log("Tutorial close: start game");
        Time.timeScale = 1;
        tutorialCanvas.SetActive(false);
        pauseGame = false;
    }
}
