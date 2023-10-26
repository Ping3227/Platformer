using UnityEngine;

public class Idle : StateMachineBehaviour
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsAnimating", false);
    }

}
