using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {

    //Will work with game controller

    public float speed = 5f;
    public Buttons[] input;

    private Rigidbody2D rb;
    private InputState inputState;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody2D>();
        inputState = GetComponent<InputState>();
	
	}
	
	// Update is called once per frame
	void Update () {

        var right = inputState.GetButtonValue(input[0]);
        var left = inputState.GetButtonValue(input[1]);
        var velX = speed;

        if(right || left)
        {
            velX *= left ? -1 : 1;
        }
        else
        {
            velX = 0;
        }

        rb.velocity = new Vector2(velX, rb.velocity.y);

    }
}
