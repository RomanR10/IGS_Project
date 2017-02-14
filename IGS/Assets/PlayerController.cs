using System.Collections;
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

    //private bool Anchor = false; //false == player 1 |||||||||||| true == player 2
    private GameObject anchor;

    private GameObject[] ClimbableSurfaces;
    private Vector3[] SurfacePositions = new Vector3[50];
    private Vector3[] SurfaceScales = new Vector3[50];
    private string SurfaceTag = "Climb Me";

    private GameObject FirstPlayer;
    private GameObject SecondPlayer;
    private bool isSecondPlayer = false;

    public GameObject[] Rope = new GameObject[25];
    //private string RopeTag = "Rope";
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


    //ModMenu
    private ModMenu modMenu;

    Vector3 direction; //second direction
    Vector3 directionOne;
    bool allowForce = true;


    float lerpTime = .5f;
    float currentLerpTime;
    private bool startLerp = false;

    private float playersDistanceFromMain = 0f;
    private float cameraFloor = 20f;
    private float fallTime = 0f;

    //STARTGAME
    private bool single = false;

    //Attach/deattach toggle
    private bool toggle = false;
    private bool p1_toggle = false;
    private bool p2_toggle = false;

    //SINGLE PLAYER
    private bool playerAttached = true;

    //Juice
    private bool juice = true;

    private bool objMoving1 = false;
    private bool objMoving2 = false;
    private Vector2 objPos1;
    private Vector2 objPos2;

    // Use this for initialization
    void Start()
    {
        PlayerOneAttached = true;
        gameStart = true;

        RopeParent = GameObject.Find("Rope_Short").gameObject;
        //modMenu = GameObject.Find("ModMenu").GetComponent<ModMenu>();

        //Debug.Log("single status: " + PlayerPrefs.GetInt("single")); //hehe single status

        if (PlayerPrefs.GetInt("single") == 0)
            single = true;
        else if (PlayerPrefs.GetInt("single") == 1)
            single = false;

        manager = GameObject.Find("GameManager");
        modMenu = manager.GetComponent<ModMenu>();
        Score_manager = manager.GetComponent<HighScore_Manager>();

        if (transform.tag == "Player2")
            isSecondPlayer = true;
        else
            isSecondPlayer = false;

        if (!isSecondPlayer) //First Player
        {
            if (GetComponent<Rigidbody2D>() != null)
                rb1 = GetComponent<Rigidbody2D>();

            //init for other gameobject
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

            //init for other gameobject
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
        //Debug.Log(ropeLength);
        SecondPlayerEnd = Rope[ropeLength - 1]; //For some reason find game objects places the last parented gameObject first second player starts on bottom
        //Debug.Log(SecondPlayerEnd.transform.position);

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

            PlayerTwoJoint.autoConfigureConnectedAnchor = false;

            transform.parent = SecondPlayerEnd.transform;

        }
        else
        {
            //First Player joint!!
            PlayerOneJoint = FirstPlayer.AddComponent<HingeJoint2D>();

            JointAngleLimits2D limits = PlayerOneJoint.limits;

            limits.min = -359;
            limits.max = 359;
            PlayerOneJoint.limits = limits;

            //PlayerOneJoint.autoConfigureConnectedAnchor = false;

            //joint.connectedBody = TopRopeSegment.GetComponent<Rigidbody>();
            PlayerOneJoint.useLimits = true;
        }



        if (juice)
        {
            //JUICEE
            p1_OgMat = GameObject.Find("Character01_StiffyMode").GetComponent<Renderer>().material;
            p2_OgMat = GameObject.Find("Character02_StiffyMode").GetComponent<Renderer>().material;

            p1_Lighter = Resources.Load("light_blue", typeof(Material)) as Material;
            p2_Lighter = Resources.Load("light_red", typeof(Material)) as Material;
        }
    }

    void Update()
    {

        if (modMenu.Motion)
        {
            allowForce = true;
        }
        else if (!modMenu.Motion)
            allowForce = false;

        if (!allowForce)
            transform.rotation = Quaternion.Euler(0, 0, 0);


        if (isSecondPlayer && !PlayerTwoAttached && PlayerOneAttached && allowForce)
        {
            direction = transform.position - FirstPlayer.transform.position;
            transform.right = Vector3.Cross(direction, Vector3.forward);
        }
        else if (!isSecondPlayer && !PlayerOneAttached && PlayerTwoAttached)
        {
            //directionOne = transform.position - SecondPlayer.transform.position;
            //transform.right = Vector3.Cross(directionOne, Vector3.forward);
        }
        //Debug.DrawLine(FirstPlayer.transform.position, SecondPlayer.transform.position, Color.red);


        if (single)
        {
            singlePlayerClimb();
        }
    }

    void FixedUpdate()
    {
        checkCollisionState();
        updatePlayerValues();

        if (!single)
        {
            if (!isSecondPlayer)
                p1_climb(PlayerOneAttached);
            else
                p2_climb(PlayerTwoAttached);
        }


        //DEATHHHHHHHHHH

        if (!PlayerOneAttached && !PlayerTwoAttached)
            fallTime += Time.deltaTime;
        else
            fallTime = 0f;

        playersDistanceFromMain = Camera.main.transform.position.y - transform.position.y;

        if (transform.position.y < -2)
            resetPlayers();

        if (playersDistanceFromMain > cameraFloor && fallTime > 10)
        {
            //Debug.Log("Kill player");
            //Debug.Log(fallTime);
        }

        //Debug.Log(Camera.main.transform.position.y - transform.position.y);


        if (!isSecondPlayer)
        {
            if (transform.position.y > SecondPlayer.transform.position.y)
                HighestPlayer = true;
            else
                HighestPlayer = false;
        }
        else
        {
            if (transform.position.y > rb1.transform.position.y)
                HighestPlayer = true;
            else
                HighestPlayer = false;
        }


        //Handle movement!!!!!!!!!!!!!!!!!!!!
        if (!single)
            coopMovement();
        else if (single)
            singlePlayerMovement();


        /*if (PlayerOneAttached && objMoving1)
        {
            FirstPlayer.transform.position = new Vector3(objPos1.x, FirstPlayer.transform.position.y, FirstPlayer.transform.position.z);
        }else if(PlayerTwoAttached && objMoving2)
        {
            SecondPlayer.transform.position = new Vector3(objPos2.x, SecondPlayer.transform.position.y, SecondPlayer.transform.position.z);
        }*/

    }

    //MOVEMENT CODE
    void coopMovement()
    {
        if (isSecondPlayer && PlayerOneAttached && !PlayerTwoAttached)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal_2");
            if (h > 0)
            {
                if (!allowForce)
                    rb2.AddForce(Vector2.right * speed, ForceMode2D.Force);
                else
                    rb2.AddForce(transform.right * -speed);
            }
            else if (h < 0)
            {
                if (!allowForce)
                    rb2.AddForce(Vector2.left * speed, ForceMode2D.Force);
                else
                    rb2.AddForce(transform.right * speed);
            }



        }
        else if (!isSecondPlayer && PlayerTwoAttached && !PlayerOneAttached)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (h > 0)
            {
                rb1.AddForce(Vector2.right * speed * 2, ForceMode2D.Force);
            }
            else if (h < 0)
            {
                rb1.AddForce(Vector2.left * speed * 2, ForceMode2D.Force);
            }
        }
    }

    void singlePlayerMovement()
    {
        if (isSecondPlayer && PlayerOneAttached && !PlayerTwoAttached)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (h > 0)
            {
                if (!allowForce)
                    rb2.AddForce(Vector2.right * speed, ForceMode2D.Force);
                else
                    rb2.AddForce(transform.right * -speed);
            }
            else if (h < 0)
            {
                if (!allowForce)
                    rb2.AddForce(Vector2.left * speed, ForceMode2D.Force);
                else
                    rb2.AddForce(transform.right * speed);

            }
        }
        else if (!isSecondPlayer && PlayerTwoAttached && !PlayerOneAttached)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (h > 0)
            {
                rb1.AddForce(Vector2.right * speed * 2, ForceMode2D.Force);
            }
            else if (h < 0)
            {
                rb1.AddForce(Vector2.left * speed * 2, ForceMode2D.Force);
            }
        }
    }

    //ATTACHING?REATTACHING

    /***
 *       ___   _   _           _    
 *      / __| | | (_)  _ __   | |__ 
 *     | (__  | | | | | '  \  | '_ \
 *      \___| |_| |_| |_|_|_| |_.__/
 *                                  
 */

    void singlePlayerClimb()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical != 0)
        {

            if (toggle == false)
            {
                toggle = true;
                if (vertical < 0 && playerAttached)
                {
                    if (PlayerOneAttached && oneReattachBuffer && !isSecondPlayer && !PlayerTwoAttached)
                    {
                        //Debug.Log("p1_deattach");
                        p1_deattach();
                        oneDisconnectBuffer = false;
                        StartCoroutine("waitForOne", .5f);
                        playerAttached = false;
                    }
                    else if (PlayerTwoAttached && twoReattachBuffer && isSecondPlayer && !PlayerOneAttached)
                    {
                        //Debug.Log("p2_deattach");
                        twoDisconnectBuffer = true;
                        StartCoroutine("waitForTwo", .5f);
                        p2_deattach();
                        playerAttached = false;
                    }
                }
                else if (vertical > 0 && !playerAttached)
                {

                    if (!PlayerTwoAttached && PlayerOneClimb && oneDisconnectBuffer && !isSecondPlayer && !PlayerOneAttached && !playerAttached)
                    {
                        playerAttached = true;

                        //Debug.Log("p1_reattach");
                        oneReattachBuffer = false;
                        StartCoroutine("waitForReattach", .5f);
                        p1_reattach();
                    }
                    else if (!PlayerOneAttached && PlayerTwoClimb && !twoDisconnectBuffer && isSecondPlayer && !PlayerTwoAttached && !playerAttached)
                    {
                        playerAttached = true;
                        //Debug.Log("p2_reattach" + oneReattachBuffer);
                        twoReattachBuffer = false;
                        StartCoroutine("waitForReattachTwo", .5f);
                        p2_reattach();
                    }
                }
            }
        }

        if (vertical == 0)
            toggle = false;

    }

    void p1_climb(bool attached)
    {
        //Handle if Playeroneattached/playeroneconnectiontosurface
        float vertical = Input.GetAxis("Vertical");
        //Debug.Log(vertical +" : "+ attached);

        if (vertical != 0)
        {

            if (p1_toggle == false)
            {
                if (vertical < 0 && attached && oneReattachBuffer)
                {
                    p1_deattach();
                    oneDisconnectBuffer = false;
                    StartCoroutine("waitForOne", .5f);
                }
                else if (vertical < 0 && !attached && PlayerOneClimb && oneDisconnectBuffer) //Handle reattaching PlayerOneClimb equals the ray cast condition
                {
                    p1_reattach();
                    oneReattachBuffer = false;
                    StartCoroutine("waitForReattach", .5f);
                }

                p1_toggle = true;

            }
        }

        if (vertical == 0)
            p1_toggle = false;
    }

    void p1_deattach()
    {
        PlayerOneAttached = false;
        PlayerOneJoint.enabled = false;
        rb1.constraints = RigidbodyConstraints2D.FreezeRotation;
        calculateForceNeeded(1);
    }

    void p1_reattach()
    {
        PlayerOneJoint.enabled = true;
        PlayerOneAttached = true;
        rb1.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb1.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void p2_climb(bool attached)
    {
        float vertical = CrossPlatformInputManager.GetAxis("Vertical_2");

        if (vertical != 0)
        {
            if (p2_toggle == false)
            {
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

            p2_toggle = true;

        }

        if (vertical == 0)
            p2_toggle = false;

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
        rb2.constraints = RigidbodyConstraints2D.FreezeAll;//startLerp = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);

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
        if (Rope[0].GetComponent<HingeJoint2D>().jointAngle > 360 || Rope[0].GetComponent<HingeJoint2D>().jointAngle < -360)
        {
            //Debug.Log("Hello am i awake");
            resetRopeAngles();
        }
    }

    private IEnumerator resetRopeAngles()
    {
        foreach (GameObject r in Rope)
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

        playerAttached = true;
        PlayerOneAttached = true;
        PlayerTwoAttached = false;
        rb2.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //   ___         _   _   _        _              
    //  / __|  ___  | | | | (_)  ___ (_)  ___   _ _  
    // | (__  / _ \ | | | | | | (_-< | | / _ \ | ' \ 
    //  \___| \___/ |_| |_| |_| /__/ |_| \___/ |_||_|
    //CollisionSystem
    void checkCollisionState()
    {

        if (!isSecondPlayer)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward + new Vector3(0,0,3), out hit))
            {
                //print("Player 1 found an object - distance: " + hit.distance + " obj name: " + hit.transform.name);

                if (hit.transform.tag == SurfaceTag)
                {
                    PlayerOneClimb = true;
                    //Debug.Log(hit.transform.position);

                    if (PlayerOneAttached)
                    {
                        hit.transform.GetComponentInChildren<PlatformSeperator>().playerConnected = true;
                        if (hit.transform.GetComponentInChildren<PlatformSeperator>().trigger)
                            objMoving1 = true;

                        if (objMoving1)
                        {
                            objPos1 = hit.transform.position;
                        }
                    }else
                        hit.transform.GetComponentInChildren<PlatformSeperator>().playerConnected = false;

                }
                else
                    PlayerOneClimb = false;

                //JUICE
                if (!single)
                {
                    if (PlayerOneClimb)
                    {
                        p1_juice(PlayerOneClimb, PlayerOneAttached);
                    }
                }
                else if (single)
                {
                    if (PlayerOneClimb && !PlayerTwoAttached)
                    {
                        p1_juice(PlayerOneClimb, PlayerOneAttached);
                    }
                }

            }
            else
                PlayerOneClimb = false;

            //JUICE
            if (!PlayerOneClimb)
            {
                p1_juice(PlayerOneClimb, PlayerOneAttached);
            }


        }
        else if (isSecondPlayer)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward + new Vector3(0, 0, 3), out hit))
            {
                // print("Player 2 found an object - distance: " + hit.distance + " obj name: " + hit.transform.name);
                if (hit.transform.tag == SurfaceTag)
                {
                    PlayerTwoClimb = true;

                    if (PlayerTwoAttached)
                    {
                        hit.transform.GetComponentInChildren<PlatformSeperator>().playerConnected = true;
                        if (hit.transform.GetComponentInChildren<PlatformSeperator>().trigger)
                            objMoving2 = true;

                        if (objMoving2)
                        {
                            objPos2 = hit.transform.position;
                        }
                    }
                    else
                        hit.transform.GetComponentInChildren<PlatformSeperator>().playerConnected = false;

                }
                else
                    PlayerTwoClimb = false;

                //JUICE
                if (!single)
                {
                    if (PlayerTwoClimb)
                    {
                        p2_juice(PlayerTwoClimb, PlayerTwoAttached);
                    }
                }
                else if (single)
                {
                    if (PlayerTwoClimb && !PlayerTwoAttached)
                    {
                        p2_juice(PlayerTwoClimb, PlayerTwoAttached);
                    }
                }

            }
            else
                PlayerTwoClimb = false;

            //JUICE
            if (!PlayerTwoClimb)
            {
                p2_juice(PlayerTwoClimb, PlayerTwoAttached);
            }

        }

        gameStart = false;

    }

    //Update player's values for eachother
    void updatePlayerValues()
    {
        if (isSecondPlayer)
        {
            PlayerOneConnectionToSurface = FirstPlayer.GetComponent<PlayerController>().PlayerOneConnectionToSurface;
            PlayerOneClimb = FirstPlayer.GetComponent<PlayerController>().PlayerOneClimb;
            PlayerOneAttached = FirstPlayer.GetComponent<PlayerController>().PlayerOneAttached;
            // oneDisconnectBuffer = FirstPlayer.GetComponent<PlayerController>().oneDisconnectBuffer;
            // oneReattachBuffer = FirstPlayer.GetComponent<PlayerController>().oneReattachBuffer;
            //PlayerDeattached = FirstPlayer.GetComponent<PlayerController>().PlayerDeattached;
            // PlayerAttached = FirstPlayer.GetComponent<PlayerController>().PlayerAttached;
        }
        else if (!isSecondPlayer)
        {
            PlayerTwoConnectionToSurface = SecondPlayer.GetComponent<PlayerController>().PlayerTwoConnectionToSurface;
            PlayerTwoClimb = SecondPlayer.GetComponent<PlayerController>().PlayerTwoClimb;
            PlayerTwoAttached = SecondPlayer.GetComponent<PlayerController>().PlayerTwoAttached;
            //twoDisconnectBuffer = SecondPlayer.GetComponent<PlayerController>().twoDisconnectBuffer;
            //twoReattachBuffer = SecondPlayer.GetComponent<PlayerController>().twoReattachBuffer;
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


    //     _   _   _   __  __   ___     ___    ___    ___    ___   ___        
    //  _ | | | | | | |  \/  | | _ \   | __|  / _ \  | _ \  / __| | __|  ___  
    // | || | | |_| | | |\/| | |  _/   | _|  | (_) | |   / | (__  | _|  (_-<  
    //  \__/   \___/  |_|  |_| |_|     |_|    \___/  |_|_\  \___| |___| /__/ 
    //Calculate force based off of player's velocity
    void calculateForceNeeded(int playerNum)
    {
        if (playerNum == 2)
        {
            //Debug.Log(rb2.velocity);
            bool direction = false; //Direction false == left and true == right
            if (rb1.velocity.x > 0)
                direction = true;
            else
                direction = false;

            applyForce(direction, 2); //Send direction of player two since player one is connected to surface + send highest player details for the apply force to figure direction
            //PlayerTwoConnectionToSurface = false;

        }
        else if (playerNum == 1)
        {

            // Debug.Log(rb1.velocity + " FirstPlayerSpeed");
            // Debug.Log(rb2.velocity + " SecondPlayerSpeed");

            bool direction = false; //Direction false == left and true == right
            if (rb2.velocity.x > 0)
                direction = true;
            else
                direction = false;

            applyForce(direction, 1); //Send direction of player two since player one is connected to surface + send highest player details for the apply force to figure direction
                                      // PlayerOneConnectionToSurface = false;
        }
    }

    //Apply force from calculated force
    void applyForce(bool direction, int attachedPlayer)
    {
        //THIS ONLY WORKS FOR WHEN PLAYER ONE IS ATTACHED 
        if (direction && attachedPlayer == 1)
        {

            rb1.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

            rb2.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

        }
        else if (!direction && attachedPlayer == 1)
        {

            rb1.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

            rb2.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);

        }
        else if (direction && attachedPlayer == 2 && rb1.velocity.x > 2)
        {

            rb2.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES

            rb1.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);


            //Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");
            //Debug.Log("PLAYER two JUMP OFF FORCE");

            // Rigidbody2D rbJoint = PlayerOneJoint.GetComponent<Rigidbody2D>();
            // rbJoint.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse);
        }
        else if (!direction && attachedPlayer == 2 && rb1.velocity.x < -2)
        {

            rb2.AddForce(Vector2.right * jumpOffSpeed, ForceMode2D.Impulse); //PLAYER ONE FORCES


            rb1.AddForce(Vector2.up * jumpOffSpeed * 2, ForceMode2D.Impulse);


            /*
             Debug.Log("PLAYER two JUMP OFF FORCE");
             Rigidbody2D rbJoint = PlayerOneJoint.GetComponent<Rigidbody2D>();
             rbJoint.AddForce(Vector2.left * jumpOffSpeed, ForceMode2D.Impulse);
             Debug.Log("ENGAGE JUMP OFF FORCE PLAYER TWO");
           */


        }


    }

    //LIGHTS CAMERA ACTION - 
    //     _          _                _             _                 _          _              _ 
    //  _ | |  _  _  (_)  __   ___    | |__   __ _  | |__   _  _      (_)  _  _  (_)  __   ___  | |
    // | || | | || | | | / _| / -_)   | '_ \ / _` | | '_ \ | || |     | | | || | | | / _| / -_) |_|
    //  \__/   \_,_| |_| \__| \___|   |_.__/ \__,_| |_.__/  \_, |    _/ |  \_,_| |_| \__| \___| (_)
    //                                                      |__/    |__/                           

    void p1_juice(bool condition, bool conditionTwo)
    {
        if (juice)
        {
            if (!single)
            {
                if (condition)
                {
                    GameObject temper = GameObject.Find("Character01_StiffyMode");
                    temper.GetComponent<Renderer>().material = p1_Lighter;
                }
            }
            else if (single)
            {
                if (condition && !conditionTwo)
                {
                    GameObject temper = GameObject.Find("Character01_StiffyMode");
                    temper.GetComponent<Renderer>().material = p1_Lighter;
                }
            }


            if (!condition)
            {
                GameObject temp = GameObject.Find("Character01_StiffyMode");
                temp.GetComponent<Renderer>().material = p1_OgMat;
            }
        }
    }

    void p2_juice(bool condition, bool conditionTwo)
    {
        if (juice)
        {
            if (!single)
            {
                if (condition)
                {
                    GameObject temper = GameObject.Find("Character02_StiffyMode");
                    temper.GetComponent<Renderer>().material = p2_Lighter;
                }
            }
            else if (single)
            {
                if (condition && !conditionTwo)
                {
                    GameObject temper = GameObject.Find("Character02_StiffyMode");
                    temper.GetComponent<Renderer>().material = p2_Lighter;
                }
            }


            if (!condition)
            {
                GameObject temp = GameObject.Find("Character02_StiffyMode");
                temp.GetComponent<Renderer>().material = p2_OgMat;
            }
        }

    }
}
