using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallMoveState { Idle, Moving, Spiky, Blocked}
public class Ball : MonoBehaviour
{
    [Header("Reference")]
    public Collider triggerCollider;
    public Collider defaultCollider;
    public Transform modelTransform;
    private Light pointLight;

    [Header("Debug")]
    public PlayerController holder;
    public Vector3 direction;
    public bool triggerEnabled;
    private Rigidbody rb;
    public BallMoveState state;
    private bool canBePicked;
    public float damageModifier = 1;

    private void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pointLight = GetComponent<Light>();
        SetState(BallMoveState.Idle);
    }

    // Change ball light intensity
    public void SetBallLightIntensity(float newIntensity)
    {
        pointLight.intensity = newIntensity;
    }

    // Ball collision with other objects
    private void OnTriggerEnter(Collider other)
    {
        //Check for a collision with an enemy
        Enemy potentialEnemy = other.GetComponent<Enemy>();
        if (potentialEnemy != null && direction != Vector3.zero && triggerEnabled)
        {
            Enemy enemy = potentialEnemy;
            enemy.AddDamage(Mathf.RoundToInt(GameManager.i.ballManager.GetDamages() * damageModifier));
            enemy.Slow(0.2f, 1);
            GameManager.i.momentumManager.IncrementMomentum(GameManager.i.momentumManager.momentumGainedPerEnemyHit);
        }
        
        //Check for a collision with a player (To pick the ball when it's on the ground)
        PlayerController potentialPlayer = other.GetComponent<PlayerController>();
        if (potentialPlayer != null && canBePicked)
        {
            potentialPlayer.TakeBall(this, 0);
        }

        //Check for a collision with a player (Ball belong to an enemy and is "spiky")
        if (potentialPlayer != null && state == BallMoveState.Spiky)
        {
            potentialPlayer.Push(direction.normalized, 5);
            potentialPlayer.AddDamage(20);
        }

        //Check for a destructible object
        // TODO


        //Check for an activable Object
        ActivableObject potentialActObject = other.GetComponent<ActivableObject>();
        if (potentialActObject != null)
        {
            potentialActObject.Activate();
            //Diminish the Momentum of the value
        }
    }

    public void SetState(BallMoveState newState)
    {
        state = newState;
        switch (newState)
        {
            case BallMoveState.Idle:
                rb.isKinematic = false;
                defaultCollider.enabled = true;
                canBePicked = true;
                break;
            case BallMoveState.Moving:
                rb.isKinematic = true;
                defaultCollider.enabled = false;
                canBePicked = false;
                break;
            case BallMoveState.Spiky:
                canBePicked = false;
                defaultCollider.enabled = true;
                break;
            case BallMoveState.Blocked:
                canBePicked = false;
                rb.isKinematic = true;
                defaultCollider.enabled = false;
                break;
        }
    }

    private iTarget GetNearestEnemy(float radius)
    {
        iTarget closestTarget = null;
        //Search for the target
        foreach (iTarget target in GameManager.i.levelManager.GetTargetableEnemies())
        {
            float range = Vector3.Distance(target.targetedTransform.position, this.transform.position);
            if (range < radius)
            {
                closestTarget = target;
                radius = range;
            }
        }
        return closestTarget;
    }
}
