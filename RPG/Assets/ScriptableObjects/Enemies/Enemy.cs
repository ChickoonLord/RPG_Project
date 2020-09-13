using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( fileName = "Enemy", menuName = "Entities/Enemy", order = 0)]
public class Enemy : Entity
{
    public ItemChance[] dropChances;
}
