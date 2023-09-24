using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<Item> items = new List<Item>();
    private Item CurrentItem ;
    void Awake()
    {
        Instance = this;
    }
    public void AddItem(Item item) {
        items.Add(item);
        if(CurrentItem == null)
        {
            CurrentItem = item;
        }
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        Destroy(item);
    }
    /// <summary>
    /// use current item
    /// </summary>
    public void UseItem() {
    }
    public void NextItem()
    {
        
    }
}
