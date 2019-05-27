using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFX : MonoBehaviour
{
    public IEnumerator SpawnGhost_C(GameObject visualsToDuplicate, Material ghostMaterial, float effectDuration, float ghostLifetime, float ghostSpawnInterval, float time)
    {
        if (time <= effectDuration || effectDuration < 0)
        {
            GameObject dup = Instantiate(visualsToDuplicate);
            dup.transform.position = visualsToDuplicate.transform.position;
            dup.transform.rotation = visualsToDuplicate.transform.rotation;

            //Freezes the animator of the ghost
            Animator potentialAnimator = dup.GetComponentInChildren<Animator>();
            if (potentialAnimator != null)
            {
                potentialAnimator.speed = 0;
            }

            //Removes useless components
            Component[] componentList = dup.GetComponentsInChildren(typeof(Component));
            foreach (Component c in componentList)
            {
                if (c.GetType() != typeof(MeshRenderer) && c.GetType() != typeof(Animator) && c.GetType() != typeof(MeshFilter) && c.GetType() != typeof(Transform) && c.GetType() != typeof(Light))
                {
                    Destroy(c);
                }
            }

            //Apply ghost material
            MeshRenderer[] meshRenderers = dup.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in meshRenderers)
            {
                mr.material = ghostMaterial;
            }
            AutoDestroy autoDestroyer = dup.AddComponent<AutoDestroy>();
            autoDestroyer.InitAutoDestruction(ghostLifetime);

            yield return new WaitForSeconds(ghostSpawnInterval);
            time += ghostSpawnInterval;
            yield return StartCoroutine(SpawnGhost_C(visualsToDuplicate, ghostMaterial, effectDuration, ghostLifetime, ghostSpawnInterval, time));
        }
    }
}
