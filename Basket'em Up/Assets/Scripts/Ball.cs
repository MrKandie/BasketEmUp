using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallMoveState { Idle, Moving, Spiky}
public class Ball : MonoBehaviour
{
    [Header("Reference")]
    public Collider triggerCollider;
    public Collider defaultCollider;
    public Transform modelTransform;

    [Header("Debug")]
    public PlayerController holder;
    public Vector3 direction;
    public bool triggerEnabled;
    public List<iTarget> hitTarget = new List<iTarget>();
    private Rigidbody rb;
    public BallMoveState state;
    private bool canBePicked;
    private GameObject highlighter;

    private void Update()
    {
        transform.localScale = new Vector3(1, 1, 1);

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetState(BallMoveState.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy potentialEnemy = other.GetComponent<Enemy>();
        if (potentialEnemy != null && direction != Vector3.zero && triggerEnabled)
        {
            Enemy enemy = potentialEnemy;
            enemy.AddDamage(GameManager.i.ballMovementManager.GetDamages());
            enemy.Slow(0.2f, 1);
            hitTarget.Add(enemy);
        }
        PlayerController potentialPlayer = other.GetComponent<PlayerController>();
        if (potentialPlayer != null && canBePicked)
        {
            potentialPlayer.TakeBall(this, 0);
            if (highlighter != null) { Destroy(highlighter); }
            hitTarget.Clear();
        }
        if (potentialPlayer != null && state == BallMoveState.Spiky)
        {
            potentialPlayer.Push(direction.normalized, 5);
            potentialPlayer.AddDamage(20);
        }
    }

    public void SetState(BallMoveState newState)
    {
        state = newState;
        switch (newState)
        {
            case BallMoveState.Idle:
                hitTarget.Clear();
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

        }
    }

    private iTarget GetNearestEnemy(float radius)
    {
        iTarget closestTarget = null;
        //Search for the target
        foreach (iTarget target in GameManager.i.levelManager.GetTargetableEnemies())
        {
            float range = Vector3.Distance(target.targetedTransform.position, this.transform.position);
            if (range < radius && hitTarget != null && !hitTarget.Contains(target))
            {
                closestTarget = target;
                radius = range;
            }
        }
        return closestTarget;
    }
}
