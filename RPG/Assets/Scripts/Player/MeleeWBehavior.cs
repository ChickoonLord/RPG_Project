using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWBehavior : WeaponBehavior
{
    [SerializeField] protected LayerMask attackLayerMask;
    public MeeleWeapon weaponItem;
    protected override void Start() {
        base.Start();
        weaponItem = WeaponItem as MeeleWeapon;
        if (!weaponItem){
            Destroy(gameObject);
            Debug.LogWarning("Incompatible Weapon Type!");
        }
    }
    public override IEnumerator Attack(){
        if (PlayerAI.isAttacking) yield break;
        PlayerAI.isAttacking = true;
        animator.SetTrigger("Attack");
        #region Calculate/Set Attack Direction
        Vector2 attackDir = SetAttackRotation();
        Vector2 knockback = weaponItem.knockback;
        if (attackDir.y > 0.5) knockback.Set(knockback.y,knockback.x);
        else if (attackDir.y < -0.5) knockback.Set(knockback.y,-knockback.x);
        if (attackDir.x != 1) knockback.Set(-knockback.x,knockback.y);
        #endregion
        yield return new WaitForSeconds(weaponItem.attackTime/2);
        #region Hit enemies in Hitbox
        Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(PlayerAI.attackPos.position,weaponItem.range,attackLayerMask);
        int enemiesHit = 0;
        foreach (Collider2D collider2D in objectsToDamage)
        {
            IDamageable iDamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null){
                iDamageable.TakeDamage(weaponItem.damage,knockback,weaponItem.damageType);
                enemiesHit ++;
            }
        }
        if (enemiesHit > 0){
            Vector2 selfKnockback = new Vector2(-knockback.x,-knockback.y) * 0.4f;
            if (attackDir.y < -0.5) selfKnockback.Set(selfKnockback.x,selfKnockback.y*4);
            PlayerAI.Knockback(selfKnockback,0.125f);
        }
        #endregion
        yield return new WaitForSeconds(weaponItem.attackTime/2);
        PlayerAI.isAttacking = false;
        PlayerAI.attackRotation.rotation = Quaternion.identity;
    }
    private void OnDrawGizmosSelected() {
        if (PlayerAI.isAttacking) UnityEditor.Handles.color = Color.red;
        else UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(PlayerAI.attackPos.position,Vector3.back,weaponItem.range);
    }
}
