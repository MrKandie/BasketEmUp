using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    public string Name;


    public CameraManager parent;
    public float minCamDistance;
    public float maxCamDistance;
    public float cameraSpeed;
    [MinMaxSlider(-360, 360)]
    public Vector2 xClamp;

    [MinMaxSlider(-360, 360)]
    public Vector2 yClamp;

    public float outOfCameraTreshold = 0.1f; //Si on augmente cette valeur, le joueur dépassera moins de la caméra
    public float inCameraTreshold = 0.2f; //Si on augmente cette valeur, la caméra se rezoomera quand les joueurs sont proche du centre de l'écran

    private Vector3 wantedPosition;
    private Vector3 wantedForward;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = parent.cameraObj.transform;
    }

    private void Update()
    {
        UpdateZoom();
        MainStateUpdate();
    }

    void UpdateZoom()
    {
        int targetsOutOfCameraCount = 0;
        int targetsInCameraTresholdCount = 0;

        foreach (var t in parent.targets)
        {
            Vector2 targetPosViewport = Camera.main.WorldToViewportPoint(t.transform.position);
            if (targetPosViewport.x < outOfCameraTreshold || targetPosViewport.y < outOfCameraTreshold || targetPosViewport.x > 1 - outOfCameraTreshold || targetPosViewport.y > 1 - outOfCameraTreshold)
            {
                targetsOutOfCameraCount++;
            }
            else if (targetPosViewport.x < 1 - inCameraTreshold && targetPosViewport.x > inCameraTreshold && targetPosViewport.y < 1 - inCameraTreshold && targetPosViewport.y > inCameraTreshold)
            {
                targetsInCameraTresholdCount++;
            }
        }
        if (targetsOutOfCameraCount > 0)
        {
            if (Vector3.Distance(cameraTransform.position, this.transform.position) < maxCamDistance)
            {
                cameraTransform.position -= cameraTransform.forward * 0.1f * cameraSpeed;
            }
        }
        else if (targetsInCameraTresholdCount >= parent.targets.Length)
        {
            if (Vector3.Distance(cameraTransform.position, this.transform.position) > minCamDistance)
            {
                cameraTransform.position += cameraTransform.forward * 0.1f * cameraSpeed;
            }
        }
    }

    void MainStateUpdate()
    {
        Transform furthestPlayer = GetFurthestPlayer();

        Vector3 dir = (furthestPlayer.position - transform.position).normalized;
        Vector3 center = new Vector3();
        foreach (var t in parent.targets)
        {
            center += t.transform.position * t.importance;
        }
        center = center / parent.targets.Length;
        wantedPosition = center;
        wantedForward = Vector3.Lerp(transform.forward, dir, 0.03f);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, 0.2f);
        transform.forward = wantedForward;

        //Clamp camera
        float eulerAngleY = transform.eulerAngles.y;
        if (eulerAngleY > 180)
        {
            eulerAngleY = eulerAngleY - 360;
        }
        eulerAngleY = Mathf.Clamp(eulerAngleY, yClamp.x, yClamp.y);

        float eulerAngleX = transform.eulerAngles.x;
        if (eulerAngleX > 180)
        {
            eulerAngleX = eulerAngleX - 360;
        }
        eulerAngleX = Mathf.Clamp(eulerAngleX, xClamp.x, xClamp.y);

        transform.eulerAngles = new Vector3(eulerAngleX, eulerAngleY, transform.eulerAngles.z);
    }

    public Transform GetFurthestPlayer()
    {
        Vector3 relativePositionA = parent.cameraRef.InverseTransformDirection(parent.player1.transform.position - parent.cameraRef.position);
        Vector3 relativePositionB = parent.cameraRef.InverseTransformDirection(parent.player2.transform.position - parent.cameraRef.position);
        if (relativePositionA.z > relativePositionB.z)
        {
            return parent.player1;
        }
        else
        {
            return parent.player2;
        }
    }
}
