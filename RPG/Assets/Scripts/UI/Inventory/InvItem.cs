[System.Serializable]
public class InvItem
{
    public Item item;
    public int stackCount = 1;

    public InvItem(Item _item, int _stack = 1){
        item = _item;
        if (item.stackable)
            stackCount = _stack;
        else 
            stackCount = 1;
    }
}
