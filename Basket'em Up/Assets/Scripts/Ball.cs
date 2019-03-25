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
        if (other.gameObject.tag == "enemy" && direction != Vector3.zero)
        {
            Enemy target = other.GetComponent<Enemy>();
            target.AddDamage(GameManager.i.momentumManager.GetMomentumDamages());
            target.Push(direction, 5);
        }
    }
}
