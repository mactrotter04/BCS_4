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

    [SerializeField] CapsuleCollider2D swordHitbox;
    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;

    bool playerHasHorizontalSpeed;
    bool isAlive = true;

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
        Death();
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

    void Death()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enimeies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Death");
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            FindAnyObjectByType<GameSession>().processPlayerDeath();
            enabled = false;
            Invoke(nameof(DisableAnimator), 1.5f);
        }
    }

    void DisableAnimator()
    {
        animator.enabled = false;
    }

    void Attack()
    {
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        swordHitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        swordHitbox.enabled = false;
    }


    void OnAttack()
    {
        Attack();
        animator.SetTrigger("Attack");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnimieHealth enemy = other.GetComponent<EnimieHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(50);
        }
    }


}
