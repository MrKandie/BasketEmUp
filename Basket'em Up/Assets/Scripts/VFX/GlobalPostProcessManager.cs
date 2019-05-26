using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalPostProcessManager : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    public PostProcessVolume myPP;

    [Space(10)]
    [Header("Values when NO momentum")]
    public float noBloomValue;
    public float noVignetteIntensityValue;
    public float noChromaticAberrationValue;
    public float noCameraFOV;
    public float noGrainValue;
    public float noSaturationValue;
    public float noContrastValue;

    [Space(10)]
    [Header("Values when FULL momentum")]
    public float fullBloomValue;
    public float fullVignetteIntensityValue;
    public float fullChromaticAberrationValue;
    public float fullCameraFOV;
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
    ChromaticAberration chromaticAberrationLayer;
    Grain grainLayer;

    float wantedBloomValue;
    float wantedVignetteValue;
    float wantedChromaticAberrationValue;
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
        myPP.profile.TryGetSettings(out chromaticAberrationLayer);
        myPP.profile.TryGetSettings(out grainLayer);
    }
    
    void Update()
    {
        UpdateWantedValues();
        UpdatePPValues();
    }

    void UpdateWantedValues()
    {
        wantedBloomValue = Mathf.Lerp(noBloomValue, fullBloomValue, GameManager.i.momentumManager.GetSmoothMomentum());
        wantedVignetteValue = Mathf.Lerp(noVignetteIntensityValue, fullVignetteIntensityValue, GameManager.i.momentumManager.GetSmoothMomentum());
        wantedChromaticAberrationValue = Mathf.Lerp(noChromaticAberrationValue, fullChromaticAberrationValue, GameManager.i.momentumManager.GetSmoothMomentum());
        Camera.main.fieldOfView = Mathf.Lerp(noCameraFOV, fullCameraFOV, GameManager.i.momentumManager.GetSmoothMomentum());
        wantedGrainValue = Mathf.Lerp(noGrainValue, fullGrainValue, GameManager.i.momentumManager.GetSmoothMomentum());
        wantedSaturationValue = Mathf.Lerp(noSaturationValue, fullSaturationValue, GameManager.i.momentumManager.GetSmoothMomentum());
        wantedContrastValue = Mathf.Lerp(noContrastValue, fullContrastValue, GameManager.i.momentumManager.GetSmoothMomentum());
    }

    void UpdatePPValues()
    {
        bloomLayer.intensity.value = Mathf.Lerp(bloomLayer.intensity.value, wantedBloomValue, lerpSpeed);
        vignetteLayer.intensity.value = Mathf.Lerp(vignetteLayer.intensity.value, wantedVignetteValue, lerpSpeed);
        chromaticAberrationLayer.intensity.value = Mathf.Lerp(chromaticAberrationLayer.intensity.value, wantedChromaticAberrationValue, lerpSpeed);
        grainLayer.intensity.value = Mathf.Lerp(grainLayer.intensity.value, wantedGrainValue, lerpSpeed);
        colorGradingLayer.saturation.value = Mathf.Lerp(colorGradingLayer.saturation.value, wantedSaturationValue, lerpSpeed);
        colorGradingLayer.contrast.value = Mathf.Lerp(colorGradingLayer.contrast.value, wantedContrastValue, lerpSpeed);
    }
}
