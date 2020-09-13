using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : UITween
{
    public InvItem invItem = null;
    public Image icon;
    public TextMeshProUGUI stackDisplay;
    public void AddItem(InvItem _item){
        invItem = _item;
        icon.enabled = true;
        icon.sprite = invItem.item.icon;
        if (invItem.stackCount > 1){
            stackDisplay.enabled = true;
            stackDisplay.text = invItem.stackCount.ToString();
        }
    }
    public void ClearSlot(){
        invItem = null;
        icon.enabled = false;
        stackDisplay.enabled = false;
    }
}