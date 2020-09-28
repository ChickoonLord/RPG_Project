using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment", order = 1)]
public class Equipment : Item
{
    public EquipmentType equipmentType;
    private void Awake() {
        type = ItemType.Equipment;
        stackable = false;
    }

    public enum EquipmentType{
        Helmet,
        Chestplate,
        Leggings,
        Misc
    }
}
