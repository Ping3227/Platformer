using Platformer.Mechanics;
using UnityEngine;


public class PlayerHitBox : MonoBehaviour
{
    Player player;
    [SerializeField] float damage;
    [SerializeField] ParticleSystem hitEffect;
    [Header("Camera Shake")]
    [SerializeField] float ShakeAmp;
    [SerializeField] float ShakeFrequency;
    [SerializeField] float ShakeDuration;
    void Start()
    {
        player = GameController.player;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth)
            {
                other.GetComponent<BossHealth>().hurt(damage);
            }
            else { 
                other.GetComponent<EnemyHealth>().hurt(damage);
            }
            
            hitEffect.transform.position = other.transform.position;
            hitEffect.transform.position += ( player.gameObject.transform.position- other.gameObject.transform.position).normalized*2;
            CamearaController.Instance.ShakeCamera(ShakeAmp, ShakeFrequency, ShakeDuration,-1);
            AudioManager.instance.Play("PlayerAttackHit");
            hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, other.gameObject.transform.position- player.gameObject.transform.position);
            hitEffect.Play();
        }
        

    }

}
