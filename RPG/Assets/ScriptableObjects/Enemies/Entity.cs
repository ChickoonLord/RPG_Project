using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( fileName = "Entity", menuName = "Entities/Basic Entity", order = 1)]
public class Entity : ScriptableObject
{
    public int maxHp = 10;
    public float speed = 2;
}
