using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [Tooltip("Object loses 1 hp per hit")] [Range(0,10)] public int hp;
    private int currentHP;

    private void Awake()
    {
        currentHP = hp;
    }
    public void Damage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, hp);
        if (currentHP <= 0)
        {
            KillObject();
        }
    }

    public void KillObject()
    {
        Destroy(gameObject);
    }
}
