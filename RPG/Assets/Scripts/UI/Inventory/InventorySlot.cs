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
        if (invItem == null || InvManager.currentOpenInventory == null) return;
        switch (parentInventory.type){//Do different things depending on what InvType it's in
            case Inventory.InvType.PlayerInv:
                if (InvManager.currentOpenInventory.type == Inventory.InvType.PlayerInv){
                    switch (invItem.item.type){//Depending on item type, either equip or consume
                        case Item.ItemType.Consumable:
                            parentInventory.Consume(slotIndex);
                            break;
                        case Item.ItemType.Equipment:
                            Equipment equipment = (Equipment)invItem.item;
                            Inventory toInventory = InvManager.equipmentSlots[(int)equipment.equipmentType];
                            parentInventory.MoveItemToInventory(slotIndex, toInventory, 0);
                            break;
                        case Item.ItemType.Weapon:
                            parentInventory.MoveItemToInventory(slotIndex, InvManager.weaponSlot, 0);
                            break;
                        default:
                            break;
                    }
                } else {
                    parentInventory.MoveItemToInventory(slotIndex, InvManager.currentOpenInventory);
                }
                break;
            case Inventory.InvType.WeaponSlot:
                parentInventory.MoveItemToInventory(slotIndex, InvManager.playerInventory);
                break;
            case Inventory.InvType.EquipmentSlot:
                parentInventory.MoveItemToInventory(slotIndex, InvManager.playerInventory);
                break;
            default:
                parentInventory.MoveItemToInventory(slotIndex, InvManager.playerInventory);
                break;
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