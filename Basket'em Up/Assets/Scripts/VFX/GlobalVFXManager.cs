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
    public float noChromaticAberrationValue;
    public float noCameraFOV;

    [Space(10)]
    [Header("Values when FULL momentum")]
    public float fullBloomValue;
    public float fullVignetteIntensityValue;
    public float fullChromaticAberrationValue;
    public float fullCameraFOV;

    [Space(10)]
    [Header("Misc Values")]
    [Range(0, 1)] public float lerpSpeed;

    Bloom bloomLayer;
    AmbientOcclusion ambientOcclusionLayer;
    ColorGrading colorGradingLayer;
    Vignette vignetteLayer;
    ChromaticAberration chromaticAberration;

    float wantedBloomValue;
    float wantedVignetteValue;
    float wantedChromaticAberrationValue;

    #endregion

    void Start()
    {
        myPP.profile.TryGetSettings(out bloomLayer);
        myPP.profile.TryGetSettings(out ambientOcclusionLayer);
        myPP.profile.TryGetSettings(out colorGradingLayer);
        myPP.profile.TryGetSettings(out vignetteLayer);
        myPP.profile.TryGetSettings(out chromaticAberration);
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
    }

    void UpdatePPValues()
    {
        bloomLayer.intensity.value = Mathf.Lerp(bloomLayer.intensity.value, wantedBloomValue, lerpSpeed);
        vignetteLayer.intensity.value = Mathf.Lerp(vignetteLayer.intensity.value, wantedVignetteValue, lerpSpeed);
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, wantedChromaticAberrationValue, lerpSpeed);
    }
}
