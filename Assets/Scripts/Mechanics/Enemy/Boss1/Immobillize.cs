using Platformer.Mechanics;
using UnityEngine;

public class Immobillize : StateMachineBehaviour
{
    
    private BoxCollider2D area;
    private Boss boss;
    [SerializeField] float ImmobillizeSize;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        area = boss.Area;
        boss.SetNextPosition(CheckPosition(GameController.player.transform.position));
        
    }
    private Vector2 CheckPosition(Vector3 Position) {
        
        if (Position.x > area.bounds.max.x - ImmobillizeSize)
        {
            
            return new Vector2(area.bounds.max.x - ImmobillizeSize,area.bounds.min.y );
        }
        
        if (Position.x < area.bounds.min.x + ImmobillizeSize)
        {
            
            return new Vector2(area.bounds.min.x + ImmobillizeSize, area.bounds.min.y);
        }
        
        return new Vector2(Position.x, area.bounds.min.y);
        
    } 
}
