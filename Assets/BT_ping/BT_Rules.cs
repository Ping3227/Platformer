
using UnityEngine;
using Panda;
using System.Collections.Generic;
using Platformer.Mechanics;

public class BT_Rules : MonoBehaviour
{
    [Header("AnimationClips")]
    private Animator anim;
    [SerializeField] AnimationClip teleportAnimation;
    [SerializeField] float TeleportTime ;
    [SerializeField] AnimationClip ImmobolizeAnimation;
    [SerializeField] float ImmobolizeTime;
    [SerializeField] GameObject ImmobolizePrefab;

    [Header("Attack")]
    [SerializeField] float AttactRange;
    [SerializeField] float AttackDamage;
    [SerializeField] float AttackDownSideRange;
    [SerializeField] float AttackDownSideDamage;
    [SerializeField] float StrongAttackRange;
    [SerializeField] float StrongAttackDamage;

    [Header("Move")]
    private Rigidbody2D rb;
    private Vector2 NextLocation;
    private List<Vector2> MoveOptions = new List<Vector2>();
    [SerializeField] Player player;
    private BoxCollider2D playerColl;
    [SerializeField] BoxCollider2D Area;
    Vector3 heightDiffer;
    
    private float DistancetoPlayer = 0f;

    [Task]
    [HideInInspector]
    public bool IsImmobilized = false;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        player = GameController.player;
        playerColl= player.gameObject.GetComponent<BoxCollider2D>();
        // Set teleport animation time
        PandaBehaviour pd = GetComponent<PandaBehaviour>();
        pd.Tick();
        heightDiffer = Vector3.up *(transform.position.y-playerColl.bounds.center.y);
        
    }
    

    #region NonTask function
    private void Move()
    {
        rb.position = NextLocation;
        if (rb.position.x < playerColl.bounds.center.x) { 
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rb.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void Immobolize() {
        ImmobolizePrefab.SetActive(true);
    }
    #endregion
    #region Location 
    [Task]
    void FindNextLocation(bool IsDodging) {
        #region Dodging player 
        if (IsDodging)
        {
            //player is on the right side
            if (playerColl.transform.position.x >= Area.bounds.center.x)
            {
                this.NextLocation=  new Vector2(Random.Range(Area.bounds.min.x, Area.bounds.center.x -playerColl.bounds.extents.x), transform.position.y);
            }
            //player is on the left side of the boss
            else {
                this.NextLocation = new Vector2(Random.Range(Area.bounds.center.x+playerColl.bounds.extents.x, Area.bounds.max.x), transform.position.y);
            }
        }
        #endregion
        #region Attacking player
        else
        {
            InRange(playerColl.transform.position + (AttackDownSideRange * Vector3.up));
            InRange(playerColl.transform.position + (AttactRange * Vector3.left) +heightDiffer);
            InRange(playerColl.transform.position + (AttactRange * Vector3.right)+heightDiffer);
            this.NextLocation = MoveOptions[Random.Range(0, MoveOptions.Count)];
            MoveOptions.Clear();
            void InRange(Vector2 position)
            {
                if (Area.bounds.min.x < position.x && position.x < Area.bounds.max.x
                     && Area.bounds.min.y < position.y && position.y < Area.bounds.max.y)
                    MoveOptions.Add(position);
            }

        }
        #endregion
        
        ThisTask.Succeed();
    }
    [Task]
    void Approach(float Approachspeed)
    {
        this.NextLocation = Vector2.MoveTowards(transform.position, playerColl.transform.position, Approachspeed);
    }
    [Task]
    bool GoBehind() {
        Vector3 tmpLocation = player.transform.position + (AttactRange * Vector3.left) + heightDiffer;
        if (!player.IsBehindPlayer(tmpLocation)) {
            tmpLocation = player.transform.position + (AttactRange * Vector3.right) + heightDiffer;
        }
        if(Area.bounds.min.x< tmpLocation.x && tmpLocation.x < Area.bounds.max.x)
        {
            this.NextLocation = tmpLocation;
            return true;
        }
        return false;
    }
[Task]
    void CalculateDistance() {
        if (!anim.GetBool("IsAnimating"))
        {
            DistancetoPlayer = Vector2.Distance(playerColl.transform.position, transform.position);
            ThisTask.Succeed();
        }
    }
    [Task]
    bool CloserThan(float distance) => DistancetoPlayer < distance;
    #endregion
    #region Animation
    [Task]
    void Teleport(float speed) {
        if (!anim.GetBool("IsAnimating") ){
            anim.Play("Teleport");
            anim.SetFloat("TeleportSpeed", speed);
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
    }
    [Task]
    void Attack() {

        if (!anim.GetBool("IsAnimating")){
            anim.Play("OneSideAttack");
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
    }
    [Task]
    void StrongAttack()
    {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("TwoSideAttack");
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
    }
    [Task]
    void FallAttack() { 
    }
    [Task]
    void Blocking() {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("Block");
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
        
    }
    #endregion

    

}
