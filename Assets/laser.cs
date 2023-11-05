using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class laser : MonoBehaviour
{
    RaycastHit2D hit;
    Vector3 LastDirection;
    Player player;
    [Header("Attack")]
    [SerializeField] LineRenderer line;
    [SerializeField] float StartDistance = 0.5f;
    [SerializeField] float HitDistance= 0.5f;
    [SerializeField] LayerMask whichToHit;
    [SerializeField] LineRenderer shootLine;
    private float shootProgress = 0f;
    [SerializeField] float AttackRange = 5f;
    [SerializeField] float AimInterval = 0.5f;
    [SerializeField] float LockInterval = 1.5f;
    [SerializeField] float AttackInterval = 1.7f;
    [SerializeField] float AttackDamage = 3f;
    private float AttackCounter = 0f;
    void Start()
    {
        player = GameController.player;
        
    }
    void Update(){

        if (AttackCounter < AimInterval ){
            if (Vector2.Distance(player.transform.position, transform.position) < AttackRange) Aim();
            else { 
                AttackCounter = 0f;
                line.enabled = false;
            }
        }
        else if (AttackCounter >= AimInterval && AttackCounter < LockInterval){
            Locking();
            Debug.Log("Locking");
        }
        else if(AttackCounter>=LockInterval){
            shoot();
        }
        

    }
    void shoot() {
        line.enabled = false;
        shootLine.enabled = true;
        AttackCounter += Time.deltaTime;
        shootProgress += Time.deltaTime / (AttackInterval-LockInterval);
        hit = Physics2D.Raycast(transform.position, LastDirection, AttackRange*shootProgress, whichToHit);
        shootLine.SetPosition(0, transform.position + LastDirection.normalized * StartDistance);
        if (hit.collider != null){
            shootLine.SetPosition(1, hit.point);
            AttackCounter = 0f;
            shootProgress = 0f;
            shootLine.enabled = false;
            if (hit.collider.CompareTag("Player")){
                
                hit.collider.GetComponent<Health>().Hurt(AttackDamage);
            }
            
        }
        else {
                shootLine.SetPosition(1, transform.position + LastDirection.normalized * shootProgress * AttackRange);
        }
        if (AttackCounter >= AttackInterval){
            AttackCounter = 0f;
            shootProgress = 0f;
            shootLine.enabled = false;
        }

     }
    void Aim() {
        hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, AttackRange, whichToHit);
        if (hit.collider.CompareTag("Player"))
        {
            AttackCounter += Time.deltaTime;
            LastDirection = player.transform.position - transform.position;
            line.SetPosition(0, transform.position + LastDirection.normalized * StartDistance);
            line.SetPosition(1, hit.point);
            line.enabled = true;
            Debug.Log("Aiming");
        }
        else
        {
            line.enabled = false;
            AttackCounter = 0f;
            Debug.Log("Not Aiming");
        }
    }
    void Locking() {
        AttackCounter += Time.deltaTime;
        hit = Physics2D.Raycast(transform.position, LastDirection, AttackRange , whichToHit);
        if (hit.collider != null){
            line.SetPosition(1, hit.point);
        }
        else{
           line.SetPosition(1, transform.position + LastDirection.normalized* AttackRange);
        }
    }
}
