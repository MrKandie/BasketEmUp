﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentumManager : MonoBehaviour
{
    [Range(0f,1f)]
    public float momentum; //Float correspondant à la puissance du momentum

    [Header("General settings")]
    public float momentumGainedPerPass = 0.2f;
    public AnimationCurve momentumLosseCurve;
    public float momentumLosseCoef = 1f;

    [Header("References")]
    public Slider slider;


    //Settings
    private float lastIncrementationTime;

    private void Awake()
    {
        InvokeRepeating("UpdateMomentumValue", 1, 1);
    }

    private void Update()
    {
        UpdateSlider();
    }

    #region functions related to momentum evolution
    private void UpdateSlider()
    {
        slider.value = Mathf.Lerp(slider.value, momentum, Time.deltaTime);
    }

    public void IncrementMomentum(float amount)
    {
        momentum += amount;
        lastIncrementationTime = Time.time;
        momentum = Mathf.Clamp(momentum, 0, 1);
        slider.value = momentum;
    }

    public void DecrementMomentum(float amount)
    {
        momentum -= amount;
        momentum = Mathf.Clamp(momentum, 0, 1);
        slider.value = momentum;
    }

    public void UpdateMomentumValue()
    {
        float timeSinceLastUpdate = Time.time - lastIncrementationTime;
        momentum -= momentumLosseCurve.Evaluate(timeSinceLastUpdate) * momentumLosseCoef;
    }
    #endregion
}
