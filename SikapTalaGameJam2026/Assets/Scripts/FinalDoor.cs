using TMPro;
using UnityEngine;

public class FinalDoor : MonoBehaviour, IInteractable
{
    public float interactDistance = 3f;
    public float interactHeight = 5f;

    Transform player;
    Outline outline;
    GameObject interactText;

    void Awake()
    {
        outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;

        GameObject canvas = GameObject.Find("Canvas - SSO");

        if (canvas != null)
        {
            Transform textTransform = canvas.transform.Find("InteractText");

            if (textTransform != null)
                interactText = textTransform.gameObject;
        }

        if (interactText != null)
            interactText.SetActive(false);
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 doorPosition = transform.position;
        Vector3 playerPosition = player.position;

        Vector2 doorXZ = new Vector2(doorPosition.x, doorPosition.z);
        Vector2 playerXZ = new Vector2(playerPosition.x, playerPosition.z);

        float horizontalDistance = Vector2.Distance(playerXZ, doorXZ);
        float verticalDistance = Mathf.Abs(playerPosition.y - doorPosition.y);

        bool canInteract =
            horizontalDistance <= interactDistance &&
            verticalDistance <= interactHeight;

        if (outline != null)
            outline.enabled = canInteract;

        if (interactText != null)
        {
            if (canInteract)
            {
                if (IInteractable.currentInteractable != this)
                {
                    IInteractable.currentInteractable = this;

                    interactText.SetActive(true);

                    interactText
                        .GetComponent<TextMeshProUGUI>()
                        .text =
                        "Press <color=yellow>E</color> to interact with the Door";
                }
            }
            else
            {
                if (IInteractable.currentInteractable == this)
                {
                    interactText.SetActive(false);
                    IInteractable.currentInteractable = null;
                }
            }
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    public void Interact()
    {
        if (interactText != null)
            interactText.SetActive(false);

        GameManager.instance.EndGame();
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