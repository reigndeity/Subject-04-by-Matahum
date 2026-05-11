using UnityEngine;

public class CCTVController : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Camera cctvCamera;
    public GameObject cctvPanel;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Control")]
    public bool isControllable = true;
    public float rotationSpeed = 60f;

    [Header("Auto Rotation")]
    public bool autoRotateHorizontal = true;
    public bool autoRotateVertical = false;

    [Header("Horizontal Limits")]
    public float maxLookLeft = 45f;
    public float maxLookRight = 45f;

    [Header("Vertical Limits")]
    public float maxLookUp = 20f;
    public float maxLookDown = 20f;

    [Header("Auto Rotation Speeds")]
    public float horizontalAutoSpeed = 30f;
    public float verticalAutoSpeed = 15f;

    float yaw;
    float pitch;

    float baseYaw;
    float basePitch;

    int horizontalDirection = 1;
    int verticalDirection = 1;

    bool isOpen;
    Transform player;

    void Awake()
    {
        Vector3 angles = transform.localEulerAngles;

        baseYaw = NormalizeAngle(angles.y);
        basePitch = NormalizeAngle(angles.x);

        yaw = baseYaw;
        pitch = basePitch;

        if (cctvCamera != null)
            cctvCamera.gameObject.SetActive(false);

        if (cctvPanel != null)
            cctvPanel.SetActive(false);
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (!isOpen)
        {
            CheckInteraction();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Close();

        if (isControllable)
            HandleManualControl();
        else
            HandleAutoRotation();

        ApplyRotation();
    }

    void CheckInteraction()
    {
        if (player == null)
            return;

        if (!Input.GetKeyDown(interactKey))
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
            Interact();
    }

    public void Interact()
    {
        Open();
    }

    void Open()
    {
        isOpen = true;

        if (cctvPanel != null)
            cctvPanel.SetActive(true);

        if (cctvCamera != null)
            cctvCamera.gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;

        if (cctvPanel != null)
            cctvPanel.SetActive(false);

        if (cctvCamera != null)
            cctvCamera.gameObject.SetActive(false);
    }

    void HandleManualControl()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontal = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontal = 1f;

        if (Input.GetKey(KeyCode.W))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S))
            vertical = -1f;

        yaw += horizontal * rotationSpeed * Time.deltaTime;
        pitch -= vertical * rotationSpeed * Time.deltaTime;

        ClampRotation();
    }

    void HandleAutoRotation()
    {
        if (autoRotateHorizontal)
        {
            yaw += horizontalDirection * horizontalAutoSpeed * Time.deltaTime;

            if (yaw >= baseYaw + maxLookRight)
            {
                yaw = baseYaw + maxLookRight;
                horizontalDirection = -1;
            }

            if (yaw <= baseYaw - maxLookLeft)
            {
                yaw = baseYaw - maxLookLeft;
                horizontalDirection = 1;
            }
        }

        if (autoRotateVertical)
        {
            pitch += verticalDirection * verticalAutoSpeed * Time.deltaTime;

            if (pitch >= basePitch + maxLookDown)
            {
                pitch = basePitch + maxLookDown;
                verticalDirection = -1;
            }

            if (pitch <= basePitch - maxLookUp)
            {
                pitch = basePitch - maxLookUp;
                verticalDirection = 1;
            }
        }
    }

    void ClampRotation()
    {
        yaw = Mathf.Clamp(yaw, baseYaw - maxLookLeft, baseYaw + maxLookRight);
        pitch = Mathf.Clamp(pitch, basePitch - maxLookUp, basePitch + maxLookDown);
    }

    void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}   