
using UnityEngine;
using Panda;
using System.Collections.Generic;

public class BT_Rules : MonoBehaviour
{
    [Header("AnimationClips")]
    private Animation anim;
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] AnimationClip teleportAnimation;
    [SerializeField] float TeleportTime ;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip blockAnimation;

    [Header("Move")]
    private Rigidbody2D rb;
    private Vector2 NextLocation;
    private List<Vector2> moveOptions = new List<Vector2>();
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D Area;
    private float DistancetoPlayer = 0f;

    [Task]
    [HideInInspector]
    public bool IsImmobilized = false;

    private void Start()
    {    
        anim = gameObject.GetComponent<Animation>();
        rb= GetComponent<Rigidbody2D>();

        // Set teleport animation time
        
        AnimationEvent ev = new AnimationEvent();
        ev.time = TeleportTime;
        ev.functionName = "Move";
        teleportAnimation.AddEvent(ev);
        
    }

    #region NonTask function
    private void Move()
    {
        rb.position = NextLocation;
    }
    #endregion
    #region Normal function
    [Task]
    void SetNextLocation(Vector2 NextLocation) {
        if (!anim.isPlaying)
        {
            this.NextLocation = NextLocation;
            ThisTask.Succeed();
        }
    }
    [Task]
    void CalculateDistance() {
        if (!anim.isPlaying) {
            DistancetoPlayer = Vector2.Distance(player.transform.position, transform.position);
            ThisTask.Succeed();
        }
    }
    [Task]
    bool FartherThan(float distance) => DistancetoPlayer > distance;
    #endregion
    #region Animation
    [Task]
    void Teleport(float speed) {
        if (!anim.isPlaying) {
            anim[teleportAnimation.name].speed = speed;
            anim.Play(teleportAnimation.name);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Attack() {
        if (!anim.isPlaying)
        {
            anim.Play(attackAnimation.name);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Blocking() { 
        if (!anim.isPlaying)
        {
            anim.Play(blockAnimation.name);
            ThisTask.Succeed();
        }
    }
    #endregion

}
