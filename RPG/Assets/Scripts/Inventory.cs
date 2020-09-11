using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int size = 20;
    public List<Item> items = new List<Item>();
    public bool Add(Item item){
        if (items.Count >= size){
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }
    public bool Remove(Item item){
        bool removeSuccessful = items.Remove(item);
        if (removeSuccessful && onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return removeSuccessful;
    }
}
