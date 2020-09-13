using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType{
    Default,
    Consumable,
    Equipment,
    Weapon
}


[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon = null;
    [TextArea(2,10)]
    public string description;
    public int value = 1;
    public ItemType type = ItemType.Default;
    public bool stackable = true;

    public virtual void Use(){
        Debug.Log(name+" used!");
    }
}