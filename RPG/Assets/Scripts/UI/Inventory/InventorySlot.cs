using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : UITween
{
    [HideInInspector] public Inventory parentInventory;
    [HideInInspector] public int slotIndex = 0;
    public InvItem invItem = null;
    [SerializeField] private Sprite defaultIcon = null;
    public Image icon;
    public TextMeshProUGUI stackDisplay;
    public void SlotClicked(){
        if (invItem != null){
            if (parentInventory == InvManager.inventory){
                if (InvManager.currentUIType == InvManager.UIType.Inventory){
                    switch (invItem.item.type){
                        case Item.ItemType.Consumable:
                            parentInventory.Consume(slotIndex);
                            break;
                        case Item.ItemType.Equipment:
                            Equipment equipment = (Equipment)invItem.item;
                            Inventory toInventory = InvManager.equipmentSlots[equipment.equipmentSlotIndex];
                            parentInventory.MoveItemToInventory(slotIndex, toInventory, 0);
                            break;
                        case Item.ItemType.Weapon:
                            parentInventory.MoveItemToInventory(slotIndex, InvManager.weaponSlot, 0);
                            break;
                        default:
                            break;
                    }
                }
            } else if (parentInventory == InvManager.weaponSlot){
                parentInventory.MoveItemToInventory(slotIndex, InvManager.inventory);
            } else {
                //If is equipment, check if in Equipment slot
                if (InvManager.currentUIType == InvManager.UIType.Inventory && invItem.item.type == Item.ItemType.Equipment){
                    Equipment equipment = (Equipment)invItem.item;
                    if (InvManager.equipmentSlots[equipment.equipmentSlotIndex] == parentInventory){
                        parentInventory.MoveItemToInventory(slotIndex, InvManager.inventory);
                        return;
                    }
                }
            }
        }
    }
    public void AddItem(InvItem _item){
        invItem = _item;
        icon.enabled = true;
        icon.sprite = invItem.item.icon;
        if (invItem.stackCount == 1){
            stackDisplay.enabled = false;
        } else {
            stackDisplay.enabled = true;
            stackDisplay.text = invItem.stackCount.ToString();
        }
    }
    public void ClearSlot(){
        invItem = null;
        if (defaultIcon) icon.sprite = defaultIcon;
        else icon.enabled = false;
        stackDisplay.enabled = false;
    }
}