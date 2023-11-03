using Platformer.Mechanics;
using UnityEngine;


public class PlayerHitBox : MonoBehaviour
{
    Player player;
    [SerializeField] float damage;
    void Start()
    {
        player = GameController.player;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){
            other.GetComponent<EnemyHealth>().hurt(damage);
        }
        

    }

}
