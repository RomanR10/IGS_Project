using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPosition : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        transform.position = new Vector2(0, Camera.main.transform.position.y + 30);
		
	}
}
