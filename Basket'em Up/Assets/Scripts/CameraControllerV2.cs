using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerV2 : MonoBehaviour
{

    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Transform player1;
    public Transform player2;
    public Transform dirToP2;

    Vector3 wantedPosition;
    Quaternion wantedRotation;

    // Update is called once per frame
    void FixedUpdate()
    {
        dirToP2.position = player1.position;
        dirToP2.rotation = Quaternion.LookRotation(player2.position - player1.position);

        wantedRotation = dirToP2.rotation * Quaternion.Euler(rotationOffset);
        wantedPosition = player1.position + positionOffset.x * dirToP2.right + positionOffset.y * dirToP2.up + positionOffset.z * dirToP2.forward;

        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, 0.2f);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, 0.2f);
    }
}
