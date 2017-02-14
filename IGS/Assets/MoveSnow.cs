using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSnow : MonoBehaviour {

    public GameObject tutScreen;

    public bool startGame = false;

    float lerpTime = 10f;
    float currentLerpTime;

    Vector3 startPos;
    Vector3 endPos;

    // Use this for initialization
    void Start () {

        startGame = tutScreen.GetComponent<Tutorial>().pauseGame;
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, transform.position.y + 20, 30);
    }

    // Update is called once per frame
    void Update () {

        startGame = tutScreen.GetComponent<Tutorial>().pauseGame;

        if (!startGame)
        {
            currentLerpTime = 0;
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, endPos, perc);
        }
		
	}
}
