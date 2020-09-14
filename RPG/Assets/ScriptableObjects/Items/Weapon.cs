using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 2)]
public class Weapon : Item
{
    private void Awake() {
        type = ItemType.Weapon;
        stackable = false;
    }
}
