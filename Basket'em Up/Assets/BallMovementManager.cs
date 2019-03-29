﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementManager : MonoBehaviour
{
    [Header("Settings")]
    public float bounceRadius;
    public AnimationCurve bounceAngleCurve;
    public AnimationCurve bounceMovementCurve;

    public AnimationCurve passMovementCurve;
    public AnimationCurve passAngleCurve;

    [Header("Vitesse de la passe en metres par seconde")]
    [MinMaxSlider(0, 150)]
    public Vector2 minMaxPassSpeed; //En mètres par seconde (Sans prendre en compte la curve)

    [Header("Vitesse du rebond en metres par seconde")]
    [MinMaxSlider(0, 150)]
    public Vector2 minMaxBounceSpeed; //En mètres par seconde (Sans prendre en compte la curve)

    [Header("Degats de la balle (100pv = full hp)")]
    [MinMaxSlider(0, 100)]
    public Vector2 minMaxPassDamage;

    [Header("Hauteur de la passe en metres")]
    [MinMaxSlider(0, 10)]
    public Vector2 minMaxPassHeight; //En mètres

    [Header("Hauteur du rebond de la balle en metres")]
    [MinMaxSlider(0, 10)]
    public Vector2 minMaxBounceHeight; //En mètres

    [Header("Coefficient de vitesse pour le rebond de la balle sur le sol")]
    public float bounceOnGroundSpeedCoef;

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

    public float GetBounceSpeed()
    {
        return Mathf.Lerp(minMaxBounceSpeed.x, minMaxBounceSpeed.y, GameManager.i.momentumManager.momentum);
    }

    public float GetBounceHeight()
    {
        return Mathf.Lerp(minMaxBounceHeight.x, minMaxBounceHeight.y, 1-GameManager.i.momentumManager.momentum);
    }
}
