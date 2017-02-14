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
    private Vector2 minDistanceTall = new Vector2(15, 15);
    private Vector2 minDistanceStandard = new Vector2(12, 10);
    private Vector2 minDsitanceWide = new Vector2(12, 10);
    private Vector2[] platformLocation = new Vector2[40];
    private List<GameObject> platformList = new List<GameObject>();

    float distanceX = 0f;
    float distanceY = 0f;
    Vector2 distance;

    int loop = 0;

    int platforms = 0;

    private bool safe = false;

    private int layer = 0;

    private string[] platformNames = { "widePlatform", "tallPlatform", "standardPlatform" };

    private float yMultipler;
    private float xMultipler;

    private bool spawnNewObjects = false;
    private bool heightLayer = false;
    private float curHeightY;
    private float heightLimit;

    void Awake()
    {
        cam = Camera.main;
        fov = cam.fieldOfView;
        Debug.Log(fov);
        camPosition = cam.transform.position;
        Debug.Log(camPosition);

    }

	// Use this for initialization
	void Start () {


        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Climb Me"))
        {
            platformLocation[platforms] = new Vector2(g.transform.position.x, g.transform.position.y);
            //Debug.Log("Found platform on load @ (" + platformLocation[platforms].x + ", " + platformLocation[platforms].y + ")");
            platforms++;
            platformList.Add(g);
        }

        heightLimit = GameObject.Find("Player").transform.position.y + 80;

        Generation();

    }

    void Update()
    {
        /*if (GameObject.Find("Player").transform.position.y > heightLimit && !heightLayer)
        {
            for(int i = 0; i < platformList.Count; i++)
            {
                if(platformList[i].transform.position.y < 60)
                {

                    GameObject clone = platformList[i];
                    clone.SetActive(false);
                    platformList.RemoveAt(i);
                    Debug.Log("Remove objs");
                }

                if (i == platformList.Count - 1)
                {
                    spawnNewObjects = true;
                    heightLayer = true;
                    curHeightY = GameObject.Find("Player").transform.position.y;
                    heightLimit = heightLimit + (GameObject.Find("Player").transform.position.y);
                }
            }
        }

        if (spawnNewObjects)
        {
            StartCoroutine("movingGenerator");
            spawnNewObjects = false;
        }*/
    }

    void MovingGeneration()
    {
        int range = Random.Range(5, 8);
        //Center - Left
        //Center - Left
        for (int i = 0; i < range; i++)
        {
            xMultipler = .1f * i;
            yMultipler = .9f * i;
            preCalc(-5 + (-15 * xMultipler), (heightLimit) + (18 * yMultipler));
        }

        //Center
        for (int i = 0; i < range; i++)
        {
            xMultipler = .15f * i;
            yMultipler = 1.15f * i;
            preCalc(0, (heightLimit) + (18 * yMultipler));
        }

        //Center - Right
        for (int i = 0; i < range; i++)
        {
            xMultipler = .15f * i;
            yMultipler = 1.15f * i;
            preCalc(30 + (15 * xMultipler), (heightLimit) + (18 * yMultipler));
        }

    }

    IEnumerator movingGenerator()
    {
        int range = Random.Range(5, 8);
        //Center - Left
        //Center - Left
        for (int i = 0; i < range; i++)
        {
            xMultipler = .1f * i;
            yMultipler = .9f * i;
            preCalc(-5 + (-15 * xMultipler), ((heightLimit / 2) + 50) + (18 * yMultipler));
        }

        //Center
        for (int i = 0; i < range; i++)
        {
            xMultipler = .15f * i;
            yMultipler = 1.15f * i;
            preCalc(0, ((heightLimit / 2) + 50) + (18 * yMultipler));
        }

        //Center - Right
        for (int i = 0; i < range; i++)
        {
            xMultipler = .15f * i;
            yMultipler = 1.15f * i;
            preCalc(30 + (15 * xMultipler), ((heightLimit / 2) + 50) + (18 * yMultipler));
        }

        yield return new WaitForSeconds(10);
        heightLayer = false;
    }


    public void Generation()
    {
        int randy = Random.Range(0, 1);

        //Spawn tall platform
        if (randy == 0) //LEFT SIDE of start platform
        {
            int range = Random.Range(6, 8);

            //Left side
            /*for (int i = 0; i < range; i++)
            {
                xMultipler = .3f * i;
                yMultipler = .3f * i;
                preCalc(-20 + (-20 * xMultipler), -10 +  (15 * yMultipler));
            }

            //Right side
            for (int i = 0; i < range; i++)
            {
                xMultipler = .3f * i;
                yMultipler = .3f * i;
                preCalc(40 +  (40 * xMultipler), -10  + (15 * yMultipler));
            }*/

            //Center - Left
            for (int i = 0; i < range; i++)
            {
                xMultipler = .1f * i;
                yMultipler = .9f * i;
                preCalc(-5 + (-15 * xMultipler), 5 + (18 * yMultipler));
            }


            //Center
            for (int i = 0; i < range; i++)
            {
                xMultipler = .15f * i;
                yMultipler = 1.15f * i;
                preCalc(0, 10 + (18 * yMultipler));
            }

            //Center - Right
            for (int i = 0; i < range; i++)
            {
                xMultipler = .15f * i;
                yMultipler = 1.15f * i;
                preCalc(30 + (15 * xMultipler), 5 + (18 * yMultipler));
            }


            //spawnTallPlatform(startPosition, distance);
            // Calculate("tallPlatform", platforms + 1, 15, -30);
            // platforms = platforms + 1;

        }

    }

    void preCalc(float xPosition, float yPosition)
    {
        safe = false;
        int randy = Random.Range(0, 3);
        //Debug.LogError("Pre calc: randy = " + randy);

        switch (randy)
        {
            case 0:
                Calculate("standardPlatform", platforms + 1, yPosition, xPosition);
                break;
            case 1:
                Calculate("tallPlatform", platforms + 1, yPosition, xPosition);
                break;
            case 2:
                Calculate("widePlatform", platforms + 1, yPosition, xPosition);
                break;
            default:
                break;
        }
    }

    //Calculate
    void Calculate(string name, int platformTrack, float layer, float xPos)
    {
        //0 - Standard
        //1 - Tall
        //2 - Wide
        switch (name)
        {
            case "standardPlatform":

                while (safe == false)
                {
                    distanceX = 0;
                    distanceY = 0;

                    if(xPos < 0)
                    {
                        distanceX = randomNumber(-minDistanceStandard.x + xPos, (-minDistanceStandard.x + xPos) * 2f);
                    }else if(xPos == 0)
                    {
                        distanceX = randomNumber(-5, 5);
                    }
                    else
                        distanceX = randomNumber(minDistanceStandard.x + xPos, minDistanceStandard.x * 2f);

                    distanceY = randomNumber(minDistanceStandard.y, minDistanceStandard.y + 15f);

                    distance = new Vector2(distanceX, distanceY + layer);

                    platformLocation[platformTrack] = distance;

                    Checker(distance, platformTrack); //length
                }

                if (safe)
                {
                    //Debug.Log("checker is true spawn position equals: " + platformLocation[platformTrack - 1]);
                    if (platformTrack > 0)
                    {
                        spawnStandardPlatform(distance);
                    }
                }

                break;
            case "tallPlatform":
                
                while(safe == false)
                {
                    distanceX = 0;
                    distanceY = 0;


                    if (xPos < 0)
                    {
                        distanceX = randomNumber(-minDistanceTall.x + xPos, (-minDistanceTall.x + xPos) * 2f);
                    }
                    else if (xPos == 0)
                    {
                        distanceX = randomNumber(-5, 5);
                    }
                    else
                        distanceX = randomNumber(minDistanceTall.x + xPos, minDistanceTall.x * 2f);


                    distanceY = randomNumber(minDistanceTall.y, minDistanceTall.y  + 15f);

                    distance = new Vector2(distanceX, distanceY + layer);

                    platformLocation[platformTrack] = distance;
                    
                    Checker(distance, platformTrack); //length
                }

                if (safe)
                {
                    //Debug.Log("checker is true spawn position equals: " + platformLocation[platformTrack]);
                    if (platformTrack > 0)
                    {
                        spawnTallPlatform(distance);
                    }
                }

                break;

            case "widePlatform":

                while (safe == false)
                {
                    distanceX = 0;
                    distanceY = 0;

                    if (xPos < 0)
                    {
                        distanceX = randomNumber(-minDsitanceWide.x + xPos, (-minDsitanceWide.x + xPos) * 2f);
                    }
                    else if (xPos == 0)
                    {
                        distanceX = randomNumber(-5, 5);
                    }
                    else
                        distanceX = randomNumber(minDsitanceWide.x + xPos, minDsitanceWide.x * 2f);

                    distanceY = randomNumber(minDsitanceWide.y, minDsitanceWide.y + 15f);

                    distance = new Vector2(distanceX, distanceY + layer);

                    platformLocation[platformTrack] = distance;

                    Checker(distance, platformTrack); //length
                }

                if (safe)
                {
                    //Debug.Log("checker is true spawn position equals: " + platformLocation[platformTrack - 1]);
                    if (platformTrack > 0)
                    {
                        spawnWidePlatform(distance);
                    }
                }

                break;
            default:
                break;
        }
    }

    //Check
    //Make this into a do-while in calculate so it can change position if need be
    void Checker(Vector3 pos, int platformTrack)
    {

        //CHANGE THIS SO IT DOESNT LOOP THROUGH ALL OBJECTS IN SCENE
        //MAKE A CIRCLE AROUND THE OBJECT TO BE AND CHECK THAT POSITION
        bool contact = false;
        
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Climb Me"))
        {
            //Debug.Log(g.transform.position + " " + platformTrack + "loop" + ": length: " + platformLocation.Length);
  
            if (pos.x < g.transform.position.x + 10 && pos.x > g.transform.position.x - 10 && pos.y < g.transform.position.y + 15 && pos.y > g.transform.position.y - 15)
            {
                    //Debug.Log("Hello" + platformTrack + "pos: (" + pos.x + ", " + pos.y + ")" + " overlapping platforms with: p" + g.transform.position.x + ", " + g.transform.position.y);
                    contact = true;
                    safe = false;
            }

            if (!contact)
                safe = true;
            else
                safe = false;
        }


    }

    //Spawn

    void spawnTallPlatform(Vector2 distance)
    {
        GameObject clone;
        clone = Instantiate(tallPlatform, new Vector3(distance.x, distance.y, 26.5f), tallPlatform.transform.rotation);
        clone.transform.SetParent(transform);
        platformList.Add(clone);
    }

    void spawnStandardPlatform(Vector2 distance)
    {
        GameObject clone;
        clone = Instantiate(standerdPlatform, new Vector3(distance.x, distance.y, 25.75f), standerdPlatform.transform.rotation);
        clone.transform.SetParent(transform);
        platformList.Add(clone);

    }

    void spawnWidePlatform(Vector2 distance)
    {
        GameObject clone;
        clone = Instantiate(widePlatform, new Vector3(distance.x, distance.y, 25.8f), widePlatform.transform.rotation);
        platformList.Add(clone);
        clone.transform.SetParent(transform);

    }

    float randomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }
}
