using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;

    public List<AudioClip> whoosh;
    public List<AudioClip> ballCatch;
    public List<AudioClip> enemyHit;
    public AudioClip dunk;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip, bool randomPitch = false)
    {
        source.pitch = 1;
        if (randomPitch)
        {
            source.pitch = Random.Range(0.8f, 1.2f);
        }
        source.PlayOneShot(clip);
    }
    public void PlayRandomSound(List<AudioClip> clips, bool randomPitch = false)
    {
        int index = Random.Range(0, clips.Count-1);
        source.pitch = 1;
        if (randomPitch)
        {
            source.pitch = Random.Range(0.8f, 1.2f);
        }
        source.PlayOneShot(clips[index]);
    }
}
