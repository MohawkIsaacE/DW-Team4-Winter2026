using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private PlayerInput PlayerInput;
    private InputAction InputActionInteract;

    private void Start()
    {
        InputActionInteract = PlayerInput.actions.FindAction($"Player/Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputActionInteract.WasPressedThisFrame())
        {
            SceneManager.LoadScene("IsaacTestScene");
        }
    }
}
