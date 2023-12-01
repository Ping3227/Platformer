using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }

    private void Start()
    {
        Play("SimpleBGM");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);

        if(s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        
        s.source.Play(); 
       
    }
    public void Stop(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        s.source.Stop();
    }


    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);
        s.source.Pause();
    }



    public void PlayDelayed(string name, float second)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName == name);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }

        s.source.PlayDelayed(second);

    }
}
