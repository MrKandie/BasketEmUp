using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rookie : Enemy
{
    Transform target;

    public override void AddDamage(int amount)
    {
        base.AddDamage(amount);
    }

    public bool hasBall;
    protected override void Update()
    {
        base.Update();
        {
            if (hasBall)
            {

            }
            else
            {
                NoBallMovement();
            }
        }
    }

    private void NoBallMovement()
    {
        target = GetNearestPlayer();
        agent.speed = speed;
        if (agent.enabled)
        {
            agent.destination = target.position;
        }
    }

    public Transform GetNearestPlayer()
    {
        List<Transform> potentialTargets = new List<Transform>();
        foreach (PlayerController p in GameManager.i.levelManager.players)
        {
            potentialTargets.Add(p.transform);
        }
        Transform nearestTarget = potentialTargets[0];
        float minDistance = Vector3.Distance(this.transform.position, nearestTarget.position);
        foreach (Transform t in potentialTargets)
        {
            if (Vector3.Distance(this.transform.position, t.position) < minDistance)
            {
                nearestTarget = t;
                minDistance = Vector3.Distance(this.transform.position, t.position);
            }
        }
        Transform target = nearestTarget;
        return target;
    }
}
