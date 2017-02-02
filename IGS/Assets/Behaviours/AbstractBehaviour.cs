using UnityEngine;
using System.Collections;

//abstract means you can't instaniate from game
public abstract class AbstractBehaviour : MonoBehaviour {

    public Buttons[] inputButtons;

    protected InputState inputState;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        inputState = GetComponent<InputState>();
        rb = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
