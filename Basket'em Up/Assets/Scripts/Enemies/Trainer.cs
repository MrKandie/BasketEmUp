using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trainer : Enemy
{
    public enum State { Fleeing, Helping, Attacking}
    public State state;
    public float ballRecuperationCD; //How much time to spawn a new ball
    public bool hasBall;
    public float passSpeed;
    public float passHeight;
    public float attackBallSpeed;
    public float attackBallHeight;
    public float attackPushStrength = 5;
    public int attackDamage = 10;
    public GameObject ballPrefab;
    public float fleeDistanceTreshold; //A partir de combien de mètres l'entraîneur se met à fuir ?
    public float fleeCheckCooldown;

    public Material ballTrailMaterial;
    float timeBeforeNextBall;
    PlayerController playerTargeted;
    Rookie rookieTargeted;
    float timeBeforeTryingToFlee;
    List<GameObject> generatedBalls = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        timeBeforeNextBall = ballRecuperationCD;
        CheckAllRegionsAround(transform.position, 36, 2);
        agent.speed = speed;
    }
    protected override void Update()
    {
        base.Update();
        UpdateCD();
        if (IsEndangered())
        {
            state = State.Fleeing;
        }
        else
        {
            DetermineCorrectAction();
        }
        switch (state)
        {
            case State.Fleeing:
                FleeFromPlayers();
                break;
            case State.Helping:
                GiveBallToNearestRookie();
                break;
            case State.Attacking:
                ThrowBallOnNearestPlayer();
                break;
        }
    }

    void DetermineCorrectAction()
    {
        rookieTargeted = FindNearestRookie();
        if (rookieTargeted != null)
        {
            state = State.Helping;
        } else
        {
            state = State.Attacking;
        }
    }

    void ThrowBallOnNearestPlayer()
    {
        if (playerTargeted != null)
        {
            transform.LookAt(playerTargeted.transform);
        } else
        {
            playerTargeted = FindNearestPlayer();
        }
        if (hasBall)
        {
            hasBall = false;
            timeBeforeNextBall = ballRecuperationCD;
            StartCoroutine(AttackPlayer_C(playerTargeted));
            playerTargeted = null;
        }
    }

    void GiveBallToNearestRookie()
    {
        if (rookieTargeted != null)
        {
            transform.LookAt(rookieTargeted.transform);
        }
        if (hasBall)
        {
            if (rookieTargeted != null)
            {
                hasBall = false;
                timeBeforeNextBall = ballRecuperationCD;
                StartCoroutine(PassBall_C(rookieTargeted));
                rookieTargeted = null;
            }
        }
    }

    bool IsEndangered()
    {
        foreach (PlayerController player in GameManager.i.levelManager.players)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < fleeDistanceTreshold)
            {
                return true;
            }
        }
        RaycastHit hit;
        PlayerController playerA = GameManager.i.levelManager.players[0];
        PlayerController playerB = GameManager.i.levelManager.players[1];
        Vector3 direction = playerB.transform.position - playerA.transform.position;
        if (Physics.Raycast(playerA.transform.position, direction, out hit))
        {
            if (hit.transform == this.transform)
            {
                return true;
            }
        }
        return false;
    }

    void CheckAllRegionsAround(Vector3 position, int posQuantity, float radius)
    {
        int angleToCheck = Random.Range(0, 360);
        for (int i = 0; i < posQuantity; i++)
        {
            Vector3 newPosition = position;
            newPosition.x = position.x + radius * Mathf.Cos(angleToCheck);
            newPosition.z = position.z + radius * Mathf.Sin(angleToCheck);
            angleToCheck += 360 / posQuantity;
            if (IsRegionSafe(newPosition))
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPosition, out hit, 5, NavMesh.AllAreas))
                {
                    agent.destination = hit.position;
                }
                return;
            }
        }
    }

    bool IsRegionSafe(Vector3 position)
    {
        NavMeshHit hit;
        position.y += 1;
        //Check if region isn't in the sky
        if (NavMesh.SamplePosition(position, out hit, 5, NavMesh.AllAreas))
        {
            //Zone is on ground
            foreach (PlayerController player in GameManager.i.levelManager.players)
            {
                if (Vector3.Distance(player.transform.position, position) < fleeDistanceTreshold)
                {
                    return false;
                }
            }
            return true;
        } else
        {
            return false;
        }
    }

    void FleeFromPlayers()
    {
        if (timeBeforeTryingToFlee <= 0)
        {
            timeBeforeTryingToFlee = fleeCheckCooldown;
            CheckAllRegionsAround(transform.position, 36, fleeDistanceTreshold);
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject ball in generatedBalls)
        {
            Destroy(ball);
        }
    }

    void UpdateCD()
    {
        if (!hasBall) {
            if (timeBeforeNextBall > 0)
            {
                timeBeforeNextBall -= Time.deltaTime;
                timeBeforeNextBall = Mathf.Clamp(timeBeforeNextBall, 0, Mathf.Infinity);
            } else
            {
                hasBall = true;
            }
        }
        if (timeBeforeTryingToFlee > 0)
        {
            timeBeforeTryingToFlee -= Time.deltaTime;
        }
    }


    PlayerController FindNearestPlayer()
    {
        PlayerController nearestPlayer = GameManager.i.levelManager.players[0];
        float nearestDistance = Vector3.Distance(nearestPlayer.transform.position, this.transform.position);
        foreach (PlayerController player in GameManager.i.levelManager.players)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < nearestDistance)
            {
                nearestPlayer = player;
                nearestDistance = Vector3.Distance(nearestPlayer.transform.position, this.transform.position);
            }
        }
        return nearestPlayer;
    }

    Rookie FindNearestRookie()
    {
        Rookie[] allRookies = FindObjectsOfType<Rookie>();
        if (allRookies.Length == 0) { return null; }
        Rookie nearestRookie = allRookies[0];
        float nearestDistance = Vector3.Distance(nearestRookie.transform.position, this.transform.position);
        foreach (Rookie rookie in allRookies)
        {
            if (!rookie.hasBall)
            {
                if (Vector3.Distance(rookie.transform.position, this.transform.position) < nearestDistance)
                {
                    nearestRookie = rookie;
                    nearestDistance = Vector3.Distance(nearestRookie.transform.position, this.transform.position);
                }
            }
        }
        if (nearestRookie.hasBall) { return null; }
        return nearestRookie;
    }

    IEnumerator AttackPlayer_C(PlayerController player)
    {
        Vector3 startPosition = hand.transform.position;
        Vector3 endPosition = player.self.position;
        Vector3 direction = endPosition- startPosition;
        endPosition = endPosition + direction.normalized * 5;
        Vector3 playerPosition = player.self.position;
        playerPosition.y = transform.position.y;
        //Rotate players towards target and play particles
        transform.rotation = Quaternion.LookRotation(playerPosition - transform.position);

        float passTime = Vector3.Distance(startPosition, endPosition) / attackBallSpeed;
        AnimationCurve speedCurve = GameManager.i.ballManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballManager.passAngleCurve;

        GameObject ball = Instantiate(ballPrefab);
        ball.transform.Find("Trail").GetComponent<TrailRenderer>().material = ballTrailMaterial;
        generatedBalls.Add(ball);
        ball.transform.position = this.hand.position;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.SetState(BallMoveState.Spiky);
        ballScript.direction = endPosition - startPosition;

        for (float i = 0; i < passTime; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            ball.transform.position = Vector3.Lerp(startPosition, endPosition, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            ball.transform.position = new Vector3(
                    ball.transform.position.x,
                    ball.transform.position.y + (angleCurve.Evaluate(i / passTime) * attackBallHeight),
                    ball.transform.position.z
                );
        }
        ball.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(3f);
        Destroy(ball);
        yield return null;
    }

    IEnumerator PassBall_C(Rookie enemy)
    {
        Vector3 startPosition = hand.transform.position;
        Vector3 endPosition = enemy.hand.position;
        Vector3 enemyPosition = enemy.transform.position;
        enemyPosition.y = transform.position.y;
        //Rotate players towards target and play particles
        transform.rotation = Quaternion.LookRotation(enemyPosition - transform.position);

        float passTime = Vector3.Distance(startPosition, endPosition) / passSpeed;
        AnimationCurve speedCurve = GameManager.i.ballManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballManager.passAngleCurve;

        GameObject ball = Instantiate(ballPrefab);
        ball.transform.Find("Trail").GetComponent<TrailRenderer>().material = ballTrailMaterial;
        ball.transform.position = this.hand.position;
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.SetState(BallMoveState.Moving);
        ballScript.defaultCollider.enabled = false;

        for (float i = 0; i < passTime; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            //Apply speed curve
            ball.transform.position = Vector3.Lerp(startPosition, enemy.hand.position, speedCurve.Evaluate(i / passTime));

            //Apply angle curve
            ball.transform.position = new Vector3(
                    ball.transform.position.x,
                    ball.transform.position.y + (angleCurve.Evaluate(i / passTime) * passHeight),
                    ball.transform.position.z
                );
        }
        ball.transform.SetParent(enemy.hand,false);
        ball.transform.localPosition = Vector3.zero;
        enemy.hasBall = true;
        yield return null;
    }
}
