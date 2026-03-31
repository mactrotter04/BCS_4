using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public TextMeshPro healthText;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float chaseSpeed = 2f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float reactDelay = 0.5f;

    PlayerController playerController;
    Transform playerTransform;
    Rigidbody2D rb2d;
    bool isChasing = false;
    bool isReacting = false;
    bool pendingFlip = false;
    float lastAttackTime = 0f;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerController>().transform;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb2d.linearVelocity = new Vector2(moveSpeed, 0f);
        if (isChasing && playerTransform != null)
        {
            float distanceToPlayer = Mathf.Abs(playerTransform.position.x - transform.position.x);

            if (distanceToPlayer <= attackRange)
            {
                rb2d.linearVelocity = Vector2.zero;

                if (Time.time >= attackCooldown + lastAttackTime)
                {
                    lastAttackTime = Time.time;
                    animator.ResetTrigger("React");
                    animator.SetTrigger("Attack");
                }
            }
            else if (!pendingFlip)
            {
                float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
                rb2d.linearVelocity = new Vector2(direction * chaseSpeed, rb2d.linearVelocity.y);
                FilipEnemyFace(-direction);
            }
            else
            {
                rb2d.linearVelocity = Vector2.zero;
            }
        }
        else if (!isChasing && !isReacting)
        {
            rb2d.linearVelocity = new Vector2(moveSpeed, rb2d.linearVelocity.y);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("ground")) return;
        if (isChasing || isReacting)
        {
            pendingFlip = true;
            return;
        }
        moveSpeed = -moveSpeed;
        FilipEnemyFace(Mathf.Sign(rb2d.linearVelocity.x));
    }

    void FilipEnemyFace(float dir)
    {
        transform.localScale = new Vector2(-dir * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        healthText.transform.localScale = new Vector2(-dir * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    public void OnPlayerDetected(Transform detectedPlayer)
    {
        if (isChasing || isReacting) return;
        playerTransform = detectedPlayer;
        StartCoroutine(ReactThenChase());
    }

    public void OnPlayerLost()
    {
        StopAllCoroutines();
        isChasing = false;
        isReacting = false;
        playerTransform = null;
        if (pendingFlip)
        {
            moveSpeed = -moveSpeed;
            pendingFlip = false;
        }
        FilipEnemyFace(-Mathf.Sign(moveSpeed));
        animator.Play("Skeloton_Walk");
    }

    IEnumerator ReactThenChase()
    {
        isReacting = true;
        isChasing = false;
        rb2d.linearVelocity = Vector2.zero;
        animator.SetTrigger("React");
        yield return new WaitForSeconds(reactDelay);
        isReacting = false;
        animator.Play("Skeloton_Walk");
        isChasing = true;
    }
}
