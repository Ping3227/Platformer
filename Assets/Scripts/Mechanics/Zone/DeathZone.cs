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
            collider.GetComponent<Player>().Dead();
            GamesceneUIController.instance.Death();
            Simulation.Schedule<PlayerDeath>(1.0f);
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}
