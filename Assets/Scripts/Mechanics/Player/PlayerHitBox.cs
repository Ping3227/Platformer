using Platformer.Mechanics;
using UnityEngine;


public class PlayerHitBox : MonoBehaviour
{
    Player player;
    [SerializeField] float damage;
    [SerializeField] ParticleSystem hitEffect;
    void Start()
    {
        player = GameController.player;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){
            other.GetComponent<EnemyHealth>().hurt(damage);
            hitEffect.transform.position = other.ClosestPoint(transform.position);
            hitEffect.transform.position += ( player.gameObject.transform.position- other.gameObject.transform.position).normalized*2;

            hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, other.gameObject.transform.position- player.gameObject.transform.position);
            hitEffect.Play();
        }
        

    }

}
