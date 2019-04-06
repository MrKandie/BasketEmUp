using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRebound : MonoBehaviour
{
    public Transform ballModel;
    public bool isTaken;
    public AnimationCurve reboundCurve;
    public float reboundHeightPlayer1;
    public float reboundHeightPlayer2;
    float timerRebound;
    public Vector3 offsetPlayer1;
    public Vector3 offsetPlayer2;
    public NarrationPlayerController takenBy;

    // Update is called once per frame
    void Update()
    {
        if (!isTaken)
        {
            ballModel.position = transform.position;
            timerRebound = 0;
        }
        else if(takenBy != null)
        {
            timerRebound += Time.deltaTime;
            if (takenBy.player1IsTrue)
            {
                transform.position = transform.parent.position + offsetPlayer1;
                ballModel.position = transform.position - 1 * reboundHeightPlayer1 * (1 - reboundCurve.Evaluate(timerRebound % 1)) * Vector3.up;
            }
            else
            {
                transform.position = transform.parent.position + offsetPlayer2;
                ballModel.position = transform.position - 1 * reboundHeightPlayer2 * (1 - reboundCurve.Evaluate(timerRebound % 1)) * Vector3.up;
            }
        }
    }
}
