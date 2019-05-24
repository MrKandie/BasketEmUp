using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour
{

    [Header("Settings")]
    public float timeOfActivation;

    [Header("Automatic Variables")]
    public bool activated;
    public bool finalActivated;
    public Collider activationCollider;
    public ActivableObjectCheck activationChecker;
    public ParticleSystem FX;


    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        activationCollider = this.GetComponent<Collider>();
        FX = GetComponentInChildren<ParticleSystem>();
    }

    public void Activate() 
    {
        if (!finalActivated) //if the all the objects are not activated together
        {
            StartCoroutine(ActivationTime());
            activationChecker.CheckIfAllObjectsActivated();
        }
        else //if all the objects are activated and you try to activate again (for DEBUG)
        {
            finalActivated = false;
            activated = false;
            FX.Stop();
        }
    }

    public IEnumerator ActivationTime()
    {
        activated = true;
        FX.Play();
        yield return new WaitForSeconds(timeOfActivation);
        if (!finalActivated)
        {
            activated = false;
            FX.Stop();
        }
    }

    public void FinalActivate()
    {
        finalActivated = true;
        activated = true;
        FX.Play();
    }
}
