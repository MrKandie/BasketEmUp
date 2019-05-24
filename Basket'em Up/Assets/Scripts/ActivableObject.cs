using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour
{

    [Header("Settings")]
    public float timeOfActivation;

    [Header("Automatic Variables")]
    public bool activated;
    public Collider activationCollider;
    public ActivableObjectCheck activationChecker;
    public ParticleSystem particles;


    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        activationCollider = this.GetComponent<Collider>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Activate()
    {
        StartCoroutine(ActivationTime());
        activationChecker.CheckIfAllObjectsActivated();
    }

    public IEnumerator ActivationTime()
    {
        activated = true;
        particles.Play();
        yield return new WaitForSeconds(timeOfActivation);
        activated = false;
        particles.Stop();
    }
}
