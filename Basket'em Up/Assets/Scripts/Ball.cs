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

    //Find a random target around 
    public void BounceOnNearbyTargets()
    {
        iTarget nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != null)
        {
            BounceOnTarget(GetNearestEnemy());
        } else
        {
            BounceOnGround();
        }
    }

    //The ball will bounce on a target
    public void BounceOnTarget(iTarget target)
    {
        StartCoroutine(BounceOnTarget_C(target));
    }

    //The ball will bounce and fall on a random nearby position on the ground, it'll create a mark so a player can catch the ball
    public void BounceOnGround()
    {
        RaycastHit hit;
        Vector3 directionNormalized = direction.normalized * 4;
        Vector3 groundPosition = transform.position + directionNormalized;
        if (Physics.Raycast(groundPosition,
            Vector3.down,
            out hit))
            {
                groundPosition.y = hit.point.y + 0.7f;
            }
        StartCoroutine(BounceOnGround_C(groundPosition));
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

    private iTarget GetNearestEnemy()
    {
        iTarget closestTarget = null;
        float closestRadius = GameManager.i.ballMovementManager.bounceRadius;
        //Search for the target
        foreach (iTarget target in GameManager.i.levelManager.GetTargetableEnemies())
        {
            float range = Vector3.Distance(target.targetedTransform.position, this.transform.position);
            if (range < closestRadius && hitTarget != null && !hitTarget.Contains(target))
            {
                closestTarget = target;
                closestRadius = range;
            }
        }
        return closestTarget;
    }

    public IEnumerator BounceOnGround_C(Vector3 position)
    {
        highlighter = Instantiate(GameManager.i.library.highlighter);
        highlighter.transform.position = position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = position;
        float passSpeed = GameManager.i.ballMovementManager.GetBounceSpeed() * GameManager.i.ballMovementManager.bounceOnGroundSpeedCoef;
        float passTime = Mathf.Clamp(Vector3.Distance(startPosition, endPosition), 4, Mathf.Infinity) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.ballMovementManager.bounceMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballMovementManager.bounceAngleCurve;
        direction = endPosition - startPosition;

        for (float i = 0; i < passTime; i += Time.deltaTime)
        {
            if (i > passTime/2 && !canBePicked)
            {
                canBePicked = true;
            }
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            transform.position = Vector3.Lerp(startPosition, endPosition, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            transform.position = new Vector3(
                    transform.position.x,
                    startPosition.y + (angleCurve.Evaluate(i / passTime) * GameManager.i.ballMovementManager.GetBounceHeight()),
                    transform.position.z
                );
        }
        Destroy(highlighter);
        transform.position = endPosition;
        SetState(BallMoveState.Idle);
        GameManager.i.momentumManager.DecrementMomentum(GameManager.i.momentumManager.momentumLosseWhenBallTouchGround);
        yield return null;
    }

    public IEnumerator BounceOnTarget_C(iTarget target)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.targetedTransform.position;
        float passSpeed = GameManager.i.ballMovementManager.GetBounceSpeed();
        float passTime = Mathf.Clamp(Vector3.Distance(startPosition, endPosition),4,Mathf.Infinity) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.ballMovementManager.bounceMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballMovementManager.bounceAngleCurve;
        direction = endPosition - startPosition;

        for (float i = 0; i < passTime; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            transform.position = Vector3.Lerp(startPosition, target.targetedTransform.position, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            transform.position = new Vector3(
                    transform.position.x,
                    startPosition.y + (angleCurve.Evaluate(i / passTime) * GameManager.i.ballMovementManager.GetBounceHeight()),
                    transform.position.z
                );
        }
        transform.position = target.targetedTransform.position;
        hitTarget.Add(target);
        target.OnBallReceived(this);
    }
}
