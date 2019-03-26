using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Debug")]
    public PlayerController holder;
    public Vector3 direction;

    private void OnTriggerEnter(Collider other)
    {
        Enemy potentialEnemy = other.GetComponent<Enemy>();
        if (potentialEnemy != null && direction != Vector3.zero)
        {
            Enemy enemy = potentialEnemy;
            enemy.AddDamage(GameManager.i.momentumManager.GetMomentumDamages());
            enemy.Push(direction, 5);
        }
    }
}
