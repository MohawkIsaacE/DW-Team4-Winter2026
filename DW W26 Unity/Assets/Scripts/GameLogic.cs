using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    [SerializeField] public int Team1Score;
    [SerializeField] public int Team2Score;

    [SerializeField] public TextMeshProUGUI Team1ScoreText;
    [SerializeField] public TextMeshProUGUI Team2ScoreText;

    public UnityEvent Team1Point;
    public UnityEvent Team2Point;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Team1Score = 0;
        Team2Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScores()
    {
        Team1ScoreText.text = $"{Team1Score}";
        Team2ScoreText.text = $"{Team2Score}";
    }
}
