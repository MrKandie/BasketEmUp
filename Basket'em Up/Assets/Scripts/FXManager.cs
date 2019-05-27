using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void EnableGhostFX(GameObject target, Material ghostMaterial, float ghostLifetime, float ghostSpawnInterval, float effectDuration = -1)
    {
        if (target.GetComponent<GhostFX>() != null) { return; }
        GhostFX gFX = target.AddComponent<GhostFX>();
        gFX.StartCoroutine(gFX.SpawnGhost_C(target, ghostMaterial, effectDuration, ghostLifetime, ghostSpawnInterval, 0));
    }

    public static void StopGhostFX(GameObject target)
    {
        GhostFX gFX = target.GetComponent<GhostFX>();
        if (gFX != null)
        {
            gFX.StopAllCoroutines();
            Destroy(gFX);
        }
    }
}
