using UnityEngine;

public class TV_Optimizer : MonoBehaviour
{
    private Player player;

    [Header("Cameras")]
    public Camera frontCamera;
    public Camera backCamera;

    [Header("Optimization")]
    public float distanceToRender = 15f;

    private bool camerasActive;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
        SetCameras(false);
    }

    /*private void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.transform.position
            );

        bool shouldRender = distance <= distanceToRender;

        if (shouldRender != camerasActive)
        {
            SetCameras(shouldRender);
        }
    }*/

    public void SetCameras(bool state)
    {
        camerasActive = state;

        if (frontCamera != null)
            frontCamera.enabled = state;

        if (backCamera != null)
            backCamera.enabled = state;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            distanceToRender
        );
    }
}