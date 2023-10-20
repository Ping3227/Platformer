using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class AttackAreaCollision : MonoBehaviour
{
    Player player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.player;
        anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Hurt");
            Debug.Log("Player进入了AttackArea！");
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy进入了AttackArea！");
        }
    }

}
