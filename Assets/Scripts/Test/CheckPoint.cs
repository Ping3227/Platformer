using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using Platformer.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("CheckPoint");
            GameController.Instance.SetCheckPoint( transform.position);
            Simulation.Schedule<ResetAll>();
            GamesceneUIController.instance.ResetItem();
        }
    }
}
