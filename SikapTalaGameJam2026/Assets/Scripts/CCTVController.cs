using UnityEngine;

public class CCTVController : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Camera cctvCamera;
    public GameObject cctvPanel;
    public GameObject objectToRotate;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public float interactHeight = 5f;
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
    Outline outline;

    void Awake()
    {
        if (objectToRotate == null)
            objectToRotate = gameObject;

        outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;

        Vector3 angles = objectToRotate.transform.localEulerAngles;

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
        UpdateOutline();

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

    void UpdateOutline()
    {
        if (outline == null || player == null)
            return;

        Vector3 cctvPosition = transform.position;
        Vector3 playerPosition = player.position;

        Vector2 cctvXZ = new Vector2(cctvPosition.x, cctvPosition.z);
        Vector2 playerXZ = new Vector2(playerPosition.x, playerPosition.z);

        float horizontalDistance = Vector2.Distance(playerXZ, cctvXZ);
        float verticalDistance = Mathf.Abs(playerPosition.y - cctvPosition.y);

        outline.enabled =
            horizontalDistance <= interactDistance &&
            verticalDistance <= interactHeight &&
            !isOpen;
    }

    void CheckInteraction()
    {
        if (player == null)
            return;

        if (!Input.GetKeyDown(interactKey))
            return;

        Vector3 cctvPosition = transform.position;
        Vector3 playerPosition = player.position;

        Vector2 cctvXZ = new Vector2(cctvPosition.x, cctvPosition.z);
        Vector2 playerXZ = new Vector2(playerPosition.x, playerPosition.z);

        float horizontalDistance = Vector2.Distance(playerXZ, cctvXZ);
        float verticalDistance = Mathf.Abs(playerPosition.y - cctvPosition.y);

        if (horizontalDistance <= interactDistance && verticalDistance <= interactHeight)
            Interact();
    }

    public void Interact()
    {
        Open();
    }

    void Open()
    {
        isOpen = true;
        Player.instance.inputLocked = true;
        //Player.instance.CameraMode();

        if (cctvPanel != null)
            cctvPanel.SetActive(true);

        if (cctvCamera != null)
            cctvCamera.gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        Player.instance.inputLocked = false;
        //Player.instance.FirstPersonMode();

        if (cctvPanel != null)
            cctvPanel.SetActive(false);

        if (cctvCamera != null)
            cctvCamera.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        objectToRotate.transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = transform.position;
        center.y -= interactHeight * 0.5f;

        Vector3 size = new Vector3(
            interactDistance * 2f,
            interactHeight,
            interactDistance * 2f
        );

        Gizmos.DrawWireCube(center, size);
    }
}