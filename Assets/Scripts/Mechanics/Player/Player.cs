using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Runtime.CompilerServices;
using UnityEngine;
[RequireComponent(typeof(Health), typeof(Stamina))]
public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Attack")]
    [SerializeField] private float AttackTime = 0.2f;

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
    [SerializeField] private float dashInvincibleTime = 0.05f;

    [Header("Ground check")]
    [Tooltip("Check Ground Distance")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Hurt")]
    [SerializeField] private ParticleSystem hurtEffect;
    [SerializeField] private float hurtInvincibleTime;
    [SerializeField] private float ShakeAmpitude;
    [SerializeField] private float ShakeFrequency;
    [SerializeField] private float ShakeDuration;
    [Tooltip("Slow Time while hurt")]
    [SerializeField][Range(0, 1)] private float TimeSlower;
    [Tooltip("Recover Time from TimeSlower")]
    [SerializeField] private float TimeRecoverRate;

    [Header("Status")]
    private bool IsFacingRight = true;
    private bool IsDashing = false;
    private bool IsJumping = false;
    private bool IsDoubleJumping = false;
    private bool IsInvincible = false;
    private bool FinishDoubleJump = false;
    private bool IsAttacking = false;
    private bool IsMoveable = true;
    private bool IsFalling;

    [Header("Counter")]
    private float jumpTimeCounter;
    private float doubleJumpTimeCounter;
    private float DashTimeCounter;
    private float ImmobileTimeCounter;
    private float InvincibleCounter;

    private float JumpApex = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Health health;
    private Stamina stamina;
    private float moveInput;
    private RaycastHit2D groundHit;


    private void Start()
    {
        if(hurtEffect) hurtEffect.Pause();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
    }
    private void Update()
    {
        Moveable();
        Invincible();
        if (IsMoveable) {
            Dash();
            if (!IsDashing)
            {
                Jump();
                Move();
                Attack();
            }
            UpdateAnimation();
        }


    }

    private void UpdateAnimation()
    {
        anim.SetBool("IsJump", IsJumping);
        anim.SetBool("IsDash", IsDashing);
    }

    private void Attack()
    {
        if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame())
        {
            anim.SetTrigger("Attack");
            ImmobileTimeCounter = AttackTime;
            IsMoveable = false;
            rb.velocity = new Vector2(0,rb.velocity.y);
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

        rb.velocity = new Vector2(moveInput * moveSpeed * (1 + JumpApex), rb.velocity.y);

    }

    private void Jump() {
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame()
                && IsGrounded() && stamina.ConsumeStamina(jumpCost * Time.deltaTime)) {

            IsJumping = true;
            FinishDoubleJump = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
        if (UserInput.instance.controls.Jumping.Jump.IsPressed())
        {

            if (jumpTimeCounter > 0 && IsJumping && stamina.ConsumeStamina(jumpCost * Time.deltaTime))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
                if (jumpTimeCounter < 0) jumpTimeCounter = 0;
                JumpApex = (jumpTime - jumpTimeCounter) * ApexBonus / jumpTime;

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
                && stamina.ConsumeStamina(doubleJumpCost)) {

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
        if (DashTimeCounter == 0 && UserInput.instance.controls.Dash.Dash.WasPressedThisFrame() && stamina.ConsumeStamina(dashCost))
        {
            
            IsDashing = true;
            

            DashTimeCounter = dashTime;
            InvincibleCounter = dashInvincibleTime;
            IsInvincible = true;
            if (IsJumping) { IsJumping = false; }
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
    private void Moveable() {
        if (ImmobileTimeCounter > 0)
        {
            ImmobileTimeCounter -= Time.deltaTime;
        }
        else {
            ImmobileTimeCounter = 0;
            IsMoveable = true;
        }

    }
    private void Invincible() {
        InvincibleCounter -= Time.deltaTime;
        if (InvincibleCounter <= 0)
        {
            IsInvincible = false;
            InvincibleCounter = 0;
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
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
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
    #region query function
    public bool IsBehindPlayer(Vector2 postion) {
        if (IsFacingRight && postion.x < transform.position.x)
        {
            return true;
        }
        if (!IsFacingRight && postion.x > transform.position.x)
        {
            return true;
        }
        return false;
    }
    #endregion
    #region trigger function
    public void Immobilized() {
        if (IsInvincible) return;
        Debug.Log("Immobolized");
        anim.SetTrigger("IsImmobolized");
        IsMoveable = false;
        ImmobileTimeCounter = 0.5f;
        rb.velocity = Vector2.zero;
    }
    public void Hurt(float damage) {
        if(IsInvincible) return;
        anim.SetTrigger("Hurt");
        hurtEffect.transform.position = transform.position;
        hurtEffect.Play();
        health.Hurt(damage);
        InvincibleCounter = hurtInvincibleTime;
        IsInvincible = true;

        var ev =Simulation.Schedule<PlayerHurt>();
        ev.RecoverRate = TimeRecoverRate;
        ev.TimeSlower = TimeSlower;
        ev.ShakeAmp = ShakeAmpitude;
        ev.ShakeFrequency = ShakeFrequency;
        ev.ShakeDuration = ShakeDuration;
    }
    public void Pounded()
    {
        if (IsInvincible) return;
        anim.SetTrigger("Pounded");
    }
    public void Dead() {
        anim.SetTrigger("Dead");
    }
    public void Reflected(float damage) { 
        anim.SetTrigger("IsReflected");
        health.Hurt(damage);
    }
    #endregion

    //[ContextMenu("TestTrigger")]
    //public void testTrigger() {
    //    anim.SetBool("IsJump", true);
    //    anim.SetTrigger("Hurt");
    //}
}



