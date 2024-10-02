using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (currentSceneName == "Level 2")
        {
            ResetScore();
            SceneManager.LoadScene("Level 1");
        }
    }

    public void RestartGame()
    {
        ResetScore();
        SceneManager.LoadScene("Level 1");
    }
}
