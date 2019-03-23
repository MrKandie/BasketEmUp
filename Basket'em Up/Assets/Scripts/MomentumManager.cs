using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    [Range(0f,1f)]
    public float momentum; //Float correspondant à la puissance du momentum

    [Header("Vitesse de la passe en metres par seconde")]
    [MinMaxSlider(0, 50)]
    public Vector2 minMaxPassSpeed; //En mètres par seconde

    [Header("Degats de la balle (100pv = full hp)")]
    [MinMaxSlider(0, 100)]
    public Vector2 minMaxPassDamage;

    [Header("Hauteur de la passe en metres")]
    [MinMaxSlider(0, 10)]
    public Vector2 minMaxPassHeight; //En mètres

    [Header("Courbes de mouvement de la balle")]
    public AnimationCurve passMovementCurve;
    public AnimationCurve passAngleCurve;

    public float GetPassSpeed()
    {
        return Mathf.Lerp(minMaxPassSpeed.x, minMaxPassSpeed.y, momentum);
    }

    public float GetPassHeight()
    {
        return Mathf.Lerp(minMaxPassHeight.x, minMaxPassHeight.y, 1-momentum);
    }

    public int GetMomentumDamages()
    {
        return Mathf.RoundToInt(Mathf.Lerp(minMaxPassDamage.x, minMaxPassDamage.y, momentum));
    }
}
