using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Health), typeof(Stamina))]
public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] float InputTimeAllowance = 0.3f;
    private float LastMoveTime;
    private bool MoveBuffer;

    [Header("Attack")]
    [SerializeField] private float AttackTime = 0.2f;
    [SerializeField] private float AttackCost = 0.2f;
    private float LastAttackTime;
    private bool AttackBuffer;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float jumpCost = 0.5f;
    [SerializeField] private float doubleJumpForce = 2f;
    [SerializeField] private float doubleJumpCost = 0.1f;
    [SerializeField] private float doubleJumpTime = 0.3f;
    [Tooltip("Bonus movement distance while jumping")]
    [SerializeField] private float ApexBonus = 0.8f;
    private float LastJumpTime;
    private bool JumpBuffer;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCost = 0.3f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashInvincibleTime = 0.05f;
    private float LastDashTime;
    private bool DashBuffer;

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
    [SerializeField] private Light2D playerLight;

    [Header("Status")]
    private bool IsFacingRight = true;
    private bool IsDashing = false;
    private bool IsJumping = false;
    private bool IsDoubleJumping = false;
    private bool IsInvincible = false;
    private bool IsHealing = false;
    private bool IsDead = false;
    private bool FinishDoubleJump = false;
    private float JumpApex = 0f;
    private bool IsMoveable = true;
    private bool IsFalling;

    [Header("Counter")]
    private float jumpTimeCounter;
    private float doubleJumpTimeCounter;
    private float DashTimeCounter;
    private float ImmobileTimeCounter;
    private float InvincibleCounter;
    private float HealCounter;

    [Header("Item")]
    [SerializeField] Item HealItem;
    [SerializeField] float HealTime = 0.5f;
    private float LastHealTime;
    private bool HealBuffer;
    [SerializeField] float HealMoveSpeed =2.0f;
    [SerializeField] float HealValue = 5f;
    [SerializeField] ParticleSystem HealParticle;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Health health;
    private Stamina stamina;
    private float moveInput;
    private RaycastHit2D groundHit;
    private Transform initialParent;
    private Vector3 initialScale;

    private void Start()
    {
        if (hurtEffect) hurtEffect.Pause();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
        initialParent = transform.parent;
        if (GameController.Instance.IsSaved) transform.position = GameController.Instance.CheckPoint;
        
    }
    private void Update() {
        if (IsDead && IsGrounded()) {
            anim.SetTrigger("Dead");
            IsDead = false;
            
            return;
            
        }
        Moveable();
        Invincible();
        CalculateBuffer();
        if (IsMoveable) {
            if (!IsHealing) {
                Dash();
                // if not dashing then attack
                Attack();
                // if moveable and not dashing 
                Jump();
            }
            Move();
            HealCheck();

        }
        UpdateAnimation();
        IsOnPlatform();
        // ItemCheck();

    }

    private void UpdateAnimation()
    {
        anim.SetBool("IsJump", IsJumping);
        anim.SetBool("IsDash", IsDashing);
        anim.SetBool("IsFalling", IsFalling);
        anim.SetBool("IsDoubleJump", IsDoubleJumping);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x / moveSpeed));
        anim.SetBool("IsHealing", IsHealing);
    }

    private void Attack() {
        if (IsDashing) return;
        if ((UserInput.instance.controls.Attack.Attack.WasPressedThisFrame() || (AttackBuffer && (LastAttackTime + InputTimeAllowance) > Time.time)))
        {
            
            stamina.ConsumeStamina(AttackCost);
            anim.SetTrigger("Attack");
            AudioManager.instance.Play("PlayerAttack");
            ImmobileTimeCounter = AttackTime;
            IsMoveable = false;
            rb.AddForce(new Vector2(rb.velocity.x * 50, 0));
            rb.velocity = new Vector2 (0,rb.velocity.y);
            AttackBuffer = false;
            if ((MoveBuffer && (LastMoveTime + InputTimeAllowance) > Time.time)) {
                Turn();
                MoveBuffer = false;
            }
        }
    }


    #region movement
    private void Move() {
        if (IsDashing || !IsMoveable) return;
        moveInput = UserInput.instance.moveInput.x;
        if (moveInput > 0 || moveInput < 0) {
            TurnCheck();
            MoveBuffer= false;
        }
        if (IsHealing) rb.velocity = new Vector2(moveInput * HealMoveSpeed, 0);
        else rb.velocity = new Vector2(moveInput * moveSpeed * (1 + JumpApex), rb.velocity.y);

    }

    private void Jump() {
        #region Special case
        if (IsDashing || !IsMoveable) {
            if (IsJumping) IsJumping = false;
            if (IsDoubleJumping) {
                FinishDoubleJump = true;
                IsDoubleJumping = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, 0);
            JumpApex = 0;
        }
        #endregion
        #region Jump
        if ((UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() || (JumpBuffer && (LastJumpTime + InputTimeAllowance) > Time.time))
                && IsGrounded() && stamina.ConsumeStamina(jumpCost * Time.deltaTime)) {
            JumpBuffer = false;
            IsJumping = true;
            FinishDoubleJump = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
        if (UserInput.instance.controls.Jumping.Jump.IsPressed()) {

            if (jumpTimeCounter > 0 && IsJumping && stamina.ConsumeStamina(jumpCost * Time.deltaTime)) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                JumpApex = (jumpTime - jumpTimeCounter) * ApexBonus / jumpTime;
                // counter 
                jumpTimeCounter -= Time.deltaTime;
                if (jumpTimeCounter < 0) jumpTimeCounter = 0;
            }
            else {
                JumpApex = 0;
                IsJumping = false;
                IsFalling = true;
            }

        }
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame()) {
            IsJumping = false;
            JumpApex = 0;
            IsFalling = true;
        }
        #endregion
        #region double jump
        // constant jump 
        if (!IsJumping && !IsGrounded() && !FinishDoubleJump
                && (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() || (JumpBuffer && (LastJumpTime + InputTimeAllowance) > Time.time))
                && stamina.ConsumeStamina(doubleJumpCost)) {

            doubleJumpTimeCounter = doubleJumpTime;
            IsDoubleJumping = true;
            FinishDoubleJump = true;
            JumpBuffer = false;

        }
        if (IsDoubleJumping) {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);

            doubleJumpTimeCounter -= Time.deltaTime;

            if (doubleJumpTimeCounter <= 0) {

                IsDoubleJumping = false;
                IsFalling = true;
            }
        }
        #endregion
    }
    private void Dash() {

        if (DashTimeCounter == 0 &&
            (UserInput.instance.controls.Dash.Dash.WasPressedThisFrame() || (DashBuffer && (LastDashTime + InputTimeAllowance) > Time.time))
            && stamina.ConsumeStamina(dashCost)) {
            IsInvincible = true;
            IsDashing = true;
            AudioManager.instance.Play("Dash");
            if (IsJumping) IsJumping = false;
            if ((MoveBuffer && (LastMoveTime + InputTimeAllowance) > Time.time)){
                Turn();
                MoveBuffer = false;
            }
            DashTimeCounter = dashTime;
            InvincibleCounter = dashInvincibleTime;
            DashBuffer = false;
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
    private void HealCheck(){
        if (IsDashing || !IsMoveable) return;
        if (IsHealing==false && GamesceneUIController.instance.RecoverNum > 0 && IsGrounded()&& (UserInput.instance.controls.Heal.Heal.WasPressedThisFrame() || (HealBuffer && (LastHealTime + InputTimeAllowance) > Time.time))) {
            IsHealing = true;
            HealCounter = HealTime;
            HealBuffer= false;

        }
        if (HealCounter > 0){
            HealCounter -= Time.deltaTime;
        }
        else{
            HealCounter = 0;
            IsHealing = false;
        }
    }
    private void Moveable() {
        if (ImmobileTimeCounter > 0){
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
    //private void ItemCheck()
    //{
    //    if (UserInput.instance.controls.SwitchItem.SwitchItem.WasPressedThisFrame())
    //    {
    //        Debug.Log("...");
    //        InventoryManager.Instance.NextItem();
    //    }
    //    else if (UserInput.instance.controls.UsedItem.UsedItem.WasPressedThisFrame() && IsMoveable && !IsDashing && !IsFalling)
    //    {
    //        if (InventoryManager.Instance.currentItem != null)
    //        {
    //            if (InventoryManager.Instance.currentItem==HealItem && InventoryManager.Instance.UsedItem(HealItem))
    //            {
    //                anim.SetTrigger("Heal");
    //                ImmobileTimeCounter = HealTime;
    //                IsMoveable = false;
    //            }
    //        }
    //    }
    //}
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
            
            IsFalling = false;
            return true;
        }
        else
        { 
            return false;
        }

    }
    #endregion
    #region platform check
    private void IsOnPlatform()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
        if (groundHit.collider != null && groundHit.collider.CompareTag("Platform")){

            transform.SetParent(groundHit.collider.transform,true);
        }
        else
        {
            transform.SetParent(initialParent, true);
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
        anim.SetTrigger("IsImmobolized");
        IsMoveable = false;
        IsHealing = false;
        ImmobileTimeCounter = 0.7f;
        rb.velocity = Vector2.zero;
    }
    public void Hurt(float damage) {
        if(IsInvincible) return;
        anim.SetTrigger("Hurt");
        health.Hurt(damage);
        InvincibleCounter = hurtInvincibleTime;
        IsInvincible = true;
        
        #region hurt effect
        LeanTween.value(60,25,ShakeDuration/2).setOnUpdate((float val)=>
        {
            playerLight.pointLightOuterRadius = val;
            
        }).setLoopPingPong(1).setEaseInOutSine();
        var ev =Simulation.Schedule<PlayerHurt>();
        ev.RecoverRate = TimeRecoverRate;
        ev.TimeSlower = TimeSlower;
        ev.ShakeAmp = ShakeAmpitude;
        ev.ShakeFrequency = ShakeFrequency;
        ev.ShakeDuration = ShakeDuration;
        AudioManager.instance.Play("PlayerHurt");
        hurtEffect.transform.position = transform.position;
        hurtEffect.Play();
        #endregion
        #region HitStun
        //IsMoveable = false;
        //ImmobileTimeCounter = 0.5f;
        //rb.velocity = Vector2.zero;
        #endregion
    }
    public void Pounded(){
        if (IsInvincible) return;
        anim.SetTrigger("Pounded");
    }
    public void Dead() {
        rb.velocity = Vector2.zero;
        IsDead = true;
        ImmobileTimeCounter= 10.0f;
        IsMoveable = false;
        // Dead event 
    }
    public void Reflected(float damage) { 
        anim.SetTrigger("IsReflected");
        health.Hurt(damage);

        InvincibleCounter = hurtInvincibleTime;
        IsInvincible = true;

        #region hurt effect
        var ev = Simulation.Schedule<PlayerHurt>();
        ev.RecoverRate = TimeRecoverRate;
        ev.TimeSlower = TimeSlower;
        ev.ShakeAmp = ShakeAmpitude;
        ev.ShakeFrequency = ShakeFrequency;
        ev.ShakeDuration = ShakeDuration;
        hurtEffect.transform.position = transform.position;
        hurtEffect.Play();
        #endregion
        #region HitStun
        //IsMoveable = false;
        //ImmobileTimeCounter = 0.5f;
        //rb.velocity = Vector2.zero;
        #endregion
    }

    
    #endregion
    void CalculateBuffer() {

        if (!IsMoveable)
        {
            // Dash 
            if (UserInput.instance.controls.Dash.Dash.WasPressedThisFrame())
            {
                DashBuffer = true;
                LastDashTime = Time.time;
            }
            else if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame())
            {
                AttackBuffer = true;
                LastAttackTime = Time.time;
            }
            else if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame())
            {
                JumpBuffer = true;
                LastJumpTime = Time.time;
            }
            else if (UserInput.instance.controls.Heal.Heal.WasPressedThisFrame()) { 
                HealBuffer = true;
                LastHealTime = Time.time;
            }

        }
        else if (IsDashing) {
            if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame())
            {
                AttackBuffer = true;
                LastAttackTime = Time.time;
            }
            else if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame())
            {
                JumpBuffer = true;
                LastJumpTime = Time.time;
            }
            else if (UserInput.instance.controls.Heal.Heal.WasPressedThisFrame())
            {
                HealBuffer = true;
                LastHealTime = Time.time;
            }
        }
        if (UserInput.instance.moveInput.x > 0 && !IsFacingRight)
        {
            MoveBuffer = true;
            LastMoveTime = Time.time;
        }
        else if (UserInput.instance.moveInput.x < 0 && IsFacingRight)
        {
            MoveBuffer = true;
            LastMoveTime = Time.time;
        }
        
    }
    public void SetParameter() {
        var index = ModeManager.instance.getNext();
        
        jumpForce = ModeManager.instance.modes[ModeManager.instance.index].Jumpforce;
        doubleJumpForce = ModeManager.instance.modes[ModeManager.instance.index].DoubleJumpforce;
        moveSpeed = ModeManager.instance.modes[ModeManager.instance.index].Movespeed;
        stamina.setParameter(index);
        health.SetParameter(index);
    }
    void Heal()
    {
        GamesceneUIController.instance.Recover();
        health.Heal(HealValue);
        AudioManager.instance.Play("Healing");
        //AudioManager.instance.Play("Heal");
        HealParticle.Play();
        
    }
}



