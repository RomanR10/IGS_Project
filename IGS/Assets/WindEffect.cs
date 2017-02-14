using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour {

    public bool player1InZone = false;
    public bool player2InZone = false;

    public WindForce force_1;
    public WindForce force_2;

    public int speed = 250;


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            player1InZone = true;

            //Player number and direction
            //This wind tunnel's direction is right 
            force_1.WindForceFunction(0, true, speed);
        }

        if(col.tag == "Player2")
        {
            player2InZone = true;

            //Player number and direction
            //This wind tunnel's direction is right 
            force_2.WindForceFunction(1, true, speed);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            player1InZone = false;
        }

        if (col.tag == "Player2")
        {
            player2InZone = false;
        }
    }
}
