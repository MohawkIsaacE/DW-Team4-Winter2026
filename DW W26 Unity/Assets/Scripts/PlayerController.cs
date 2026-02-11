using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Color PlayerColor { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 10f;
    [field: SerializeField] public float JumpForce { get; private set; } = 5f;

    public bool DoJump { get; private set; }

    // Player input information
    private PlayerInput PlayerInput;
    private InputAction InputActionMove;
    private InputAction InputActionJump;
    private InputAction InputActionAttack;
    public bool canPickup;
    public bool hasThrown;

    // Player-item interaction
    public bool hasItem;
    private GameObject item;
    public bool isSpicy;
    public float spicyTimer;

    // Assign color value on spawn from main spawner
    public void AssignColor(Color color)
    {
        // record color
        PlayerColor = color;

        // Assign to sprite renderer
        if (SpriteRenderer == null)
            Debug.Log($"Failed to set color to {name} {nameof(PlayerController)}.");
        else
            SpriteRenderer.color = color;
    }

    // Set up player input
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInput = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        InputActionMove = playerInput.actions.FindAction($"Player/Move");
        InputActionJump = playerInput.actions.FindAction($"Player/Jump");
        InputActionAttack = playerInput.actions.FindAction($"Player/Attack");
    }

    // Assign player number on spawn
    public void AssignPlayerNumber(int playerNumber)
    {
        this.PlayerNumber = playerNumber;
    }

    // Runs each frame
    public void Update()
    {
        canPickup = false;
        hasThrown = false;

        // Read the "Jump" action state, which is a boolean value
        if (InputActionAttack.WasPressedThisFrame() && !hasItem)
        {
            // Pick up an item if you don't already have one
            canPickup = true;
        }
        else if (InputActionAttack.WasPressedThisFrame() && hasItem)
        {
            // Throw the item you are holding
            hasThrown = true;
        }

        // Lower the spicy timer if player is spicy
        if (isSpicy && spicyTimer > 0)
        {
            spicyTimer -= Time.deltaTime;
        }
        else
        {
            isSpicy = false;
        }
    }

    // Runs each phsyics update
    void FixedUpdate()
    {
        if (Rigidbody2D == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody2D)} is null.");
            return;
        }

        // MOVE
        // Reset player movement each frame so it's snappy
        Rigidbody2D.linearVelocity = Vector2.zero;

        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();

        // Using force to move
        Vector2 moveForce = moveValue * MoveSpeed;

        // Invert controls if the player is spicy
        if (isSpicy) moveForce = -moveForce;

        // Apply force each frame
        Rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
    }

    // OnValidate runs after any change in the inspector for this script.
    private void OnValidate()
    {
        Reset();
    }

    // Reset runs when a script is created and when a script is reset from the inspector.
    private void Reset()
    {
        // Get if null
        if (Rigidbody2D == null)
            Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
