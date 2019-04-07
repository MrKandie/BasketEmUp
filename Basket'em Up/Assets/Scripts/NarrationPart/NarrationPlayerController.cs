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
    public SoundManager soundManager;

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
        soundManager.PlayRandomSound(soundManager.whoosh, true);
        ball.transform.parent = null;
        ballRebound.isTaken = false;
        ballRebound.takenBy = null;
        
        if (player1IsTrue)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / timeToPass)
            {
                ball.position = Vector3.Lerp(myHand.position+ballRebound.offsetPlayer1, otherPlayerHand.position + ballRebound.offsetPlayer2, i);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            for (float i = 0; i < 1; i += Time.deltaTime / timeToPass)
            {
                ball.position = Vector3.Lerp(myHand.position + ballRebound.offsetPlayer2, otherPlayerHand.position + ballRebound.offsetPlayer1, i);
                yield return new WaitForEndOfFrame();
            }
        }
        otherPlayerController.ballInHand = true;
        ball.transform.parent = otherPlayerHand;
        ballRebound.isTaken = true;
        ballRebound.takenBy = otherPlayerController;
        soundManager.PlayRandomSound(soundManager.ballCatch, true);
        yield return null;
    }

    void UpdatePosition()
    {
        if (player1IsTrue)
        {
            Vector3 inputVector1 = Input.GetAxis("Horizontal_1") * Camera.main.transform.right;
            Vector3 inputVector2 = Input.GetAxis("Vertical_1") * Camera.main.transform.forward;
            Vector3 finalVector = inputVector1 * horizontalSpeedMultiplier + inputVector2;
            rb.AddForce(finalVector * acceleration);
        }
        else
        {
            Vector3 inputVector1 = Input.GetAxis("Horizontal_2") * Camera.main.transform.right;
            Vector3 inputVector2 = Input.GetAxis("Vertical_2") * Camera.main.transform.forward;
            Vector3 finalVector = inputVector1 * horizontalSpeedMultiplier + inputVector2;
            rb.AddForce(finalVector * acceleration);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        myAnim.SetFloat("Blend", rb.velocity.magnitude / maxSpeed);
    }

    void UpdateRotation()
    {
        if (rb.velocity.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), 0.5f);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
