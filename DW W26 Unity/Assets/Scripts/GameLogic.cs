using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] public int Team1Score;
    [SerializeField] public int Team2Score;

    [SerializeField] public TextMeshProUGUI Team1ScoreText;
    [SerializeField] public TextMeshProUGUI Team2ScoreText;

    [Header("Game Time")]
    [SerializeField] public TextMeshProUGUI timerLeft;
    [SerializeField] public TextMeshProUGUI timerRight;
    [SerializeField] public float currentGameTime { get; private set; }
    private float maxGameTime = 180f; // 180f = 3 minutes
    private float newItemTimer;

    [Header("Item Spawning")]
    public GameObject[] items = new GameObject[4];
    private GameObject newItemLeft;
    private GameObject newItemRight;
    private int currentItemVariety;
    [field: SerializeField] public Transform[] itemSpawnPoints { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Team1Score = 0;
        Team2Score = 0;
        newItemTimer = 0;
        currentGameTime = maxGameTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Time handling and display
        currentGameTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentGameTime / 60);
        int seconds = Mathf.FloorToInt(currentGameTime - minutes * 60);
        string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerLeft.text = $"Time: {gameTimeString}";
        timerRight.text = $"Time: {gameTimeString}";

        // Game end condition
        // When game time runs out
        if (currentGameTime <= 0f)
        {
            // Decide which scene to load based on which team had most points
            if (Team1Score > Team2Score) // Left team winners
            {
                SceneManager.LoadScene("LeftTeamWin");
            }
            else if (Team1Score < Team2Score) // Right team winners
            {
                SceneManager.LoadScene("RightTeamWin");
            }
            else // Tie
            {
                SceneManager.LoadScene("NoTeamWin");
            }
        }

        // Randomly spawn a new item for each time at an interval
        // More items get added as the game progresses
        if (currentGameTime < maxGameTime / 4) // Quarter of the way through the time
        {
            // Adds spicy
            currentItemVariety = 4;
        }
        else if (currentGameTime < maxGameTime / 2) // Half of the way through the time
        {
            // Adds chips
            currentItemVariety = 3;
        }
        else if (currentGameTime < maxGameTime / 4 * 3) // Three quarters of the way through the time
        {
            // Adds pizza
            currentItemVariety = 2;
        }

        if (newItemTimer >= 2f)
        {
            // Spawn item on the left side
            newItemLeft = Instantiate(items[Random.Range(0, currentItemVariety)], itemSpawnPoints[0].transform.position, Quaternion.identity);
            newItemLeft.transform.SetParent(GameObject.Find("ItemStorage").transform);

            // Spawn item on the right side
            newItemRight = Instantiate(items[Random.Range(0, currentItemVariety)], itemSpawnPoints[1].transform.position, Quaternion.identity);
            newItemRight.transform.SetParent(GameObject.Find("ItemStorage").transform);

            // Reset the spawn timer
            newItemTimer = 0;
        }

        newItemTimer += Time.deltaTime;
    }

    public void UpdateScores()
    {
        Team1ScoreText.text = $"{Team1Score}";
        Team2ScoreText.text = $"{Team2Score}";
    }
}
