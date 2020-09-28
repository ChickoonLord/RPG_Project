using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    [HideInInspector] public Weapon WeaponItem;
    [HideInInspector] public PlayerAI PlayerAI;
    protected Animator animator;
    protected virtual void Awake() {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start(){
        animator.SetFloat("AttackSpeed",1/WeaponItem.attackTime);
    }
    protected Vector2 SetAttackRotation(){
        float vertDir = Input.GetAxisRaw("Vertical");
        Quaternion attackDir = Quaternion.Euler(0,0,90*vertDir);
        if (!PlayerAI.facingRight){
            attackDir = Quaternion.Euler(0,0,-90*vertDir);
        }
        PlayerAI.attackRotation.rotation = attackDir;
        if (PlayerAI.facingRight) return new Vector2(1,vertDir);
        else return new Vector2(-1,vertDir);
    }
    public abstract IEnumerator Attack();
}
