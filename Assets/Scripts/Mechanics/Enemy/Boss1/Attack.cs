using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;
public class Attack : StateMachineBehaviour { 
    private GameObject player;
    private Bounds playerColl;
    private Rigidbody2D rb;
    private Bounds coll;
    BoxCollider2D area;
    List<Vector2> options = new List<Vector2>();
    [SerializeField] float attackDistance;
    [SerializeField] float teleportTime;
    Vector2 targetPosition;
    float CurrentTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        playerColl = player.GetComponent<Renderer>().bounds;
        rb = animator.GetComponent<Rigidbody2D>();
        area = animator.GetComponent<Boss>().Area;
        
        InRange(new Vector2(playerColl.center.x,playerColl.max.y)+(attackDistance*Vector2.up));
        InRange(new Vector2(playerColl.min.x, playerColl.center.y) + (attackDistance*Vector2.left));
        InRange(new Vector2(playerColl.max.x, playerColl.center.y)+ (attackDistance*Vector2.right));
        void InRange(Vector3 position)
        {
            if (position.x > area.bounds.min.x && position.x < area.bounds.max.x
                && position.y > area.bounds.min.y && position.y < area.bounds.max.y)
                options.Add(position);
        }
        targetPosition = options[Random.Range(0, options.Count)];
        options.Clear();
        CurrentTime = teleportTime;
        animator.SetFloat("Distance", Vector2.Distance(animator.transform.position, player.transform.position));
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CurrentTime <= 0) {
            rb.position = targetPosition;

        }
        else
        {
            CurrentTime -= Time.deltaTime;
        }
        
    }
    
}
