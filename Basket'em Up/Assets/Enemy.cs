using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public int HPmax = 100;

    int HPcurrent;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        HPcurrent = HPmax;
    }

    public void AddDamage(int amount)
    {
        HPcurrent -= amount;
        HPcurrent = Mathf.Clamp(HPcurrent, 0, HPmax);

        if (HPcurrent <= 0)
        {
            Kill();
        }
    }

    //Push the enemy toward a direction
    public void Push(Vector3 direction, float magnitude)
    {
        direction = direction.normalized;
        direction = direction * magnitude;
        rb.AddForce(direction, ForceMode.Impulse);
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
