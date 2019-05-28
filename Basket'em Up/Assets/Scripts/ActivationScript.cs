using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationScript : MonoBehaviour
{


    public void Event()
    {
        StartCoroutine(Action());

    }

    public IEnumerator Action()
    {
        float temp = 0;

        while (temp < 1)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + (Vector3.up * 2), temp);
            temp += Time.deltaTime/2;
            yield return null;
        }
    }
}
