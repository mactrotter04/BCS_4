using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int points = 20;

    PlayerController playerController;
    EnemyController enemyController;
    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        if (enemyController)
        {
            enemyController.healthText = GetComponentInChildren<TextMeshPro>();
        }
       //// else if (playerController)
       // {
       //     enemyController.healthText = null;
       // }
       
    }

    public int GetHealth()
    {
        return health;
    }
    public void TakeDamage(int damage)
    {
        health -= damage; //health = health - damage;

        if (health <= 0)
        {
            if (playerController)
            {
                 playerController.PlayerDeath();
            }
            else
            {
                Destroy(gameObject);
                FindFirstObjectByType<GameSession>().AddToScore(points);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Damage damage = other.GetComponent<Damage>();

        if (damage != null)
        {
            TakeDamage(damage.GetDamage());
        }
    }



}
