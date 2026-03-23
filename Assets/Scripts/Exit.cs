using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int currntSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currntSceneIndex + 1;

            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("no more scenes to load");
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
