using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : EntityAI
{
    //Movement variables;
    public float jumpForce = 10;
    private float inpJumpTime = 1;
    private float airTime = 1;
    public float wallJumpTime = 1;
    public float airControl = 3;
    public float wallJumpUpForce = 7;
    public float wallJumpSideForce = 4.5f;
    private bool touchingFloor;
    private bool touchingTop;
    private bool touchingRight;
    private bool touchingLeft;
    [SerializeField] protected LayerMask platformLayerMask;
    [SerializeField] protected LayerMask attackLayerMask;
    float iFrames = 0;
    float attackCooldown = 0;
    private float inpAttackTime = 1;
    public GameObject weaponPrefab;
    private Transform attackPos;
    private Transform attackRotation;
    public Controls controls;
    public static PlayerAI instance;
    protected override void Awake()
    {
        if (instance){
            Destroy(instance.gameObject);
        }
        instance = this;

        base.Awake();
        hp = 20;

        controls = new Controls();
        controls.Player.Jump.performed += ctx => JumpPressed();
        controls.Player.Attack.performed += ctx => AttackPressed();
        controls.Player.Interact.performed += ctx => InteractPressed();

        attackRotation = transform.Find("attackRotation");
        attackPos = attackRotation.Find("attackPos");
    }
    protected override void Update() {
        base.Update();

        if (attackCooldown > 0){
            attackCooldown -= Time.deltaTime;
        }
        if (iFrames > 0){
            iFrames -= Time.deltaTime;
        }
        if (inpJumpTime < 1) inpJumpTime += Time.deltaTime;
        if (inpAttackTime < 1) inpAttackTime += Time.deltaTime;
        if (attackCooldown <= 0 && inpAttackTime < 0.1){
            inpAttackTime = 1;
            attackCooldown = 0.5f;
            StartCoroutine("Attack");
        }
    }
    private void AttackPressed(){
        inpAttackTime = 0;
    }
    IEnumerator Attack(){
        isAttacking = true;
        #region Calculate/Set Attack Direction
        float vertDir = Input.GetAxisRaw("Vertical");
        Quaternion attackDir = Quaternion.Euler(0,0,90*vertDir);
        Vector2 knockback = new Vector2(2,1);
        if (vertDir > 0.5){
            knockback = new Vector2(knockback.y,knockback.x);
        } else if (vertDir < -0.5){
            knockback = new Vector2(-knockback.y,-knockback.x);
        }
        if (!facingRight){
            attackDir = Quaternion.Euler(0,0,-90*vertDir);
            knockback = new Vector2(-knockback.x,knockback.y);
        }
        attackRotation.rotation = attackDir;
        
        #endregion
        Instantiate(weaponPrefab,attackPos.position,attackDir,transform);
        yield return new WaitForSeconds(0.125f);
        #region Hit enemies in Hitbox
        Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(attackPos.position,0.7f,attackLayerMask);
        int enemiesHit = 0;
        foreach (Collider2D collider2D in objectsToDamage)
        {
            IDamageable iDamageable = collider2D.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null){
                iDamageable.TakeDamage(1,knockback,DamageType.slash);
                enemiesHit ++;
            }
        }
        if (enemiesHit > 0){
            Vector2 selfKnockback = new Vector2(-knockback.x,-knockback.y) * 0.4f;
            if (vertDir < -0.5) selfKnockback.Set(selfKnockback.x,selfKnockback.y*4);
            Knockback(selfKnockback,0.125f);
        }
        #endregion
        yield return new WaitForSeconds(0.125f);
        isAttacking = false;
    }
    public override int TakeDamage(int damage, Vector2? knockback = null, DamageType damageType = DamageType.none)
    {
        if (iFrames > 0){
            return -1;
        }
        iFrames = 0.6f;
        return base.TakeDamage(damage, knockback, damageType);
    }
    private void InteractPressed(){
        GameObject _nearestInteractable = FindNearestWithTag("Interactable");
        if (!_nearestInteractable)
            return;
        Interactable _interactable = _nearestInteractable.GetComponent<Interactable>();
        if (_interactable.playerInRadius){
            _interactable.Interact();
        }
    }
    void FixedUpdate(){
        #region Detect Collisions With Terrain
        //Detect if touching Terrain
        RaycastHit2D bottomRaycastHit = Physics2D.BoxCast(col.bounds.center, new Vector2(0.8f,0.8f), 0f, Vector2.down, 0.15f, platformLayerMask);
        RaycastHit2D topRaycastHit = Physics2D.BoxCast(col.bounds.center, new Vector2(0.8f,0.8f), 0f, Vector2.up, 0.15f, platformLayerMask);
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(col.bounds.center, new Vector2(0.8f,0.8f), 0f, Vector2.right, 0.15f, platformLayerMask);
        RaycastHit2D leftRaycastHit = Physics2D.BoxCast(col.bounds.center, new Vector2(0.8f,0.8f), 0f, Vector2.left, 0.15f, platformLayerMask);
        if (bottomRaycastHit.collider != null){
            touchingFloor = true;
            //Debug.Log("floor");
        } else {
            touchingFloor = false;
            //Debug.Log("no floor");
        } if (rightRaycastHit.collider != null){
            touchingRight = true;
            //Debug.Log("right");
        } else {
            touchingRight = false;
        } if (leftRaycastHit.collider != null){
            touchingLeft = true;
            //Debug.Log("left");
        } else {
            touchingLeft = false;
        } if (topRaycastHit.collider != null){
            touchingTop = true;
        } else {
            touchingTop = false;
        }
        //Set airTime based on If touching Floor
        if (!touchingFloor) {
            airTime += 1 * Time.deltaTime;
        } else {
            airTime = 0;
        }
        //Set wallJumpTime based of If touching Wall
        if (!touchingRight && !touchingLeft || touchingFloor) {
            wallJumpTime += 1 * Time.deltaTime;
        } else {
            wallJumpTime = 0;
        }
        #endregion
        #region Movement
        //Move
        float moveDir = Input.GetAxisRaw("Horizontal");
        if (!stunned){
            if (touchingFloor) {
                rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
            } else {
                if (moveDir > 0){ 
                    if (rb.velocity.x < moveSpeed) {
                        rb.velocity += new Vector2(moveSpeed * Time.deltaTime * airControl, 0);
                    }
                }
                else if (moveDir < 0){
                    if (rb.velocity.x > -moveSpeed) {
                        rb.velocity += new Vector2(-moveSpeed * Time.deltaTime * airControl, 0);
                    }
                }
            }
        }
        if (!IsPerformingAction()){
            if (moveDir > 0.1){
                facingRight = true;
            }else if (moveDir < -0.1){
                facingRight = false;
            }
        }
        #endregion
        //Jump
        if (!touchingTop && !stunned && rb.velocity.y <= 0.05 && inpJumpTime < 0.1){
            if (airTime < 0.1) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                inpJumpTime = 1;
            }
            else if (wallJumpTime < 0.05 && wallJumpUpForce > 0) {
                if (touchingLeft && rb.velocity.x <= 0){
                    rb.velocity = new Vector2(wallJumpSideForce, Mathf.Max(wallJumpUpForce, rb.velocity.y));
                }
                else if (touchingRight && rb.velocity.x >= 0) {
                    rb.velocity = new Vector2(wallJumpSideForce * -1, Mathf.Max(wallJumpUpForce, rb.velocity.y));
                }
            }
        }
    }
    private void JumpPressed(){
        inpJumpTime = 0;
    }
    private void OnEnable() {
        controls.Enable();
    }
    private void OnDisable() {
        controls.Disable();
    }
}
