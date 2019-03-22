using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    public float momentum; //Float correspondant à la puissance du momentum

    public float passSpeed; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public AnimationCurve passMovementCurve; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public AnimationCurve passAngleCurve; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public float passMaxHeight; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public int momentumDamages; //Sera généré automatiquement en fonction de la valeur de momentum plus tard
    public float passDuration; //Sera généré automatiquement en fonction de la valeur de momentum plus tard

    //TODO
    public float GetPassSpeed()
    {
        return passSpeed;
    }

    //TODO
    public AnimationCurve GetPassMovementCurve()
    {
        return passMovementCurve;
    }

    //TODO
    public AnimationCurve GetPassAngleCurve()
    {
        return passAngleCurve;
    }

    //TODO
    public float GetPassDuration()
    {
        return passDuration;
    }

    //TODO
    public int GetMomentumDamages()
    {
        return momentumDamages;
    }
}
