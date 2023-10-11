using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestController : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    // 检查碰撞对象的标签是否为"Enemy"
    //    if (other.gameObject.CompareTag("Attack_Area"))
    //    {
    //        // 处理碰撞事件，例如销毁敌人或触发其他行为
    //        anim.SetBool("BeingAttacked", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("BeingAttacked", false);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TestTest");
        // 检查碰撞对象的标签是否为"Attack_Area"
        if (other.gameObject.CompareTag("Attack_Area"))
        {
            // 处理碰撞事件，例如播放被攻击动画
            anim.SetBool("BeingAttacked", true);

            // 启动协程来自动取消"BeingAttacked"状态并销毁物体
            StartCoroutine(BeingAttackedTimer());
        }
    }

    private IEnumerator BeingAttackedTimer()
    {
        // 等待一段时间
        yield return new WaitForSeconds(2.0f); // 2秒，你可以根据需要调整时间

        // 取消"BeingAttacked"状态
        anim.SetBool("BeingAttacked", false);

        // 销毁游戏对象
        Destroy(gameObject);
    }
}
