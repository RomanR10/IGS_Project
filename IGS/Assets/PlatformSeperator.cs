using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSeperator : MonoBehaviour {

    public float force = 3;

    public bool trigger = false;
    public bool playerConnected = false;
   // private Vector3 colPosition;
    //private Rigidbody2D rigidbody2D;

    void Start()
    {
        //rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!trigger)
        {
            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            thisBody.isKinematic = true;
        }else if (trigger)
        {
            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            thisBody.isKinematic = false;
        }

        if(playerConnected)
        {
            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            thisBody.isKinematic = true;
        }
        else if (!playerConnected)
       {
            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            thisBody.isKinematic = false;
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Pusher")
        {
            trigger = true;
            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            Rigidbody thatBody = col.GetComponentInParent<Rigidbody>();
            var rel = new Vector2(thisBody.position.x, 0) - new Vector2(thatBody.position.x, 0);

            rel.Normalize();
            thisBody.AddForce(rel * force);
        }


        //Debug.Log(col.tag);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Pusher" && trigger)
        {
            trigger = false;

        }
        //Debug.Log(col.tag);
    }
}
