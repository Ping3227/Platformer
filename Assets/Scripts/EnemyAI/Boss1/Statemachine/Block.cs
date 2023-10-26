using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : StateMachineBehaviour
{
   public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
        animator.SetFloat("Distance", Vector2.Distance(animator.transform.position, GameController.player.transform.position));
   }
   
}
