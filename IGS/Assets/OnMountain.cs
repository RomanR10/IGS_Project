using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMountain : MonoBehaviour {

    public GameObject mountain;

    private Vector3 mountainScale;
    private Vector3 mountainPosition;
    private Vector3 magTowardsCenter;


	// Use this for initialization
	void Start () {

        if (mountain != null)
        {
            mountainScale = mountain.transform.localScale;
            mountainPosition = mountain.transform.position;
            Debug.Log(mountainScale);
            Debug.Log(mountainPosition);
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 playerPosition = this.transform.position;

        //magTowardsCenter = new Vector3(playerPosition.x - (mountainPosition.x + (mountainScale.x / 2)), playerPosition.y, playerPosition.z - (mountainPosition.z + (mountainScale.z / 2)));

        //Debug.Log(magTowardsCenter);
		
	}

    void FixedUpdate()
    {
        Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            //Debug.Log("Object found: " + hit.collider.name);
            float distance = hit.distance;
            //Debug.Log(distance);

            //float force = 10;

            //t//his.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3())

            //Debug.DrawLine(transform.position, hit.point);
            //if (hit.collider.name == "Mountain")
                //Debug.DrawLine(transform.position, hit.point);


        }
    }
}
