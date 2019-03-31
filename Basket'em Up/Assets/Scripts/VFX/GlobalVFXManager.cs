using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalVFXManager : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    public PostProcessVolume myPP;

    [Space(10)]
    [Header("Values when NO momentum")]
    public float noBloomValue;
    public float noVignetteIntensityValue;
    public float noGrainValue;
    public float noSaturationValue;
    public float noContrastValue;

    [Space(10)]
    [Header("Values when FULL momentum")]
    public float fullBloomValue;
    public float fullVignetteIntensityValue;
    public float fullGrainValue;
    public float fullSaturationValue;
    public float fullContrastValue;

    [Space(10)]
    [Header("Misc Values")]
    [Range(0, 1)] public float lerpSpeed;

    Bloom bloomLayer;
    AmbientOcclusion ambientOcclusionLayer;
    ColorGrading colorGradingLayer;
    Vignette vignetteLayer;
    Grain grainLayer;

    float wantedBloomValue;
    float wantedVignetteValue;
    float wantedGrainValue;
    float wantedSaturationValue;
    float wantedContrastValue;

    #endregion

    void Start()
    {
        myPP.profile.TryGetSettings(out bloomLayer);
        myPP.profile.TryGetSettings(out ambientOcclusionLayer);
        myPP.profile.TryGetSettings(out colorGradingLayer);
        myPP.profile.TryGetSettings(out vignetteLayer);
        myPP.profile.TryGetSettings(out grainLayer);
    }
    
    void Update()
    {
        UpdateWantedValues();
        UpdatePPValues();
    }

    void UpdateWantedValues()
    {
        wantedBloomValue = Mathf.Lerp(noBloomValue, fullBloomValue, GameManager.i.momentumManager.momentum);
        wantedVignetteValue = Mathf.Lerp(noVignetteIntensityValue, fullVignetteIntensityValue, GameManager.i.momentumManager.momentum);
        wantedGrainValue = Mathf.Lerp(noGrainValue, fullGrainValue, GameManager.i.momentumManager.momentum);
        wantedSaturationValue = Mathf.Lerp(noSaturationValue, fullSaturationValue, GameManager.i.momentumManager.momentum);
        wantedContrastValue = Mathf.Lerp(noContrastValue, fullContrastValue, GameManager.i.momentumManager.momentum);
    }

    void UpdatePPValues()
    {
        bloomLayer.intensity.value = Mathf.Lerp(bloomLayer.intensity.value, wantedBloomValue, lerpSpeed);
        vignetteLayer.intensity.value = Mathf.Lerp(vignetteLayer.intensity.value, wantedVignetteValue, lerpSpeed);
        grainLayer.intensity.value = Mathf.Lerp(grainLayer.intensity.value, wantedGrainValue, lerpSpeed);
        colorGradingLayer.saturation.value = Mathf.Lerp(colorGradingLayer.saturation.value, wantedSaturationValue, lerpSpeed);
        colorGradingLayer.contrast.value = Mathf.Lerp(colorGradingLayer.contrast.value, wantedContrastValue, lerpSpeed);
    }
}
