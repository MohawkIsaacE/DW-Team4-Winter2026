using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenReadyCheck : MonoBehaviour
{
    [SerializeField] string playerTag;
    [SerializeField] bool[] playerReady = new bool[6];
    [SerializeField] Scene titleScene;
    [SerializeField] PlayerInput[] playerInput = new PlayerInput[6];
    [SerializeField] GameObject[] players = new GameObject[6];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleScene = SceneManager.GetSceneByBuildIndex(1);
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag);

        for (int i = 0; i < players.Length; i++)
        {
            if (players.Length != 0)
            {
                playerInput[i] = players[i].GetComponent<PlayerInput>();
            }
            else
            {
                return;
            }

            foreach (var p in players)
            {
                if (SceneManager.GetActiveScene() == titleScene && playerInput[i].actions["Attack"].IsPressed())
                {
                    playerReady[i] = true;
                }
            }
        }
    }
}
