﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModMenu : MonoBehaviour {

    public GameObject modMenuObj;
    

    public Slider speedSliderUI;
    public Slider jumpSliderUI;
    public Toggle awesomeToggle;
    public GameObject star1;
    public GameObject star2;


    private bool menuOn = false;
    private bool awesome = false;

    void Start()
    {
        modMenuObj.SetActive(false);
        star1.SetActive(false);
        star2.SetActive(false);
        foreach (Transform child in speedSliderUI.transform)
        {
            if (child.name == "Speed_text_slider")
                child.GetComponent<Text>().text = speedSliderUI.value.ToString();
        }
        foreach (Transform child in jumpSliderUI.transform)
        {
            if (child.name == "JumpSpeed_text_slider")
                child.GetComponent<Text>().text = jumpSliderUI.value.ToString();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)&& !menuOn)
        {
            modMenuObj.SetActive(true);
            menuOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && menuOn)
        {
            modMenuObj.SetActive(false);
            menuOn = false;
        }
    }

    public void speedSlider()
    {
        foreach (Transform child in speedSliderUI.transform)
        {
            if (child.name == "Speed_text_slider")
                child.GetComponent<Text>().text = speedSliderUI.value.ToString();
        }
        GameObject.Find("Player").GetComponent<PlayerController>().speed = speedSliderUI.value;
        GameObject.Find("Player2").GetComponent<PlayerController>().speed = speedSliderUI.value;

    }

    public void jumpSlider()
    {
        foreach (Transform child in jumpSliderUI.transform)
        {
            if (child.name == "JumpSpeed_text_slider")
                child.GetComponent<Text>().text = jumpSliderUI.value.ToString();
        }

        GameObject.Find("Player").GetComponent<PlayerController>().jumpOffSpeed = speedSliderUI.value;
        GameObject.Find("Player2").GetComponent<PlayerController>().jumpOffSpeed = speedSliderUI.value;

    }

    public void onToggle()
    {
        awesome = awesomeToggle.isOn;

        if (awesome)
        {
            star1.SetActive(true);
            star2.SetActive(true);
        }else
        {
            star1.SetActive(false);
            star2.SetActive(false);
        }

    }


}