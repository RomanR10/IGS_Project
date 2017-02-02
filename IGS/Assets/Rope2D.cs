using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope2D : MonoBehaviour {

    private string lastBone_name = "Bone024";
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
         rb = GetComponent<Rigidbody2D>();

        GetComponent<HingeJoint2D>().connectedBody = transform.parent.GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void FixedUpdate () {

        if(transform.name == lastBone_name)
        {
            if (GameObject.Find("Player2").GetComponent<PlayerController>().PlayerTwoAttached)
            {
                //transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.constraints = rb.constraints | RigidbodyConstraints2D.FreezePosition;
            }
            else
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
		
	}
}
