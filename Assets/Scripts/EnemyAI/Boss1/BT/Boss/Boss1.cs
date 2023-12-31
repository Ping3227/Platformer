
using UnityEngine;
using Panda;
using System.Collections.Generic;
using Platformer.Mechanics;


public class Boos1 : MonoBehaviour
{
    [Header("Animation")]
    private Animator anim;
    [SerializeField] GameObject ImmobolizePrefab;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject Drone;
    [SerializeField] BoxCollider2D DroneArea;
    [SerializeField] Transform SpawnPoint;

    [Header("Attack")]
    [SerializeField] float AttackRange;
    [SerializeField] float AttackDownSideRange;
    private bool fallAttackNext = false;
    private int NormalAttackCount = 0;
    private int FailAttackCount = 0;
    [SerializeField] ParticleSystem AttackEffect;

    [Header("Move")]
    private Rigidbody2D rb;
    private List<Vector2> MoveOptions = new List<Vector2>();
    private Vector2 NextLocation;
    [SerializeField] BoxCollider2D Area;
    [SerializeField] Player player;
    private BoxCollider2D playerColl;
    private float DistancetoPlayer = 0f;
    [SerializeField] Vector3 height;
    [SerializeField] float SmallestMoveDistance;

    [Header("status")]
    BossHealth health;
    PandaBehaviour _BT;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<BossHealth>();
        player = GameController.player;
        playerColl = player.gameObject.GetComponent<BoxCollider2D>();
        _BT = GetComponent<PandaBehaviour>();
       
        _BT.Tick();

    }


    #region NonTask function
    // Animation event can't use boolean parameter
    private void Move(int FacingPlayer = 0)
    {
        rb.velocity = Vector2.zero;
        rb.position = NextLocation;
        if (FacingPlayer == 0) {
            if (rb.position.x < playerColl.bounds.center.x)
            {
                rb.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }
    void DroneSpawnInAnimation() {
        if (Drone != null)
        {
            var NewDrone = Instantiate(Drone, SpawnPoint.position, Quaternion.identity).GetComponent<Flys>();
            NewDrone.PatrolArea = DroneArea;
            NewDrone.AttackArea = Area;
        }
    }
    #endregion
    #region MovePattern
    [Task]
    void DodgePlayer() {

        //player is on the right side
        if (playerColl.transform.position.x >= Area.bounds.center.x )
        {
            NextLocation = new Vector2(Random.Range(Area.bounds.min.x, Area.bounds.center.x - playerColl.bounds.extents.x), transform.position.y);
            while (SmallestMoveDistance > Mathf.Abs(NextLocation.x - transform.position.x)) {
                NextLocation = new Vector2(Random.Range(Area.bounds.min.x, Area.bounds.center.x - playerColl.bounds.extents.x), transform.position.y);
            }

        }
        //player is on the left side of the boss
        else
        {
            NextLocation = new Vector2(Random.Range(Area.bounds.min.x, Area.bounds.center.x - playerColl.bounds.extents.x), transform.position.y);
            while (SmallestMoveDistance > Mathf.Abs(NextLocation.x - transform.position.x)) {
                NextLocation = new Vector2(Random.Range(Area.bounds.center.x + playerColl.bounds.extents.x, Area.bounds.max.x), transform.position.y);
            }
        }

        ThisTask.Succeed();
    }
    [Task]
    void GoToPlayer() {

        InRange(new Vector3(playerColl.transform.position.x, height.y + AttackDownSideRange));
        InRange(new Vector3(playerColl.transform.position.x + AttackRange, height.y));
        InRange(new Vector3(playerColl.transform.position.x - AttackRange, height.y));
        int choice = Random.Range(0, MoveOptions.Count);
        NextLocation = MoveOptions[choice];
        MoveOptions.Clear();
        if (choice == 0) fallAttackNext = true;
        else fallAttackNext = false;
        void InRange(Vector2 position)
        {
            if (Area.bounds.min.x < position.x && position.x < Area.bounds.max.x
                 && Area.bounds.min.y < position.y && position.y < Area.bounds.max.y
                 && SmallestMoveDistance < Vector2.Distance(position, transform.position))

                MoveOptions.Add(position);
        }
        ThisTask.Succeed();
    }
    [Task]
    void GoAbovePlayer()
    {
        NextLocation = new Vector3(playerColl.transform.position.x, height.y + AttackDownSideRange);
        fallAttackNext = true;

        ThisTask.Succeed();
    }
    [Task]
    bool ApproachPlayer(float Approachspeed)
    {
        NextLocation = Vector2.MoveTowards(transform.position, playerColl.transform.position, Approachspeed);
        NextLocation.y = transform.position.y;
        return true;
    }
    [Task]
    bool GoBehindPlayer() {
        Vector3 tmpLocation;
        if (playerColl.transform.position.x < transform.position.x)
        {
            tmpLocation = player.transform.position + (AttackRange * Vector3.left);
        }
        else {
            tmpLocation = player.transform.position + (AttackRange * Vector3.right);
        }
        tmpLocation.y = height.y;
        if (Area.bounds.min.x < tmpLocation.x && tmpLocation.x < Area.bounds.max.x)
        {
            NextLocation = tmpLocation;
            return true;
        }
        return false;
    }
    [Task]
    bool GoToCorner()
    {

        if (playerColl.transform.position.x >= Area.bounds.center.x) {
            NextLocation = new Vector2(Area.bounds.min.x, transform.position.y);
        }
        else {
            NextLocation = new Vector2(Area.bounds.max.x, transform.position.y);
        }
        return true;
    }
    [Task]
    bool GoBeforePlayer()
    {
        Vector3 tmpLocation;
        if (playerColl.transform.position.x < transform.position.x)
        {
            tmpLocation = player.transform.position + (AttackRange * Vector3.right);
        }
        else
        {
            tmpLocation = player.transform.position + (AttackRange * Vector3.left);
        }
        tmpLocation.y = height.y;
        if (Area.bounds.min.x < tmpLocation.x && tmpLocation.x < Area.bounds.max.x)
        {
            NextLocation = tmpLocation;
            return true;
        }
        return false;
    }
    [Task]
    bool GoForward(float distance) {
        float tmpX = transform.position.x + (transform.localEulerAngles.y == 180 ? distance : -distance);
        tmpX = Mathf.Clamp(tmpX, Area.bounds.min.x, Area.bounds.max.x);
        if (transform.root.localEulerAngles.y == 0)
        {
            NextLocation = new Vector2(tmpX, transform.position.y);
        }
        else
        {
            NextLocation = new Vector2(tmpX, transform.position.y);
        }
        return true;
    }
    [Task]
    void CalculateDistance() {
        if (!anim.GetBool("IsAnimating"))
        {
            DistancetoPlayer = Vector2.Distance(playerColl.transform.position, transform.position);
            ThisTask.Succeed();
        }
    }
    #endregion
    #region condition
    [Task]
    bool MissMoreThan(int number) => FailAttackCount > number;
    [Task]
    bool NormalAttackMoreThan(int number) => NormalAttackCount > number;
    [Task]
    bool CloserThan(float distance) => DistancetoPlayer < distance;
    [Task]
    bool IsPlayerInfront() => (player.transform.position.x > transform.position.x && transform.localEulerAngles.y == 180) ||
                            (transform.position.x > player.transform.position.x && transform.localEulerAngles.y == 0);
    [Task]
    bool IsPlayerLowerthan(float height) => playerColl.bounds.center.y < transform.position.y + height;
    [Task]
    [HideInInspector]
    public bool IsImmobilized = false;
    [Task]
    private bool ResetImmobilized()
    {
        IsImmobilized = false;
        return true;
    }
    [Task]
    private bool IsCumulateDamageGreaterThan(float damage) {
        if (health._cumulateDamage >= damage) {
            health.ResetCumulateDamage();
            return true;
        }
        else return false;
    }
    #endregion
    #region Animation
    [Task]
    void Teleport(float speed, bool FacingPlayer) {
        
        if (!anim.GetBool("IsAnimating")) {
            
            if (FacingPlayer)
            {
                anim.Play("Teleport");

            }
            else {
                anim.Play("Teleport(special)");
            }
            AudioManager.instance.Play("BossSuddenly");
            anim.SetFloat("TeleportSpeed", speed);
            anim.SetBool("IsAnimating", true);
            ThisTask.Succeed();
        }
    }
    [Task]
    void LeaveBomb()
    {
        
        if (Bomb != null && Bomb.activeSelf == false)
        {
            Bomb.transform.position = transform.position;
            Bomb.SetActive(true);
        }
        ThisTask.Succeed();


    }
    [Task]
    void Attack(float speed, bool IsSpecialAttack, bool FacingPlayer = true) {

        if (!anim.GetBool("IsAnimating")) {
            if (fallAttackNext)
            {

                anim.Play("FallAttack");
                anim.SetBool("IsAnimating", true);
                AudioManager.instance.PlayDelayed("BossFallAttack", 0.39f);
                // Used leatween to continue call a function in a constant interval until its done
                LeanTween.value(gameObject, 0, 1, 0.5f).setOnUpdate((float val) =>
                {
                    CamearaController.Instance.ShakeCamera(val * 2, 0.1f, 0.3f, 1);

                }).setDelay(0.39f);
                fallAttackNext = false;

            }
            else {
                if (FacingPlayer)
                {
                    anim.Play("NormalAttack");
                    AudioManager.instance.PlayDelayed("BossAttack", 1.0f);
                }
                else anim.Play("NormalAttack(special)");
                anim.SetBool("IsAnimating", true);
                anim.SetFloat("AttackSpeed", speed);
            }
            if (IsSpecialAttack)
            {
                NormalAttackCount = 0;
            }
            else {
                NormalAttackCount++;
            }
            FailAttackCount++;

            ThisTask.Succeed();
        }
    }
    [Task]
    void ConsecutiveAttack(){
        AttackEffect.Play();
        ThisTask.Succeed();
    }
    [Task]
    void DelayFallAttack()
    {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("DelayFallAttack");
            anim.SetBool("IsAnimating", true);
            AudioManager.instance.PlayDelayed("BossFallAttack", 0.50f);
            // Used leatween to continue call a function in a constant interval until its done
            LeanTween.value(gameObject, 0, 1, 0.5f).setOnUpdate((float val) =>
            {
                CamearaController.Instance.ShakeCamera(val * 2, 0.1f, 0.3f, 1);

            }).setDelay(0.50f);
            fallAttackNext = false;
            ThisTask.Succeed();
        }
    }
    [Task]
    void StrongAttack()
    {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("TwoSideAttack");
            AudioManager.instance.PlayDelayed("BossTwosideAttack", 0.1f);
            anim.SetBool("IsAnimating", true);
            NormalAttackCount = 0;
            FailAttackCount++;
            ThisTask.Succeed();
        }
    }
    [Task]
    void RayAim(float speed,bool IsOnTarget =true) {
        if (!anim.GetBool("IsAnimating"))
        {
            
            if (IsOnTarget){
                anim.Play("RayAim");
            }
            else
            {
                anim.Play("RayAimFollow");
            }
            anim.SetBool("IsAnimating", true);
            anim.SetFloat("RayAimSpeed", speed);

            ThisTask.Succeed();
        }
    }
    [Task]
    void RayAttack(float speed, bool spin, bool explode) {
        if (!anim.GetBool("IsAnimating"))
        {
            if (spin) {
                anim.Play("RayAttack(spin)");
            }
            else {
                anim.Play("RayAttack");
            }

            anim.SetBool("IsAnimating", true);
            anim.SetFloat("RayAttackSpeed", speed);
            AudioManager.instance.PlayDelayed("BossLazer", 0.0f);
            NormalAttackCount = 0;
            FailAttackCount++;
            ThisTask.Succeed();
        }
    }
    [Task]
    void RaySweep()
    {
        if (!anim.GetBool("IsAnimating"))
        {
            anim.Play("RaySweep");
            anim.SetBool("IsAnimating", true);
            AudioManager.instance.PlayDelayed("BossLazer", 0.0f);
            NormalAttackCount = 0;
            FailAttackCount++;
            ThisTask.Succeed();
        }
    }
    [Task]
    void Immobolize() {
        Debug.Log(player.transform.rotation.eulerAngles);
        if (!anim.GetBool("IsAnimating"))
        {
            if (ImmobolizePrefab && ImmobolizePrefab.activeSelf == false)
            {
                // right 0, 0, 0 left 0, 180, 0
                float spawnpoint = player.transform.position.x + (player.transform.rotation == Quaternion.Euler(0, 0, 0) ? 1 : -1) * 2;
                spawnpoint = Mathf.Clamp(spawnpoint, Area.bounds.min.x, Area.bounds.max.x);
                ImmobolizePrefab.transform.position = new Vector3(spawnpoint, Area.bounds.min.y);
                ImmobolizePrefab.SetActive(true);
            }
            ThisTask.Succeed();
        }
    }
    [Task]
    void SpawnDrone()
    {
        if (!anim.GetBool("IsAnimating"))
        {
           
            if (Drone!=null)
            {
                
                var NewDrone =Instantiate(Drone, transform.position, Quaternion.identity).GetComponent<Flys>();
                NewDrone.PatrolArea = Area;
                NewDrone.AttackArea = Area;
            }
            ThisTask.Succeed();
        }
    }
    #endregion
    #region trigger 
    public void AttackSuccess()
    {
        FailAttackCount = 0;
    }
    #endregion
    public void NextStage(AnimationClip clip, TextAsset BTscript) {
        Debug.Log("play animation");
        anim.Play(clip.name); 
        anim.SetBool("IsAnimating", true);
        if (BTscript != null)
        {
            Debug.Log("set new scripts");
            _BT.enabled = false;
            _BT.scripts = new TextAsset[1] { BTscript};
            _BT.Compile(BTscript.text);
            _BT.Reset();
            LeanTween.delayedCall(gameObject, clip.length, () =>
            {
                _BT.enabled = true;

                _BT.Tick();

            });
        }
        else {
            AudioManager.instance.StopAllBackground();
            AudioManager.instance.Play("SimpleBGM");
        }
    }
}
