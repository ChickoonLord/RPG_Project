using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : EntityAI
{
    #region PlayerEntity Variables
    [SerializeField] private PlayerEntity playerEntity = null;
    private float jumpForce;
    private float airControl;
    private float wallJumpUpForce;
    private float wallJumpSideForce;
    #endregion
    #region Touching Walls
    private float airTime = 1;
    [HideInInspector] public float wallJumpTime = 1;
    private bool touchingFloor;
    private bool touchingTop;
    private bool touchingRight;
    private bool touchingLeft;
    [SerializeField] protected LayerMask platformLayerMask;
    #endregion
    float iFrames = 0;
    float attackCooldown = 0;
    private float inpAttackTime = 1;
    private float inpJumpTime = 1;
    [SerializeField] private GameObject weaponObject = null;
    private WeaponBehavior weaponBehavior = null;
    [HideInInspector] public Transform attackPos;
    [HideInInspector] public Transform attackRotation;
    private Controls controls;
    public static PlayerAI instance;
    protected override void Awake()
    {
        if (instance){
            Destroy(instance.gameObject);
        }
        instance = this;

        base.Awake();
        hp = playerEntity.maxHp;
        moveSpeed = playerEntity.speed;
        jumpForce = playerEntity.jumpForce;
        airControl = playerEntity.airControl;
        wallJumpUpForce = playerEntity.wallJumpUpForce;
        if (wallJumpUpForce > 0) wallJumpSideForce = playerEntity.wallJumpSideForce;

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

        //Animations for touching walls
        if (rb.velocity.y <= -2){
            if (facingRight){
                animator.SetBool("SlidingRightWall",touchingRight);
                if (!touchingRight) animator.SetBool("SlidingLeftWall",touchingLeft);
                else animator.SetBool("SlidingLeftWall",false);
            } else {
                animator.SetBool("SlidingRightWall",touchingLeft);
                if (!touchingRight) animator.SetBool("SlidingLeftWall",touchingRight);
                else animator.SetBool("SlidingLeftWall",false);
            }
        } else {
            animator.SetBool("SlidingRightWall",false);
            animator.SetBool("SlidingLeftWall",false);
        }
        
        if (inpJumpTime < 1) inpJumpTime += Time.deltaTime;
        if (inpAttackTime < 1) inpAttackTime += Time.deltaTime;

        //Trigger Attack
        if (attackCooldown <= 0 && inpAttackTime < 0.1){
            if (weaponBehavior){
                weaponBehavior.StartCoroutine("Attack");
                inpAttackTime = 1;
                attackCooldown = weaponBehavior.WeaponItem.cooldown;
            }
        }
    }
    private void WeaponChanged(){
        Debug.Log("Weapon Changed");
        if (weaponObject) Destroy(weaponObject);
        if (!InvManager.weaponSlot.IsFull()){
            weaponBehavior = null;
            return;
        }
        Weapon weaponItem = InvManager.weaponSlot.items[0].item as Weapon;
        weaponObject = Instantiate(weaponItem.weaponPrefab,attackRotation.position,Quaternion.identity,attackRotation);
        weaponBehavior = weaponObject.GetComponent<WeaponBehavior>();
        weaponBehavior.WeaponItem = weaponItem;
        weaponBehavior.PlayerAI = this;
    }
    private void AttackPressed(){
        if (GameManager.paused) return;
        inpAttackTime = 0;
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
        RaycastHit2D bottomRaycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size*0.9f, 0f, Vector2.down, 0.15f, platformLayerMask);
        RaycastHit2D topRaycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size*0.9f, 0f, Vector2.up, 0.15f, platformLayerMask);
        RaycastHit2D rightRaycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size*0.9f, 0f, Vector2.right, 0.15f, platformLayerMask);
        RaycastHit2D leftRaycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size*0.9f, 0f, Vector2.left, 0.15f, platformLayerMask);
        touchingFloor = (bottomRaycastHit.collider != null);
        touchingRight = (rightRaycastHit.collider != null);
        touchingLeft = (leftRaycastHit.collider != null);
        touchingTop = (topRaycastHit.collider != null);
        #endregion
        #region Movement
        //Move Animations
        animator.SetFloat("MoveSpeed",Mathf.Abs(rb.velocity.x));
        animator.SetBool("InAir",!touchingFloor);
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
        if (GameManager.paused) return;
        inpJumpTime = 0;
    }
    private void OnEnable() {
        controls.Enable();
        InvManager.weaponSlot.onItemChangedCallback += WeaponChanged;
    }
    private void OnDisable() {
        controls.Disable();
        InvManager.weaponSlot.onItemChangedCallback -= WeaponChanged;
    }
}
