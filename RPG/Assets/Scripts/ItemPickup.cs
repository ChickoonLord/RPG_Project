using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public InvItem invItem;
    private SpriteRenderer spriteRenderer;
    protected void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = invItem.item.icon;
    }
    public override void Interact()
    {
        bool pickupSuccessful = InvManager.inventory.Add(invItem);
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
}
