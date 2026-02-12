using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenReadyCheck : MonoBehaviour
{
    //holy variables
    [SerializeField] string playerTag; //type player so the script can find the spawned player prefab clones
    [SerializeField] Scene titleScene; //script will automatically select scene with the index 1 as title scene, please don't screw this up
    [SerializeField] string targetScene; //type in the name of the target scene to load when players are ready and press start
    [SerializeField] GameObject[] players = new GameObject[6]; //array for the actual player prefabs
    [SerializeField] PlayerInput[] playerInput = new PlayerInput[6]; //grab the player inputs and put them in the array to check what buttons they press
    [SerializeField] bool[] playerReady = new bool[6]; //array to see which players are ready
    [SerializeField] bool playersReady4; //bool for 4 players
    [SerializeField] bool playersReady6; //bool for 6 players

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleScene = SceneManager.GetSceneByBuildIndex(1);
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);

        //bools are true if the correct number of players are ready
        playersReady4 = playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3];
        playersReady6 = playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3] && playerReady[4] && playerReady[5];

        for (int i = 0; i < players.Length; i++)
        {
            //to avoid null errors or whatever, i don't know if this part is still needed but i'm too scared to take it out
            if (players.Length != 0)
            {
                playerInput[i] = players[i].GetComponent<PlayerInput>();
            }
            else
            {
                return;
            }

            //first part: sets players to ready if they press the west button or left click
            //second part: disconnects player and resets values if they press the east button or the c key
            foreach (var p in players)
            {
                if (SceneManager.GetActiveScene() == titleScene && playerInput[i].actions["Attack"].IsPressed())
                {
                    playerReady[i] = true;
                }

                if (SceneManager.GetActiveScene() == titleScene && playerInput[i].actions["Crouch"].IsPressed())
                {
                    Destroy(players[i]);
                    playerReady[i] = false;
                    playerInput[i] = null;
                }
            }

            //if 4 or 6 players are ready and someone presses jump, the game will go to the target scene
            //set the target scene by typing in its name in the box in the inspector
            if (playersReady4 == true || playersReady6 == true)
            {
                if (playerInput[i].actions["Jump"].IsPressed())
                {
                    SceneManager.LoadScene(targetScene);
                }
            }
        }
    }
}
