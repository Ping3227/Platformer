using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmobolizeField : MonoBehaviour
{
    public void EndAnimation() {
        Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().Immobilized();
        }
    }
}
