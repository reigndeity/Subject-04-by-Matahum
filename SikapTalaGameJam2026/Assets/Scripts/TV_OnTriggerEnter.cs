using UnityEngine;

public class TV_OnTriggerEnter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.CameraMode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.FirstPersonMode();
        }
    }
}
