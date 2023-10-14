using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class Immobillize : StateMachineBehaviour
{
    private GameObject player;
    private BoxCollider2D area;
    Vector2 targetPosition;
    [SerializeField] GameObject ImmobillizeField;
    [SerializeField] float ImmobillizeSize;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameController.player;
        animator.SetFloat("Distance", Vector2.Distance(animator.transform.position, player.transform.position));
        targetPosition =CheckPosition(player.transform.position);
        Instantiate(ImmobillizeField,targetPosition , Quaternion.identity);
    }
    private Vector2 CheckPosition(Vector3 Position) {
        if(Position.x > area.bounds.max.x-ImmobillizeSize )
        {
            return new Vector2(area.bounds.max.x - ImmobillizeSize,area.bounds.min.y );
        }
        if(Position.x < area.bounds.min.x + ImmobillizeSize)
        {
            return new Vector2(area.bounds.min.x + ImmobillizeSize, area.bounds.min.y);
        }
        return new Vector2(Position.x, area.bounds.min.y);
        
    } 
}
