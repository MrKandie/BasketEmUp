using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MomentumManager : MonoBehaviour
{

    [Range(0f,1f)]
    public float momentum; //Float correspondant à la puissance du momentum

    [Header("General settings")]
    public float momentumGainedPerEnemyHit = 0.2f;
    public float momentumLosseWhenBallTouchGround = 0.5f;
    public AnimationCurve momentumLosseCurve;
    public float momentumLosseCoef = 1f;
    public float healFactor; // en valeur/secondes

    [Header("References")]
    public Slider slider;


    //Settings
    private float lastIncrementationTime;

    public float GetSmoothMomentum()
    {
        return slider.value;
    }

    private void Awake()
    {
        InvokeRepeating("UpdateMomentumValue", 1, 1);
    }

    private void Update()
    {
        UpdateSlider();
        UpdateBallPointLight();
    }

    public void UseMomentumToHeal()
    {
        momentum -= healFactor / 60; //parce que 60fps
    }


    #region functions related to momentum evolution
    private void UpdateSlider()
    {
        slider.value = Mathf.Lerp(slider.value, momentum, Time.deltaTime);
    }

    private void UpdateBallPointLight()
    {
        GameManager.i.levelManager.activeBall.SetBallLightIntensity(GameManager.i.ballManager.GetBallLightIntensity());
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
