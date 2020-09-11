using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon = null;
    public int value = 1;

    public virtual void Use(){
        Debug.Log(name+" used!");
    }
}