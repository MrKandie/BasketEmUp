using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationPlayerController : MonoBehaviour
{
    public Animator myAnim;
    public bool player1IsTrue;
    public float acceleration;
    public float maxSpeed;
    public float horizontalSpeedMultiplier;
    Vector3 actuelDirection;
    public Rigidbody rb;
    public bool ballInHand;
    public Transform otherPlayerHand;
    public NarrationPlayerController otherPlayerController;
    public Transform myHand;
    public Transform ball;
    public BallRebound ballRebound;
    public float timeToPass;

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ballInHand)
        {
            PassBall();
        }
    }

    void PassBall()
    {
        ballInHand = false;
        StartCoroutine(PassBallCoroutine());
    }

    IEnumerator PassBallCoroutine()
    {
        ball.transform.parent = null;
        ballRebound.isTaken = false;
        for (float i = 0; i < 1; i+=Time.deltaTime/timeToPass)
        {
            ball.position = Vector3.Lerp(myHand.position, otherPlayerHand.position, i);
            yield return new WaitForEndOfFrame();
        }
        otherPlayerController.ballInHand = true;
        ball.transform.parent = otherPlayerHand;
        ballRebound.isTaken = true;
        yield return null;
    }

    void UpdatePosition()
    {
        if (player1IsTrue)
        {
            Vector3 inputVector1 = Input.GetAxis("Horizontal_1") * Camera.main.transform.right;
            Vector3 inputVector2 = Input.GetAxis("Vertical_1") * Camera.main.transform.forward;
            Vector3 finalVector = inputVector1 * horizontalSpeedMultiplier + inputVector2;
            print(finalVector.normalized);
            rb.AddForce(finalVector * acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
        }
    }

    void UpdateRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), 0.8f);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
