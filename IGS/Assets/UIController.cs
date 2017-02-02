using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private Text p1_heightText;
    private Text p2_heightText;
    private int playersHeightINIT;

	// Use this for initialization
	void Start () {

        p1_heightText = GameObject.Find("p1_height").GetComponent<Text>();
        p2_heightText = GameObject.Find("p2_height").GetComponent<Text>();

        playersHeightINIT = (int)GameObject.Find("Player").GetComponent<Transform>().transform.position.y;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        int p1_height = (int)GameObject.Find("Player").GetComponent<Transform>().transform.position.y - playersHeightINIT;
        int p2_height = (int)GameObject.Find("Player2").GetComponent<Transform>().transform.position.y - playersHeightINIT;


        p1_heightText.text = "P1: " + p1_height.ToString();
        p2_heightText.text = "P2: " + p2_height.ToString();
    }
}

