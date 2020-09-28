using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : EntityAI
{
    public Enemy enemy;
    protected float detectionRange = 0;
    protected GameObject target = null;
    protected override void Awake() {
        base.Awake();
        
        hp = enemy.maxHp;
        moveSpeed = enemy.speed;
    }
    protected virtual void UpdateTarget(){
        if (detectionRange > 0){
            target = FindNearestWithTag("Player", detectionRange);
        }
    }
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            float collisionDistance = other.transform.position.x - transform.position.x;
            Vector2 knockback = new Vector2(2,1);
            if (collisionDistance > 0.1)
                knockback.Set(knockback.x,knockback.y);
            else if (collisionDistance < -0.1)
                knockback.Set(-knockback.x,knockback.y);
            else 
                knockback.Set(0,knockback.x+knockback.y);
            other.gameObject.GetComponent<IDamageable>().TakeDamage(1, knockback);
        }
    }
}
