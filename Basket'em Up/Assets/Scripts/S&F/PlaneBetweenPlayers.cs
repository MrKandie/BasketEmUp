using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBetweenPlayers : MonoBehaviour
{
    [Header("Components")]
    public Transform player1;
    public Transform player2;
    public Renderer myRend;

    [Space]
    [Header("Tweak")]
    public AnimationCurve goingUpCurve;
    public AnimationCurve goingDownCurve;
    public float transitionTime;

    int nbEnemyInsideZone;
    bool opacityOn;
    
    void Update()
    {
        Vector3 newPosition = (player1.position + player2.position) / 2;
        newPosition.y = (player1.position.y + player2.position.y) / 2 - 0.95f;
        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(player2.position - player1.position) * Quaternion.Euler(90, 0, 0);
        transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(player1.position, player2.position), transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            UpdateEnemiesInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            UpdateEnemiesInZone(false);
        }
    }

    void UpdateEnemiesInZone(bool _more)
    {
        if (_more)
        {
            nbEnemyInsideZone++;
            if (!opacityOn)
            {
                opacityOn = true;
                myRend.material.SetFloat("_Opacity", 1);
                //StopAllCoroutines();
                //StartCoroutine(ChangeOpacity(true));
            }
        }
        else
        {
            nbEnemyInsideZone--;
            if (nbEnemyInsideZone <= 0 && opacityOn)
            {
                opacityOn = false;
                myRend.material.SetFloat("_Opacity", 0);
                //StartCoroutine(ChangeOpacity(false));
            }
        }
    }

    IEnumerator ChangeOpacity(bool _more)
    {
        if (_more)
        {
            for (float i = 0; i < 1; i += Time.deltaTime/transitionTime)
            {
                myRend.material.SetFloat("_Opacity", goingUpCurve.Evaluate(i));
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            for (float i = 0; i < 1; i += Time.deltaTime / transitionTime)
            {
                myRend.material.SetFloat("_Opacity", goingDownCurve.Evaluate(i));
                yield return new WaitForEndOfFrame();
            }
        }

        if (_more)
        {
            myRend.material.SetFloat("_Opacity", 1);
        }
        else
        {
            myRend.material.SetFloat("_Opacity", 0);
        }

        yield return null;
    }
}
