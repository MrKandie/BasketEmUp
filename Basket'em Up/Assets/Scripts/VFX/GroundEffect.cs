using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffect : MonoBehaviour
{
    public float endSliderValue;
    public AnimationCurve sliderCurve;
    public Renderer myRend;
    public float timeForEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MovingSlider());
    }

    IEnumerator MovingSlider()
    {
        for (float i = 0; i < 1; i+=Time.deltaTime / timeForEffect)
        {
            myRend.material.SetFloat("_Progression", sliderCurve.Evaluate(i) * endSliderValue);
            print(sliderCurve.Evaluate(i) * endSliderValue);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
