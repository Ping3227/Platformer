using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;
using UnityEngine.Events;

public class EnemyHitBox : MonoBehaviour
{
    Player player;
    [SerializeField] UnityEvent OnHit;
    [SerializeField] float damage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.player;
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.Hurt(damage);
            OnHit?.Invoke();
        }
        
    }

}
