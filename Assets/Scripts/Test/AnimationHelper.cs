using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] AudioSource[] audioSources;
    public void PlayEffect() { 
        foreach (var particle in particles)
        {
            particle.Play();
        }
        foreach (var audioSource in audioSources)
        {
            audioSource.Play();
        }
    }
    public void EndAnimation()
    {
        gameObject.SetActive(false);
    }
}
