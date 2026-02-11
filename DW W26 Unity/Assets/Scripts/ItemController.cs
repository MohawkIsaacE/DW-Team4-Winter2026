using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Pickup code help from https://discussions.unity.com/t/how-to-pick-up-an-item-in-2d/243149

    public bool IsPickupAllowed;
    private GameObject player;
    private int itemNum;
    private int playerNum;
    public bool hasBeenThrown;

    public GameLogic gameLogic;
    [SerializeField] public Rigidbody2D rb { get; private set; }
    [SerializeField] public float throwSpeed { get; private set; } = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsPickupAllowed = true;
        rb = GetComponent<Rigidbody2D>();
        gameLogic = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // When the player has no item and presses the pickup key, pick up the item
        if (IsPickupAllowed && !player.GetComponent<PlayerController>().hasItem && player.GetComponent<PlayerController>().canPickup)
        {
            player.GetComponent<PlayerController>().hasItem = true;
            playerNum = player.GetComponent<PlayerController>().PlayerNumber;
            itemNum = playerNum;
            PickUp();
            IsPickupAllowed = false;
        }

        // When the player has an item, throw it
        if (player.GetComponent<PlayerController>().hasItem && player.GetComponent<PlayerController>().hasThrown)
        {
            player.GetComponent<PlayerController>().hasItem = false;
            Throw();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if item has been thrown yet first
        if (collision.gameObject.CompareTag("Player") && hasBeenThrown)
        {
            if (itemNum / 2 == 0) // Left team
            {
                Destroy(gameObject);
                // add points to left team
                gameLogic.Team1Score += 1;
            }
            else if (itemNum / 2 == 1) // Right team
            {
                Destroy(gameObject);
                // add points to right team
                gameLogic.Team2Score += 1;
            }
            else
            {
                Debug.Log("Error: No player found");
            }
            gameLogic.UpdateScores();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            IsPickupAllowed = true;
        }
    }

    public void PickUp()
    {
        if (IsPickupAllowed && player != null)
        {
            // Attach the item to the player that picked it up
            this.transform.SetParent(player.transform);
            // Put the item in front of the player depending on which team
            if (itemNum / 2 == 0) // Left team
            {
                this.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
            }
            else if (itemNum / 2 == 1) // Right team
            {
                this.transform.position = new Vector2(player.transform.position.x - 1, player.transform.position.y);
            }
            else
            {
                Debug.Log("Error: No player found");
            }
        }
    }

    public void Throw()
    {
        // Remove the item from the player
        this.transform.SetParent(GameObject.Find("ItemStorage").transform);
        // Make sure no one else can pick up the item
        IsPickupAllowed = false;
        hasBeenThrown = true;

        // Start moving the item (throw it)
        // Direction depends on who threw it
        if (itemNum / 2 == 0) // Left team
        {
            // This is where custom throws would go
            rb.linearVelocity = Vector2.right * throwSpeed;
        }
        else if (itemNum / 2 == 1) // Right team
        {
            // This is where custom throws would go
            rb.linearVelocity = Vector2.left * throwSpeed;
        }
        else
        {
            Debug.Log("Error: No player found");
        }
    }
}
