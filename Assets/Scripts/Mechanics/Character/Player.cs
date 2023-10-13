using Platformer.Mechanics;
using Platformer.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float jumpCost = 0.5f;
    [SerializeField] private float doubleJumpForce = 2f;
    [SerializeField] private float doubleJumpCost = 0.1f;
    [SerializeField] private float doubleJumpTime = 0.3f;
    [Tooltip("Bonus movement distance while jumping")]
    [SerializeField] private float ApexBonus = 0.8f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCost = 0.3f;
    [SerializeField] private float dashTime = 0.1f;

    [Header("Ground check")]
    [Tooltip("Check Ground Distance")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    private bool IsFacingRight = true;
    private bool IsDashing = false;
    private bool IsJumping = false;
    private bool IsDoubleJumping =false;
    private bool FinishDoubleJump = false;
    private bool IsAttacking = false;
    private bool IsFalling;
    private float jumpTimeCounter;
    private float DashTimeCounter;
    private float doubleJumpTimeCounter;
    private float JumpApex =0f;
   
    

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
        Dash();
        if (!IsDashing) {
            Jump();
            Move();
            Attack();
        }
        UpdateAnimation();
        
    }

    private void UpdateAnimation()
    {
        anim.SetBool("IsJump", IsJumping);
        anim.SetBool("IsDash", IsDashing);
    }

    private void Attack()
    {
        if(UserInput.instance.controls.Attack.Attack.WasPressedThisFrame())
        {
            anim.SetTrigger("Attack");
        }
    }


    #region movement
    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;
        if (moveInput > 0 || moveInput < 0)
        {
            TurnCheck();
        }
        
        rb.velocity = new Vector2(moveInput * moveSpeed* (1+JumpApex) , rb.velocity.y);
        
    }
    
    private void Jump() {
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame()
                && IsGrounded() && stamina.ConsumeStamina(jumpCost*Time.deltaTime)) { 
            
            IsJumping = true;
            FinishDoubleJump = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if(UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            
            if (jumpTimeCounter > 0 && IsJumping&& stamina.ConsumeStamina(jumpCost * Time.deltaTime))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
                if(jumpTimeCounter<0) jumpTimeCounter = 0;
                JumpApex= (jumpTime-jumpTimeCounter)*ApexBonus/jumpTime;
                
            }
            else
            {
                JumpApex = 0;
                IsJumping = false;
            }
            
        }
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            IsJumping = false;
            JumpApex = 0;
        }
        /// Below is double jump logic 
        if (!IsJumping && !IsGrounded() && !FinishDoubleJump
                && UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame()
                && stamina.ConsumeStamina(doubleJumpCost) ) {

            doubleJumpTimeCounter = doubleJumpTime;
            IsDoubleJumping = true;
            FinishDoubleJump = false;
        }
        if (IsDoubleJumping) {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            doubleJumpTimeCounter -= Time.deltaTime;
            
            if (doubleJumpTimeCounter <= 0) {
                FinishDoubleJump = true;
                IsDoubleJumping = false;
            }
        }
       
        
    }
    private void Dash()
    {
        if(DashTimeCounter ==0&& UserInput.instance.controls.Dash.Dash.WasPressedThisFrame()&& stamina.ConsumeStamina(dashCost))
        {
            IsDashing = true;
            DashTimeCounter = dashTime;
        }
        DashTimeCounter -= Time.deltaTime;
        if (DashTimeCounter <= 0) {
            DashTimeCounter = 0;
            IsDashing = false;
        }
        if (IsDashing) {
            if (IsFacingRight) {
                rb.velocity = new Vector2(dashForce, rb.velocity.y);
            }
            else {
                rb.velocity = new Vector2(-dashForce, rb.velocity.y);
            }
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
    private void DrawDebug()
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



