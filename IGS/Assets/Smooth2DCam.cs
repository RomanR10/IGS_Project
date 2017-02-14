using UnityEngine;
using System.Collections;

public class Smooth2DCam : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;
    private Transform Player;
    private Transform Player2;
    public float offsetY;
    public Camera cam;
    private Transform lastTarget;

    public float lerpTime = .35f;
    private float lerpTimeZoom = 5f;
    float currentLerpTime;
    float currentLerpTimeZoom = 0;

    Vector3 startPos;
    Vector3 endPos;

    public bool camInit = false;

    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Transform>();
        Player2 = GameObject.Find("Player2").GetComponent<Transform>();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!camInit && !GameObject.Find("GameManager").GetComponent<Tutorial>().pauseGame)
        {
            //increment timer once per frame
            currentLerpTimeZoom += Time.deltaTime;
            if (currentLerpTimeZoom > lerpTimeZoom)
            {
                currentLerpTimeZoom = lerpTimeZoom;
            }

            //lerp!
            float perc = currentLerpTimeZoom / lerpTimeZoom;
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 70f, perc);


            if (currentLerpTime == lerpTimeZoom)
                camInit = true;

        }

        bool follow_p1 = GameObject.Find("Player").GetComponent<PlayerController>().PlayerOneAttached;
        bool follow_p2 = GameObject.Find("Player2").GetComponent<PlayerController>().PlayerTwoAttached;

        follow_p1 = true;
        follow_p2 = false;

        if (!GameObject.Find("GameManager").GetComponent<Tutorial>().pauseGame)
        {
            if (follow_p1)
            {
                startPos = transform.position;
                endPos = Player2.transform.position;
                endPos = new Vector3(endPos.x, endPos.y + offsetY, -10);

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

                lastTarget = Player2;
            }
            else if (follow_p2 && !follow_p1)
            {


                startPos = transform.position;
                endPos = Player.transform.position;
                endPos = new Vector3(endPos.x, endPos.y + offsetY, -10);

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

                lastTarget = Player;
            }
            else
            {
                startPos = transform.position;
                endPos = lastTarget.transform.position;
                endPos = new Vector3(endPos.x, endPos.y + offsetY, -10);

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

                lastTarget = Player;
            }
        }
    }
}
