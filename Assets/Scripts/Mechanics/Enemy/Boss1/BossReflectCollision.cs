using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class BossAttackCollision : MonoBehaviour
{
    GameObject player;
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
        if (other.CompareTag("Attack_Area"))
        {
            anim.SetTrigger("IsReflected");
            Debug.Log("Player进入了反擊領域！");
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy进入了AttackArea！");
        }
    }
}
