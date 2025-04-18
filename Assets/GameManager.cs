using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalScore = 0; // Persistent total score
    // public Text scoreText; // UI text to display score

    [DllImport("__Internal")]
    private static extern void SendScore(int score, int game);

    void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore(); // Load the saved score when the game starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        totalScore += points;
        SaveScore(); // Save new score
        UpdateScoreUI();
    }

    public void SendFinalScore()
    {
        SendScore(totalScore, 67); // Send the score
    }

    private void UpdateScoreUI()
    {
        // if (scoreText != null)
        // {
        //     scoreText.text = "Score: " + totalScore;
        // }
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("SavedScore", totalScore);
        PlayerPrefs.Save(); // Save the score permanently
    }

    private void LoadScore()
    {
        totalScore = PlayerPrefs.GetInt("SavedScore", 0); // Load score, default is 0
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("SavedScore");
        totalScore = 0;
        UpdateScoreUI();
    }
}
