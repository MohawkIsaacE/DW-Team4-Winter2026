using UnityEngine;

public class ItemChipBehaivour : MonoBehaviour
{
    private Vector2 direction;
    private float moveTimer;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Random.insideUnitCircle;
        rb = GetComponent<Rigidbody2D>();
        // Magic number - adjust until it feels right
        // Updated Wednesday 3:42pm
        moveTimer = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Move in the direction chosen for 1 second, then stop
        if (moveTimer > 0)
        {
            rb.linearVelocity = direction * 2;
            moveTimer -= Time.deltaTime;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
