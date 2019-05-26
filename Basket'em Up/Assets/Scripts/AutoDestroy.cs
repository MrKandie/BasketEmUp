using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allow the autodestruction of the gameobject attached, after calling "InitAutoDestruction"
public class AutoDestroy : MonoBehaviour
{
    public void InitAutoDestruction(float time)
    {
        StartCoroutine(Destroy_C(time));
    }

    public IEnumerator Destroy_C(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
