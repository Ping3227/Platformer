using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject {
    
    public Sprite icon;
    public GameObject Object;
    public string Name;
    public int counts;
}