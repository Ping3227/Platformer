using Panda;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Animation")]
    private Animator anim;
    [Header("Attack")]
    [Tooltip("Try to keep in this Range while Attacking")]
    [SerializeField] float AttackRange;

    [Header("Move")]
    private Rigidbody2D rb;
    private List<Vector2> MoveOptions = new List<Vector2>();
    private Vector2 NextLocation;
    [SerializeField] BoxCollider2D Area;
    [SerializeField] Player player;
    private BoxCollider2D playerColl;
    private float DistancetoPlayer = 0f;

    
    [SerializeField] float SmallestMoveDistance;

    [Header("status")]
    EnemyHealth health;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        player = GameController.player;
        playerColl = player.gameObject.GetComponent<BoxCollider2D>();
        PandaBehaviour pd = GetComponent<PandaBehaviour>();
        pd.Tick();

    }
    #region Animation
    private void Update()
    {
        if (!anim.GetBool("IsAnimating")) { 
        // checking player position 
        // approaching player 
        }
    }
    #region Attack
    [Task]
    void SlowAttack()
    {
    }
    [Task]
    void FastAttack()
    {
    }
    [Task]
    void JumpAttack()
    {
    }
    #endregion
    // trigger while go into attack range
    #region dodge
    void JumpBackWard() {
        
    }
    void RollForward() { 
    }
    #endregion
    #endregion
    #region condition
    [Task]
    bool IsPlayerInRange(float distance) => DistancetoPlayer <= distance;
    [Task]
    bool IsPlayerBehind() => (transform.rotation.eulerAngles.y ==0)? transform.position.x > player.transform.position.x: transform.position.x < player.transform.position.x;
    
    #endregion
}
