using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAI : MonoBehaviour, IDamageable
{
    public int hp;
    protected int maxHp;
    public bool facingRight = true;
    protected bool isAttacking = false;
    protected bool stunned = false;
    private float stunTime = 0;
    protected Rigidbody2D rb;
    protected Collider2D col;
    public float moveSpeed = 5;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    public virtual int TakeDamage(int damage, Vector2? knockback = null, DamageType damageType = DamageType.none){
        hp -= damage;
        if (knockback != null){
            Knockback(knockback ?? Vector2.zero);
        }
        if (hp <= 0){
            Die();
        }
        return damage;
    }
    protected void Knockback(Vector2 knockback, float duration = 0.3f){
        if (stunTime < duration)
            stunTime = duration;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockback*160);
    }
    protected virtual void Die(){
        Destroy(gameObject);
    }
    public virtual void Heal(int amount){
        hp = Mathf.Clamp(hp+amount,hp,maxHp);
    }
    protected virtual void Update() {
        if (stunTime>0){
            stunned = true;
            stunTime -= Time.deltaTime;
        } else {
            stunned = false;
        }

        if (facingRight){
            transform.localScale = new Vector2(1,1);
        } else {
            transform.localScale = new Vector2(-1,1);
        }
    }
    protected virtual bool IsPerformingAction(){
        if (isAttacking || stunned){
            return true;
        } else {
            return false;
        }
    }
    protected virtual GameObject FindNearestWithTag(string tag = "Player", float maxRange = Mathf.Infinity){
        GameObject[] _targets;
        _targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float sqrDistance = Mathf.Pow(maxRange,2);
        Vector3 position = transform.position;
        foreach (GameObject potentialTarget in _targets){
            float _sqrDistance = (potentialTarget.transform.position-position).sqrMagnitude;
            if (_sqrDistance < sqrDistance)
            {
                closest = potentialTarget;
                sqrDistance = _sqrDistance;
            }
        }
        return closest;
    }
}
