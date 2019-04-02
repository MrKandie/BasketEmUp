using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerV3 : MonoBehaviour
{
    [System.Serializable]
    public class CameraTarget
    {
        public Transform transform;
        [Range(0,1)]
        public float importance;
    }

    public Transform player1;
    public Transform player2;
    private Transform furthestPlayer;
    public Transform cameraDirection;
    public Camera cameraObj;
    public float minCamDistance;
    public float maxCamDistance;
    public float cameraSpeed;

    public float outOfCameraTreshold = 0.1f; //Si on augmente cette valeur, le joueur dépassera moins de la caméra
    public float inCameraTreshold = 0.2f; //Si on augmente cette valeur, la caméra se rezoomera quand les joueurs sont proche du centre de l'écran
    

    public CameraTarget[] targets;

    Vector3 wantedPosition;
    Vector3 wantedForward;

    public enum CameraState { Main, Dunk, Shoot};
    CameraState myState = CameraState.Main;

    void FixedUpdate()
    {
        switch (myState)
        {
            case CameraState.Main:
                MainStateUpdate();
                UpdateZoom();
                break;
            case CameraState.Dunk:
                break;
            case CameraState.Shoot:
                break;
        }
    }

    void UpdateZoom()
    {
        int targetsOutOfCameraCount = 0;
        int targetsInCameraTresholdCount = 0;
        foreach (CameraTarget t in targets)
        {
            Vector2 targetPosViewport = Camera.main.WorldToViewportPoint(t.transform.position);
            if (targetPosViewport.x < outOfCameraTreshold || targetPosViewport.y < outOfCameraTreshold || targetPosViewport.x > 1- outOfCameraTreshold  || targetPosViewport.y > 1- outOfCameraTreshold)
            {
                targetsOutOfCameraCount++;
            } else if (targetPosViewport.x < 1-inCameraTreshold && targetPosViewport.x > inCameraTreshold && targetPosViewport.y < 1-inCameraTreshold && targetPosViewport.y > inCameraTreshold)
            {
                targetsInCameraTresholdCount++;
            }
        }
        if (targetsOutOfCameraCount > 0)
        {
            if (Vector3.Distance(cameraObj.transform.position, this.transform.position) < maxCamDistance)
            {
                cameraObj.transform.position -= cameraObj.transform.forward * 0.1f * cameraSpeed;
            }
        } else if (targetsInCameraTresholdCount >= targets.Length)
        {
            if (Vector3.Distance(cameraObj.transform.position, this.transform.position) > minCamDistance)
            {
                cameraObj.transform.position += cameraObj.transform.forward * 0.1f * cameraSpeed;
            }
        }
    }

    void MainStateUpdate()
    {
        furthestPlayer = GetFurthestPlayer();

        Vector3 dir = (furthestPlayer.position - transform.position).normalized;
        Vector3 center = new Vector3();
        foreach (CameraTarget t in targets)
        {
            center += t.transform.position * t.importance;
        }
        center = center / targets.Length;
        wantedPosition = center;
        wantedForward = Vector3.Lerp(transform.forward, dir, 0.01f);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, 0.2f);
        transform.forward = wantedForward;

        //Clamp camera
        float eulerAngleY = transform.eulerAngles.y;
        if (eulerAngleY > 180)
        {
            eulerAngleY = eulerAngleY - 360;
        }
        eulerAngleY = Mathf.Clamp(eulerAngleY, -40, 40);

        float eulerAngleX = transform.eulerAngles.x;
        if (eulerAngleX > 180)
        {
            eulerAngleX = eulerAngleX - 360;
        }
        eulerAngleX = Mathf.Clamp(eulerAngleX, -15, 360);

        transform.eulerAngles = new Vector3(eulerAngleX, eulerAngleY, transform.eulerAngles.z);
    }

    public Transform GetFurthestPlayer()
    {
        Vector3 relativePositionA = cameraDirection.InverseTransformDirection(player1.transform.position - cameraDirection.position);
        Vector3 relativePositionB = cameraDirection.InverseTransformDirection(player2.transform.position - cameraDirection.position);
        if (relativePositionA.z > relativePositionB.z)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }
}
