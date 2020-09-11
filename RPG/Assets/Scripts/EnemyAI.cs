using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : EntityAI
{
    protected float detectionRange = 0;
    protected GameObject target = null;
    protected virtual void UpdateTarget(){
        if (detectionRange > 0){
            target = FindPlayer();
        }
    }
    protected GameObject FindPlayer(){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = detectionRange;
        Vector3 position = transform.position;
        foreach (GameObject potentialTarget in players){
            float _distance = Vector2.Distance(potentialTarget.transform.position, position);
            if (_distance < distance)
            {
                closest = potentialTarget;
                distance = _distance;
            }
        }
        return closest;
    }
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player"){
            float collisionDistance = other.transform.position.x - transform.position.x;
            if (collisionDistance > 0.2)
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, new Vector2(2,1));
            else if (collisionDistance < -0.2)
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, new Vector2(-2,1));
            else 
                other.gameObject.GetComponent<IDamageable>().TakeDamage(1, new Vector2(0,3f));
        }
    }
}
