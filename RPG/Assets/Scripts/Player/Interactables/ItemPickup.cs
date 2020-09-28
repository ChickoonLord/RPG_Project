using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public InvItem invItem;
    private SpriteRenderer spriteRenderer;
    protected void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (invItem.item) spriteRenderer.sprite = invItem.item.icon;
        else Destroy(gameObject);
    }
    public override void Interact(){
        bool pickupSuccessful = false;
        switch (invItem.item.type)
        {
            case Item.ItemType.Weapon:
                pickupSuccessful = AttemptToEquip(InvManager.weaponSlot,invItem);
                break;
            case Item.ItemType.Equipment:
                Equipment equipment = invItem.item as Equipment;
                if (equipment) pickupSuccessful = AttemptToEquip(InvManager.equipmentSlots[(int)equipment.equipmentType],invItem);
                break;
            default:
                pickupSuccessful = InvManager.playerInventory.Add(invItem);
                break;
        }
        if (pickupSuccessful){
            base.Interact();
            Destroy(gameObject);
        }
    }
    private void OnInspectorUpdate() {
        Debug.Log("InspectorUpdate");
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = invItem.item.icon;
    }
    private bool AttemptToEquip(Inventory attemtToAddInv, InvItem item){
        if (!attemtToAddInv.Add(item)) return InvManager.playerInventory.Add(item);
        else return true;
    }
}
