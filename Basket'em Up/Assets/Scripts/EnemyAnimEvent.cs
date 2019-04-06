using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    public void KillEnemyComponent()
    {
        Rookie rookieRef = transform.parent.GetComponentInParent<Rookie>();
        if (rookieRef != null)
        {
            rookieRef.Kill();
        }
        else
        {
            Trainer trainerRef = transform.parent.GetComponentInParent<Trainer>();
            trainerRef.Kill();
        }
    }
}
