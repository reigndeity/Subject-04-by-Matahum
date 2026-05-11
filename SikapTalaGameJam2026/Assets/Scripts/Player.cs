using UnityEngine;

public class Player : MonoBehaviour
{
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
        playerMovement.Move();
    }
}