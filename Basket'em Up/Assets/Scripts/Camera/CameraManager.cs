using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("CombatCameraBehaviour")]
    public CameraBehaviour combatBehav;

    [Header("PlatformCameraBehaviour")]
    public CameraBehaviour platformBehav;

    [Header("ActivatedBehaviour")]
    public string activatedBehav;

    public enum CameraState { Combat, Platform};
    CameraState myState = CameraState.Platform;

    [System.Serializable]
    public class CameraTarget
    {
        public Transform transform;
        [Range(0, 1)]
        public float importance;
    }

    [Header("SharedVariables")]
    public Transform player1;
    public Transform player2;
    private Transform furthestPlayer;
    public Camera cameraObj;
    public Transform cameraRef;

    public CameraTarget[] targets;

    private void Awake()
    {
        combatBehav.parent = this;
        platformBehav.parent = this;
        ChangeCameraMode(CameraState.Platform);
    }

    public void ChangeCameraMode(CameraState state)
    {
        switch(myState)
        {
            case CameraState.Combat:
                combatBehav.enabled = true;
                platformBehav.enabled = false;
                activatedBehav = "Combat";
                    break;
            case CameraState.Platform:
                combatBehav.enabled = false;
                platformBehav.enabled = true;
                activatedBehav = "Platform";
                break;
        }
        return;
    }

}
