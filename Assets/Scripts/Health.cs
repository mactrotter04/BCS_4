using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int points = 20;
    [SerializeField] float iFrameDuration = 0.5f;
    [SerializeField] float deathDelay = 0.5f;

    float lastDamageTime = -999f;
    Animator animator;
    PlayerController playerController;
    EnemyController enemyController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();

        if (enemyController)
        {
            enemyController.healthText = GetComponentInChildren<TextMeshPro>();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + iFrameDuration) return;
        lastDamageTime = Time.time;

        health -= damage;

        if (playerController != null)
        {
            FindFirstObjectByType<GameSession>().HPText.text = health.ToString();
        }

        if (enemyController != null)
        {
            enemyController.healthText.text = health.ToString() + ":HP";
        }

        if (health <= 0)
        {
            if (playerController)
            {
                playerController.PlayerDeath();
            }
            else
            {
                animator.SetTrigger("Death");
                FindFirstObjectByType<GameSession>().AddToScore(points);
                Destroy(gameObject, deathDelay);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer == null) return;
        if (other.CompareTag(gameObject.tag)) return;
        if (playerController != null && playerController.isAttacking) return;
        TakeDamage(damageDealer.GetDamage());
    }
}
