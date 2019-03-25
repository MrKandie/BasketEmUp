using System.Collections;
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

    #region functions passing values
    public float GetPassSpeed()
    {
        return Mathf.Lerp(minMaxPassSpeed.x, minMaxPassSpeed.y, momentum);
    }

    public float GetPassHeight()
    {
        return Mathf.Lerp(minMaxPassHeight.x, minMaxPassHeight.y, 1 - momentum);
    }

    public int GetMomentumDamages()
    {
        return Mathf.RoundToInt(Mathf.Lerp(minMaxPassDamage.x, minMaxPassDamage.y, momentum));
    }
    #endregion

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
        Debug.Log(momentumLosseCurve.Evaluate(timeSinceLastUpdate));
    }
    #endregion
}
