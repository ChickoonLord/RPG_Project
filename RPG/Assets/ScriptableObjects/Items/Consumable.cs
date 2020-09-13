using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable", order = 1)]
public class Consumable : Item
{
    private void Awake() {
        type = ItemType.Consumable;
    }
}
