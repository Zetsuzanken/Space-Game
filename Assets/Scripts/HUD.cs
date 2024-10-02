using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    public GameObject LifePrefab;
    public GameObject LivesPanel;
    public Text ScoreText;
    public Image EnergyBar;
    public GameObject EndGamePanel;
    public TextMeshProUGUI EndGameText;
    public TextMeshProUGUI EndGameScoreText;

    private List<GameObject> lifeIcons = new List<GameObject>();
    private int score;

    private const string WIN_MESSAGE = "You Won!";
    private const string LOSE_MESSAGE = "You Lost!";

    private bool isWin = false;

    public TextMeshProUGUI RestartButtonText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeLives(3);
        EndGamePanel.SetActive(false);
        SetScore(GameManager.Instance.Score);
    }

    public void InitializeLives(int maxLives)
    {
        foreach (GameObject icon in lifeIcons)
        {
            Destroy(icon);
        }
        lifeIcons.Clear();

        for (int i = 0; i < maxLives; i++)
        {
            GameObject lifeIcon = Instantiate(LifePrefab, LivesPanel.transform);
            lifeIcons.Add(lifeIcon);
        }
    }

    public void SetScore(int newScore)
    {
        ScoreText.text = "Score: " + newScore;
        EndGameScoreText.text = "Your score: " + newScore;
    }

    public void AddScore(int value)
    {
        GameManager.Instance.AddScore(value);
        SetScore(GameManager.Instance.Score);
    }

    public void SetEnergy(float value)
    {
        EnergyBar.fillAmount = value;
    }

    public void SetEnergyBarOpacity(float opacity)
    {
        Color color = EnergyBar.color;
        color.a = opacity;
        EnergyBar.color = color;
    }

    public void SetLives(int lives)
    {
        lives = Mathf.Clamp(lives, 0, lifeIcons.Count);

        for (int i = 0; i < lifeIcons.Count; i++)
        {
            lifeIcons[i].SetActive(i < lives);
        }
    }

    public void RestartPressed()
    {
        EndGamePanel.SetActive(false);
        if (isWin)
        {
            GameManager.Instance.LoadNextLevel();
        }
        else
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void ShowWinMessage()
    {
        isWin = true;
        EndGameText.text = WIN_MESSAGE;
        RestartButtonText.text = "Next Level";
        EndGamePanel.SetActive(true);
    }

    public void ShowLoseMessage()
    {
        isWin = false;
        EndGameText.text = LOSE_MESSAGE;
        RestartButtonText.text = "Restart";
        EndGamePanel.SetActive(true);
    }
}
