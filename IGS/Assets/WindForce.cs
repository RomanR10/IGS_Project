using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour {

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void WindForceFunction(int player, bool direction, int speed)
    {
        if(player == 0 && transform.name == "Player")
        {
            if (direction)
            {
                rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
            }else if (!direction)
            {
                rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
        }

        if (player == 1 && transform.name == "Player2")
        {
            if (direction)
            {
                rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
            }
            else if (!direction)
            {
                rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
        }
    }
}
