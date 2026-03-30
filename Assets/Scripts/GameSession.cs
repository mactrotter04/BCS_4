using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int PlayerHP = 100;
    [SerializeField] float gameSessionResetDelay = 2f;
    

    [Header ("scores")]
    public TextMeshProUGUI HPText;
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    PlayerController playerController;

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
        HPText.text = PlayerHP.ToString();
        scoreText.text = score.ToString();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void processPlayerDeath()
    {
        if (playerController.GetComponent<Health>().GetHealth() <= 0)
        {
            Invoke(nameof(ReloadCurrentLevel), gameSessionResetDelay);
        }
        else
        {
            ResetGameSession();
        }
    }

    void ResetGameSession()
    {
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        Invoke(nameof(ReloadGame), gameSessionResetDelay);
        
    }

    void ReloadGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }


   
        
        
    

    void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

}
