using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Simple", order = 0)]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon = null;
    [TextArea(2,10), Multiline] public string description;
    public int value = 1;
    public Rarity rarity = Rarity.Common;
    [HideInInspector] public ItemType type = ItemType.Simple;
    public bool stackable = true;


    public enum ItemType{
        Simple,
        Consumable,
        Equipment,
        Weapon
    }
    public enum Rarity{
        Common,
        Uncommon,
        Rare
    }
}