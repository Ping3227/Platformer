using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    //Debug.Log("Test");
    //    if (other.CompareTag("Player"))
    //    {
    //        // 处理碰撞事件，例如玩家进入了AttackArea
    //        Debug.Log("Player进入了AttackArea！");
    //    }
    //    else if (other.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Enemy进入了AttackArea！");
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Test");
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // 处理碰撞事件，例如玩家与AttackArea发生物理碰撞
    //        Debug.Log("Player与AttackArea发生了碰撞！");
    //    }
    //    else if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Enemy与AttackArea发生了碰撞！");
    //    }
    //}

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Test");
    //        // 处理碰撞事件，例如玩家与敌人碰撞
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player进入了AttackArea！");
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy进入了AttackArea！");
        }
    }

}
