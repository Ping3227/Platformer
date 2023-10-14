using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAttack : StateMachineBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    Bounds coll;
    BoxCollider2D area;
    Vector2 targetPosition;
    bool IsMoved ;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        rb = animator.GetComponent<Rigidbody2D>();
        area = animator.GetComponent<Boss>().Area;
        coll = animator.GetComponent<Renderer>().bounds;
        IsMoved = false;
        if (player.transform.position.x > area.bounds.center.x) {
            targetPosition = new Vector2(area.bounds.min.x+coll.extents.x, area.bounds.min.y + coll.extents.y);
        }
        else
        {
            targetPosition = new Vector2(area.bounds.max.x-coll.extents.x, area.bounds.min.y+coll.extents.y);
        }
        animator.SetFloat("Distance", Vector2.Distance(animator.transform.position, player.transform.position));
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsMoved) {
            rb.position = targetPosition;
            IsMoved = true;
        }
        
        
    }
}
