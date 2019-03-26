using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerV2 : MonoBehaviour
{

    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Transform player1;
    public Transform player2;
    Vector3 dirToP2;

    // Update is called once per frame
    void Update()
    {
        dirToP2 = player2.position - player1.position;
        transform.rotation = Quaternion.LookRotation(dirToP2) * Quaternion.Euler(rotationOffset);
        transform.position = player1.position + positionOffset;
    }
}
