using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBetweenPlayers : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float height;

    void Update()
    {
        transform.position = Vector3.Lerp(player1.position, player2.position, 0.5f) + Vector3.up* height;
    }
}
