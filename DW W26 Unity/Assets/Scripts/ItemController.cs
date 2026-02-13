using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Pickup code help from https://discussions.unity.com/t/how-to-pick-up-an-item-in-2d/243149

    public bool IsPickupAllowed;
    private GameObject player;
    private int itemNum;
    private int playerNum;
    private bool isLeftTeam;
    public bool hasBeenThrown;
    private float distanceTimer;
    private bool hasSpawnedChips;

    public ItemData data;

    public GameLogic gameLogic;
    public GameObject chipPrefab;
    [SerializeField] public Rigidbody2D rb { get; private set; }
    [SerializeField] public float throwSpeed { get; private set; } = 60f;
    private float chipThrowSpeed = 20f;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsPickupAllowed = true;
        rb = GetComponent<Rigidbody2D>();
        gameLogic = GameObject.Find("GameManager").GetComponent<GameLogic>();
        hasSpawnedChips = false;
        hasBeenThrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasSpawnedChips) return;

        // Stop moving the item after a certain amount of time (non-variable throw distance)
        if (hasBeenThrown && distanceTimer >= 0)
        {
            distanceTimer -= Time.deltaTime;
        }
        else if (hasBeenThrown)
        {
            rb.linearVelocity = Vector2.zero;

            if (data.itemName == Item.Donut) Destroy(gameObject);
            if (data.itemName == Item.Pizza) Destroy(gameObject);
            if (data.itemName == Item.Spicy) Destroy(gameObject);

            if (data.itemName == Item.Chips)
            {
                SpawnChipHazards();
                hasSpawnedChips = true;
                Destroy(gameObject);
            }
        }

        if (player == null) return;
        if (hasBeenThrown) return;

        // When the player has no item and presses the pickup key, pick up the item
        if (IsPickupAllowed && !player.GetComponent<PlayerController>().hasItem && player.GetComponent<PlayerController>().canPickup)
        {
            player.GetComponent<PlayerController>().hasItem = true;
            playerNum = player.GetComponent<PlayerController>().PlayerNumber;
            itemNum = playerNum;

            // Check which player team has picked up the item
            if (itemNum % 2 == 1) // Left team
            {
                isLeftTeam = true;
            }
            else if (itemNum % 2 == 0) // Right team
            {
                isLeftTeam = false;
            }
            else
            {
                Debug.Log("Error: No team found");
            }

            IsPickupAllowed = false;
            rb.linearVelocity = Vector2.zero;
            PickUp();
        }

        // When the player has an item, throw it
        if (player.GetComponent<PlayerController>().hasItem && player.GetComponent<PlayerController>().hasThrown)
        {
            player.GetComponent<PlayerController>().hasItem = false;
            distanceTimer = 1f; // About half way with 20f throwSpeed
            Throw();
        }
    }

    private void FixedUpdate()
    {
        // Move down the conveyor if it hasn't been picked up yet
        if (IsPickupAllowed)
        {
            // Reset movement
            rb.linearVelocity = Vector2.down * 5;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if item has been thrown yet first
        if (collision.gameObject.CompareTag("Player") && hasBeenThrown)
        {
            player = collision.gameObject;
            if (player.GetComponent<PlayerController>().PlayerNumber == itemNum) return;
            if (isLeftTeam) // Left team
            {
                Destroy(gameObject);
                // add points to left team
                gameLogic.Team1Score += 1;
                audioManager.Instance.source.PlayOneShot(audioManager.Instance.hitNoise);
            }
            else if (!isLeftTeam)
            {
                Destroy(gameObject);
                // add points to right team
                gameLogic.Team2Score += 1;
                audioManager.Instance.source.PlayOneShot(audioManager.Instance.hitNoise);
            }
            else
            {
                Debug.Log("Error: No player found");
            }

            // Spicy logic
            if (data.itemName == Item.Spicy)
            {
                player.GetComponent<PlayerController>().isSpicy = true;
                player.GetComponent<PlayerController>().spicyTimer = 5f;
                // The player needs a way to tell they are spicy - smoke maybe?
                //audioManager.Instance.PlayOneShot(audioManager.Instance.spicyNoise);
                //TEST COMMENT
            }

            if (data.itemName == Item.Chips)
            {
                SpawnChipHazards();
                hasSpawnedChips = true;
                audioManager.Instance.source.PlayOneShot(audioManager.Instance.chipCrunch);
            }

            gameLogic.UpdateScores();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            // Maybe add VFX for players getting hit
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
            // Maybe add some VFX for when an item hits a wall
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.GetComponent<PlayerController>().canPickup)
        {
            //player = null;
            IsPickupAllowed = true;
        }
    }

    public void PickUp()
    {
        if (player != null)
        {
            // Attach the item to the player that picked it up
            this.transform.SetParent(player.transform);
            // Put the item in front of the player depending on which team
            if (isLeftTeam) // Left team
            {
                // To the right of player
                this.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
            }
            else if (!isLeftTeam) // Right team
            {
                // To the left of player
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

        // Stop all movement first
        rb.linearVelocity = Vector2.zero;

        // Start moving the item (throw it)

        if (data.itemName == Item.Pizza)
        {
            ThrowPizza();
        }
        else if (data.itemName == Item.Donut)
        {
            ThrowDonut();
        }
        else if (data.itemName == Item.Spicy)
        {
            ThrowSpicy();
        }
        else if (data.itemName == Item.Chips)
        {
            ThrowChips();
        }
        else
        {
            Debug.Log($"This item has no name! {this.gameObject.name}");
        }
    }

    private void ThrowPizza()
    {
        // Detect if player is moving up, down, or not at all

        // TEMP
        // Direction depends on who threw it
        if (isLeftTeam) // Left team
        {
            rb.linearVelocity = Vector2.right * throwSpeed;
        }
        else if (!isLeftTeam) // Right team
        {
            rb.linearVelocity = Vector2.left * throwSpeed;
        }
        else
        {
            Debug.Log("Error: No player found");
        }
    }

    private void ThrowDonut()
    {
        // Direction depends on who threw it
        if (isLeftTeam) // Left team
        {
            rb.linearVelocity = Vector2.right * throwSpeed;
        }
        else if (!isLeftTeam) // Right team
        {
            rb.linearVelocity = Vector2.left * throwSpeed;
        }
        else
        {
            Debug.Log("Error: No player found");
        }
    }

    private void ThrowSpicy()
    {
        // Direction depends on who threw it
        // Throws faster than other foods and inverts player controls
        if (isLeftTeam) // Left team
        {
            rb.linearVelocity = Vector2.right * throwSpeed;
        }
        else if (!isLeftTeam) // Right team
        {
            rb.linearVelocity = Vector2.left * throwSpeed;
        }
        else
        {
            Debug.Log("Error: No player found");
        }
        //distanceTimer = 0.5f;
    }

    private void ThrowChips()
    {
        // TEMP
        // Direction depends on who threw it
        if (isLeftTeam) // Left team
        {
            rb.linearVelocity = Vector2.right * chipThrowSpeed;
        }
        else if (!isLeftTeam) // Right team
        {
            rb.linearVelocity = Vector2.left * chipThrowSpeed;
        }
        else
        {
            Debug.Log("Error: No player found");
        }

        // Throw out a bag of chips that explodes into a ground hazard
    }

    private void SpawnChipHazards()
    {
        GameObject newChip;

        // Spawn 5 chips in random directions
        for (int i = 0; i < 5; i++)
        {
            newChip = Instantiate(chipPrefab, gameObject.transform);
            newChip.transform.SetParent(GameObject.Find("ChipStorage").transform);
        }
    }
}
