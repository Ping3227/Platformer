using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAttack : StateMachineBehaviour
{
    Player player;
    
    Bounds coll;
    BoxCollider2D area;
    Boss boss;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        
        area = animator.GetComponent<Boss>().Area;
        coll = animator.GetComponent<Renderer>().bounds;
        boss = animator.GetComponent<Boss>();
        if (player.transform.position.x > area.bounds.center.x) {
            boss.SetNextPosition(new Vector2(area.bounds.min.x+coll.extents.x, area.bounds.min.y + coll.extents.y));
        }
        else
        {
             boss.SetNextPosition( new Vector2(area.bounds.max.x-coll.extents.x, area.bounds.min.y+coll.extents.y));
        }
        
    }
    
}
