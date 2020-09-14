using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 1)]
public class Equipment : Item
{
    public int equipmentSlotIndex;
    private void Awake() {
        type = ItemType.Equipment;
        stackable = false;
    }
}
