using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRebound : MonoBehaviour
{
    public Transform ballModel;
    public bool isTaken;
    public AnimationCurve reboundCurve;
    public float reboundHeight;
    float timerRebound;
    public Vector3 offsetPlayer1;
    public Vector3 offsetPlayer2;

    // Update is called once per frame
    void Update()
    {
        if (!isTaken)
        {
            ballModel.position = transform.position;
            timerRebound = 0;
        }
        else
        {
            transform.position = transform.parent.position;
            timerRebound += Time.deltaTime;
            ballModel.position = transform.position - 1 * reboundHeight * (1-reboundCurve.Evaluate(timerRebound % 1)) * Vector3.up;
        }
    }
}
