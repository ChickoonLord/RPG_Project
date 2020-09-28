using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    public static InvManager instance;
    public static Inventory playerInventory;
    public static Inventory weaponSlot;
    public static Inventory[] equipmentSlots;
    public InventoryUI inventoryUI;
    public InventoryUI weaponSlotUI;
    public InventoryUI[] equipmentSlotUI;
    public static Inventory currentOpenInventory{
        get{
            if (!GameManager.currentlyOpenUI) return null;
            InventoryUI inventoryUI = (InventoryUI)GameManager.currentlyOpenUI;
            if (inventoryUI) return inventoryUI.inventory;
            else return null;
        }
    }
    private void Awake() {
        if (instance){
            Destroy(gameObject);
            return;
        }
        instance = this;

        playerInventory = new Inventory(20, Inventory.InvType.PlayerInv);
        inventoryUI.Initialize(playerInventory);

        weaponSlot = new Inventory(1, Inventory.InvType.WeaponSlot);
        weaponSlotUI.Initialize(weaponSlot);

        equipmentSlots = new Inventory[equipmentSlotUI.Length];
        for (int i = 0; i < equipmentSlotUI.Length; i++){
            equipmentSlots[i] = new Inventory(1, Inventory.InvType.EquipmentSlot);
            equipmentSlotUI[i].Initialize(equipmentSlots[i]);
        }
    }
}
