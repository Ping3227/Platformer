using Panda;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Flys : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D rb;
    private Vector2 StartPoint;
    private Vector2 EndPoint;
    private Vector2 SupportPoint1;
    private Vector2 SupportPoint2;
    private float progress;
    public BoxCollider2D PatrolArea;
    [SerializeField] float SmallestMoveDistance;
    [SerializeField] float Speed;

    private bool IsMoving= false;
    enum State {  Moving, Attacking };
    private State state =State.Attacking ;

    [Header("Attack")]
    [SerializeField] Player player;
    public BoxCollider2D AttackArea;
    private Animator anim;

    private void Start()
    {
        if(player==null) player = GameController.player;
        StartPoint = transform.position;
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        // Decision part 
        if (IsMoving == false){
            // if player in area 
            if(player.transform.position.x >= AttackArea.bounds.min.x && player.transform.position.x <= AttackArea.bounds.max.x &&
                    player.transform.position.y >= AttackArea.bounds.min.y && player.transform.position.y <= AttackArea.bounds.max.y && 
                    state.Equals(State.Moving) ){
                anim.SetBool("IsAnimating", true);
                anim.Play("RayAim");
                state = State.Attacking;

            }
            else if(anim.GetBool("IsAnimating")==false){
                DecidePath();
                state = State.Moving;
                IsMoving = true;
            }
            
        }
        else { 
            Move();
        }
    }
    #region movement
    void DecidePath() {
        if (player.transform.position.x >= PatrolArea.bounds.center.x){
            EndPoint = new Vector2(Random.Range(PatrolArea.bounds.min.x, PatrolArea.bounds.center.x), Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y));
            while (SmallestMoveDistance > Mathf.Abs(EndPoint.x - transform.position.x)){
                EndPoint = new Vector2(Random.Range(PatrolArea.bounds.min.x, PatrolArea.bounds.center.x), Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y));
            }
        }
        //player is on the left side of the boss
        else{
            EndPoint = new Vector2(Random.Range(PatrolArea.bounds.max.x, PatrolArea.bounds.center.x), Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y));
            while (SmallestMoveDistance > Mathf.Abs(EndPoint.x - transform.position.x)){
                EndPoint = new Vector2(Random.Range(PatrolArea.bounds.max.x, PatrolArea.bounds.center.x), Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y));
            }
        }
        SupportPoint1 = Vector2.Lerp(StartPoint, EndPoint, 0.33f);
        SupportPoint1.y= Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y);
        SupportPoint2 = Vector2.Lerp(StartPoint, EndPoint, 0.66f);
        SupportPoint2.y=Random.Range(PatrolArea.bounds.min.y, PatrolArea.bounds.max.y);

    }
    void Move() {
        // move along the path
        progress += Time.deltaTime * Speed*Random.Range(0.8f,1.2f);
        // Decide next position
        Vector2 nextPosition;   
        if (progress >= 1)
        {
            IsMoving = false;
            progress = 0;
            nextPosition = EndPoint;
            StartPoint = EndPoint;
        }
        else {
            nextPosition = (1 - progress) * (1 - progress) * (1 - progress) * StartPoint +
                3 * (1 - progress) * (1 - progress) * progress * SupportPoint1 +
                3 * (1 - progress) * progress * progress * SupportPoint2 +
            progress * progress * progress * EndPoint;
        }

        // rotate to face the next position
        transform.right = nextPosition - (Vector2)transform.position;
        // move to the next position
        rb.MovePosition(nextPosition);
    }
    #endregion
    #region attack
    #endregion
}
