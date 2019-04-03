using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rookie : Enemy
{
    PlayerController target;
    public int CaCDamages;
    public float CaCCooldown;
    public float CaCPushForce;
    public float chargeSpeed;
    public float chargeCooldown;
    public int chargeDamage;
    public float chargePushForce;
    public float maxChargeDistance; //In meters
    public float maxChargeTime; //In seconds

    float CaCCurrentCooldown;
    float chargeCurrentCooldown;
    bool charging;
    Coroutine chargeCoroutine;
    public bool hasBall;

    private void OnCollisionEnter(Collision collision)
    {
        if (charging)
        {
            charging = false;
            StopCoroutine(chargeCoroutine);
            if (collision.gameObject.GetType().IsSubclassOf(typeof(Enemy)))
            {
                collision.gameObject.GetComponent<Enemy>().AddDamage(chargeDamage);
                collision.gameObject.GetComponent<Enemy>().Push(transform.forward, chargePushForce);
            }
            PlayerController potentialHitPlayer = collision.gameObject.GetComponent<PlayerController>();
            if (potentialHitPlayer != null)
            {
                potentialHitPlayer.AddDamage(chargeDamage);
                potentialHitPlayer.Push(transform.forward, chargePushForce);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        {
            if (CaCCurrentCooldown > 0)
            {
                target = null;
                agent.destination = this.transform.position;
                CaCCurrentCooldown -= Time.deltaTime;
                CaCCurrentCooldown = Mathf.Clamp(CaCCurrentCooldown, 0, Mathf.Infinity);
            }
            else if (hasBall)
            {
                if (charging)
                {
                    target = null;
                }
                else
                {
                    BallMovement();
                    chargeCurrentCooldown -= Time.deltaTime;
                    chargeCurrentCooldown = Mathf.Clamp(chargeCurrentCooldown, 0, Mathf.Infinity);
                }
            }
            else
            {
                NoBallMovement();
            }
        }
    }

    private void BallMovement()
    {
        target = GetNearestPlayer();
        if (agent.enabled)
        {
            agent.speed = 0;
            agent.destination = transform.position;
        }
        Vector3 lookAtPosition = target.transform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
        if (chargeCurrentCooldown <= 0)
        {
            Charge();
            chargeCurrentCooldown = chargeCooldown;
        }
    }

    private void NoBallMovement()
    {
        target = GetNearestPlayer();
        agent.speed = speed;
        if (agent.enabled)
        {
            agent.destination = target.transform.position;
        }
        if (Vector3.Distance(target.transform.position, transform.position) < 2 && CaCCurrentCooldown <= 0)
        {
            HitPlayer(target);
            CaCCurrentCooldown = CaCCooldown;
        }
    }

    public void Charge()
    {
        chargeCoroutine = StartCoroutine(Charge_C());
    }

    public void HitPlayer(PlayerController player)
    {
        player.AddDamage(CaCDamages);
        player.Push(Vector3.Normalize(player.transform.position - transform.position), CaCPushForce);
    }

    public PlayerController GetNearestPlayer()
    {
        List<PlayerController> potentialTargets = new List<PlayerController>();
        foreach (PlayerController p in GameManager.i.levelManager.players)
        {
            potentialTargets.Add(p);
        }
        PlayerController nearestTarget = potentialTargets[0];
        float minDistance = Vector3.Distance(this.transform.position, nearestTarget.transform.position);
        foreach (PlayerController t in potentialTargets)
        {
            if (Vector3.Distance(this.transform.position, t.transform.position) < minDistance)
            {
                nearestTarget = t;
                minDistance = Vector3.Distance(this.transform.position, t.transform.position);
            }
        }
        PlayerController target = nearestTarget;
        return target;
    }

    IEnumerator Charge_C()
    {
        charging = true;
        PermaDisableNavmeshAgent();
        Vector3 startPosition = transform.position;
        float travelledDistance = 0;
        float startTime = Time.time;
        while (travelledDistance < maxChargeDistance || Time.time - startTime < maxChargeTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * chargeSpeed);
            travelledDistance = Vector3.Distance(transform.position, startPosition);
            yield return new WaitForEndOfFrame();
        }
        charging = false;
        EnableNavmeshAgent();
        yield return null;
    }
}
