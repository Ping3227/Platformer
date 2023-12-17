using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Disappear : InteractActor
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] Collider2D[] colliders;
    public override void Action()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
    public void Operating(bool IsOpen)
    {
        if(IsOpen)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }
        }
        else
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
        }
        
    }
    
}
