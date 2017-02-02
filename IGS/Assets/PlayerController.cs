﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    public float speed = 100f;
    public float jumpOffSpeed = 200f;

    public bool PlayerOneConnectionToSurface = false;
    public bool PlayerTwoConnectionToSurface = false;
    public bool HighestPlayer = false;
    public bool PlayerOneAttached = false;
    public bool PlayerTwoAttached = false;

    private bool oneDisconnectBuffer = false;
    private bool oneReattachBuffer = true;

    private bool twoDisconnectBuffer = false;
    private bool twoReattachBuffer = false;

    private bool Anchor = false; //false == player 1 |||||||||||| true == player 2
    private GameObject anchor;

    private GameObject[] ClimbableSurfaces;
    private Vector3[] SurfacePositions = new Vector3[50];
    private Vector3[] SurfaceScales = new Vector3[50];
    private string SurfaceTag = "Climb Me";

    private GameObject FirstPlayer;
    private GameObject SecondPlayer;
    private bool isSecondPlayer = false;

    public GameObject[] Rope = new GameObject[25];
    private string RopeTag = "Rope";
    private GameObject SecondPlayerEnd;
    private GameObject TopRopeSegment;
    private GameObject RopeParent;

    private Rigidbody2D rb1;
    private Rigidbody2D rb2;
    private HingeJoint2D PlayerOneJoint;
    private HingeJoint2D PlayerTwoJoint;

    private bool gameStart = false;
    public bool PlayerOneClimb = false;
    public bool PlayerTwoClimb = false;

    private Vector3 playerOneStartPosition;
    private Vector3 playerTwoStartPosition;

    private GameObject ROPEobj;

    //JUICE BABYYYYYYY OHHHHHHYEAHHHHHHHHHHHHH
    private Material p1_OgMat;
    private Material p1_Lighter;

    private Material p2_OgMat;
    private Material p2_Lighter;

    //GameManager
    private GameObject manager;
    private HighScore_Manager Score_manager;


    Vector3 direction; //second direction
    Vector3 directionOne;
    bool allowForce = true;


    float lerpTime = .5f;
    float currentLerpTime;
    private bool startLerp = false;

    // Use this for initialization
    void Start()
    {
        PlayerOneAttached = true;
        gameStart = true;

        RopeParent = GameObject.Find("Rope_Short").gameObject;

        manager = GameObject.Find("GameManager");
        Score_manager = manager.GetComponent<HighScore_Manager>();

        if (transform.tag == "Player2")
            isSecondPlayer = true;
        else
            isSecondPlayer = false;

        if (!isSecondPlayer) //First Player
        {
            if (GetComponent<Rigidbody2D>() != null)
                rb1 = GetComponent<Rigidbody2D>();

            rb2 = GameObject.Find("Player2").GetComponent<Rigidbody2D>();

            //Used for resetting player on respawn
            playerOneStartPosition = transform.position;

            //ROPEobj = gameObject.transform.parent.gameObject;
            //Debug.Log(gameObject.transform.parent);
            //Debug.Log(ROPEobj);



        }
        else if (isSecondPlayer) //Second Player
        {
            if (GetComponent<Rigidbody2D>() != null)
                rb2 = GetComponent<Rigidbody2D>();

            rb1 = GameObject.Find("Player").GetComponent<Rigidbody2D>();

            //Used for resetting player on respawn
            playerTwoStartPosition = transform.position;

            //ROPEobj = FirstPlayer.transform.parent.GetComponent<GameObject>();
        }


        if (!isSecondPlayer)
        {
            SecondPlayer = GameObject.Find("Player2");
            FirstPlayer = gameObject;
        }
        else
        {
            SecondPlayer = gameObject;
            FirstPlayer = GameObject.Find("Player");
        }

        FirstPlayer.transform.parent = GameObject.Find("ROPE").transform;
        RopeParent.transform.parent = FirstPlayer.transform;
        anchor = FirstPlayer.transform.parent.gameObject; //ANCHOR IF THE BLUE PLAYER STARTS GAME ANCHORED
        //Mountain = GameObject.Find("Mountain");


        //Surfaces


        if (GameObject.FindGameObjectsWithTag(SurfaceTag) != null)
            ClimbableSurfaces = GameObject.FindGameObjectsWithTag(SurfaceTag);
        else
            Debug.Log("Please add tag: '" + SurfaceTag + "' to climbable surfaces");

        int i = 0;

        foreach (GameObject Surface in ClimbableSurfaces)
        {
            SurfacePositions[i] = Surface.transform.position;
            SurfaceScales[i] = Surface.transform.localScale;

            i++;

        }

        //Rope

        //if (GameObject.FindGameObjectsWithTag(RopeTag) != null)
        //Rope = GameObject.FindGameObjectsWithTag(RopeTag);

        int ropeLength = Rope.Length;
        Debug.Log(ropeLength);
        SecondPlayerEnd = Rope[ropeLength - 1]; //For some reason find game objects places the last parented gameObject first second player starts on bottom
        Debug.Log(SecondPlayerEnd.transform.position);

        TopRopeSegment = Rope[0];

        if (isSecondPlayer)
        {
            //Second Players joint!!
            PlayerTwoJoint = SecondPlayer.AddComponent<HingeJoint2D>();

            PlayerTwoJoint.connectedBody = SecondPlayerEnd.GetComponent<Rigidbody2D>();
            //PlayerTwoJoint.useLimits = true;
            //PlayerTwoJoint.useMotor = true;
            JointAngleLimits2D limits = PlayerTwoJoint.limits;

            limits.min = -359;
            limits.max = 359;

            PlayerTwoJoint.limits = limits;

            transform.parent = SecondPlayerEnd.transform;

        } else
        {
            //First Player joint!!
            PlayerOneJoint = FirstPlayer.AddComponent<HingeJoint2D>();

            JointAngleLimits2D limits = PlayerOneJoint.limits;

            limits.min = -359;
            limits.max = 359;
            PlayerOneJoint.limits = limits;


            //joint.connectedBody = TopRopeSegment.GetComponent<Rigidbody>();
            PlayerOneJoint.useLimits = true;
        }




        //JUICEE
        p1_OgMat = GameObject.Find("Character01_StiffyMode").GetComponent<Renderer>().material;
        p2_OgMat = GameObject.Find("Character02_StiffyMode").GetComponent<Renderer>().material;

        p1_Lighter = (Material)Resources.Load("light_blue", typeof(Material)) as Material;
        p2_Lighter = Resources.Load("light_red", typeof(Material)) as Material;
        


    }



    void Update()
    {
        //Rigidbody2D rb2 = GetComponent<Rigidbody2D>();
        //Debug.Log(rb.velocity);
        //Debug.Log(PlayerOneJoint.jointAngle);
        //Debug.Log(PlayerTwoJoint.jointAngle + " WHATS UP PLAYER TWO JOINT");
        //Debug.Log(Rope[0].GetComponent<HingeJoint2D>().jointAngle);

        /*timeCounter += Time.deltaTime * speed;
        float x = Mathf.Cos(timeCounter) * width;
        float y = Mathf.Sin(timeCounter) * height;

        //SecondPlayer.transform.position = new Vector3(x + SecondPlayer.transform.position.x, y + SecondPlayer.transform.position.y, SecondPlayer.transform.position.z);
        SecondPlayer.transform.position = new Vector3(x + anchor.transform.position.x, y + anchor.transform.position.y, SecondPlayer.transform.position.z);

        Vector2 center = anchor.transform.position;
        float radius = 6.3f;
        angle += Input.GetAxis("Horizontal") * Time.deltaTime * speed / radius;
        var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        SecondPlayer.transform.position = center + offset;*/

        if (isSecondPlayer && !PlayerTwoAttached && PlayerOneAttached)
        {
            //direction = transform.position - FirstPlayer.transform.position;
            //transform.right = Vector3.Cross(direction, Vector3.forward);
        }
        else if (!isSecondPlayer && !PlayerOneAttached && PlayerTwoAttached)
        {
            //directionOne = transform.position - SecondPlayer.transform.position;
            //transform.right = Vector3.Cross(directionOne, Vector3.forward);
            Debug.Log("Hello");
        }
        Debug.DrawLine(FirstPlayer.transform.position, SecondPlayer.transform.position, Color.red);

    }


    void FixedUpdate()
    {
        checkCollisionState();
        updatePlayerValues();

        if(!isSecondPlayer)
            p1_climb(PlayerOneAttached);
        else
            p2_climb(PlayerTwoAttached);


            /*if (allowForce && isSecondPlayer)
            {
                rb2.AddForce(transform.right * speed);
            }*/


            //checkRopeAngles();

            /* if (Rope[0].GetComponent<HingeJoint2D>().jointAngle > 360 || Rope[0].GetComponent<HingeJoint2D>().jointAngle < -360)
             {
                 resetRopeAngles();
             }*/

            if (transform.position.y < -2)
            resetPlayers();

        if (!isSecondPlayer)
        {
            if (transform.position.y > SecondPlayer.transform.position.y)
                HighestPlayer = true;
            else
                HighestPlayer = false;
        }
        else
        {
            if (transform.position.y > GameObject.Find("Player").transform.position.y)
                HighestPlayer = true;
            else
                HighestPlayer = false;
        }

        if (isSecondPlayer && PlayerOneAttached && !PlayerTwoAttached)
        {
            //Debug.Log(PlayerTwoJoint.jointAngle);
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal_2");
            if (h > 0)
            {
                rb2.AddForce(Vector2.right * speed, ForceMode2D.Force);
                //rb2.AddForce(transform.right * -speed);

                //if(transform.position.y >= )
                //transform.RotateAround(FirstPlayer.transform.position, Vector2.right, 100 * Time.deltaTime);
                //transform.LookAt(FirstPlayer.transform.position);


            }
            else if (h < 0)
            {
                rb2.AddForce(Vector2.left * speed, ForceMode2D.Force);
                //rb2.AddForce(transform.right * speed);

            }



        }
        else if (!isSecondPlayer && PlayerTwoAttached && !PlayerOneAttached)
        {
            //Debug.Log(PlayerTwoJoint.jointAngle);
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (h > 0)
            {
                //Debug.Log("Hello");
                rb1.AddForce(Vector2.right * speed * 2, ForceMode2D.Force);
                //rb1.AddForce(transform.right * -speed);

                //if(transform.position.y >= )
                //transform.RotateAround(FirstPlayer.transform.position, Vector2.right, 100 * Time.deltaTime);
                //transform.LookAt(FirstPlayer.transform.position);


            }
            else if (h < 0)
            {
                rb1.AddForce(Vector2.left * speed * 2, ForceMode2D.Force);
                //rb1.AddForce(transform.right * speed);

            }
        }
    }

    //ATTACHING?REATTACHING

    void p1_climb(bool attached)
    {
       //Handle if Playeroneattached/playeroneconnectiontosurface
        float vertical = Input.GetAxis("Vertical");
        //Debug.Log(vertical +" : "+ attached);

        if (vertical < 0 && attached && oneReattachBuffer)
        {
            p1_deattach();
            oneDisconnectBuffer = false;
            StartCoroutine("waitForOne", .5f);
        }
        else if(vertical < 0 && !attached && PlayerOneClimb && oneDisconnectBuffer) //Handle reattaching PlayerOneClimb equals the ray cast condition
        {
            p1_reattach();
            oneReattachBuffer = false;
            StartCoroutine("waitForReattach", .5f);
        }
    }

    void p1_deattach()
    {
        PlayerOneAttached = false;
        PlayerOneJoint.enabled = false;
        calculateForceNeeded(1);
    }

    void p1_reattach()
    {
        PlayerOneJoint.enabled = true;
        PlayerOneAttached = true;
    }

    void p2_climb(bool attached)
    {
        //Handle if Playeroneattached/playeroneconnectiontosurface
        float vertical = CrossPlatformInputManager.GetAxis("Vertical_2");
        //Debug.Log(vertical);
        //Debug.Log(Anchor);
        //Debug.Log(twoDisconnectBuffer);

       // if (attached)
        //    Debug.Log(vertical + " : 'player2' " + attached);

        //if (vertical > 0 && PlayerTwoClimb && attached)
        //{
        //    rb2.AddForce(Vector2.up * speed * Time.deltaTime);
        //}


        if (vertical < 0 && attached && twoReattachBuffer)
        {
            p2_deattach();
            twoDisconnectBuffer = true;
            StartCoroutine("waitForTwo", .5f);
        }
        else if (vertical < 0 && !attached && PlayerTwoClimb && !twoDisconnectBuffer) //Handle reattaching PlayerOneClimb equals the ray cast condition
        {
           // Debug.Log("reattach");
            p2_reattach();
            twoReattachBuffer = false;
            StartCoroutine("waitForReattachTwo", .5f);
        }
    }

    void p2_deattach()
    {
        PlayerTwoAttached = false;
        rb2.constraints = RigidbodyConstraints2D.FreezeRotation;
        //PlayerTwoJoint.enabled = false;
        calculateForceNeeded(2);
    }

    void p2_reattach()
    {
        //PlayerTwoJoint.enabled = true;
        PlayerTwoAttached = true;
        startLerp = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.rotation = Vector3.Lerp(transform.rotation, Quater, perc);
    }

    private IEnumerator lerpReset()
    {

        //transform.rotation = Quaternion.Euler(Mathf.Lerp(transform.rotation.x, 0, perc), Mathf.Lerp(transform.rotation.y, 0, perc), Mathf.Lerp(transform.rotation.z, 0, perc));

        yield return new WaitForSeconds(lerpTime);

        startLerp = false;
        Debug.Log("Lerp reset complete");
        rb2.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | rb2.constraints;

    }

    private IEnumerator waitForOne(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        oneDisconnectBuffer = true;
    }
    private IEnumerator waitForReattach(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        oneReattachBuffer = true;
    }
    private IEnumerator waitForReattachTwo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        twoReattachBuffer = true;
    }

    private IEnumerator waitForTwo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        twoDisconnectBuffer = false;
    }

    void checkRopeAngles()
    {
        if(Rope[0].GetComponent<HingeJoint2D>().jointAngle > 360 || Rope[0].GetComponent<HingeJoint2D>().jointAngle < -360)
        {
            Debug.Log("Hello am i awake");
            resetRopeAngles();
        }
    }

    private IEnumerator resetRopeAngles()
    {
        foreach(GameObject r in Rope)
        {
            r.GetComponent<HingeJoint2D>().useLimits = true;
        }
        yield return new WaitForSeconds(2);
        foreach (GameObject r in Rope)
        {
            r.GetComponent<HingeJoint2D>().useLimits = false;
        }
    }

    private IEnumerator ReAttachWait(bool attached)
    {
        yield return new WaitForSeconds(.1f);
        PlayerOneAttached = attached;
        PlayerOneConnectionToSurface = attached;
    }

    //GAME LOOP RESET PLAYERS
    void resetPlayers()
    {
        if (!isSecondPlayer)
        {
            //Record score yay!
            //Score_manager.Death(1);

            rb1.velocity = Vector3.zero;
            transform.position = playerOneStartPosition;
            PlayerOneJoint.enabled = true;
            PlayerOneConnectionToSurface = true;
        }
        else if (isSecondPlayer)
        {
            rb2.velocity = Vector3.zero;
            //transform.position = playerTwoStartPosition;
        }

        //rb1.velocity = new Vector3(0, 0, 0);
        //rb2.velocity = new Vector3(0, 0, 0);

        PlayerOneAttached = true;
        PlayerTwoAttached = false;
    }

    void checkCollisionState()
    {

        if (!isSecondPlayer)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                //print("Player 1 found an object - distance: " + hit.distance + " obj name: " + hit.transform.name);

                if (hit.transform.tag == SurfaceTag)
                    PlayerOneClimb = true;
                else
                    PlayerOneClimb = false;

                //JUICE
                if (PlayerOneClimb)
                {
                    GameObject temper = GameObject.Find("Character01_StiffyMode");
                    temper.GetComponent<Renderer>().material = p1_Lighter;
                }
            }
            else
                PlayerOneClimb = false;

            //JUICE
            if (!PlayerOneClimb)
            {
                GameObject temp = GameObject.Find("Character01_StiffyMode");
                temp.GetComponent<Renderer>().material = p1_OgMat;
            }


        }
        else if (isSecondPlayer)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
               // print("Player 2 found an object - distance: " + hit.distance + " obj name: " + hit.transform.name);
                if (hit.transform.tag == SurfaceTag)
                    PlayerTwoClimb = true;
                else
                    PlayerTwoClimb = false;

                //JUICE
                if (PlayerTwoClimb)
                {
                    GameObject temper = GameObject.Find("Character02_StiffyMode");
                    temper.GetComponent<Renderer>().material = p2_Lighter;
                }

            }
            else
                PlayerTwoClimb = false;

            //JUICE
            if (!PlayerTwoClimb)
            {
                GameObject temp = GameObject.Find("Character02_StiffyMode");
                temp.GetComponent<Renderer>().material = p2_OgMat;
            }

        }

        gameStart = false;

    }

    void updatePlayerValues()
    {
        if (isSecondPlayer)
        {
            PlayerOneConnectionToSurface = FirstPlayer.GetComponent<PlayerController>().PlayerOneConnectionToSurface;
            PlayerOneClimb = FirstPlayer.GetComponent<PlayerController>().PlayerOneClimb;
            PlayerOneAttached = FirstPlayer.GetComponent<PlayerController>().PlayerOneAttached;
            //PlayerDeattached = FirstPlayer.GetComponent<PlayerController>().PlayerDeattached;
           // PlayerAttached = FirstPlayer.GetComponent<PlayerController>().PlayerAttached;
        }
        else if (!isSecondPlayer)
        {
            PlayerTwoConnectionToSurface = SecondPlayer.GetComponent<PlayerController>().PlayerTwoConnectionToSurface;
            PlayerTwoClimb = SecondPlayer.GetComponent<PlayerController>().PlayerTwoClimb;
            PlayerTwoAttached = SecondPlayer.GetComponent<PlayerController>().PlayerTwoAttached;
            //PlayerDeattached = SecondPlayer.GetComponent<PlayerController>().PlayerDeattached;
            //PlayerAttached = SecondPlayer.GetComponent<PlayerController>().PlayerAttached;

        }


        PlayerOneJoint = FirstPlayer.GetComponent<HingeJoint2D>();
        PlayerTwoJoint = SecondPlayer.GetComponent<HingeJoint2D>();
    }

    void freezePlayerTwo()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        RigidbodyConstraints ogConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | ogConstraints;
    }

    void calculateForceNeeded(int playerNum)
    {
        if(playerNum == 2)
        {
            //Debug.Log(rb2.velocity);
            bool direction = false; //Direction false == left and true == right
            if (rb1.velocity.x > 0)
                direction = true;
            else
                direction = false;

            applyForce(direction, 2); //Send direction of player two since player one is connected to surface + send highest player details for the apply force to figure direction
            PlayerTwoConnectionToSurface = false;

        }
        else if(playerNum == 1)
        {

           // Debug.Log(rb1.velocity + " FirstPlayerSpeed");
           // Debug.Log(rb2.velocity + " SecondPlayerSpeed");

            bool direction = false; //Direction false == left and true == right
            if (rb2.velocity.x > 0)
                direction = true;
            else
                direction = false;

            applyForce(direction, 1); //Send direction of player two since player one is connected to surface + send highest player details for the apply force to figure direction
            PlayerOneConnectionToSurface = false;
        }
    }

    void applyForce(bool direction, int attachedPlayer)
    {
        //THIS ONLY WORKS FOR WHEN PLAYER ONE IS ATTACHED 
        if (direction && attachedPlayer == 1)
        {

            rb1.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

           // Debug.Log("PLAYER ONE JUMP OFF FORCE");

           Rigidbody2D rbJoint = PlayerTwoJoint.GetComponent<Rigidbody2D>();
           rbJoint.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse);

            rb2.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

            //Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");

        }
        else if (!direction && attachedPlayer == 1)
        {

            rb1.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

           // Debug.Log("PLAYER ONE JUMP OFF FORCE");

             Rigidbody2D rbJoint = PlayerTwoJoint.GetComponent<Rigidbody2D>();
             rbJoint.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse);

            rb2.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

           // Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");
        }
        else if (direction && attachedPlayer == 2)
        {

            rb2.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

            //Debug.Log("PLAYER two JUMP OFF FORCE");

            Rigidbody2D rbJoint = PlayerOneJoint.GetComponent<Rigidbody2D>();
            rbJoint.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse);

            rb1.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

            //Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");

        }
        else if (!direction && attachedPlayer == 2)
        {

            rb2.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

            //Debug.Log("PLAYER two JUMP OFF FORCE");

            Rigidbody2D rbJoint = PlayerOneJoint.GetComponent<Rigidbody2D>();
            rbJoint.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse);

            rb1.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

            // Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");
        }


    }

}