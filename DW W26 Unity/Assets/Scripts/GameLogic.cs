using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    [SerializeField] public int Team1Score {  get; private set; }
    [SerializeField] public int Team2Score {  get; private set; }

    [SerializeField] public TextMeshProUGUI Team1ScoreText;
    [SerializeField] public TextMeshProUGUI Team2ScoreText;

    public UnityEvent Team1Point;
    public UnityEvent Team2Point;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Team1ScoreText.text = $"{Team1Score}";
        Team2ScoreText.text = $"{Team2Score}";
    }

    public void UpdateScores()
    {
        
    }
}
