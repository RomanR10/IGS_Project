using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopRope : MonoBehaviour { 

    //ATTACHS ROPE TO PLAYER ONE


	void Start () {

        GetComponent<HingeJoint2D>().connectedBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        //Debug.Log("Hello");
		
	}

    void FixedUpdate()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().PlayerOneAttached)
        {
            GetComponent<HingeJoint2D>().connectedBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            RigidbodyConstraints2D constraints2D = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = constraints2D;
        }else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            //RigidbodyConstraints2D constraints2D = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        /*
        else if(GameObject.Find("Player2").GetComponent<PlayerController>().PlayerTwoAttached)
            GetComponent<HingeJoint2D>().connectedBody = GameObject.FindGameObjectWithTag("Player2B").GetComponent<Rigidbody2D>();*/

    }

}
