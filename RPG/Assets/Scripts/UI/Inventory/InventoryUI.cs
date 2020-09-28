using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UITween
{
    public Inventory inventory = null;
    public Transform slotsParent;
    private InventorySlot[] invSlots;

    public virtual void Initialize(Inventory _inventory){
        invSlots = slotsParent.GetComponentsInChildren<InventorySlot>();
        inventory = _inventory;
        for (int i = 0; i < invSlots.Length; i++){
            invSlots[i].parentInventory = inventory;
            invSlots[i].slotIndex = i;
        }
        if (enabled) OnEnable();
    }
    protected virtual void UpdateUI(){
        //Debug.Log("Update GUI");
        for (int i = 0; i < invSlots.Length; i++) {
            if (i < inventory.items.Count){
                invSlots[i].AddItem(inventory.items[i]);
            } else {
                invSlots[i].ClearSlot();
            }
        }
    }
    protected void OnEnable() {
        if (inventory == null) return;

        inventory.onItemChangedCallback += UpdateUI;
        UpdateUI();
    }
    private void OnDisable() {
        if (inventory == null) return;

        inventory.onItemChangedCallback -= UpdateUI;
    }
}