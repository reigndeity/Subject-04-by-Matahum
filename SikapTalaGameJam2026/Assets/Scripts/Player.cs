using UnityEngine;

public class Player : MonoBehaviour
{
    public static bool inputLocked;

    PlayerMovement playerMovement;
    PlayerCamera playerCamera;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    void Update()
    {
        playerCamera.Look();

        if (inputLocked)
            return;

        playerMovement.Move();
    }
}