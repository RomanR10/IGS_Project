using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EaseTools;
using UnityStandardAssets.CinematicEffects;

public class ModMenu : MonoBehaviour {

    public GameObject modMenuObj;
    public EaseUI modMenuUI;
    public EaseUI MovementMods;
    public EaseUI Effects;

    //public GameObject effectsObj;

    public Slider speedSliderUI;
    public Slider jumpSliderUI;
    public Toggle awesomeToggle;
    public Toggle camToggle;
    public GameObject star1;
    public GameObject star2;

    public Toggle p1CicularMotion;
    public Text MotionText;
    public bool Motion = false;

    private bool menuOn = false;
    private bool awesome = false;
    private bool moving = false;

    public GameObject snowParticle;
    public GameObject hailParticle;

    public Slider snowSlider;
    public Slider hailSlider;

    private bool movementMods = false;

    private bool cameraEffects = false;

    void Start()
    {
        star1.SetActive(false);
        star2.SetActive(false);
        movementMods = true;
        cameraEffects = true;
        Effects.ScaleOut();

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
        if (Input.GetKeyDown(KeyCode.M)&& !menuOn && !moving)
        {

            if (!menuOn && !moving)
            {
                modMenuUI.MoveIn();
                MovementMods.MoveIn();
                moving = true;
                StartCoroutine("wait", modMenuUI.DurationPos);
                menuOn = true;
            }

        }
        else if (Input.GetKeyDown(KeyCode.M) && menuOn && !moving)
        {

            if (menuOn && !moving)
            {
                modMenuUI.MoveOut();
                moving = true;
                StartCoroutine("wait", modMenuUI.DurationPos);
                menuOn = false;
            }
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moving = false;

    }

    public void snowSliderFun()
    {       
            foreach (Transform child in snowSlider.transform)
            {
                if (child.name == "snow_text_slider")
                    child.GetComponent<Text>().text = snowSlider.value.ToString();
            }

        snowParticle.GetComponent<EllipsoidParticleEmitter>().maxEmission = snowSlider.value;
    }

    public void hailSliderFun()
    {
        foreach (Transform child in hailSlider.transform)
        {
            if (child.name == "Hail_text_rate")
                child.GetComponent<Text>().text = hailSlider.value.ToString();
        }

        var emission = hailParticle.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = hailSlider.value;

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

        GameObject.Find("Player").GetComponent<PlayerController>().jumpOffSpeed = jumpSliderUI.value;
        GameObject.Find("Player2").GetComponent<PlayerController>().jumpOffSpeed = jumpSliderUI.value;

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

    public void onMotionToggle()
    {
        Motion = p1CicularMotion.isOn;

        if (Motion)
        {
            MotionText.text = "Player Motion: Circular Motion";
        }
        else if (!Motion)
        {
            MotionText.text = "Player Motion: Swing Motion";
        }
    }

    public void onExit()
    {
        if (!moving)
        {
            modMenuUI.MoveOut();
            menuOn = false;
            moving = true;
            StartCoroutine("wait", modMenuUI.DurationPos);
        }


    }


    public void onMovementMods()
    {
        if (!moving)
        {
            moving = true;
            if (!movementMods)
            {
                Effects.ScaleOut();

                MovementMods.ScaleIn();

                //MovementMods.MoveIn();
            }
            else
                MovementMods.ScaleIn();

            StartCoroutine("wait", MovementMods.DurationPos);

            movementMods = true;
        }


    }

    public void onEffectMod()
    {
        if (!moving)
        {
            moving = true;

            if(movementMods)
                MovementMods.ScaleOut();

            movementMods = false;

            StartCoroutine("wait", Effects.DurationPos);

            Effects.ScaleIn();

            //Effects.MoveIn();
        }

    }

    public void onCamEffects()
    {
        cameraEffects = camToggle.isOn;

        if (cameraEffects)
        {
            Camera.main.GetComponent<Bloom>().enabled = true;
            Camera.main.GetComponent<AntiAliasing>().enabled = true;
            Camera.main.GetComponent<AmbientOcclusion>().enabled = true;
            Camera.main.GetComponent<TonemappingColorGrading>().enabled = true;
            Camera.main.GetComponent<LensAberrations>().enabled = true;

        }else if (!cameraEffects)
        {
            Camera.main.GetComponent<Bloom>().enabled = false;
            Camera.main.GetComponent<AntiAliasing>().enabled = false;
            Camera.main.GetComponent<AmbientOcclusion>().enabled = false;
            Camera.main.GetComponent<TonemappingColorGrading>().enabled = false;
            Camera.main.GetComponent<LensAberrations>().enabled = false;
        }
    }

}
