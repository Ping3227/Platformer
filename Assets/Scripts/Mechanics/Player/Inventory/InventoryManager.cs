using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] List<Item> items = new List<Item>();
    private int CurrentIndex=-1 ;
    [SerializeField] Transform itemSlot;
    [HideInInspector] public Item currentItem;
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
        CurrentIndex= items.Count >0?0:-1;
        if (CurrentIndex != -1) {
            UpdateItem();
        }
        

    }
    
    public void AddItem(Item item) {
        if (!items.Contains(item)) {
            Debug.Log("AddItem");
            items.Add(item);
            if (CurrentIndex == -1)
            {
                CurrentIndex = 0;
                itemSlot.Find("Image").GetComponent<Image>().color = new Vector4(255, 255, 255, 255);
                itemSlot.Find("Image").GetComponent<Image>().sprite = item.icon;
                items[CurrentIndex].counts=1;
                
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
           
        }
        else if(items.Count == 1)
        {
            CurrentIndex = 0;
        }
        else
        {
            CurrentIndex = -1;
        }
        UpdateItem();
    }
   
    public void Clear() {
        items.Clear();
        CurrentIndex = -1;
        UpdateItem();
    }
    public bool UsedItem(Item item) {
        if (items.Contains(item)) {
            if (item.counts >1)
            {
                item.counts--;
                UpdateItem();
                return true;
            }
            if (item.counts == 1) { 
                item.counts--;
                if (item.Isdelpletable)
                {
                    items.Remove(item);
                    if (item == currentItem) {
                        NextItem();
                    }
                }
                UpdateItem();
                return true;
            }
            
            
        }
        return false;
    }
    public bool Contain(Item item)
    {
        return items.Contains(item);
    }
    private void UpdateItem() {
        if (CurrentIndex != -1)
        {
            currentItem = items[CurrentIndex];
            itemSlot.Find("Image").GetComponent<Image>().color = new Vector4(255, 255, 255, 255);
            itemSlot.Find("Image").GetComponent<Image>().sprite = items[CurrentIndex].icon;
            itemSlot.Find("Number").GetComponent<TMP_Text>().text = items[CurrentIndex].counts.ToString();
            
        }
        else {
            itemSlot.Find("Image").GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
            itemSlot.Find("Image").GetComponent<Image>().sprite = null;
            itemSlot.Find("Number").GetComponent<TMP_Text>().text = "";
            //currentItem = null;
        }
        
    }
}
