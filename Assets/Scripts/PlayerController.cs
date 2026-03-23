using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator animator;

    bool playerHasHorizontalSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
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
}
