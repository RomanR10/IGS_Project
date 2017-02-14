using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnMove : MonoBehaviour {

    private GameObject player2;
    private Rigidbody2D rb2;


	// Use this for initialization
	void Start () {

        player2 = GameObject.Find("Player2");
        rb2 = player2.GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update() {

        if (rb2.velocity.x > 5)
        {
            if (gameObject.name == "Right_Key")
                gameObject.SetActive(false);
        } else if (rb2.velocity.x < -5)
        {
            if (gameObject.name == "Left_Key")
                gameObject.SetActive(false);

        }

    }
}
