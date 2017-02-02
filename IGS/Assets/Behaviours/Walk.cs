using UnityEngine;
using System.Collections;

public class Walk : AbstractBehaviour {

    public float speed = 50f;

    public float runMultiplier = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);
        var run = inputState.GetButtonValue(inputButtons[2]);



        if (right || left)
        {
            var tmpSpeed = speed;

            if(run && runMultiplier > 0) //important because multipler cant be zero
            {
                tmpSpeed *= runMultiplier;
            }

            var velX = tmpSpeed * (float) inputState.direction;

            rb.velocity = new Vector2(velX, rb.velocity.y);
        }
	
	}
}
