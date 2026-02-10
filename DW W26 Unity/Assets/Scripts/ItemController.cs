using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Pickup code help from https://discussions.unity.com/t/how-to-pick-up-an-item-in-2d/243149

    private bool IsPickupAllowed;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null) return;

        if (IsPickupAllowed && !player.GetComponent<PlayerController>().hasItem && player.GetComponent<PlayerController>().canPickup)
        {
            player.GetComponent<PlayerController>().hasItem = true;
            PickUp();
        }

        if (player.GetComponent<PlayerController>().hasItem && !player.GetComponent<PlayerController>().canPickup)
        {
            player.GetComponent<PlayerController>().hasItem = false;
            Throw();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            IsPickupAllowed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            IsPickupAllowed = false;
        }
    }

    public void PickUp()
    {
        if (IsPickupAllowed && player != null)
        {
            // Attach the item to the player that picked it up
            this.transform.SetParent(player.transform);
            // Put the item in front of the player
            this.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
        }
    }

    public void Throw()
    {
        // Remove the item from the player
        this.transform.SetParent(GameObject.Find("ItemStorage").transform);
        IsPickupAllowed = false;
    }
}
