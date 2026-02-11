using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    [SerializeField] public int Team1Score;
    [SerializeField] public int Team2Score;

    [SerializeField] public TextMeshProUGUI Team1ScoreText;
    [SerializeField] public TextMeshProUGUI Team2ScoreText;

    [SerializeField] public float gameTime {  get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Team1Score = 0;
        Team2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Randomly spawn a new item for each time at an interval
        // More items get added as the game progresses
    }

    public void UpdateScores()
    {
        Team1ScoreText.text = $"{Team1Score}";
        Team2ScoreText.text = $"{Team2Score}";
    }
}
