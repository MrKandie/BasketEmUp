using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NarrationTriggers : MonoBehaviour
{
    [Header("Trigger 0 Ref")]
    public CinemachineVirtualCamera bossZoomCamera;


    private void OnTriggerEnter(Collider other)
    {
        NarrationTriggerID otherID = other.GetComponent<NarrationTriggerID>();
        if (otherID != null)
        {
            switch (otherID.id)
            {
                case 0:
                    bossZoomCamera.m_Priority = 11;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }
}
