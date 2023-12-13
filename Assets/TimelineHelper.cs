using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHelper : MonoBehaviour
{
    public void play(string sound) {
        AudioManager.instance.Play(sound);
    }
    public void stop(string sound)
    {
        AudioManager.instance.Stop(sound);
    }
}
