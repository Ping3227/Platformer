using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;
public class Attack : StateMachineBehaviour { 
    private Player player;
    private Bounds playerColl;
    
    private BoxCollider2D area;
    private Boss boss;

    private List<Vector2> options = new List<Vector2>();
    
    [Header("Attack")]
    [SerializeField] float attackDistance;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        playerColl = player.GetComponent<Renderer>().bounds;
        
        area = animator.GetComponent<Boss>().Area;
        boss = animator.GetComponent<Boss>();
        
        FindPosition();
    }
    
    private void FindPosition() {
        InRange(new Vector2(playerColl.center.x, playerColl.max.y) + (attackDistance * Vector2.up));
        InRange(new Vector2(playerColl.min.x, playerColl.center.y) + (attackDistance * Vector2.left));
        InRange(new Vector2(playerColl.max.x, playerColl.center.y) + (attackDistance * Vector2.right));

        boss.SetNextPosition(options[Random.Range(0, options.Count)]);
        options.Clear();

        void InRange(Vector2 position)
        {
            if (position.x > area.bounds.min.x && position.x < area.bounds.max.x
                && position.y > area.bounds.min.y && position.y < area.bounds.max.y)
                options.Add(position);
        }
        
    }
}
