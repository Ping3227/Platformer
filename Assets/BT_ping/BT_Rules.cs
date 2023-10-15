
using UnityEngine;
using Panda;
using System.Collections.Generic;

public class BT_Rules : MonoBehaviour
{
    [SerializeField] Animator animator;
    private List<Vector2> moveOptions = new List<Vector2>();
    [Task]
    public bool IsImmobilized =false;
    [Task]
    public bool Far = true;
    [Task]
    public bool IsInfront = true;
    [Task]
    public bool IsBlocked = false;
    [Task]
    void Teleport() {
        //animator.SetTrigger("Teleport");
        moveOptions.Clear();
        ThisTask.Succeed();
    }
    [Task]
    void Attack() {
        
    }
    [Task]
    void Immobilize() {
    }
    [Task]
    void BackAttack()
    {

    }
    [Task]
    void FastTeleport() { 
    }
    [Task]
    void FastAttack() { 
    }
    [Task]
    void Blocking() { 
    }
    [Task]
    void counterattack() { 
    }
    [Task]
    void TeleportBack()
    {
    }
}
