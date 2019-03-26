using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Debug")]
    public PlayerController holder;
    public Vector3 direction;
    public bool triggerEnabled;
    public List<iTarget> hitTarget = new List<iTarget>();

    private void OnTriggerEnter(Collider other)
    {
        Enemy potentialEnemy = other.GetComponent<Enemy>();
        if (potentialEnemy != null && direction != Vector3.zero && triggerEnabled)
        {
            Enemy enemy = potentialEnemy;
            enemy.AddDamage(GameManager.i.ballMovementManager.GetDamages());
            enemy.Push(direction, 5);
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
        EndMovement();
    }

    //Cancel the movement and enable physic on the ball
    public void EndMovement()
    {
        hitTarget.Clear();
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

    public IEnumerator BounceOnTarget_C(iTarget target)
    {
        yield return null;
        Vector3 startPosition = transform.position;
        Debug.Log("Bouncing on " + target.targetedTransform.name);
        Vector3 endPosition = target.targetedTransform.position;

        float passSpeed = GameManager.i.ballMovementManager.GetBounceSpeed();
        float passTime = Vector3.Distance(startPosition, endPosition) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.ballMovementManager.bounceMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballMovementManager.bounceAngleCurve;

        direction = endPosition - startPosition;
        for (float i = 0; i < passTime; i += Time.deltaTime)
        {
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
        transform.position = endPosition;
        hitTarget.Add(target);
        target.OnBallReceived(this);
    }
}
