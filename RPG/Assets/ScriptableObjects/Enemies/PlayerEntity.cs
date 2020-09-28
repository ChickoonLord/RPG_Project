using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu( fileName = "Entity", menuName = "Entities/Player Entity", order = 2)]
public class PlayerEntity : Entity
{
    public float jumpForce = 10;
    public float airControl = 3;
    public float wallJumpUpForce = 0;
    public float wallJumpSideForce = 8;
    
}
