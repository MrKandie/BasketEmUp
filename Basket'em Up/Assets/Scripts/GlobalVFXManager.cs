using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalVFXManager : MonoBehaviour
{
    PostProcessVolume myPP;
    Bloom bloomLayer;
    AmbientOcclusion ambientOcclusionLayer;
    ColorGrading colorGradingLayer;

    // Start is called before the first frame update
    void Start()
    {
        myPP = GetComponent<PostProcessVolume>();
        myPP.profile.TryGetSettings(out bloomLayer);
        myPP.profile.TryGetSettings(out ambientOcclusionLayer);
        myPP.profile.TryGetSettings(out colorGradingLayer);
    }

    // Update is called once per frame
    void Update()
    {
        bloomLayer.intensity.value = Mathf.Sin(Time.time) * 50 + 50;
    }
}
