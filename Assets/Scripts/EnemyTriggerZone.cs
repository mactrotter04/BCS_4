using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{
    EnemyController enemyController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyController.OnPlayerDetected(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>() != null)
        {
            enemyController.OnPlayerLost();
        }
    }
}
