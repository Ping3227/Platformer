using Platformer.Core;
using Platformer.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pounded : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) { 
            collider.GetComponent<Player>().Pounded();
            Simulation.Schedule<PlayerDeath>();
        }
    }
}
