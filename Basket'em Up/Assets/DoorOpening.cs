using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public Animator doorAnimator;

    private void Awake()
    {
        doorAnimator.enabled = false;
    }
    public void OpenDoor()
    {
        doorAnimator.enabled = true;
    }

    public void EndAnimation()
    {
        doorAnimator.enabled = false;
    }
}
