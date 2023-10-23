using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<Item> items = new List<Item>();
    private int CurrentIndex=-1 ;
    [SerializeField] Transform itemSlot;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            
        }
        
    }
    public void AddItem(Item item) {
        if (!items.Contains(item)) {
            Debug.Log("AddItem");
            items.Add(item);
            if (CurrentIndex == -1)
            {
                CurrentIndex = 0;
                itemSlot.Find("Image").GetComponent<Image>().sprite = item.icon;
                items[CurrentIndex].counts=1;
                itemSlot.Find("Number").GetComponent<TMP_Text>().text = items[CurrentIndex].counts.ToString();
            }
        }
        else
        {
            items.Find(x => x == item).counts++;
            if (CurrentIndex == items.IndexOf(item))
            {
                itemSlot.Find("Number").GetComponent<TMP_Text>().text = items[CurrentIndex].counts.ToString();
            }
        }
        
        
    }
    public void NextItem()
    {
        if (items.Count >= 2) {
            
            CurrentIndex= (CurrentIndex == items.Count - 1)? 0 : ++CurrentIndex;
            itemSlot.Find("Image").GetComponent<Image>().sprite = items[CurrentIndex].icon;
            itemSlot.Find("Number").GetComponent<TMP_Text>().text = items[CurrentIndex].counts.ToString();
        }
    }
    public bool Contain(Item item) {
        return items.Contains(item);
    }
}
