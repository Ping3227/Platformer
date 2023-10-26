using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class AttackHitBox : MonoBehaviour
{
    Player player;
    Animator anim;
    [SerializeField] float damage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.player;
        anim = player.GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Hurt");
            player.Hurt(damage);    
        }
        
    }

}
