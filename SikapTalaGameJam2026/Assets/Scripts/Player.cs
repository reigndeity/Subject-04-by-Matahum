using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public bool inputLocked;

    PlayerMovement playerMovement;
    PlayerCamera playerCamera;

    SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        if (instance == null) instance = this;

        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if (!inputLocked)
            playerCamera.Look();

        if (inputLocked)
            return;

        playerMovement.Move();
    }

    public void FirstPersonMode()
    {
        skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        skinnedMeshRenderer.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void CameraMode()
    {
        skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        skinnedMeshRenderer.gameObject.layer = LayerMask.NameToLayer("Invisible");
    }
}