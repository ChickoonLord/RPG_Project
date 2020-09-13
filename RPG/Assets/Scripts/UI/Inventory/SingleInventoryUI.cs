using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InventorySlot))]
public class SingleInventoryUI : InventoryUI
{
    private InventorySlot invSlot;
    public override void Initialize(Inventory _inventory){
        invSlot = GetComponent<InventorySlot>();
        inventory = _inventory;
        if (enabled) OnEnable();
    }
    protected override void UpdateUI()
    {
        if (inventory.items.Count > 0)
            invSlot.AddItem(inventory.items[0]);
        else
            invSlot.ClearSlot();
    }
}
