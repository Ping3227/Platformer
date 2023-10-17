
using UnityEngine;
using Platformer.Mechanics;

public class BossReflectCollision : MonoBehaviour
{
    Player player;
    [SerializeField] float damage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.player.gameObject.GetComponent<Player>();
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Debug.Log("Player hurt ");
            player.Hurt(damage);
        }
       
    }
}
