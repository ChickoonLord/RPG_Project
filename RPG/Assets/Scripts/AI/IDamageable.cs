using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType {
        none,
        slash,
        explosion,
        fire
    }
public interface IDamageable
{
    int TakeDamage(int damage, Vector2? knockback = null, DamageType damageType = DamageType.none);
}
