using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationPlayerController : MonoBehaviour
{
    public Animator myAnim;
    public bool player1IsTrue;
    Vector3 actuelDirection;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
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
            rb.velocity = Vector3.Lerp(rb.velocity, inputVector2 + inputVector1, 0.1f);
        }
        else
        {
            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal_1"), 0, Input.GetAxis("Vertical_1"));
            rb.velocity = Vector3.Lerp(rb.velocity, inputVector, 0.1f);
        }
    }

    void UpdateRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), 0.1f);
    }
}
