using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    GameObject child1;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        // 检测是否按下K键
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 设置攻击参数为true，触发攻击状态
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            // 当不攻击时，将攻击参数设置为false
            animator.SetBool("IsAttacking", false);
        }
    }
        
    //private void OnTriggerEnter2D(Collider2D other)
    //    {
    //        // 检查碰撞对象的标签是否为"Enemy"
    //        if (other.gameObject.CompareTag("Enemy"))
    //        {
    //            // 处理碰撞事件，例如销毁敌人或触发其他行为
    //            Debug.Log("子对象碰到了Enemy！");
    //        }
    //    }
}