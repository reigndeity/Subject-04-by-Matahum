using UnityEngine;

public class MainMenuCameraParallax : MonoBehaviour
{
    public float positionAmount = 0.1f;
    public float rotationAmount = 2f;
    public float smoothSpeed = 5f;
    public float idleDelay = 2f;

    public bool invertX;
    public bool invertY;

    public float influence = 0f;

    Vector3 startPos;
    Quaternion startRot;

    Vector3 lastMousePos;
    float idleTimer;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        Vector3 currentMousePos = Input.mousePosition;

        if (currentMousePos != lastMousePos)
        {
            idleTimer = 0f;
            lastMousePos = currentMousePos;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }

        float mouseX = (currentMousePos.x / Screen.width - 0.5f) * 2f;
        float mouseY = (currentMousePos.y / Screen.height - 0.5f) * 2f;

        if (invertX) mouseX *= -1f;
        if (invertY) mouseY *= -1f;

        Vector3 targetPos;
        Quaternion targetRot;

        if (idleTimer >= idleDelay)
        {
            targetPos = startPos;
            targetRot = startRot;
        }
        else
        {
            targetPos = startPos + new Vector3(mouseX * positionAmount, mouseY * positionAmount, 0f) * influence;
            targetRot = startRot * Quaternion.Euler(-mouseY * rotationAmount * influence, mouseX * rotationAmount * influence, 0f);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}