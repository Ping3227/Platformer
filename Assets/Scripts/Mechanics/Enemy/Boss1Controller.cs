using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 检测按下D键
        if (Input.GetKeyDown(KeyCode.D))
        {
            // 设置动画参数，触发移动动画
            animator.SetBool("IsMoving", true);
        }

        // 检测松开D键
        if (Input.GetKeyUp(KeyCode.D))
        {
            // 设置动画参数，停止移动动画
            animator.SetBool("IsMoving", false);
        }
    }

    private void FixedUpdate()
    {
        // 在FixedUpdate中处理物理移动
        if (Input.GetKey(KeyCode.D))
        {
            // 移动玩家
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            // 停止玩家移动
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
}
