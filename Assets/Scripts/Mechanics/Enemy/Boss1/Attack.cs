using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;
public class Attack : StateMachineBehaviour { 
    GameObject player;
    BoxCollider2D playerColl;
    Rigidbody2D rb;
    BoxCollider2D area;
    List<Vector2> options = new List<Vector2>();
    [SerializeField] float attackDistance;
    [SerializeField] float teleportTime;
    Vector2 targetPosition;
    float CurrentTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        playerColl =player.GetComponent<BoxCollider2D>();
        rb = animator.GetComponent<Rigidbody2D>();
        area = animator.GetComponent<Boss>().Area;
        
        InRange(new Vector2(playerColl.bounds.center.x,playerColl.bounds.max.y)+(attackDistance*Vector2.up));
        InRange(new Vector2(playerColl.bounds.min.x, playerColl.bounds.center.y) + (attackDistance*Vector2.left));
        InRange(new Vector2(playerColl.bounds.max.x, playerColl.bounds.center.y)+ (attackDistance*Vector2.right));
        CurrentTime = teleportTime;
        void InRange(Vector3 position) {
            if(position.x>area.bounds.min.x && position.x<area.bounds.max.x 
                && position.y>area.bounds.min.y && position.y<area.bounds.max.y)
                options.Add(position);
        };
        Debug.Log(options.Count);
        targetPosition= options[Random.Range(0,options.Count)];
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
        animator.SetFloat("Distance", Vector2.Distance(animator.transform.position, player.transform.position));
    }
    
}
