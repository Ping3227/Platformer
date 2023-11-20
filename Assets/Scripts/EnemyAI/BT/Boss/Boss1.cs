
using UnityEngine;
using Panda;
using System.Collections.Generic;
using Platformer.Mechanics;
using System.Runtime.CompilerServices;
using UnityEditor;
using JetBrains.Annotations;

public class Boos1 : MonoBehaviour
{
    [Header("Animation")]
    private Animator anim;
    [SerializeField] GameObject ImmobolizePrefab;

    [Header("Attack")]
    [SerializeField] float AttackRange;
    [SerializeField] float AttackDownSideRange;
    private bool fallAttackNext= false;
    private int NormalAttackCount = 0;
    private int FailAttackCount = 0;

    [Header("Move")]
    private Rigidbody2D rb;
    private List<Vector2> MoveOptions = new List<Vector2>();
    private Vector2 NextLocation;
    [SerializeField] BoxCollider2D Area;
    [SerializeField] Player player;
    private BoxCollider2D playerColl;
    private float DistancetoPlayer = 0f;
    private Vector3 heightDiffer;

    [Header("status")]
    EnemyHealth health;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        player = GameController.player;
        playerColl= player.gameObject.GetComponent<BoxCollider2D>();
        PandaBehaviour pd = GetComponent<PandaBehaviour>();
        heightDiffer = Vector3.up * (transform.position.y - playerColl.bounds.center.y);
        pd.Tick();
        
    }
    private void Update()
    {
        Debug.Log($"IsPlayerInfront: {IsPlayerInfront()}, Angle : {transform.localEulerAngles}");
        
    }

    
    #region NonTask function
    private void Move()
    {
        rb.position = NextLocation;
        if (rb.position.x < playerColl.bounds.center.x) { 
            rb.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    #endregion
    #region MovePattern
    [Task]
    void DodgePlayer() {
        //player is on the right side
        if (playerColl.transform.position.x >= Area.bounds.center.x)
        {
            this.NextLocation = new Vector2(Random.Range(Area.bounds.min.x, Area.bounds.center.x - playerColl.bounds.extents.x), transform.position.y);
        }
        //player is on the left side of the boss
        else
        {
            this.NextLocation = new Vector2(Random.Range(Area.bounds.center.x + playerColl.bounds.extents.x, Area.bounds.max.x), transform.position.y);
        }
        ThisTask.Succeed();
    }
    [Task]
    void GoToPlayer() {

        InRange(playerColl.transform.position + (AttackDownSideRange * Vector3.up));
        InRange(playerColl.transform.position + (AttackRange * Vector3.left) + heightDiffer);
        InRange(playerColl.transform.position + (AttackRange * Vector3.right) + heightDiffer);
        int choice = Random.Range(0, MoveOptions.Count);
        NextLocation = MoveOptions[choice];
        MoveOptions.Clear();
        if (choice==0) fallAttackNext = true;
        
        void InRange(Vector2 position)
        {
            if (Area.bounds.min.x < position.x && position.x < Area.bounds.max.x
                 && Area.bounds.min.y < position.y && position.y < Area.bounds.max.y)
                MoveOptions.Add(position);
        }
        ThisTask.Succeed();
    }
    [Task]
    bool ApproachPlayer(float Approachspeed)
    {
        this.NextLocation = Vector2.MoveTowards(transform.position, playerColl.transform.position, Approachspeed);
        return true;
    }
    [Task]
    bool GoBehindPlayer() {
        Vector3 tmpLocation;
        if (playerColl.transform.position.x < transform.position.x)
        {
            tmpLocation = player.transform.position + (AttackRange * Vector3.left) + heightDiffer;
        }
        else {
            tmpLocation = player.transform.position + (AttackRange * Vector3.right) + heightDiffer;
        }
        if(Area.bounds.min.x< tmpLocation.x && tmpLocation.x < Area.bounds.max.x)
        {
            this.NextLocation = tmpLocation;
            return true;
        }
        return false;
    }
    [Task]
    void CalculateDistance() {
        if (!anim.GetBool("IsAnimating"))
        {
            DistancetoPlayer = Vector2.Distance(playerColl.transform.position, transform.position);
            ThisTask.Succeed();
        }
    }
    #endregion
    #region condition
    [Task]
    bool MissMoreThan(int number)=> FailAttackCount > number;
    [Task]
    bool NormalAttackMoreThan(int number) => NormalAttackCount > number;
    [Task]
    bool CloserThan(float distance) => DistancetoPlayer < distance;
    [Task]
    bool IsPlayerInfront() => (player.transform.position.x < transform.position.x && transform.localEulerAngles.y == 180) ||
                            (transform.position.x < player.transform.position.x && transform.localEulerAngles.y == 0);
    [Task]
    [HideInInspector]
    public bool IsImmobilized = false;
    [Task]
    private bool ResetImmobilized()
    {
        IsImmobilized = false;
        return true;
    }
    [Task]
    private bool IsCumulateDamageGreaterThan(float damage) {
        if (health._cumulateDamage >= damage){
            health.ResetCumulateDamage();
            return true;
        }   
        else return false;
    }
    #endregion
    #region Animation
    [Task]
    void Teleport(float speed) {
        if (!anim.GetBool("IsAnimating") ){
            anim.Play("Teleport");
            anim.SetFloat("TeleportSpeed", speed);
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Attack(float speed,bool IsSpecialAttack) {

        if (!anim.GetBool("IsAnimating")){
            if (fallAttackNext)
            {
                anim.Play("FallAttack");
                anim.SetBool("IsAnimating", true);
                fallAttackNext = false;
               
            }
            else {
                anim.Play("NormalAttack");
                anim.SetBool("IsAnimating", true);
                anim.SetFloat("AttackSpeed", speed);
            }
            if (IsSpecialAttack)
            {
                NormalAttackCount = 0;
            }
            else {
                NormalAttackCount++;
            }
            FailAttackCount++;
            
            ThisTask.Succeed();
        }
    }
    [Task]
    void StrongAttack()
    {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("TwoSideAttack");
            anim.SetBool("IsAnimating", true);
            NormalAttackCount = 0;
            FailAttackCount++;
            ThisTask.Succeed();
        }
    }
    [Task]
    void RayAim(float speed) {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("RayAim");
            anim.SetBool("IsAnimating", true);
            anim.SetFloat("RayAimSpeed", speed);
            ThisTask.Succeed();
        }
    }
    [Task]
    void RayAttack(float speed) {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("RayAttack");
            anim.SetBool("IsAnimating", true);
            anim.SetFloat("RayAttackSpeed", speed);
            NormalAttackCount = 0;
            FailAttackCount++;
            ThisTask.Succeed();
        }
    }
    [Task]
    void Blocking() {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("Block");
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
        
    }
    [Task]
    void Immobolize() {
        if (!anim.GetBool("IsAnimating"))
        {
            if (ImmobolizePrefab)
            {
                ImmobolizePrefab.transform.position = new Vector3(player.transform.position.x,Area.bounds.min.y);
                ImmobolizePrefab.SetActive(true);
            }
            ThisTask.Succeed();
        }
    }
    #endregion
    #region trigger 
    public void AttackSuccess()
    {
        FailAttackCount = 0;
    }
    #endregion

}
