using UnityEngine;

public class ScenePersist : MonoBehaviour
{

    void Awake()
    {
        //singleton pattern 
        int numScenePersist = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;

        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
