using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    public float momentum; //Float correspondant à la puissance du momentum

    public float passSpeed; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public AnimationCurve passMovementCurve; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public AnimationCurve passAngleCurve; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public float passMaxHeight;

    //TODO
    public float GetPassSpeed(float momentum)
    {
        return passSpeed;
    }

    //TODO
    public AnimationCurve GetPassMovementCurve(float momentum)
    {
        return passMovementCurve;
    }

    //TODO
    public AnimationCurve GetPassAngleCurve(float momentum)
    {
        return passAngleCurve;
    }

    //TODO
    public float GetPassDuration(float momentum)
    {
        return 0.5f;
    }
}
