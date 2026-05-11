using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public Camera playerCamera;

    [Header("UI Panel Cursor Control")]
    public GameObject[] uiPanels;

    [Header("Look Limits")]
    public float maxLookUp = 90f;
    public float maxLookDown = 90f;
    public bool limitHorizontalLook = false;
    public float maxLookLeft = 360f;
    public float maxLookRight = 360f;

    bool cursorLocked = true;
    float pitch;
    float yaw;

    void Awake()
    {
        SetCursorState(true);
        yaw = transform.eulerAngles.y;
    }

    public void Look()
    {
        HandleCursor();

        if (!cursorLocked)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookUp, maxLookDown);

        yaw += mouseX;

        if (limitHorizontalLook)
            yaw = Mathf.Clamp(yaw, -maxLookLeft, maxLookRight);

        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void HandleCursor()
    {
        if (IsAnyPanelActive())
        {
            SetCursorState(false);
            return;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
            SetCursorState(false);
        else
            SetCursorState(true);
    }

    bool IsAnyPanelActive()
    {
        if (uiPanels == null || uiPanels.Length == 0)
            return false;

        for (int i = 0; i < uiPanels.Length; i++)
        {
            if (uiPanels[i] != null && uiPanels[i].activeInHierarchy)
                return true;
        }

        return false;
    }

    void SetCursorState(bool locked)
    {
        if (cursorLocked == locked)
            return;

        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}