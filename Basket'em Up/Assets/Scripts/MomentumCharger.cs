using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumCharger : MonoBehaviour
{
    public float chargeTime = 3f; //Time to charge the ball, in seconds
    private List<PlayerController> playersInside = new List<PlayerController>();
    public Transform pedestralTransform;
    private bool ballIsInPedestral = false;
    private float ballCharge = 0;
    private Ball chargingBall;
    private PlayerController playerChargingBall;
    private GameObject ballChargingFX;
    private Vector3 ballChargingFXDefaultSize;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController potentialPlayer = other.GetComponent<PlayerController>();
        if (potentialPlayer != null) 
        {
            playersInside.Add(potentialPlayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController potentialPlayer = other.GetComponent<PlayerController>();
        if (potentialPlayer != null)
        {
            playersInside.Remove(potentialPlayer);
            if (playerChargingBall == potentialPlayer)
            {
                EndCharge();
            }
        }
    }

    private void Update()
    {
        if (playersInside.Count > 0)
        {
            foreach (PlayerController p in playersInside)
            {
                if (p.possessedBall != null)
                {
                    if (Input.GetAxis("Action_" + p.inputIndex.ToString()) !=0)
                    {
                        if (chargingBall == null)
                        {
                            StartCharge(p);
                        }
                    }
                }
                if (playerChargingBall != null)
                {
                    if (Input.GetAxis("Action_" + p.inputIndex.ToString()) != 0)
                    {
                        if (ballIsInPedestral)
                        {
                            ballCharge += Time.deltaTime;
                            ballChargingFX.transform.localScale = Vector3.Lerp(Vector3.one, ballChargingFXDefaultSize, ballCharge / chargeTime);
                            GameManager.i.momentumManager.IncrementMomentum(ballCharge / chargeTime - GameManager.i.momentumManager.momentum);
                            if (ballCharge >= chargeTime)
                            {
                                EndCharge();
                                return;
                            }
                        }
                    }

                    if (Input.GetAxis("Action_" + p.inputIndex.ToString()) == 0)
                    {
                        EndCharge();
                    }
                }
            }
        }
    }

    private IEnumerator PlaceBallInPedestral_C(Ball ball, float time)
    {
        Vector3 initialPosition = ball.transform.position;
        for (int i = 0; i < time; i++)
        {
            ball.transform.position = Vector3.Lerp(initialPosition, pedestralTransform.position, i / 1f);
            yield return new WaitForEndOfFrame();
        }
        ball.transform.position = pedestralTransform.position;
        ballIsInPedestral = true;
    }

    private void StartCharge(PlayerController p)
    {
        if (chargingBall == null)
        {
            playerChargingBall = p;
            chargingBall = p.possessedBall;
            ballChargingFX = Instantiate(GameManager.i.library.ballChargingFX, chargingBall.transform, false);
            ballChargingFX.transform.localPosition = Vector3.zero;
            ballChargingFXDefaultSize = ballChargingFX.transform.localScale;
            p.DropBall();
            ballCharge = 0;
            p.Freeze();
            chargingBall.SetState(BallMoveState.Blocked);
            StopAllCoroutines();
            StartCoroutine(PlaceBallInPedestral_C(chargingBall, 0.3f));
            ballCharge = GameManager.i.momentumManager.momentum * chargeTime;
        }
    }

    private void EndCharge()
    {
        if( chargingBall != null)
        {
            StopAllCoroutines();
            ballCharge = 0;
            Destroy(ballChargingFX);
            playerChargingBall.UnFreeze();
            playerChargingBall.TakeBall(chargingBall, 0.1f);
            chargingBall = null;
            playerChargingBall = null;
        }
    }
}
