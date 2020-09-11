using UnityEngine;
[RequireComponent(typeof(PlayerAI))]
public class BetterJump : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float wallSlideSpeed = 1;
    private Rigidbody2D rb;
    private PlayerAI playerAI;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerAI = GetComponent<PlayerAI>();
    }
    void Update() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if (rb.velocity.y > 0 && Input.GetAxisRaw("Jump") != 1) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (playerAI.wallJumpTime < 0.01){
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Max(rb.velocity.y,-wallSlideSpeed));
        }
    }
}
