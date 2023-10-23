using Platformer.Core;
using Platformer.Gameplay;
using Platformer.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) {
            Simulation.Schedule<PlayerDeath>();
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}
