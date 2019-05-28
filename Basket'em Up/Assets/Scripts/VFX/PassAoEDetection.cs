using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassAoEDetection : MonoBehaviour
{
    public int damage;
    public float range;

    private void Start()
    {
        range = gameObject.GetComponent<ParticleSystem>().main.startSize.constant;
    }
    
    private void OnParticleTrigger()
    {
            foreach (var enemy in GameManager.i.levelManager.enemies)
            {
                if (enemy != null)
                {
                    float distance = (enemy.transform.position - transform.position).magnitude;
                    if (distance <= range)
                    {
                        enemy.AddDamage(damage);
                    }
                }
            }
    }
}

