using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] public int Team1Score;
    [SerializeField] public int Team2Score;

    [SerializeField] public TextMeshProUGUI Team1ScoreText;
    [SerializeField] public TextMeshProUGUI Team2ScoreText;

    [Header("Game Time")]
    [SerializeField] public TextMeshProUGUI timer;
    [SerializeField] public float gameTime { get; private set; }
    private float newItemTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Team1Score = 0;
        Team2Score = 0;
        newItemTimer = 0;
        gameTime = 180;
    }

    // Update is called once per frame
    void Update()
    {
        // Time handling and display
        gameTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        timer.text = $"Time: {gameTimeString}";

        // Randomly spawn a new item for each time at an interval
        // More items get added as the game progresses
    }

    public void UpdateScores()
    {
        Team1ScoreText.text = $"{Team1Score}";
        Team2ScoreText.text = $"{Team2Score}";
    }
}
