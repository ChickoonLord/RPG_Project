using UnityEngine;
[System.Serializable]
public class InvItem
{
    public Item item;
    [SerializeField, Min(1)] private int _stackCount = 1; //The actual internal stack count

    public InvItem(Item _item, int _stack = 1){
        item = _item;
        stackCount = _stack;
    }
    public int stackCount{ //The Get Set version of stack count
        get{
            if (item.stackable) return Mathf.Max(_stackCount,1);
            else return 1;
        }
        set{
            if (item.stackable) _stackCount = value;
            else _stackCount = 1;
        }
    }
}
