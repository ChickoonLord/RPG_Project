using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 2)]
public class Equipment : Item
{
    private void Awake() {
        type = ItemType.Equipment;
        stackable = false;
    }
}
