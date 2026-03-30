using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpforce = 5f;

    [Header("Attack")]
    [SerializeField] CapsuleCollider2D swordHitbox;
    [SerializeField] float attackDuration = 0.2f;
    [SerializeField] float attackCoolDown = 1f;

    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;

    bool isAttacking = false;
    bool playerHasHorizontalSpeed;
    bool isAlive = true;
    float lastAttackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        Walk();
        FlipSprite();
        PlayerDeath();
        CheckGround();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void Walk()
    {
        Vector2 playerVelosity = new Vector2(moveInput.x * moveSpeed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = playerVelosity;

        animator.SetBool("IsWalking", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        /*if player isnt touching ground then skip below*/

        if (value.isPressed)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpforce);
        }
        
    }

    void CheckGround()
    {
        bool isGrounded = capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        animator.SetBool("Jump", !isGrounded);
    }

    public void PlayerDeath()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enimeies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Death");
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            FindAnyObjectByType<GameSession>().processPlayerDeath();
            enabled = false;
         
        }
    }
    void Attack()
    {
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        isAttacking = true;
        swordHitbox.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        swordHitbox.enabled = false;
        isAttacking = false;
    }


    void OnAttack()
    {
        if (!isAlive) return;
        if (Time.time < lastAttackTime + attackCoolDown) return;
        lastAttackTime = Time.time;
        Attack();
        animator.SetTrigger("Attack");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Health enemy = other.GetComponent<Health>();

        if (enemy != null)
        {
            enemy.TakeDamage(50);
        }
    }


}
