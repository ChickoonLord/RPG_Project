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
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, knockback);
            else if (collisionDistance < -0.1)
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, new Vector2(-knockback.x,knockback.y));
            else 
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, new Vector2(0,knockback.x+knockback.y));
        }
    }
}
