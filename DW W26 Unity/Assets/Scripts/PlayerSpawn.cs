using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    [field: SerializeField] public Color[] PlayerColors { get; private set; }
    [field: SerializeField] public int PlayerCount { get; private set; }
    public bool[] playerSlots = new bool[6];

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Increment player count
        PlayerCount++;

        //for each slot in player slot, count through the slots
        //if the slot is false then make it true and then stop
        //if the slot is true then keep going up until a false slot and then make the slot true
        foreach (bool slot in playerSlots)
        {
            int slotIndex = 0;
            if (slot == false)
            {
                playerSlots[slotIndex] = true;
                slotIndex++;
                break;
            }
            if (slot == true)
            {
                slotIndex++;
                return;
            }
            else
            {
                playerSlots[slotIndex] = true;
                PlayerCount = slotIndex;
                break;
            }
        }

        int maxPlayerCount = Mathf.Min(SpawnPoints.Length, PlayerColors.Length);
        if (maxPlayerCount < 1)
        {
            string msg =
                $"You forgot to assign {name}'s {nameof(PlayerSpawn)}.{nameof(SpawnPoints)}" +
                $"and {nameof(PlayerSpawn)}.{nameof(PlayerColors)}!";
            Debug.Log(msg);
        }

        // Prevent adding in more than max number of players
        if (PlayerCount >= maxPlayerCount)
        {
            // Delete new object
            string msg =
                $"Max player count {maxPlayerCount} reached. " +
                $"Destroying newly spawned object {playerInput.gameObject.name}.";
            Debug.Log(msg);
            Destroy(playerInput.gameObject);
            return;
        }

        // Assign spawn transform values
        playerInput.transform.position = SpawnPoints[PlayerCount].position;
        playerInput.transform.rotation = SpawnPoints[PlayerCount].rotation;
        Color color = PlayerColors[PlayerCount];

        // Set up player controller
        PlayerController playerController = playerInput.gameObject.GetComponent<PlayerController>();
        playerController.AssignPlayerInputDevice(playerInput);
        playerController.AssignPlayerNumber(PlayerCount);
        playerController.AssignColor(color);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        // Not handling anything right now.
        Debug.Log("Player left...");

        //for each slot in player slot, count through the slots
        //if the slot is true then make it false and then stop
        //if the slot is false then keep going up until a true slot and then make the slot false
        foreach (bool slot in playerSlots)
        {
            int slotIndex = 0;
            if (slot == true)
            {
                playerSlots[slotIndex] = false;
                PlayerCount = slotIndex;
                break;
            }
            if (slot == false)
            {
                slotIndex++;
                return;
            }
            else
            {
                playerSlots[slotIndex] = false;
                PlayerCount = slotIndex;
                break;
            }
        }
    }
}
