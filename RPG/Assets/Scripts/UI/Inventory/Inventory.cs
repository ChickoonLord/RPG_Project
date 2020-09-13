using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int maxSize = 20;
    public List<InvItem> items = new List<InvItem>();

    public Inventory(int invSize = 20, List<InvItem> startingInv = null){
        maxSize = invSize;
        if (startingInv != null){
            items = startingInv;
        }
    }
    public bool Add(InvItem invItem, int replaceIndex = -1){
        if (invItem.item.stackable){
            InvItem _item = FindItem(invItem.item);
            if (_item == null){
                if (items.Count >= maxSize)
                    return false;
                if (replaceIndex != -1 && replaceIndex < items.Count) items[replaceIndex] = invItem;
                else items.Add(invItem);
            } else {
                _item.stackCount += invItem.stackCount;
            }
        } else {
            if (items.Count >= maxSize)
                return false;
            if (replaceIndex != -1 && replaceIndex < items.Count) items[replaceIndex] = invItem;
            else items.Add(invItem);
        }
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }
    public bool Remove(Item item, int amount = 1, bool mustBeExact = true){
        bool removeSuccessful;
        int itemIndex = FindItemIndex(item);
        if (itemIndex == -1){
            removeSuccessful = false;
        } else {
            if (items[itemIndex].stackCount > amount){
                items[itemIndex].stackCount -= amount;
                removeSuccessful = true;
            } else if (items[itemIndex].stackCount == amount) {
                items.RemoveAt(itemIndex);
                removeSuccessful = true;
            } else {
                if (mustBeExact){
                    removeSuccessful = false;
                } else {
                    items.RemoveAt(itemIndex);
                    removeSuccessful = true;
                }
            }
        }
        if (removeSuccessful && onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return removeSuccessful;
    }
    #region Functions to find items in inventory
    public bool ItemExists(Item item){
        return items.Exists(x => x.item == item);
    }
    public bool ItemExists(InvItem invItem){
        return ItemExists(invItem.item);
    }
    public int FindItemIndex(Item item){
        return items.FindIndex(x => x.item == item);
    }
    public int FindItemIndex(InvItem invItem){
        return FindItemIndex(invItem.item);
    }
    public InvItem FindItem(Item item){
        int itemIndex = FindItemIndex(item);
        if (itemIndex == -1){
            return null;
        } else {
            return items[itemIndex];
        }
    }
    public InvItem FindItemInInventory(InvItem invItem){
        return FindItem(invItem.item);
    }
    #endregion
}
