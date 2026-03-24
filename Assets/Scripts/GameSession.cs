using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int PlayerLives = 3;
    [SerializeField] float gameSessionResetDelay = 2f;

    void Awake()
    {
        //singleton pattern 
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if (numGameSessions > 1)
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

    public void processPlayerDeath()
    {
        if (PlayerLives > 0)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void ResetGameSession()
    {
        Invoke(nameof(ReloadGame), gameSessionResetDelay);
        Destroy(gameObject);
    }

    void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }


    void TakeLife()
    {
        PlayerLives--;
        Invoke(nameof(ReloadCurrentLevel), gameSessionResetDelay);
    }

    void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
