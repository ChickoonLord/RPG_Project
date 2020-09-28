using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable/Blank", order = 3)]
public class Consumable : Item {
    [Range(0,1)] public float chanceToConsume = 1;
    private void Awake() {
        type = Item.ItemType.Consumable;
    }
    public virtual bool Use(){
        Debug.Log(name+" used!");
        return true;
    }
}
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Consumable/Bag", order = 4)]
public class ConsumableBag : Consumable {
    public ItemChance[] itemChances;
    public override bool Use()
    {
        foreach (ItemChance itemChance in itemChances)
        {
            InvManager.playerInventory.Add(itemChance.GetInvItem());
        }
        return base.Use();
    }
}
