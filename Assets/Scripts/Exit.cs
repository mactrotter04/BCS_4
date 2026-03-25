using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [Header ("Exit")]
    [SerializeField] float exitTime = 5f;
    [SerializeField] TextMeshPro extractionTimerText;


    bool InsideExit;
    Coroutine extractionCoroutine;
    float currentTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        extractionTimerText.gameObject.SetActive(false);
        extractionTimerText.text = "Extraction in: " + exitTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InsideExit = true;
            extractionTimerText.gameObject.SetActive(true);
            if (extractionCoroutine == null)
            {
                extractionCoroutine = StartCoroutine(updateTimer());
            }

            currentTime = exitTime;
            UpdateTimerText();
        }
    }
        

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InsideExit = false;
            extractionTimerText.gameObject.SetActive(false);
            if (extractionCoroutine != null)
            {
                StopCoroutine(updateTimer());
                extractionCoroutine = null;
            }


            currentTime = exitTime;
            UpdateTimerText();
        }
    }

    void LoadNextLevel()
    {
        int currntSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currntSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("no more scenes to load");

        }

        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator updateTimer()
    {
        currentTime = exitTime; //5s
        UpdateTimerText();

        while (currentTime > 0f) //while (true)
        {
            if(!InsideExit)
            {
                yield break;
            }

            currentTime -= Time.deltaTime;

            if(currentTime < 0f)
            {
                currentTime = 0f;
            }

            UpdateTimerText();
            yield return null;
        }

        extractionCoroutine = null;
        LoadNextLevel();
    }

    void UpdateTimerText()
    {
        extractionTimerText.text = "Extraction in: " + Mathf.CeilToInt (currentTime).ToString();
    }

}