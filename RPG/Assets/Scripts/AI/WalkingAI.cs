using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAI : EnemyAI
{
    private Transform terrainDetection;
    private bool touchingFloor;
    [SerializeField] protected LayerMask platformLayerMask;
    protected override void Awake() {
        base.Awake();

        terrainDetection = transform.Find("terrainDetection");
        InvokeRepeating("UpdateTarget",1,1);
    }
    protected override void Update() {
        base.Update();

        //Face towards player
        if (target && !IsPerformingAction()){
            if (target.transform.position.x < rb.position.x){
                facingRight = false;
            } else if (target.transform.position.x > rb.position.x){
                facingRight = true;
            }
        }
    }
    private void FixedUpdate() {
        //Detect Terrain
        RaycastHit2D bottomRaycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size*0.9f, 0f, Vector2.down, 0.15f, platformLayerMask);
        RaycastHit2D groundInfo = Physics2D.Raycast(terrainDetection.position, Vector2.down, 0.5f, platformLayerMask);
        if (bottomRaycastHit.collider != null) touchingFloor = true;
        else touchingFloor = false;
        //Move
        if (!IsPerformingAction() && touchingFloor){
            if (groundInfo.collider == false) facingRight = !facingRight; //Turn Around when hit wall

            if (facingRight){
                rb.velocity = new Vector2(moveSpeed,rb.velocity.y);
            } else {
                rb.velocity = new Vector2(-moveSpeed,rb.velocity.y);
            }
        }
    }
}
