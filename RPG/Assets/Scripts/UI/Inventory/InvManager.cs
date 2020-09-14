using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    public static InvManager instance;
    public static Inventory inventory;
    public static Inventory weaponSlot;
    public static Inventory[] equipmentSlots;
    public InventoryUI inventoryUI;
    public InventoryUI weaponSlotUI;
    public InventoryUI[] equipmentSlotUI;
    public static UIType currentUIType = UIType.Inventory;
    public static Inventory currentOpenInventory;
    private void Awake() {
        if (instance){
            Destroy(gameObject);
            return;
        }
        instance = this;

        inventory = new Inventory(20);
        inventoryUI.Initialize(inventory);

        weaponSlot = new Inventory(1);
        weaponSlotUI.Initialize(weaponSlot);

        equipmentSlots = new Inventory[equipmentSlotUI.Length];
        for (int i = 0; i < equipmentSlotUI.Length; i++){
            equipmentSlots[i] = new Inventory(1);
            equipmentSlotUI[i].Initialize(equipmentSlots[i]);
        }
    }

    public enum UIType{
        None,
        Inventory,
        Container,
        Shop
    }
}
