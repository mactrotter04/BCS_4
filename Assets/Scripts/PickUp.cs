using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<GameSession>().AddToScore(pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
