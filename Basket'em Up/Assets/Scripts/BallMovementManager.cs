using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementManager : MonoBehaviour
{
    [Header("Settings")]
    public AnimationCurve passMovementCurve;
    public AnimationCurve passAngleCurve;

    [Header("Vitesse de la passe en metres par seconde")]
    [MinMaxSlider(0, 150)]
    public Vector2 minMaxPassSpeed; //En mètres par seconde (Sans prendre en compte la curve)

    [Header("Degats de la balle (100pv = full hp)")]
    [MinMaxSlider(0, 100)]
    public Vector2 minMaxPassDamage;

    [Header("Hauteur de la passe en metres")]
    [MinMaxSlider(0, 10)]
    public Vector2 minMaxPassHeight; //En mètres


    public float GetPassSpeed()
    {
        return Mathf.Lerp(minMaxPassSpeed.x, minMaxPassSpeed.y, GameManager.i.momentumManager.momentum);
    }

    public float GetPassHeight()
    {
        return Mathf.Lerp(minMaxPassHeight.x, minMaxPassHeight.y, 1 - GameManager.i.momentumManager.momentum);
    }

    public int GetDamages()
    {
        return Mathf.RoundToInt(Mathf.Lerp(minMaxPassDamage.x, minMaxPassDamage.y, GameManager.i.momentumManager.momentum));
    }
}
