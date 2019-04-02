using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationPlayerController : MonoBehaviour
{
    public Animator myAnim;
    public bool player1IsTrue;
    public float acceleration;
    public float maxSpeed;
    Vector3 actuelDirection;
    public Rigidbody rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    void UpdatePosition()
    {
        if (player1IsTrue)
        {
            Vector3 inputVector1 = Input.GetAxis("Horizontal_1") * Camera.main.transform.right;
            Vector3 inputVector2 = Input.GetAxis("Vertical_1") * Camera.main.transform.forward;
            Vector3 finalVector = inputVector1 + inputVector2;
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
