using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : Enemy
{
    public enum State { Fleeing, Attacking, Helping}
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

    float timeBeforeNextBall;
    PlayerController playerTargeted;
    Rookie rookieTargeted;

    protected override void Awake()
    {
        base.Awake();
        timeBeforeNextBall = ballRecuperationCD;
    }
    protected override void Update()
    {
        base.Update();
        UpdateCD();
        IsEndangered();
        switch (state)
        {
            case State.Fleeing:
                FleeFromPlayers();
                break;
            case State.Attacking:
                ThrowBallOnNearestPlayer();
                break;
            case State.Helping:
                GiveBallToNearestRookie();
                break;
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
            Rookie nearestRookie = FindNearestRookie();
            if (nearestRookie != null)
            {
                state = State.Helping;
            }
            else
            {
                hasBall = false;
                timeBeforeNextBall = ballRecuperationCD;
                StartCoroutine(AttackPlayer_C(playerTargeted));
                playerTargeted = null;
            }
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
            if (rookieTargeted == null)
            {
                rookieTargeted = FindNearestRookie();
            }
            if (rookieTargeted != null)
            {
                hasBall = false;
                timeBeforeNextBall = ballRecuperationCD;
                StartCoroutine(PassBall_C(rookieTargeted));
                rookieTargeted = null;
            }
            else
            {
                state = State.Attacking;
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

    void CheckAllRegionsAround(Vector3 position)
    {
        for (int i = 0; i < 36; i++)
        {
            //Check 360°
        }
    }

    bool IsRegionSafe(Vector3 position)
    {
        position.y += 1;
        //Check if region isn't in the sky
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 5))
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
        //Find a safe region in a radius
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
        Vector3 playerPosition = player.self.position;
        playerPosition.y = transform.position.y;
        //Rotate players towards target and play particles
        transform.rotation = Quaternion.LookRotation(playerPosition - transform.position);

        float passTime = Vector3.Distance(startPosition, endPosition) / attackBallSpeed;
        AnimationCurve speedCurve = GameManager.i.ballMovementManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballMovementManager.passAngleCurve;

        GameObject ball = Instantiate(ballPrefab);
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
        yield return new WaitForSeconds(1f);
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
        AnimationCurve speedCurve = GameManager.i.ballMovementManager.passMovementCurve;
        AnimationCurve angleCurve = GameManager.i.ballMovementManager.passAngleCurve;

        GameObject ball = Instantiate(ballPrefab);
        ball.transform.position = this.hand.position;
        ball.GetComponent<Ball>().SetState(BallMoveState.Moving);

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
