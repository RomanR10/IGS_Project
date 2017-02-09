using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    public GameObject widePlatform;
    public GameObject tallPlatform;
    public GameObject standerdPlatform;

    private Camera cam;
    private float fov;
    private Vector3 camPosition;

    private float flub;
    private Vector3 startPosition = new Vector3(-6.3f, 16.09f, 25);
    private Vector2 minDistanceTall = new Vector2(10, 15);
    private Vector2 minDistanceStandard = new Vector2(10, 10);
    private Vector2 minDsitanceWide = new Vector2(10, 10);
    private Vector2[] platformLocation = new Vector2[10];

    float distanceX = 0f;
    float distanceY = 0f;
    Vector2 distance;

    int loop = 0;

    private bool safe = false;

    void Awake()
    {
        cam = Camera.main;
        fov = cam.fieldOfView;
        Debug.Log(fov);
        camPosition = cam.transform.position;
        Debug.Log(camPosition);

        AwakeGeneration();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AwakeGeneration()
    {
        int randy = Random.Range(0, 1);
        Debug.Log(randy);

        //Spawn tall platform
        if (randy == 0) //LEFT SIDE of start platform
        {
            //spawnTallPlatform(startPosition, distance);
            Calculate(1, 0);
            Calculate(1, 1);
            Calculate(1, 2);

        }
        /*else if (randy == 1) //RIGHT SIDE of start platform
        {
            float distanceX = Random.Range(minDistanceTall.x + 0f, minDistanceTall.x * 2f);
            float distanceY = Random.Range(minDistanceTall.y + 0f, minDistanceTall.y + 8f);
            //Debug.Log(distanceX);
            Vector2 distance = new Vector2(distanceX, distanceY);
            platformLocation[0] = distance;

            spawnTallPlatform(startPosition, distance);

            distanceX = Random.Range(-minDistanceStandard.x + 0f, -minDistanceStandard.x + (-minDistanceStandard.x * .5f));
            Debug.Log(distanceX + "standardPlatform");
            distanceY = Random.Range(minDistanceStandard.y + 0f, minDistanceStandard.y + 10f);
            distance = new Vector2(distanceX, distanceY + platformLocation[0].y / 4);
            platformLocation[1] = distance;

            spawnStandardPlatform(platformLocation[0], distance);

            distanceX = Random.Range(-minDsitanceWide.x + 0f, -minDsitanceWide.x + (-minDsitanceWide.x * .5f));
            Debug.Log(distanceX + "standardPlatform");
            distanceY = Random.Range(minDsitanceWide.y + 0f, minDsitanceWide.y + 10f);
            distance = new Vector2(distanceX, distanceY + platformLocation[1].y / 4);
            platformLocation[2] = distance;

            spawnWidePlatform(platformLocation[1], distance);

            distanceX = Random.Range(-minDistanceStandard.x + 0f, -minDistanceStandard.x + (-minDistanceStandard.x * .5f));
            Debug.Log(distanceX + "standardPlatform");
            distanceY = Random.Range(minDistanceStandard.y + 0f, minDistanceStandard.y + 10f);
            distance = new Vector2(distanceX, distanceY + platformLocation[2].y / 4);
            platformLocation[3] = distance;

            spawnStandardPlatform(platformLocation[2], distance);

            distanceX = Random.Range(-minDsitanceWide.x + 0f, -minDsitanceWide.x + (-minDsitanceWide.x * .5f));
            Debug.Log(distanceX + "standardPlatform");
            distanceY = Random.Range(minDsitanceWide.y + 0f, minDsitanceWide.y + 10f);
            distance = new Vector2(distanceX, distanceY + platformLocation[3].y / 4);
            platformLocation[4] = distance;

            spawnWidePlatform(platformLocation[3], distance);

        }*/
    }

    //Calculate
    void Calculate(int cond, int platformTrack)
    {
        //0 - Standard
        //1 - Tall
        //2 - Wide
        switch (cond)
        {
            case 0:

                distanceX = Random.Range(minDistanceStandard.x + 0f, minDistanceStandard.x + (minDistanceStandard.x * .5f));
                Debug.Log(distanceX + "standardPlatform");
                Debug.Log(minDistanceStandard.x + (minDistanceStandard.x * .5f));
                distanceY = Random.Range(minDistanceStandard.y + 0f, minDistanceStandard.y + 10f);
                distance = new Vector2(distanceX, distanceY + platformLocation[0].y / 2);
                platformLocation[platformTrack] = distance;

                break;
            case 1:

                /*do
                {
                    //distanceX = 0;
                    //distanceY = 0;
                    distanceX = Random.Range(-minDistanceTall.x + 0f, -minDistanceTall.x * 2f);
                    distanceY = Random.Range(minDistanceTall.y + 0f, minDistanceTall.y + 8f);
                    distance = new Vector2(distanceX, distanceY);
                    platformLocation[platformTrack] = distance;
                    Debug.Log(distance + "tallPlatform");
                    Checker(distance, platformTrack);
                    //Debug.Log(Checker(distance, platformTrack));

                    //Need to adjust distance to coordinate with the respective spawn position 
                    //Right now all it is outputting is the random range created

                } while (safe == false);

                if (safe)
                {
                    Debug.Log("checker is true");
                    if (platformTrack > 0)
                    {
                        spawnTallPlatform(platformLocation[platformTrack - 1], distance);

                    }
                    else if (platformTrack == 0)
                    {
                        spawnTallPlatform(startPosition, distance);
                    }
                }*/

                break;

            case 2:

                distanceX = Random.Range(minDsitanceWide.x + 0f, minDsitanceWide.x + (minDsitanceWide.x * .5f));
                Debug.Log(distanceX + "widePlatform");
                distanceY = Random.Range(minDsitanceWide.y + 0f, minDsitanceWide.y + 10f);
                distance = new Vector2(distanceX, distanceY + platformLocation[1].y / 2);
                platformLocation[platformTrack] = distance;

                break;
            default:
                break;
        }
    }

    //Check
    //Make this into a do-while in calculate so it can change position if need be
    void Checker(Vector3 pos, int platformTrack)
    {
        loop = 0;
        bool contact = false;

        foreach (Vector2 p in platformLocation)
        {
            //Debug.Log(p + " " + platformTrack + "loop" + ": length: " + platformLocation.Length);

            if(platformTrack < 3)
            {
                if (pos.x < p.x + 2 && pos.x > p.x - 2 && pos.y < p.y + 10 && pos.y > p.y - 10 && loop != platformTrack)
                {
                    Debug.Log("Hello" + platformTrack + " overlapping platforms with: p" + p.x + ", " + p.y);
                    safe = false;
                    contact = true;
                }

                if(loop == 2 && !contact)
                {
                    safe = true;
                }

            }


            /*else
            {
                Debug.Log(platformTrack + " NO OVERLAP");
                loop++;
                return true;
            }*/

        }
    }

    //Spawn

    void spawnTallPlatform(Vector2 spawnLoc, Vector2 distance)
    {
        Debug.Log("spawn obj");
        GameObject clone;
        clone = Instantiate(tallPlatform, new Vector3(spawnLoc.x + distance.x, spawnLoc.y + distance.y, 25.5f), tallPlatform.transform.rotation);
        clone.transform.SetParent(transform);
    }

    void spawnStandardPlatform(Vector2 spawnLoc, Vector2 distance)
    {
        GameObject clone;
        clone = Instantiate(standerdPlatform, new Vector3(spawnLoc.x + distance.x, spawnLoc.y + distance.y, 25f), tallPlatform.transform.rotation);
        clone.transform.SetParent(transform);
    }

    void spawnWidePlatform(Vector2 spawnLoc, Vector2 distance)
    {
        GameObject clone;
        clone = Instantiate(widePlatform, new Vector3(spawnLoc.x + distance.x, spawnLoc.y + distance.y, 25f), tallPlatform.transform.rotation);
        clone.transform.SetParent(transform);
    }
}
