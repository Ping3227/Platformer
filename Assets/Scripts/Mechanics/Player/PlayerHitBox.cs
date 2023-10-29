using UnityEngine;


public class PlayerHitBox : MonoBehaviour
{
    
    [SerializeField] float damage;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"hit enemy : {damage}");
        }

    }

}
