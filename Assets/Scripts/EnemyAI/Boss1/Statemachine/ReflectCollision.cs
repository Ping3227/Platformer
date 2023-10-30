using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class ReflectCollision : MonoBehaviour
{
    EnemyHealth health;
    private void Start()
    {
       health = GetComponentInParent<EnemyHealth>();
    }
    private void OnDisable()
    {
        health.IsReflecting= false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            health.IsReflecting = true;
            Debug.Log($"Enemy immune {Time.time}");
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            health.IsReflecting = false;
        }

    }
}
