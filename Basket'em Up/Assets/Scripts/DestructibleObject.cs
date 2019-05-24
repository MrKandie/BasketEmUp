using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [MinMaxSlider(0, 500)] public Vector2 hp;
    private int currentHP;
    public void TakeDamage(int amount)
    {
        
    }
}
