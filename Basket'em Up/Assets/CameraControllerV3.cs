using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerV3 : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    private Transform furthestPlayer;
    public Transform cameraDirection;

    Vector3 wantedPosition;
    Vector3 wantedForward;

    // Update is called once per frame
    void FixedUpdate()
    {
        furthestPlayer = GetFurthestPlayer();

        Vector3 dir = (furthestPlayer.position - transform.position).normalized;

        wantedPosition = Vector3.Lerp(player1.position, player2.position, 0.5f);
        wantedForward = Vector3.Lerp(transform.forward, dir, 0.01f);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, 0.2f);
        transform.forward = wantedForward;

        //Clamp camera
        float eulerAngle = transform.eulerAngles.y;
        if (eulerAngle > 180)
        {
            eulerAngle = eulerAngle - 360;
        }
        eulerAngle = Mathf.Clamp(eulerAngle, -40, 40);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, eulerAngle, transform.eulerAngles.z);
    }

    public Transform GetFurthestPlayer()
    {
        Vector3 relativePositionA = cameraDirection.InverseTransformDirection(player1.transform.position - cameraDirection.position);
        Vector3 relativePositionB = cameraDirection.InverseTransformDirection(player2.transform.position - cameraDirection.position);
        if (relativePositionA.z > relativePositionB.z)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }
}
