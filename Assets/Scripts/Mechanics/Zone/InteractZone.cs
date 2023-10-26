using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class InteractZone : MonoBehaviour
{
    public enum TriggerType
    {
        Once, Everytime,
    }

    [Tooltip("This is the gameobject which will trigger the director to play.  For example, the player.")]
    public GameObject triggeringGameObject;
    public PlayableDirector director;
    public TriggerType triggerType;
    public UnityEvent OnDirectorPlay;
    public UnityEvent OnDirectorFinish;
    protected bool m_AlreadyTriggered;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != triggeringGameObject)
            return;

        if (triggerType == TriggerType.Once && m_AlreadyTriggered)
            return;
        Debug.Log("Triggering director");
        director.Play();
        m_AlreadyTriggered = true;
        OnDirectorPlay.Invoke();
        Invoke("FinishInvoke", (float)director.duration);
    }

    void FinishInvoke()
    {
        OnDirectorFinish.Invoke();
    }
}
