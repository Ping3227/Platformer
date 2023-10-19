
using UnityEngine;
using Panda;
using System.Collections.Generic;
using System.Collections;

public class BT_Rules : MonoBehaviour
{
    [Header("AnimationClips")]
    private Animator anim;
    
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
        anim = GetComponent<Animator>();
        
        rb= GetComponent<Rigidbody2D>();

        // Set teleport animation time
        
        AnimationEvent ev = new AnimationEvent();
        ev.time = TeleportTime;
        ev.functionName = "Move";
        teleportAnimation.AddEvent(ev);
        PandaBehaviour pd = GetComponent<PandaBehaviour>();
        pd.Tick();
        
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
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            this.NextLocation = NextLocation;
            ThisTask.Succeed();
        }
    }
    [Task]
    void CalculateDistance() {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
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
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
            anim.speed = speed;
            anim.Play(teleportAnimation.name);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Attack() {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            anim.speed = 1.0f;
            anim.Play(attackAnimation.name);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Blocking() { 
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            anim.speed = 1.0f;
            anim.Play(blockAnimation.name);
            ThisTask.Succeed();
        }
    }
    #endregion

    

}
