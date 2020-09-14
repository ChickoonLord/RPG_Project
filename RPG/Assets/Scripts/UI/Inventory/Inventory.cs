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

    public Inventory(int invSize = 20, IList<InvItem> startingInv = null){
        maxSize = invSize;
        if (startingInv != null){
            items = (List<InvItem>)startingInv;
        }
    }
    #region Add, Remove, Move, and Consume Item functions
    public bool Add(InvItem invItem, int replaceIndex = -1){
        if (invItem.item.stackable){
            InvItem _item = FindItem(invItem.item);
            if (_item == null){
                if (items.Count >= maxSize)
                    return false;
                if (replaceIndex >= 0 && replaceIndex < items.Count) items[replaceIndex] = invItem;
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
    public bool Remove(int index, int amount = 1, bool mustBeExact = true){
        bool removeSuccessful;
        if (items[index].stackCount > amount){
            items[index].stackCount -= amount;
            removeSuccessful = true;
        } else if (items[index].stackCount == amount) {
            items.RemoveAt(index);
            removeSuccessful = true;
        } else {
            if (mustBeExact){
                removeSuccessful = false;
            } else {
                items.RemoveAt(index);
                removeSuccessful = true;
            }
        }
        if (removeSuccessful && onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return removeSuccessful;
    }
    public bool Remove(Item item, int amount = 1, bool mustBeExact = true){
        int itemIndex = FindItemIndex(item);
        if (itemIndex == -1) return false;
        return Remove(itemIndex,amount,mustBeExact);
    }
    public bool MoveItemToInventory(int fromIndex, Inventory toInventory, int replaceIndex = -1){
        bool moveSuccessful;
        InvItem itemBeingSwapped = null;
        if (toInventory.items.Count >= toInventory.maxSize){
            if (replaceIndex < 0 || replaceIndex >= toInventory.items.Count) return false;
            itemBeingSwapped = toInventory.items[replaceIndex];
            moveSuccessful = toInventory.Add(items[fromIndex],replaceIndex);
        } else {
            moveSuccessful = toInventory.Add(items[fromIndex]);
        }
        if (!moveSuccessful) return false;
        if (itemBeingSwapped == null){
            items.RemoveAt(fromIndex);
            if (moveSuccessful && onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        } else {
            Add(itemBeingSwapped,fromIndex);
        }
        return moveSuccessful;
    }
    public bool MoveItemToInventory(Item item, Inventory toInventory, int replaceIndex = -1){
        return MoveItemToInventory(FindItemIndex(item),toInventory,replaceIndex);
    }
    public bool Consume(Consumable item){
        return Consume(FindItemIndex(item));
    }
    public bool Consume(int index){
        Consumable item = (Consumable)items[index].item;
        if (item == null) return false;
        bool consumeSuccessful = true;;
        if (Random.value <= item.chanceToConsume && item.chanceToConsume != 0){
            consumeSuccessful = Remove(index);
        }
        if (consumeSuccessful) consumeSuccessful = item.Use();
        return consumeSuccessful;
    }
    #endregion
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