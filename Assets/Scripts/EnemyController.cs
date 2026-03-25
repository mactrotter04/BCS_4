using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb2d;
    public TextMeshPro  healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ground"))
        {
            moveSpeed = -moveSpeed;
            FilipEnemyFace();
        }
    }

    void FilipEnemyFace()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb2d.linearVelocity.x) *Mathf.Abs(transform.localScale.x)),transform.localScale.y);
        healthText.transform.localScale = new Vector2(-(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x)), transform.localScale.y);
    }

}
