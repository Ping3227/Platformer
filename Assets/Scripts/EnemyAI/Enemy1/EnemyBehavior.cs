using Panda.Examples.Shooter;
using Platformer.Mechanics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    [Header("Patrol")]
    [SerializeField] BoxCollider2D PatrolArea;
    [SerializeField] float PatrolSpeed = 3f;
    [SerializeField] float errorAllowed = 0.1f;
    private Vector2 PatrolPoint =new Vector2();
    private bool FinishPatrol;
    private bool IsFacingRight = true;

    [Header("Chase")]
    private Player player;
    private bool IsChasing = false;
    private float DistanceToPlayer;
    [SerializeField] BoxCollider2D ChaseArea;
    [SerializeField] float ChaseSpeed = 5f;
    [SerializeField] float maxJumpForce = 5f;
    [SerializeField] float ChaseRange = 5f;
    [SerializeField] float AttackRange = 1f;
    private RaycastHit2D groundHit;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float extraHeight = 0.25f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        player = GameController.player;
        PatrolPoint = transform.position;
    }
    void Patrol() {
        if (FinishPatrol) {
            PatrolPoint.x= Random.Range(PatrolArea.bounds.min.x, PatrolArea.bounds.max.x);
        }
        rb.MovePosition(Vector2.MoveTowards(transform.position, PatrolPoint, PatrolSpeed * Time.deltaTime));
        
        
    }
    void Chase(){
        if(DistanceToPlayer > AttackRange){
            rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, ChaseSpeed * Time.deltaTime));
            if (player.transform.position.y > transform.position.y + 0.5f){
                Jump();
            }
            
        }
        else{
            
            Attack();
        }
    }
    void Jump() {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);
        if (groundHit.collider){
            rb.AddForce(new Vector2(0, maxJumpForce));
        }
    }
    void Attack()
    {
        anim.SetTrigger("Attack");
    }
    void CalculateDistance()
    {
        FinishPatrol = Vector2.Distance(transform.position, PatrolPoint) < errorAllowed;
        DistanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (DistanceToPlayer <= ChaseRange &&
                player.transform.position.x > ChaseArea.bounds.min.x && player.transform.position.x < ChaseArea.bounds.max.x &&
                player.transform.position.y > ChaseArea.bounds.min.y && player.transform.position.y < ChaseArea.bounds.max.y)
        {
            IsChasing = true;
        }
        else
        {
            IsChasing = false;
        }

    }
    private void TurnCheck() {
        if (IsChasing) {
            IsFacingRight = player.transform.position.x > transform.position.x;
            if(IsFacingRight){
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else{
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            
        }
        else {
            IsFacingRight = PatrolPoint.x > transform.position.x;
            if (IsFacingRight){
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else{
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
    }
    private void Update()
    {
        Debug.Log(transform.position.x == PatrolPoint.x);
        CalculateDistance();
        if (IsChasing){
            Chase();
        }
        else{
            Patrol();
            Debug.Log("Patrol");
        }
        TurnCheck();
    }
    
}
