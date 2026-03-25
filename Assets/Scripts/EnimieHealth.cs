using Unity.VisualScripting;
using UnityEngine;

public class EnimieHealth : MonoBehaviour
{
    [SerializeField] int enemyHealth = 100;
    int currentHealth;
    [SerializeField] int pointsForEnemy = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       currentHealth = enemyHealth;
       GetComponentInParent<EnemyController>().healthText.text = enemyHealth.ToString() + ":HP";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GetComponentInParent<EnemyController>().healthText.text = currentHealth.ToString() + ":HP";

        if (currentHealth <= 0 )
        {
            Die();
        }


        
    }
    void Die()
    {
        Destroy(gameObject);
        FindFirstObjectByType<GameSession>().AddToScore(pointsForEnemy);
    }

    







}

