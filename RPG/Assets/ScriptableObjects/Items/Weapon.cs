using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapons/Basic", order = 2)]
public class Weapon : Item
{
    public GameObject weaponPrefab;
    public int damage = 1;
    public float attackTime = 0.3f;
    public float cooldown = 0.6f;
    public Vector2 knockback = new Vector2(2,1);
    public DamageType damageType = DamageType.slash;
    private void Awake() {
        type = ItemType.Weapon;
        stackable = false;
    }
}
