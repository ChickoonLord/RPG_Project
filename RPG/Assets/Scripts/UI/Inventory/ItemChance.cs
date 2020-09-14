using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemChance
{
    public Item[] possibleItems;
    [Range(0,1)] public float chance = 1;
    public int amount = 1;
    public int optionalMaxAmount = 0;
    public InvItem GetInvItem(){
        if (Random.value <= chance || possibleItems.Length <= 0){
            return null;
        }
        int itemID = Random.Range(0,possibleItems.Length);
        if (optionalMaxAmount > amount){
            return new InvItem(possibleItems[itemID],Random.Range(amount,optionalMaxAmount+1));
        } else {
            return new InvItem(possibleItems[itemID],amount);
        }
    }
    public ItemChance(IList<Item> _possibleItems, float _chance = 1, int _amount = 1, int _optionalMaxAmount = 0){
        possibleItems = (Item[])_possibleItems;
        chance = _chance;
        amount = _amount;
        optionalMaxAmount = _optionalMaxAmount;
    }
    public ItemChance(Item _possibleItem, float _chance = 1, int _amount = 1, int _optionalMaxAmount = 0){
        Item[] possibleItems = new Item[1];
        possibleItems[0] = _possibleItem;
        chance = _chance;
        amount = _amount;
        optionalMaxAmount = _optionalMaxAmount;
    }
}
