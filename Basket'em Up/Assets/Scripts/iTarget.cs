using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iTarget
{
    Transform targetedTransform { get; set; }
    void OnBallReceived(Ball ball);
    void OnTargetedBySomeone(Transform target);
}

