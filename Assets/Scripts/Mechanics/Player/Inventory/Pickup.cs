using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    
    [SerializeField] Item item;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        { 
            Debug.Log("Pickup");
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
