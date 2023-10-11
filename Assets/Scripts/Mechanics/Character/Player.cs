using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField ] private float jumpTime = 0.5f;

    [Header("Ground check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    private bool IsFacingRight = true;

    private bool IsJumping = false;
    private bool IsFalling;
    private float jumpTimeCounter;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Health health;
    private Stamina stamina;
    private float moveInput;
    private RaycastHit2D groundHit;
    
    
    private void Start()
    {
        coll= GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
    }
    private void Update()
    {
        
        Move();
        Jump();
    }
    #region movement
    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;
        if (moveInput > 0 || moveInput < 0)
        {
            TurnCheck();
        }
        rb.velocity = new Vector2(moveInput * 5, rb.velocity.y);
    }
    
    private void Jump() {
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame()&&IsGrounded()) { 
            Debug.Log($"Jumping {IsGrounded()}");
            IsJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
            
        }
        if(UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            if (jumpTimeCounter > 0 && IsJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                IsJumping = false;
            }
            
        }
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            IsJumping = false;
        }
        
    }
    #endregion
    #region Turn check
    private void TurnCheck()
    {
        if (UserInput.instance.moveInput.x > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (UserInput.instance.moveInput.x < 0 && IsFacingRight)
        {
            Turn();
        }
    }
    private void Turn()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = false;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = true;
        }
    }
    #endregion
    #region Ground check
    private bool IsGrounded() { 
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight,whatIsGround);
        if (groundHit.collider != null) {
            
            return true;
            
        }
        else
        {
            
            return false;
            
        }   

    }
    #endregion
    #region Debug function
    private void OnDrawGizmos()
    {
        Color rayColor;
        if (IsGrounded())
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(coll.bounds.center + new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight), Vector2.right * (coll.bounds.extents.x * 2), rayColor);
    }
    #endregion 
}



