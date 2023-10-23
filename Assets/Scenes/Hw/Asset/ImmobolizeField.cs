using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobolizeField : MonoBehaviour
{
    [SerializeField] BT_Rules Enemy;
    public void EndAnimation() {
        gameObject.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().Immobilized();
            Enemy.IsImmobilized = true;
        }
    }
}
